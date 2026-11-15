import { isDeepStrictEqual } from 'node:util';
import { COLLECTIONS } from '../catalog/index.js';
import {
  ApprovalError,
  catalogFingerprint,
  computePlanFingerprint
} from './approvals.js';

const VALIDATION_LEVEL = 'strict';
const VALIDATION_ACTION = 'error';
const INDEX_METADATA = new Set([
  'v',
  'key',
  'name',
  'ns',
  'background',
  '2dsphereIndexVersion',
  'textIndexVersion',
  'buildUUID'
]);

const keyEntries = (key) => Object.entries(key);

const cleanOptions = (options) => Object.fromEntries(
  Object.entries(options ?? {}).filter(([, value]) => value !== undefined)
);

const actualIndexOptions = (index) => Object.fromEntries(
  Object.entries(index).filter(([key]) => !INDEX_METADATA.has(key))
);

const indexOptions = (index) => cleanOptions(
  index.options ?? actualIndexOptions(index)
);

const isTtlIndex = (index) => (
  indexOptions(index).expireAfterSeconds !== undefined
);

const compareIndexes = (expected, actual) => {
  const keyMatches = isDeepStrictEqual(
    keyEntries(expected.key),
    keyEntries(actual.key)
  );
  const optionsMatches = isDeepStrictEqual(
    cleanOptions(expected.options ?? {}),
    indexOptions(actual)
  );

  return {
    keyMatches,
    optionsMatches
  };
};

export const indexesEquivalent = (expected, actual) => {
  const comparison = compareIndexes(expected, actual);
  return comparison.keyMatches && comparison.optionsMatches;
};

export const describeIndex = (index) => {
  const options = indexOptions(index);
  const parts = [`key=${JSON.stringify(index.key)}`];
  if (options.unique) parts.push('unique');
  if (options.expireAfterSeconds !== undefined) {
    parts.push(`expireAfterSeconds=${options.expireAfterSeconds}`);
  }
  if (options.partialFilterExpression) {
    parts.push(
      `partialFilterExpression=${JSON.stringify(options.partialFilterExpression)}`
    );
  }
  return `${index.name} ${parts.join(' ')}`;
};

const readCollection = async (db, definition) => {
  const documents = await db
    .listCollections({ name: definition.name }, { nameOnly: false })
    .toArray();
  const info = documents[0];

  if (!info) {
    return {
      exists: false,
      info: null,
      indexes: []
    };
  }

  const indexes = await db.collection(definition.name).listIndexes().toArray();
  return {
    exists: true,
    info,
    indexes
  };
};

const readTtlImpact = async (db, definition, current) => {
  if (!current.exists) {
    return {
      documents: 0,
      basis: 'collection-missing'
    };
  }

  try {
    const collection = db.collection(definition.name);
    if (typeof collection.estimatedDocumentCount !== 'function') {
      return {
        documents: null,
        basis: 'unavailable'
      };
    }

    return {
      documents: await collection.estimatedDocumentCount(),
      basis: 'estimatedDocumentCount'
    };
  } catch {
    return {
      documents: null,
      basis: 'unavailable'
    };
  }
};

