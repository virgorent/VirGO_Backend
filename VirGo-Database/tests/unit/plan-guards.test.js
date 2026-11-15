import test from 'node:test';
import assert from 'node:assert/strict';
import { COLLECTIONS_BY_NAME } from '../../src/catalog/index.js';
import {
  buildPlan,
  computePlanFingerprint,
  verifyInitialization
} from '../../src/init/index.js';
import { createFakeDb } from '../helpers/fakeDb.js';

const users = COLLECTIONS_BY_NAME.get('users');
const sessions = COLLECTIONS_BY_NAME.get('sessions');
const CLUSTER = `sha256:${'9'.repeat(64)}`;
const ID_INDEX = { name: '_id_', key: { _id: 1 } };

const fakeDb = (definition, indexes, documentCount = 0) => createFakeDb({
  collections: {
    [definition.name]: {
      validator: definition.validator,
      indexes,
      documentCount
    }
  }
});

const planFor = (db, definitions, options = {}) => buildPlan(
  db,
  definitions,
  {
    environment: 'production',
    clusterIdentity: CLUSTER,
    ...options
  }
);

const waiverDocument = (plan, waivers) => ({
  planFingerprint: plan.planFingerprint,
  approvedBy: 'responsable-operaciones',
  expiresAt: '2027-01-01T00:00:00.000Z',
  waivers
});

const ttlDocument = (plan, ttlApprovals) => ({
  planFingerprint: plan.planFingerprint,
  approvedBy: 'responsable-operaciones',
  expiresAt: '2027-01-01T00:00:00.000Z',
  ttlApprovals
});

test('clasifica un indice faltante y mantiene la accion de creacion', async () => {
  const db = fakeDb(users, [ID_INDEX]);
  const plan = await planFor(db, [users], { environment: 'test' });

  const difference = plan.differences.find((item) => item.type === 'missing');
  assert.equal(difference.index.name, 'email_1_unique');
  assert.equal(plan.errors.length, 0);
  assert.equal(plan.actions.some((action) => (
    action.type === 'createIndex' && action.index.name === 'email_1_unique'
  )), true);
  assert.equal(plan.drift, true);
  assert.equal(plan.exactMatch, false);
  assert.equal(plan.ok, false);
});

test('bloquea un indice extra sin waiver y lo acepta con waiver', async () => {
  const extra = { name: 'legacy_1', key: { legacy: 1 } };
  const db = fakeDb(users, [
    ID_INDEX,
    { name: 'email_1_unique', key: { email: 1 }, unique: true },
    extra
  ]);

  const blocked = await planFor(db, [users], { environment: 'test' });
  assert.equal(blocked.errors.length, 1);
  assert.equal(blocked.errors[0].code, 'extra');
  assert.equal(blocked.differences.find(
    (item) => item.type === 'extra'
  ).status, 'blocked');

  const document = waiverDocument(blocked, [{
    collection: 'users',
    type: 'extraIndex',
    index: 'legacy_1',
    reason: 'Indice heredado',
    riskAccepted: 'Revisado manualmente'
  }]);
  const waived = await planFor(db, [users], {
    environment: 'test',
    waiverDocument: document
  });

  assert.equal(waived.errors.length, 0);
  assert.equal(waived.exactMatch, false);
  assert.equal(waived.ok, true);
  assert.equal(waived.differences.find(
    (item) => item.type === 'extra'
  ).status, 'waived');
});

test('bloquea un nombre distinto sin waiver y lo acepta con waiver', async () => {
  const db = fakeDb(users, [
    ID_INDEX,
    { name: 'email_1', key: { email: 1 }, unique: true }
  ]);
  const blocked = await planFor(db, [users], { environment: 'test' });

  assert.equal(blocked.errors.length, 1);
  assert.equal(blocked.errors[0].code, 'nameMismatch');
  assert.equal(blocked.actions.length, 0);

  const document = waiverDocument(blocked, [{
    collection: 'users',
    type: 'nameMismatch',
    index: 'email_1_unique',
    actualIndex: 'email_1',
    reason: 'Indice equivalente heredado',
    riskAccepted: 'Revisado manualmente'
  }]);
  const waived = await planFor(db, [users], {
    environment: 'test',
    waiverDocument: document
  });

  assert.equal(waived.errors.length, 0);
  assert.equal(waived.actions.length, 0);
  assert.equal(waived.exactMatch, false);
  assert.equal(waived.ok, true);
});

