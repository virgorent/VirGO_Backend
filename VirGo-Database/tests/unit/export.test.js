import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import {
  buildApiSchemas,
  checkApiSchemas,
  serializeApiSchema,
  validateApiSchemas,
  writeApiSchemas
} from '../../src/export/schema.js';

test('todos los strings exportados tienen limite', () => {
  assert.deepEqual(validateApiSchemas(), []);
});

test('exporta exactamente las 22 colecciones con contrato de la API', () => {
  const schemas = buildApiSchemas();
  assert.equal(schemas.length, 22);

  for (const schema of schemas) {
    assert.equal(schema.validationLevel, 'strict', schema.collection);
    assert.equal(schema.validationAction, 'error', schema.collection);
    assert.ok(schema.validator, schema.collection);
    assert.ok(Array.isArray(schema.indexes), schema.collection);
  }
});

test('serializa JSON determinista con LF y salto final', () => {
  const [schema] = buildApiSchemas();
  const text = serializeApiSchema(schema);

  assert.equal(text.includes('\r\n'), false);
  assert.equal(text.endsWith('\n'), true);
  assert.deepEqual(JSON.parse(text), schema);
  assert.equal(serializeApiSchema(schema), text);
});

test('writeApiSchemas y checkApiSchemas detectan cambios', () => {
  const dir = fs.mkdtempSync(path.join(os.tmpdir(), 'virgo-schema-'));

  try {
    const missing = checkApiSchemas(dir);
    assert.equal(missing.length, 22);
    assert.ok(missing.every((difference) => difference.reason === 'missing'));

    writeApiSchemas(dir);
    assert.deepEqual(checkApiSchemas(dir), []);

    const usersFile = path.join(dir, 'users.json');
    fs.writeFileSync(usersFile, '{}\n', 'utf8');
    assert.deepEqual(checkApiSchemas(dir), [{
      collection: 'users',
      file: usersFile,
      reason: 'different'
    }]);

    fs.writeFileSync(path.join(dir, 'extra.json'), '{}\n', 'utf8');
    const differences = checkApiSchemas(dir);
    assert.equal(differences.some((difference) => (
      difference.collection === 'extra' && difference.reason === 'unexpected'
    )), true);
  } finally {
    fs.rmSync(dir, { recursive: true, force: true });
  }
});

test('validateApiSchemas detecta strings sin maxLength', () => {
  const schemas = [{
    collection: 'test',
    validator: {
      $jsonSchema: {
        bsonType: 'object',
        properties: {
          name: {
            bsonType: 'string'
          }
        }
      }
    }
  }];

  assert.deepEqual(validateApiSchemas(schemas), ['test.name: sin maxLength']);
});