const inspectCollection = async (db, definition) => {
  const current = await readCollection(db, definition);
  const validatorMatches = current.exists
    ? isDeepStrictEqual(
      current.info.options?.validator,
      definition.validator
    )
    : false;
  const validationLevelMatches = current.exists
    ? current.info.options?.validationLevel === VALIDATION_LEVEL
    : false;
  const validationActionMatches = current.exists
    ? current.info.options?.validationAction === VALIDATION_ACTION
    : false;
  const needsValidatorUpdate = current.exists
    && (!validatorMatches || !validationLevelMatches || !validationActionMatches);

  const missingIndexes = [];
  const mismatchedIndexes = [];
  const satisfiedIndexes = [];
  const extraIndexes = [];
  const nameMismatches = [];
  const keyMismatches = [];
  const optionsMismatches = [];
  const differences = [];
  const matchedNames = new Set();

  const findUnmatched = (predicate) => current.indexes.find((existing) => (
    existing.name !== '_id_'
    && !matchedNames.has(existing.name)
    && predicate(existing)
  ));

  for (const desired of definition.indexes) {
    const byName = current.indexes.find((existing) => (
      existing.name === desired.name
    ));

    if (byName) {
      matchedNames.add(byName.name);
      const comparison = compareIndexes(desired, byName);

      if (comparison.keyMatches && comparison.optionsMatches) {
        satisfiedIndexes.push(byName);
      } else if (!comparison.keyMatches) {
        keyMismatches.push({ desired, existing: byName });
        mismatchedIndexes.push({ desired, existing: byName });
        differences.push({
          collection: definition.name,
          type: 'keyMismatch',
          desired,
          existing: byName,
          ttl: isTtlIndex(desired) || isTtlIndex(byName)
        });
      } else {
        optionsMismatches.push({ desired, existing: byName });
        mismatchedIndexes.push({ desired, existing: byName });
        differences.push({
          collection: definition.name,
          type: 'optionsMismatch',
          desired,
          existing: byName,
          ttl: isTtlIndex(desired) || isTtlIndex(byName)
        });
      }
      continue;
    }

    const equivalent = findUnmatched((existing) => {
      const comparison = compareIndexes(desired, existing);
      return comparison.keyMatches && comparison.optionsMatches;
    });

    if (equivalent) {
      matchedNames.add(equivalent.name);
      nameMismatches.push({ desired, existing: equivalent });
      differences.push({
        collection: definition.name,
        type: 'nameMismatch',
        desired,
        existing: equivalent,
        ttl: isTtlIndex(desired) || isTtlIndex(equivalent)
      });
      continue;
    }

    const sameKey = findUnmatched((existing) => (
      compareIndexes(desired, existing).keyMatches
    ));

    if (sameKey) {
      matchedNames.add(sameKey.name);
      optionsMismatches.push({ desired, existing: sameKey });
      mismatchedIndexes.push({ desired, existing: sameKey });
      differences.push({
        collection: definition.name,
        type: 'optionsMismatch',
        desired,
        existing: sameKey,
        ttl: isTtlIndex(desired) || isTtlIndex(sameKey)
      });
      continue;
    }

    missingIndexes.push(desired);
    differences.push({
      collection: definition.name,
      type: isTtlIndex(desired) ? 'ttlMissing' : 'missing',
      index: desired,
      ttl: isTtlIndex(desired)
    });
  }

  for (const existing of current.indexes) {
    if (existing.name === '_id_' || matchedNames.has(existing.name)) continue;

    extraIndexes.push(existing);
    differences.push({
      collection: definition.name,
      type: isTtlIndex(existing) ? 'ttlExtra' : 'extra',
      index: existing,
      ttl: isTtlIndex(existing)
    });
  }

  const ttlDifferences = differences.filter((difference) => (
    difference.type === 'ttlMissing' || difference.type === 'ttlExtra'
  ));

  if (ttlDifferences.length > 0) {
    const impact = await readTtlImpact(db, definition, current);
    for (const difference of ttlDifferences) {
      difference.impact = impact;
    }
  }

  const actions = [];
  if (!current.exists) {
    actions.push({
      type: 'createCollection',
      collection: definition.name
    });
  } else if (needsValidatorUpdate) {
    actions.push({
      type: 'collMod',
      collection: definition.name
    });
  }

  for (const index of missingIndexes) {
    actions.push({
      type: 'createIndex',
      collection: definition.name,
      index,
      requiresTtlApproval: isTtlIndex(index)
    });
  }

  return {
    name: definition.name,
    exists: current.exists,
    validatorMatches,
    validationLevelMatches,
    validationActionMatches,
    needsValidatorUpdate,
    missingIndexes,
    mismatchedIndexes,
    satisfiedIndexes,
    extraIndexes,
    nameMismatches,
    keyMismatches,
    optionsMismatches,
    differences,
    actions,
    errors: [],
    warnings: []
  };
};

const errorForDifference = (difference, message) => ({
  collection: difference.collection,
  type: difference.type === 'ttlMissing' || difference.type === 'ttlExtra'
    ? 'ttlApprovalRequired'
    : 'indexConflict',
  code: difference.type,
  message,
  index: difference.index ? describeIndex(difference.index) : undefined,
  desired: difference.desired ? describeIndex(difference.desired) : undefined,
  existing: difference.existing ? describeIndex(difference.existing) : undefined
});

