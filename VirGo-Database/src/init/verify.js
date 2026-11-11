import { buildPlan } from './plan.js';

export const verifyInitialization = async (db, collections) => {
  const plan = await buildPlan(db, collections);

  return {
    ok: !plan.drift,
    plan,
    missingCollections: plan.collections
      .filter((collection) => !collection.exists)
      .map((collection) => collection.name),
    validatorDrift: plan.collections
      .filter((collection) => collection.needsValidatorUpdate)
      .map((collection) => collection.name),
    missingIndexes: plan.collections
      .filter((collection) => collection.missingIndexes.length > 0)
      .map((collection) => ({
        collection: collection.name,
        indexes: collection.missingIndexes.map((index) => index.name)
      })),
    mismatchedIndexes: plan.collections
      .filter((collection) => collection.mismatchedIndexes.length > 0)
      .map((collection) => ({
        collection: collection.name,
        indexes: collection.mismatchedIndexes.map(({ desired }) => desired.name)
      })),
    extraIndexes: plan.collections
      .filter((collection) => collection.extraIndexes.length > 0)
      .map((collection) => ({
        collection: collection.name,
        indexes: collection.extraIndexes.map((index) => index.name)
      }))
  };
};
