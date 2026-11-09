import fs from 'node:fs';
import path from 'node:path';
import { COLLECTIONS } from '../catalog/index.js';

const rootSchemas = (validator) => {
  if (validator.$jsonSchema) return [validator.$jsonSchema];
  return (validator.$and ?? [])
    .filter((item) => item.$jsonSchema)
    .map((item) => item.$jsonSchema);
};

const unboundedStrings = (node, prefix = '', found = []) => {
  if (!node || typeof node !== 'object') return found;

  if (
    node.bsonType === 'string'
    && node.enum === undefined
    && node.pattern === undefined
    && node.maxLength === undefined
  ) {
    found.push(prefix);
  }

  for (const [key, value] of Object.entries(node.properties ?? {})) {
    unboundedStrings(value, prefix ? `${prefix}.${key}` : key, found);
  }

  if (node.items) {
    unboundedStrings(node.items, `${prefix}[]`, found);
  }

  return found;
};

export const API_SCHEMA_DEFAULTS = {
  validationLevel: 'strict',
  validationAction: 'error'
};

export const toApiSchema = (collection) => ({
  collection: collection.name,
  validationLevel: API_SCHEMA_DEFAULTS.validationLevel,
  validationAction: API_SCHEMA_DEFAULTS.validationAction,
  validator: collection.validator,
  indexes: collection.indexes
});

export const buildApiSchemas = () => COLLECTIONS.map(toApiSchema);

export const serializeApiSchema = (schema) => `${JSON.stringify(schema, null, 2)}\n`;

export const validateApiSchemas = (schemas = buildApiSchemas()) => {
  const errors = [];

  for (const schema of schemas) {
    for (const root of rootSchemas(schema.validator)) {
      for (const field of unboundedStrings(root)) {
        errors.push(`${schema.collection}.${field}: sin maxLength`);
      }
    }
  }

  return errors;
};

export const schemaPath = (outDir, schema) => path.join(outDir, `${schema.collection}.json`);

export const writeApiSchemas = (outDir, schemas = buildApiSchemas()) => {
  fs.mkdirSync(outDir, { recursive: true });

  return schemas.map((schema) => {
    const file = schemaPath(outDir, schema);
    fs.writeFileSync(file, serializeApiSchema(schema), 'utf8');
    return file;
  });
};

const normalize = (text) => text.replace(/\r\n/g, '\n');

export const checkApiSchemas = (outDir, schemas = buildApiSchemas()) => {
  const differences = [];
  const expected = new Set(schemas.map((schema) => `${schema.collection}.json`));

  for (const schema of schemas) {
    const file = schemaPath(outDir, schema);
    const wanted = serializeApiSchema(schema);
    let current;

    try {
      current = fs.readFileSync(file, 'utf8');
    } catch {
      differences.push({ collection: schema.collection, file, reason: 'missing' });
      continue;
    }

    if (normalize(current) !== wanted) {
      differences.push({ collection: schema.collection, file, reason: 'different' });
    }
  }

  if (fs.existsSync(outDir)) {
    for (const file of fs.readdirSync(outDir).filter((name) => name.endsWith('.json'))) {
      if (!expected.has(file)) {
        differences.push({
          collection: file.slice(0, -'.json'.length),
          file: path.join(outDir, file),
          reason: 'unexpected'
        });
      }
    }
  }

  return differences;
};
