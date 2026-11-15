import { isDeepStrictEqual } from 'node:util';
import { COLLECTIONS_BY_NAME } from '../catalog/index.js';
import {
  catalogFingerprint,
  computePlanFingerprint
} from './approvals.js';

const VALIDATION_LEVEL = 'strict';
const VALIDATION_ACTION = 'error';
const ALLOWED_ACTIONS = new Set(['createCollection', 'collMod', 'createIndex']);
const SHA256_PATTERN = /^sha256:[0-9a-f]{64}$/;
const INDEX_METADATA = new Set(['v', 'key', 'name', 'ns', 'background']);

const indexOptions = (index) => (
  index.options ?? Object.fromEntries(
    Object.entries(index).filter(([key]) => !INDEX_METADATA.has(key))
  )
);

const isTtlIndex = (index) => (
  indexOptions(index).expireAfterSeconds !== undefined
);

export class ApplyError extends Error {
  constructor(message, {
    cause,
    results,
    failedAction,
    planFingerprint
  } = {}) {
    super(message, { cause });
    this.name = 'ApplyError';
    this.results = results ?? [];
    this.failedAction = failedAction ?? null;
    this.planFingerprint = planFingerprint ?? null;
    this.planInvalidated = true;
    this.requiresReinspection = true;
  }
}

const assertPlanFingerprints = (plan) => {
  if (typeof plan.catalogFingerprint !== 'string'
    || !SHA256_PATTERN.test(plan.catalogFingerprint)) {
    throw new Error('El plan no tiene un catalogFingerprint valido.');
  }

  if (typeof plan.planFingerprint !== 'string'
    || !SHA256_PATTERN.test(plan.planFingerprint)) {
    throw new Error('El plan no tiene un planFingerprint valido.');
  }

  if (plan.catalogFingerprint !== catalogFingerprint()) {
    throw new Error(
      'El catalogFingerprint del plan no coincide con el catalogo actual.'
    );
  }

  if (plan.planFingerprint !== computePlanFingerprint(plan)) {
    throw new Error(
      'El planFingerprint no coincide con el contenido del plan. '
      + 'Recalcule el plan antes de aplicar.'
    );
  }
};

const assertTtlApproval = (plan, action, declared) => {
  if (!action.requiresTtlApproval && !isTtlIndex(declared)) return;
  if (plan.environment !== 'production') return;

  if (!action.ttlApproval) {
    throw new Error(
      `El indice TTL ${action.index.name} de ${action.collection} requiere `
      + 'una aprobacion TTL valida en production.'
    );
  }

  const expectedExpire = indexOptions(declared).expireAfterSeconds;
  if (action.ttlApproval.expireAfterSeconds !== expectedExpire) {
    throw new Error(
      `La aprobacion TTL de ${action.index.name} no coincide con `
      + 'expireAfterSeconds del catalogo.'
    );
  }

  if (!Number.isInteger(action.ttlApproval.maxDeletion)
    || action.ttlApproval.maxDeletion < 0) {
    throw new Error(
      `La aprobacion TTL de ${action.index.name} no tiene un maxDeletion valido.`
    );
  }
};

const assertSafePlan = (plan) => {
  if (!plan || typeof plan !== 'object' || !Array.isArray(plan.actions)) {
    throw new Error('El plan no tiene una lista de acciones valida.');
  }

  if (!Array.isArray(plan.errors)) {
    throw new Error('El plan no tiene una lista de errores valida.');
  }

  assertPlanFingerprints(plan);

  if (plan.errors.length > 0) {
    throw new Error(
      'El plan contiene conflictos o aprobaciones pendientes. '
      + 'No se aplicaron cambios.'
    );
  }

  for (const action of plan.actions) {
    if (!action || typeof action !== 'object') {
      throw new Error('El plan contiene una accion invalida.');
    }

    if (!ALLOWED_ACTIONS.has(action.type)) {
      throw new Error(`Accion no soportada o destructiva: ${action.type}`);
    }

    const definition = COLLECTIONS_BY_NAME.get(action.collection);
    if (!definition) {
      throw new Error(
        `La coleccion ${action.collection} no pertenece al catalogo.`
      );
    }

    if (action.type !== 'createIndex') continue;

    const declared = definition.indexes.find(
      (index) => index.name === action.index?.name
    );
    if (!declared) {
      throw new Error(
        `El indice ${action.index?.name} no esta declarado para ${action.collection}.`
      );
    }

    const normalize = (index) => ({
      name: index.name,
      key: index.key,
      options: index.options ?? {}
    });

    if (!isDeepStrictEqual(normalize(declared), normalize(action.index))) {
      throw new Error(
        `El indice ${action.index.name} no coincide con el catalogo de ${action.collection}.`
      );
    }

    assertTtlApproval(plan, action, declared);
  }
};

const createCollection = async (db, collection) => {
  const definition = COLLECTIONS_BY_NAME.get(collection);
  try {
    await db.createCollection(collection, {
      validator: definition.validator,
      validationLevel: VALIDATION_LEVEL,
      validationAction: VALIDATION_ACTION
    });
    return 'created';
  } catch (error) {
    if (error?.codeName === 'NamespaceExists') {
      return 'exists';
    }
    throw error;
  }
};

export const applyPlan = async (db, plan) => {
  assertSafePlan(plan);

  const results = [];
  let currentAction = null;

  try {
    for (const action of plan.actions) {
      currentAction = action;

      if (action.type === 'createCollection') {
        const definition = COLLECTIONS_BY_NAME.get(action.collection);
        const outcome = await createCollection(db, action.collection);
        if (outcome === 'exists') {
          await db.command({
            collMod: action.collection,
            validator: definition.validator,
            validationLevel: VALIDATION_LEVEL,
            validationAction: VALIDATION_ACTION
          });
          results.push({
            ...action,
            result: 'validator-updated-after-race'
          });
        } else {
          results.push({
            ...action,
            result: 'created'
          });
        }
        continue;
      }

      if (action.type === 'collMod') {
        const definition = COLLECTIONS_BY_NAME.get(action.collection);
        await db.command({
          collMod: action.collection,
          validator: definition.validator,
          validationLevel: VALIDATION_LEVEL,
          validationAction: VALIDATION_ACTION
        });
        results.push({
          ...action,
          result: 'updated'
        });
        continue;
      }

      await db.collection(action.collection).createIndex(
        action.index.key,
        {
          ...action.index.options,
          name: action.index.name
        }
      );
      results.push({
        ...action,
        result: 'created'
      });
    }
  } catch (error) {
    const message = error instanceof Error ? error.message : String(error);
    throw new ApplyError(
      `Fallo al aplicar ${currentAction?.type ?? 'accion'} en `
      + `${currentAction?.collection ?? 'desconocida'}: ${message}. `
      + `Operaciones aplicadas antes del fallo: ${results.length}. `
      + 'Las operaciones aplicadas son idempotentes, pero el plan anterior '
      + 'queda invalidado; ejecute plan de nuevo antes de reintentar.',
      {
        cause: error,
        results,
        failedAction: currentAction,
        planFingerprint: plan.planFingerprint
      }
    );
  }

  return results;
};