test('bloquea diferencias de clave y opciones incluso con un waiver', async () => {
  const keyDb = fakeDb(users, [
    ID_INDEX,
    { name: 'email_1_unique', key: { email: -1 }, unique: true }
  ]);
  const keyPlan = await planFor(keyDb, [users], { environment: 'test' });
  assert.equal(keyPlan.errors[0].code, 'keyMismatch');

  const document = waiverDocument(keyPlan, [{
    collection: 'users',
    type: 'extraIndex',
    index: 'email_1_unique',
    reason: 'Intento de waiver invalido',
    riskAccepted: 'No aplica'
  }]);
  await assert.rejects(
    () => planFor(keyDb, [users], {
      environment: 'test',
      waiverDocument: document
    }),
    /no corresponde a ninguna diferencia/
  );

  const optionsDb = fakeDb(users, [
    ID_INDEX,
    { name: 'email_1_unique', key: { email: 1 } }
  ]);
  const optionsPlan = await planFor(optionsDb, [users], { environment: 'test' });
  assert.equal(optionsPlan.errors[0].code, 'optionsMismatch');
});

test('exige aprobacion TTL para un TTL faltante en production', async () => {
  const db = fakeDb(sessions, [
    ID_INDEX,
    { name: 'tokenHash_1_unique', key: { tokenHash: 1 }, unique: true },
    { name: 'userId_1', key: { userId: 1 } }
  ], 3);

  const blocked = await planFor(db, [sessions]);
  assert.equal(blocked.errors.length, 1);
  assert.equal(blocked.errors[0].code, 'ttlMissing');
  assert.equal(blocked.differences[0].impact.documents, 3);

  const document = ttlDocument(blocked, [{
    collection: 'sessions',
    index: 'purgeAt_ttl',
    type: 'ttlMissing',
    expireAfterSeconds: 0,
    estimatedImpact: {
      documents: 3,
      basis: 'estimatedDocumentCount'
    },
    maxDeletion: 3
  }]);
  const approved = await planFor(db, [sessions], {
    ttlApprovalDocument: document
  });

  assert.equal(approved.errors.length, 0);
  assert.equal(approved.actions[0].requiresTtlApproval, true);
  assert.equal(approved.actions[0].ttlApproval.maxDeletion, 3);
  assert.equal(approved.differences[0].status, 'approved');
  assert.equal(approved.exactMatch, false);
  assert.equal(approved.ok, false);
});

test('registra el TTL faltante como advertencia fuera de production', async () => {
  const db = fakeDb(sessions, [
    ID_INDEX,
    { name: 'tokenHash_1_unique', key: { tokenHash: 1 }, unique: true },
    { name: 'userId_1', key: { userId: 1 } }
  ], 3);

  const plan = await planFor(db, [sessions], { environment: 'development' });
  assert.equal(plan.errors.length, 0);
  assert.equal(plan.warnings.length, 1);
  assert.equal(plan.differences[0].status, 'warning');
  assert.equal(plan.actions[0].ttlApproval, null);
});