const warningForDifference = (difference, message) => ({
  collection: difference.collection,
  type: difference.status === 'waived' ? 'waivedException' : 'ttlWarning',
  code: difference.type,
  message
});

const differenceMessage = (difference) => {
  if (difference.type === 'extra') {
    return `Existe un indice no declarado: ${describeIndex(difference.index)}. `
      + 'Sin un waiver valido el plan queda bloqueado. No se elimina automaticamente.';
  }

  if (difference.type === 'nameMismatch') {
    return `El indice ${difference.desired.name} coincide en clave y opciones con `
      + `${difference.existing.name}, pero el nombre no coincide. `
      + 'Sin un waiver valido el plan queda bloqueado.';
  }

  if (difference.type === 'keyMismatch') {
    return `El indice ${difference.desired.name} ya existe con otra clave. `
      + `Esperado: ${describeIndex(difference.desired)}. `
      + `Actual: ${describeIndex(difference.existing)}.`;
  }

  if (difference.type === 'optionsMismatch') {
    return `El indice ${difference.desired.name} ya existe con otras opciones. `
      + `Esperado: ${describeIndex(difference.desired)}. `
      + `Actual: ${describeIndex(difference.existing)}.`;
  }

  if (difference.type === 'ttlMissing') {
    return `El indice TTL ${describeIndex(difference.index)} no existe. `
      + 'En production se requiere una aprobacion TTL especifica.';
  }

  if (difference.type === 'ttlExtra') {
    return `Existe un indice TTL no declarado: ${describeIndex(difference.index)}. `
      + 'En production se requiere una aprobacion TTL especifica; '
      + 'no se acepta un waiver generico.';
  }

  return `Diferencia de indice no cubierta: ${difference.type}.`;
};

const ttlApprovalKey = (item) => [
  item.collection,
  item.type,
  item.index,
  item.expireAfterSeconds
].join('|');

const waiverKey = (item) => [
  item.collection,
  item.type,
  item.index,
  item.actualIndex ?? ''
].join('|');

const ttlExpireAfterSeconds = (index) => {
  const options = indexOptions(index);
  return options.expireAfterSeconds;
};

const findTtlApproval = (items, difference) => items.find((item) => (
  item.collection === difference.collection
  && item.type === difference.type
  && item.index === difference.index.name
  && item.expireAfterSeconds === ttlExpireAfterSeconds(difference.index)
));

const findWaiver = (items, difference) => {
  if (difference.type === 'extra') {
    return items.find((item) => (
      item.collection === difference.collection
      && item.type === 'extraIndex'
      && item.index === difference.index.name
    ));
  }

  return items.find((item) => (
    item.collection === difference.collection
    && item.type === 'nameMismatch'
    && item.index === difference.desired.name
    && item.actualIndex === difference.existing.name
  ));
};

const assertPlanDocumentFingerprint = (document, plan, label) => {
  if (!document) return;
  if (document.planFingerprint !== plan.planFingerprint) {
    throw new ApprovalError(
      `El ${label} no corresponde al plan actual. `
      + 'Recalcule el plan y genere una nueva aprobacion.'
    );
  }
};

