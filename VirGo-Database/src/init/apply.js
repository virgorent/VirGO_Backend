import { COLLECTIONS_BY_NAME } from '../catalog/index.js';

const VALIDATION_LEVEL = 'strict';
const VALIDATION_ACTION = 'error';

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
  if (plan.errors.length > 0) {
    throw new Error(
      'El plan contiene conflictos de indices. No se aplicaron cambios.'
    );
  }

  const results = [];

  for (const action of plan.actions) {
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

    if (action.type === 'createIndex') {
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
      continue;
    }

    throw new Error(`Accion no soportada: ${action.type}`);
  }

  return results;
};