test('bloquea un TTL adicional en production y no acepta waiver generico', async () => {
  const definition = {
    ...sessions,
    indexes: sessions.indexes.filter((index) => index.name !== 'purgeAt_ttl')
  };
  const db = fakeDb(definition, [
    ID_INDEX,
    { name: 'tokenHash_1_unique', key: { tokenHash: 1 }, unique: true },
    { name: 'userId_1', key: { userId: 1 } },
    { name: 'purgeAt_ttl', key: { purgeAt: 1 }, expireAfterSeconds: 0 }
  ], 4);

  const plan = await planFor(db, [definition]);
  assert.equal(plan.errors.length, 1);
  assert.equal(plan.errors[0].code, 'ttlExtra');

  const document = waiverDocument(plan, [{
    collection: 'sessions',
    type: 'extraIndex',
    index: 'purgeAt_ttl',
    reason: 'Intento de cubrir TTL con waiver',
    riskAccepted: 'No permitido'
  }]);
  await assert.rejects(
    () => planFor(db, [definition], { waiverDocument: document }),
    /no corresponde a ninguna diferencia/
  );

  const approvedDocument = ttlDocument(plan, [{
    collection: 'sessions',
    index: 'purgeAt_ttl',
    type: 'ttlExtra',
    expireAfterSeconds: 0,
    estimatedImpact: {
      documents: 4,
      basis: 'estimatedDocumentCount'
    },
    maxDeletion: 4
  }]);
  const approved = await planFor(db, [definition], {
    ttlApprovalDocument: approvedDocument
  });
  assert.equal(approved.errors.length, 0);
  assert.equal(approved.differences[0].status, 'approved');
  assert.equal(approved.exactMatch, false);
  assert.equal(approved.ok, true);
});

test('rechaza una aprobacion TTL cuyo maxDeletion no cubre el impacto', async () => {
  const db = fakeDb(sessions, [
    ID_INDEX,
    { name: 'tokenHash_1_unique', key: { tokenHash: 1 }, unique: true },
    { name: 'userId_1', key: { userId: 1 } }
  ], 5);
  const plan = await planFor(db, [sessions]);
  const document = ttlDocument(plan, [{
    collection: 'sessions',
    index: 'purgeAt_ttl',
    type: 'ttlMissing',
    expireAfterSeconds: 0,
    estimatedImpact: {
      documents: 5,
      basis: 'estimatedDocumentCount'
    },
    maxDeletion: 4
  }]);

  await assert.rejects(
    () => planFor(db, [sessions], { ttlApprovalDocument: document }),
    /impacto estimado/
  );
});

test('verify distingue ok de exactMatch con una excepcion aprobada', async () => {
  const db = fakeDb(users, [
    ID_INDEX,
    { name: 'email_1', key: { email: 1 }, unique: true }
  ]);
  const blocked = await planFor(db, [users], { environment: 'test' });
  const document = waiverDocument(blocked, [{
    collection: 'users',
    type: 'nameMismatch',
    index: 'email_1_unique',
    actualIndex: 'email_1',
    reason: 'Indice equivalente heredado',
    riskAccepted: 'Revisado manualmente'
  }]);
  const result = await verifyInitialization(db, [users], {
    environment: 'test',
    clusterIdentity: CLUSTER,
    waiverDocument: document
  });

  assert.equal(result.ok, true);
  assert.equal(result.exactMatch, false);
  assert.equal(result.waivedIssues.length, 1);
});

test('verify falla si hay acciones pendientes', async () => {
  const db = fakeDb(users, [ID_INDEX]);
  const result = await verifyInitialization(db, [users], {
    environment: 'test'
  });

  assert.equal(result.ok, false);
  assert.equal(result.exactMatch, false);
  assert.equal(result.missingIndexes.length, 1);
});

test('planFingerprint ignora metadatos pero detecta cambios de acciones', async () => {
  const db = fakeDb(users, [ID_INDEX]);
  const plan = await planFor(db, [users], { environment: 'test' });
  const same = {
    ...plan,
    generatedAt: '2030-01-01T00:00:00.000Z',
    approvals: {
      ttl: null,
      waiver: null
    }
  };
  const changed = {
    ...plan,
    actions: [
      ...plan.actions,
      { type: 'collMod', collection: 'users' }
    ]
  };

  assert.equal(computePlanFingerprint(same), plan.planFingerprint);
  assert.notEqual(computePlanFingerprint(changed), plan.planFingerprint);
});
