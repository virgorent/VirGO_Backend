import { isDeepStrictEqual } from 'node:util';
import { COLLECTIONS_BY_NAME } from '../catalog/index.js';

const VALIDATION_LEVEL = 'strict';
const VALIDATION_ACTION = 'error';
const ALLOWED_ACTIONS = new Set(['createCollection', 'collMod', 'createIndex']);

export class ApplyError extends Error {
  constructor(message, { cause, results, failedAction } = {}) {
    super(message, { cause });
    this.name = 'ApplyError';
    this.results = results ?? [];
    this.failedAction = failedAction ?? null;
  }
}

const assertSafePlan = (plan) => {
  if (!plan || typeof plan !== 'object' || !Array.isArray(plan.actions)) {
    throw new Error('El plan no tiene una lista de acciones valida.');
  }

  if (!Array.isArray(plan.errors)) {
    throw new Error('El plan no tiene una lista de errores valida.');
  }

  if (plan.errors.length > 0) {
    throw new Error(
      'El plan contiene conflictos de indices. No se aplicaron cambios.'
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
      + 'Las operaciones aplicadas son idempotentes; corrija la causa y reejecute plan/apply.',
      {
        cause: error,
        results,
        failedAction: currentAction
      }
    );
  }

  return results;
};
