import test from 'node:test';
import assert from 'node:assert/strict';
import { INDEXES } from '../../src/catalog/index.js';
import {
  ApplyError,
  applyPlan
} from '../../src/init/index.js';

const fakeDb = ({ failIndex = false } = {}) => {
  const calls = [];

  return {
    calls,
    async createCollection(name) {
      calls.push(['createCollection', name]);
    },
    async command(command) {
      calls.push(['command', command.collMod]);
    },
    collection(name) {
      return {
        async createIndex(key, options) {
          calls.push(['createIndex', name, key, options]);
          if (failIndex) {
            throw new Error('fallo simulado');
          }
        }
      };
    }
  };
};

test('rechaza planes con conflictos antes de escribir', async () => {
  const db = fakeDb();
  await assert.rejects(
    () => applyPlan(db, {
      actions: [],
      errors: [{ type: 'indexConflict' }]
    }),
    /conflictos de indices/
  );
  assert.equal(db.calls.length, 0);
});

test('rechaza acciones no soportadas o destructivas', async () => {
  const db = fakeDb();
  for (const action of [
    { type: 'dropCollection', collection: 'users' },
    { type: 'deleteMany', collection: 'users' },
    { type: 'createIndex', collection: 'users' }
  ]) {
    await assert.rejects(
      () => applyPlan(db, { actions: [action], errors: [] }),
      /no soportada|no pertenece|no esta declarado/
    );
  }
  assert.equal(db.calls.length, 0);
});

test('rechaza colecciones fuera del catalogo', async () => {
  const db = fakeDb();
  await assert.rejects(
    () => applyPlan(db, {
      actions: [{ type: 'createCollection', collection: 'intrusos' }],
      errors: []
    }),
    /no pertenece al catalogo/
  );
  assert.equal(db.calls.length, 0);
});

test('rechaza un indice manipulado con el mismo nombre', async () => {
  const db = fakeDb();
  await assert.rejects(
    () => applyPlan(db, {
      actions: [{
        type: 'createIndex',
        collection: 'users',
        index: {
          name: 'email_1_unique',
          key: { email: -1 },
          options: { unique: true }
        }
      }],
      errors: []
    }),
    /no coincide con el catalogo/
  );
  assert.equal(db.calls.length, 0);
});

test('aplica solo acciones declaradas y devuelve resultados', async () => {
  const db = fakeDb();
  const results = await applyPlan(db, {
    errors: [],
    actions: [
      { type: 'createCollection', collection: 'venues' },
      { type: 'collMod', collection: 'users' },
      {
        type: 'createIndex',
        collection: 'users',
        index: INDEXES.users[0]
      }
    ]
  });

  assert.equal(results.length, 3);
  assert.equal(results[0].result, 'created');
  assert.equal(results[1].result, 'updated');
  assert.equal(results[2].result, 'created');
  assert.deepEqual(db.calls[1], ['command', 'users']);
});

test('reporta fallos parciales con las operaciones ya aplicadas', async () => {
  const db = fakeDb({ failIndex: true });

  await assert.rejects(
    () => applyPlan(db, {
      errors: [],
      actions: [
        { type: 'createCollection', collection: 'users' },
        {
          type: 'createIndex',
          collection: 'users',
          index: INDEXES.users[0]
        }
      ]
    }),
    (error) => {
      assert.ok(error instanceof ApplyError);
      assert.equal(error.results.length, 1);
      assert.equal(error.results[0].type, 'createCollection');
      assert.equal(error.failedAction.type, 'createIndex');
      assert.match(error.message, /idempotentes/);
      return true;
    }
  );
});
