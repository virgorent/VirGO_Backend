import test from 'node:test';
import assert from 'node:assert/strict';
import { COLLECTION_NAMES } from '../../src/catalog/index.js';
import {
  assertDemoEnvironment,
  buildDemoDocuments,
  countDemoDocuments,
  demoId
} from '../../src/seed/demo.js';

test('el DEMO cubre las 22 colecciones con documentos', () => {
  const documents = buildDemoDocuments();

  assert.deepEqual(
    [...documents.keys()].sort(),
    [...COLLECTION_NAMES].sort()
  );
  for (const [collection, docs] of documents) {
    assert.ok(docs.length > 0, collection);
  }
});

test('los identificadores DEMO son deterministas y unicos', () => {
  const first = buildDemoDocuments();
  const second = buildDemoDocuments();
  const ids = [];
  const compare = [];

  for (const [collection, docs] of first) {
    assert.deepEqual(
      docs.map((doc) => doc._id),
      second.get(collection).map((doc) => doc._id)
    );
    for (const doc of docs) {
      ids.push(`${collection}:${doc._id}`);
      compare.push(doc._id);
    }
  }

  assert.equal(new Set(compare).size, compare.length);
  assert.equal(new Set(ids).size, ids.length);
  assert.equal(demoId('same'), demoId('same'));
  assert.notEqual(demoId('same'), demoId('other'));
});

test('el conteo DEMO coincide con los documentos declarados', () => {
  const documents = buildDemoDocuments();
  const expected = [...documents.values()]
    .reduce((total, docs) => total + docs.length, 0);

  assert.equal(countDemoDocuments(documents), expected);
});

test('el DEMO nunca se permite en production ni en la base virgo', () => {
  assert.throws(
    () => assertDemoEnvironment({
      nodeEnv: 'production',
      dbName: 'virgo'
    }),
    /solo se permite en development o test/
  );
  assert.throws(
    () => assertDemoEnvironment({
      nodeEnv: 'development',
      dbName: 'virgo'
    }),
    /solo se permite en development o test/
  );
  assert.doesNotThrow(() => assertDemoEnvironment({
    nodeEnv: 'development',
    dbName: 'virgo_dev'
  }));
});
