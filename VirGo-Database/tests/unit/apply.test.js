import test from 'node:test';
import assert from 'node:assert/strict';
import { INDEXES } from '../../src/catalog/index.js';
import {
  ApplyError,
  applyPlan,
  catalogFingerprint,
  computePlanFingerprint
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

const makePlan = (overrides = {}) => {
  const plan = {
    database: 'virgo_test',
    environment: 'development',
    clusterIdentity: null,
    catalogFingerprint: catalogFingerprint(),
    generatedAt: '2026-01-01T00:00:00.000Z',
    collections: [],
    actions: [],
    differences: [],
    errors: [],
    warnings: [],
    drift: false,
    hasChanges: false,
    exactMatch: false,
    ok: false,
    summary: {
      collections: 0,
      createCollections: 0,
      updateValidators: 0,
      createIndexes: 0,
      indexConflicts: 0,
      extraIndexes: 0,
      nameMismatches: 0,
      keyMismatches: 0,
      optionsMismatches: 0,
      ttlMissing: 0,
      ttlExtra: 0,
      waived: 0,
      ttlApproved: 0,
      warnings: 0
    },
    ...overrides
  };

  plan.hasChanges = plan.actions.length > 0;
  plan.drift = plan.hasChanges;
  plan.planFingerprint = computePlanFingerprint(plan);
  return plan;
};

const ttlIndex = INDEXES.sessions.find((index) => index.name === 'purgeAt_ttl');

test('rechaza planes con conflictos antes de escribir', async () => {
  const db = fakeDb();
  await assert.rejects(
    () => applyPlan(db, makePlan({
      errors: [{ type: 'indexConflict' }]
    })),
    /conflictos o aprobaciones pendientes/
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
      () => applyPlan(db, makePlan({ actions: [action] })),
      /no soportada|no pertenece|no esta declarado/
    );
  }
  assert.equal(db.calls.length, 0);
});

test('rechaza colecciones fuera del catalogo', async () => {
  const db = fakeDb();
  await assert.rejects(
    () => applyPlan(db, makePlan({
      actions: [{ type: 'createCollection', collection: 'intrusos' }]
    })),
    /no pertenece al catalogo/
  );
  assert.equal(db.calls.length, 0);
});

test('rechaza un indice manipulado con el mismo nombre', async () => {
  const db = fakeDb();
  await assert.rejects(
    () => applyPlan(db, makePlan({
      actions: [{
        type: 'createIndex',
        collection: 'users',
        index: {
          name: 'email_1_unique',
          key: { email: -1 },
          options: { unique: true }
        }
      }]
    })),
    /no coincide con el catalogo/
  );
  assert.equal(db.calls.length, 0);
});

test('rechaza planes sin huellas o con huellas obsoletas', async () => {
  const db = fakeDb();
  const withoutFingerprint = makePlan({
    actions: [{ type: 'createCollection', collection: 'venues' }]
  });
  delete withoutFingerprint.planFingerprint;

  await assert.rejects(
    () => applyPlan(db, withoutFingerprint),
    /planFingerprint valido/
  );

  const stale = makePlan({
    actions: [{ type: 'createCollection', collection: 'venues' }]
  });
  stale.actions.push({ type: 'collMod', collection: 'users' });

  await assert.rejects(
    () => applyPlan(db, stale),
    /no coincide con el contenido/
  );
});

test('rechaza planes con catalogFingerprint de otro catalogo', async () => {
  const db = fakeDb();
  const plan = makePlan({
    actions: [{ type: 'createCollection', collection: 'venues' }]
  });
  plan.catalogFingerprint = `sha256:${'0'.repeat(64)}`;

  await assert.rejects(
    () => applyPlan(db, plan),
    /catalogFingerprint del plan no coincide/
  );
});

test('rechaza TTL sin aprobacion en production antes de escribir', async () => {
  const db = fakeDb();
  const plan = makePlan({
    environment: 'production',
    actions: [{
      type: 'createIndex',
      collection: 'sessions',
      index: ttlIndex,
      requiresTtlApproval: true,
      ttlApproval: null
    }]
  });

  await assert.rejects(
    () => applyPlan(db, plan),
    /requiere una aprobacion TTL valida/
  );
  assert.equal(db.calls.length, 0);
});

test('acepta TTL aprobado en production', async () => {
  const db = fakeDb();
  const plan = makePlan({
    environment: 'production',
    actions: [{
      type: 'createIndex',
      collection: 'sessions',
      index: ttlIndex,
      requiresTtlApproval: true,
      ttlApproval: {
        expireAfterSeconds: 0,
        maxDeletion: 1
      }
    }]
  });

  const results = await applyPlan(db, plan);
  assert.equal(results.length, 1);
  assert.equal(results[0].result, 'created');
});

test('aplica solo acciones declaradas y devuelve resultados', async () => {
  const db = fakeDb();
  const results = await applyPlan(db, makePlan({
    actions: [
      { type: 'createCollection', collection: 'venues' },
      { type: 'collMod', collection: 'users' },
      {
        type: 'createIndex',
        collection: 'users',
        index: INDEXES.users[0]
      }
    ]
  }));

  assert.equal(results.length, 3);
  assert.equal(results[0].result, 'created');
  assert.equal(results[1].result, 'updated');
  assert.equal(results[2].result, 'created');
  assert.deepEqual(db.calls[1], ['command', 'users']);
});

test('reporta fallos parciales e invalida el plan anterior', async () => {
  const db = fakeDb({ failIndex: true });
  const plan = makePlan({
    actions: [
      { type: 'createCollection', collection: 'users' },
      {
        type: 'createIndex',
        collection: 'users',
        index: INDEXES.users[0]
      }
    ]
  });

  await assert.rejects(
    () => applyPlan(db, plan),
    (error) => {
      assert.ok(error instanceof ApplyError);
      assert.equal(error.results.length, 1);
      assert.equal(error.results[0].type, 'createCollection');
      assert.equal(error.failedAction.type, 'createIndex');
      assert.equal(error.planFingerprint, plan.planFingerprint);
      assert.equal(error.planInvalidated, true);
      assert.equal(error.requiresReinspection, true);
      assert.match(error.message, /idempotentes/);
      assert.match(error.message, /invalidado/);
      return true;
    }
  );
});