export const resolvePlanGuards = (plan, {
  environment = plan.environment ?? 'development',
  ttlApprovalDocument,
  waiverDocument,
  now = new Date()
} = {}) => {
  assertPlanDocumentFingerprint(ttlApprovalDocument, plan, 'ttl-approval-file');
  assertPlanDocumentFingerprint(waiverDocument, plan, 'waiver-file');

  const ttlItems = ttlApprovalDocument?.ttlApprovals ?? [];
  const waiverItems = waiverDocument?.waivers ?? [];
  const usedTtl = new Set();
  const usedWaivers = new Set();
  const differences = plan.differences.map((difference) => ({
    ...difference,
    status: 'detected',
    blocking: false
  }));
  const errors = [];
  const warnings = [];

  for (const difference of differences) {
    if (difference.type === 'missing') {
      difference.status = 'action';
      continue;
    }

    if (difference.type === 'extra' || difference.type === 'nameMismatch') {
      if (difference.ttl) {
        difference.status = 'blocked';
        difference.blocking = true;
        errors.push(errorForDifference(
          difference,
          `El indice TTL ${describeIndex(difference.desired ?? difference.index)} `
          + 'tiene un nombre distinto o es adicional. No se acepta waiver para TTL; '
          + 'corrija el nombre manualmente o use una aprobacion TTL especifica '
          + 'cuando aplique a un TTL faltante o adicional.'
        ));
        continue;
      }

      const waiver = findWaiver(waiverItems, difference);
      if (waiver) {
        usedWaivers.add(waiverKey(waiver));
        difference.status = 'waived';
        difference.waiver = waiver;
        warnings.push(warningForDifference(
          difference,
          `Excepcion aprobada por waiver: ${describeIndex(
            difference.desired ?? difference.index
          )}.`
        ));
      } else {
        difference.status = 'blocked';
        difference.blocking = true;
        errors.push(errorForDifference(
          difference,
          differenceMessage(difference)
        ));
      }
      continue;
    }

    if (
      difference.type === 'keyMismatch'
      || difference.type === 'optionsMismatch'
    ) {
      difference.status = 'blocked';
      difference.blocking = true;
      errors.push(errorForDifference(difference, differenceMessage(difference)));
      continue;
    }

    if (
      difference.type === 'ttlMissing'
      || difference.type === 'ttlExtra'
    ) {
      if (environment !== 'production') {
        difference.status = 'warning';
        warnings.push(warningForDifference(
          difference,
          `${differenceMessage(difference)} `
          + `Entorno ${environment}: se registra como advertencia.`
        ));
        continue;
      }

      const approval = findTtlApproval(ttlItems, difference);
      if (!approval) {
        difference.status = 'blocked';
        difference.blocking = true;
        errors.push(errorForDifference(difference, differenceMessage(difference)));
        continue;
      }

      const impact = difference.impact?.documents;
      if (Number.isInteger(impact) && approval.maxDeletion < impact) {
        throw new ApprovalError(
          `La aprobacion TTL de ${difference.collection}.${difference.index.name} `
          + `autoriza ${approval.maxDeletion} documentos, pero el impacto estimado `
          + `es ${impact}.`
        );
      }

      usedTtl.add(ttlApprovalKey(approval));
      difference.status = 'approved';
      difference.ttlApproval = approval;
      warnings.push(warningForDifference(
        difference,
        `TTL aprobado: ${describeIndex(difference.index)}.`
      ));
    }
  }

  for (const item of ttlItems) {
    if (!usedTtl.has(ttlApprovalKey(item))) {
      throw new ApprovalError(
        `La aprobacion TTL ${item.collection}.${item.index} no corresponde a `
        + 'ninguna diferencia del plan actual.'
      );
    }
  }

  for (const item of waiverItems) {
    if (!usedWaivers.has(waiverKey(item))) {
      throw new ApprovalError(
        `El waiver ${item.collection}.${item.index} no corresponde a ninguna `
        + 'diferencia del plan actual.'
      );
    }
  }

  const byCollection = new Map();
  for (const difference of differences) {
    const collection = byCollection.get(difference.collection) ?? [];
    collection.push(difference);
    byCollection.set(difference.collection, collection);
  }

  const collections = plan.collections.map((collection) => {
    const collectionDifferences = byCollection.get(collection.name) ?? [];
    const collectionErrors = collectionDifferences
      .filter((difference) => difference.blocking)
      .map((difference) => errorForDifference(
        difference,
        differenceMessage(difference)
      ));
    const collectionWarnings = warnings.filter(
      (warning) => warning.collection === collection.name
    );

    return {
      ...collection,
      differences: collectionDifferences,
      errors: collectionErrors,
      warnings: collectionWarnings
    };
  });

  const actions = plan.actions.map((action) => {
    if (action.type !== 'createIndex' || !action.requiresTtlApproval) {
      return action;
    }

    const difference = differences.find((item) => (
      item.collection === action.collection
      && item.type === 'ttlMissing'
      && item.index.name === action.index.name
    ));

    return {
      ...action,
      ttlApproval: difference?.ttlApproval ?? null
    };
  });

  const hasChanges = actions.length > 0;
  const drift = hasChanges || differences.length > 0 || errors.length > 0;
  const exactMatch = !hasChanges && differences.length === 0 && errors.length === 0;
  const ok = errors.length === 0 && actions.length === 0;

  const summary = {
    collections: collections.length,
    createCollections: collections.filter((collection) => !collection.exists).length,
    updateValidators: collections.filter(
      (collection) => collection.needsValidatorUpdate
    ).length,
    createIndexes: actions.filter((action) => action.type === 'createIndex').length,
    indexConflicts: errors.length,
    extraIndexes: differences.filter((difference) => (
      difference.type === 'extra' || difference.type === 'ttlExtra'
    )).length,
    nameMismatches: differences.filter(
      (difference) => difference.type === 'nameMismatch'
    ).length,
    keyMismatches: differences.filter(
      (difference) => difference.type === 'keyMismatch'
    ).length,
    optionsMismatches: differences.filter(
      (difference) => difference.type === 'optionsMismatch'
    ).length,
    ttlMissing: differences.filter(
      (difference) => difference.type === 'ttlMissing'
    ).length,
    ttlExtra: differences.filter(
      (difference) => difference.type === 'ttlExtra'
    ).length,
    waived: differences.filter(
      (difference) => difference.status === 'waived'
    ).length,
    ttlApproved: differences.filter(
      (difference) => difference.status === 'approved'
    ).length,
    warnings: warnings.length
  };

  return {
    ...plan,
    environment,
    collections,
    actions,
    differences,
    errors,
    warnings,
    drift,
    hasChanges,
    exactMatch,
    ok,
    summary,
    approvals: {
      ttl: ttlApprovalDocument
        ? {
          approvedBy: ttlApprovalDocument.approvedBy,
          expiresAt: ttlApprovalDocument.expiresAt,
          planFingerprint: ttlApprovalDocument.planFingerprint
        }
        : null,
      waiver: waiverDocument
        ? {
          approvedBy: waiverDocument.approvedBy,
          expiresAt: waiverDocument.expiresAt,
          planFingerprint: waiverDocument.planFingerprint
        }
        : null
    }
  };
};

