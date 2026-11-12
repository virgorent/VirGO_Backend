import test from 'node:test';
import assert from 'node:assert/strict';
import { randomUUID } from 'node:crypto';
import { COLLECTION_NAMES } from '../../src/catalog/index.js';
import {
  applyPlan,
  buildPlan
} from '../../src/init/index.js';
import {
  buildDemoDocuments,
  countDemoDocuments,
  purgeDemo,
  seedDemo
} from '../../src/seed/demo.js';
import { createTestDb, dropTestDb } from '../helpers/testDb.js';

const connection = await createTestDb();

const totalDocuments = async (db) => {
  let total = 0;
  for (const collection of COLLECTION_NAMES) {
    total += await db.collection(collection).countDocuments();
  }
  return total;
};

test('seed DEMO contra MongoDB local', {
  skip: connection ? false : 'MongoDB local no disponible en MONGODB_TEST_URI'
}, async (t) => {
  const db = connection.db;
  const documents = buildDemoDocuments();
  const expected = countDemoDocuments(documents);

  t.after(async () => {
    await dropTestDb(connection);
  });

  await t.test('crea el esquema antes del seed', async () => {
    await applyPlan(db, await buildPlan(db));
    const verification = await buildPlan(db);
    assert.equal(verification.errors.length, 0);
  });

  await t.test('siembra DEMO de forma idempotente', async () => {
    const first = await seedDemo(db, documents);
    assert.equal(first.documents, expected);
    assert.equal(await totalDocuments(db), expected);

    const second = await seedDemo(db, documents);
    assert.equal(second.documents, expected);
    assert.equal(await totalDocuments(db), expected);
  });

  await t.test('la limpieza es selectiva y no toca otros documentos', async () => {
    const foreignId = randomUUID();
    await db.collection('contactMessages').insertOne({
      _id: foreignId,
      userId: randomUUID(),
      name: 'Documento Ajeno',
      email: 'ajeno@example.com',
      message: 'Este documento no pertenece al DEMO.',
      createdAt: new Date('2026-11-03T12:00:00.000Z')
    });

    const before = await totalDocuments(db);
    assert.equal(before, expected + 1);

    const result = await purgeDemo(db, documents);
    assert.equal(result.deleted, expected);
    assert.equal(await totalDocuments(db), 1);

    for (const collection of COLLECTION_NAMES) {
      const remaining = await db.collection(collection).countDocuments();
      if (collection === 'contactMessages') {
        assert.equal(remaining, 1, collection);
      } else {
        assert.equal(remaining, 0, collection);
      }
    }

    const foreign = await db.collection('contactMessages').findOne({
      _id: foreignId
    });
    assert.ok(foreign);
  });

  await t.test('el seed se puede repetir despues de la limpieza', async () => {
    await seedDemo(db, documents);
    assert.equal(await totalDocuments(db), expected + 1);
  });
});
