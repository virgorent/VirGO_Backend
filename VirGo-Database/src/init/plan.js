import { isDeepStrictEqual } from 'node:util';
import { COLLECTIONS } from '../catalog/index.js';

const VALIDATION_LEVEL = 'strict';
const VALIDATION_ACTION = 'error';

const keyEntries = (key) => Object.entries(key);

export const indexesEquivalent = (expected, actual) => {
  if (!isDeepStrictEqual(keyEntries(expected.key), keyEntries(actual.key))) {
    return false;
  }

  if (Boolean(expected.options.unique) !== Boolean(actual.unique)) {
    return false;
  }

  if (expected.options.expireAfterSeconds !== actual.expireAfterSeconds) {
    return false;
  }

  return isDeepStrictEqual(
    expected.options.partialFilterExpression,
    actual.partialFilterExpression
  );
};

export const describeIndex = (index) => {
  const parts = [`key=${JSON.stringify(index.key)}`];
  if (index.options.unique) parts.push('unique');
  if (index.options.expireAfterSeconds !== undefined) {
    parts.push(`expireAfterSeconds=${index.options.expireAfterSeconds}`);
  }
  if (index.options.partialFilterExpression) {
    parts.push(
      `partialFilterExpression=${JSON.stringify(index.options.partialFilterExpression)}`
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
  const matchedNames = new Set();

  for (const desired of definition.indexes) {
    const byName = current.indexes.find((existing) => existing.name === desired.name);

    if (byName) {
      matchedNames.add(byName.name);
      if (indexesEquivalent(desired, byName)) {
        satisfiedIndexes.push(byName);
      } else {
        mismatchedIndexes.push({
          desired,
          existing: byName
        });
      }
      continue;
    }

    const equivalent = current.indexes.find(
      (existing) => existing.name !== '_id_' && indexesEquivalent(desired, existing)
    );

    if (equivalent) {
      matchedNames.add(equivalent.name);
      satisfiedIndexes.push(equivalent);
      continue;
    }

    missingIndexes.push(desired);
  }

  const extraIndexes = current.indexes.filter(
    (existing) => existing.name !== '_id_' && !matchedNames.has(existing.name)
  );

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
      index
    });
  }

  const errors = mismatchedIndexes.map(({ desired, existing }) => ({
    collection: definition.name,
    type: 'indexConflict',
    message:
      `El indice ${desired.name} ya existe con otra definicion. `
      + `Esperado: ${describeIndex(desired)}. Actual: ${describeIndex({
        name: existing.name,
        key: existing.key,
        options: existing
      })}.`
  }));

  const warnings = extraIndexes.map((existing) => ({
    collection: definition.name,
    type: 'extraIndex',
    message:
      `Existe un indice no declarado: ${describeIndex({
        name: existing.name,
        key: existing.key,
        options: existing
      })}.`
  }));

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
    actions,
    errors,
    warnings
  };
};

export const buildPlan = async (db, collections = COLLECTIONS) => {
  const inspected = [];

  for (const definition of collections) {
    inspected.push(await inspectCollection(db, definition));
  }

  const actions = inspected.flatMap((collection) => collection.actions);
  const errors = inspected.flatMap((collection) => collection.errors);
  const warnings = inspected.flatMap((collection) => collection.warnings);
  const drift = inspected.some((collection) => (
    !collection.exists
    || collection.needsValidatorUpdate
    || collection.missingIndexes.length > 0
    || collection.mismatchedIndexes.length > 0
  ));

  return {
    database: db.databaseName,
    generatedAt: new Date().toISOString(),
    collections: inspected,
    actions,
    errors,
    warnings,
    drift,
    hasChanges: actions.length > 0,
    summary: {
      collections: inspected.length,
      createCollections: inspected.filter((collection) => !collection.exists).length,
      updateValidators: inspected.filter(
        (collection) => collection.needsValidatorUpdate
      ).length,
      createIndexes: actions.filter((action) => action.type === 'createIndex').length,
      indexConflicts: errors.length,
      extraIndexes: warnings.length
    }
  };
};