export const buildPlan = async (db, collections = COLLECTIONS, options = {}) => {
  const environment = options.environment
    ?? process.env.NODE_ENV
    ?? 'development';
  const clusterIdentity = options.clusterIdentity ?? null;
  const catalog = catalogFingerprint(collections);
  const inspected = [];

  for (const definition of collections) {
    inspected.push(await inspectCollection(db, definition));
  }

  const actions = inspected.flatMap((collection) => collection.actions);
  const differences = inspected.flatMap((collection) => collection.differences);
  const hasChanges = actions.length > 0;
  const drift = hasChanges || differences.length > 0;

  const plan = {
    database: db.databaseName,
    environment,
    clusterIdentity,
    catalogFingerprint: catalog,
    generatedAt: new Date().toISOString(),
    collections: inspected,
    actions,
    differences,
    errors: [],
    warnings: [],
    drift,
    hasChanges,
    exactMatch: false,
    ok: false,
    summary: {
      collections: inspected.length,
      createCollections: inspected.filter((collection) => !collection.exists).length,
      updateValidators: inspected.filter(
        (collection) => collection.needsValidatorUpdate
      ).length,
      createIndexes: actions.filter((action) => action.type === 'createIndex').length,
      indexConflicts: 0,
      extraIndexes: differences.filter((difference) => (
        difference.type === 'extra' || difference.type === 'ttlExtra'
      )).length,
      nameMismatches: differences.filter(
        (difference) => difference.type === 'nameMismatch'
      ).length,
      keyMismatches: differences.filter(
        (difference) => difference.type === 'keyMismatch'
      ).length,
      optionsMismatches: differences.filter(
        (difference) => difference.type === 'optionsMismatch'
      ).length,
      ttlMissing: differences.filter(
        (difference) => difference.type === 'ttlMissing'
      ).length,
      ttlExtra: differences.filter(
        (difference) => difference.type === 'ttlExtra'
      ).length,
      waived: 0,
      ttlApproved: 0,
      warnings: 0
    }
  };

  plan.planFingerprint = computePlanFingerprint(plan);

  return resolvePlanGuards(plan, {
    environment,
    ttlApprovalDocument: options.ttlApprovalDocument,
    waiverDocument: options.waiverDocument,
    now: options.now
  });
};
