import { describeIndex } from './plan.js';

const formatAction = (action) => {
  if (action.type === 'createCollection') {
    return `+ createCollection ${action.collection} (validator, validationLevel=strict, validationAction=error)`;
  }
  if (action.type === 'collMod') {
    return `+ collMod ${action.collection} (validator, validationLevel=strict, validationAction=error)`;
  }
  if (action.type === 'createIndex') {
    return `+ createIndex ${action.collection} ${describeIndex(action.index)}`;
  }
  return `+ ${action.type} ${action.collection}`;
};

export const formatPlan = (plan) => {
  const lines = [
    `Base de datos: ${plan.database}`,
    `Colecciones declaradas: ${plan.summary.collections}`,
    `Plan: ${plan.summary.createCollections} colecciones por crear, `
      + `${plan.summary.updateValidators} validadores por actualizar, `
      + `${plan.summary.createIndexes} indices por crear.`
  ];

  if (!plan.hasChanges) {
    lines.push('Estado: sin cambios pendientes.');
  }

  for (const collection of plan.collections) {
    if (collection.actions.length === 0) continue;
    lines.push('', `[${collection.name}]`);
    for (const action of collection.actions) {
      lines.push(formatAction(action));
    }
  }

  if (plan.warnings.length > 0) {
    lines.push('', 'Advertencias:');
    for (const warning of plan.warnings) {
      lines.push(`~ ${warning.collection}: ${warning.message}`);
    }
  }

  if (plan.errors.length > 0) {
    lines.push('', 'Conflictos bloqueantes:');
    for (const error of plan.errors) {
      lines.push(`! ${error.collection}: ${error.message}`);
    }
  }

  return lines.join('\n');
};

export const formatVerification = (result) => {
  const lines = [
    `Base de datos: ${result.plan.database}`,
    `Verificacion: ${result.ok ? 'OK' : 'DRIFT'}`
  ];

  if (result.missingCollections.length > 0) {
    lines.push(`Colecciones faltantes: ${result.missingCollections.join(', ')}`);
  }

  if (result.validatorDrift.length > 0) {
    lines.push(`Validadores desactualizados: ${result.validatorDrift.join(', ')}`);
  }

  for (const item of result.missingIndexes) {
    lines.push(`Indices faltantes en ${item.collection}: ${item.indexes.join(', ')}`);
  }

  for (const item of result.mismatchedIndexes) {
    lines.push(`Indices conflictivos en ${item.collection}: ${item.indexes.join(', ')}`);
  }

  for (const item of result.extraIndexes) {
    lines.push(`Indices adicionales en ${item.collection}: ${item.indexes.join(', ')}`);
  }

  return lines.join('\n');
};
