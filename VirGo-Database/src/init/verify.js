import { buildPlan } from './plan.js';

export const verifyInitialization = async (db, collections, options = {}) => {
  const plan = await buildPlan(db, collections, options);

  return {
    ok: plan.errors.length === 0 && plan.actions.length === 0,
    exactMatch: plan.exactMatch,
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
    nameMismatches: plan.collections
      .filter((collection) => collection.nameMismatches.length > 0)
      .map((collection) => ({
        collection: collection.name,
        indexes: collection.nameMismatches.map(({ desired, existing }) => ({
          desired: desired.name,
          actual: existing.name
        }))
      })),
    extraIndexes: plan.collections
      .filter((collection) => collection.extraIndexes.length > 0)
      .map((collection) => ({
        collection: collection.name,
        indexes: collection.extraIndexes.map((index) => index.name)
      })),
    blockingIssues: plan.errors,
    warnings: plan.warnings,
    waivedIssues: plan.differences.filter(
      (difference) => difference.status === 'waived'
    ),
    ttlApprovals: plan.differences.filter(
      (difference) => difference.status === 'approved'
    )
  };
};
