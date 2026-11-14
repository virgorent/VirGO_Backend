import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import {
  catalogFingerprint,
  clusterIdentityFromUri,
  loadApprovalFile,
  parseStrictJson,
  readJsonFile,
  validateApprovalDocument
} from '../../src/init/approvals.js';

const NOW = new Date('2026-01-01T00:00:00.000Z');
const HASH = (char) => `sha256:${char.repeat(64)}`;
const context = {
  clusterIdentity: HASH('a'),
  environment: 'production',
  database: 'virgo',
  catalogFingerprint: HASH('b')
};
const planFingerprint = HASH('c');

const ttlItem = {
  collection: 'sessions',
  index: 'purgeAt_ttl',
  type: 'ttlMissing',
  expireAfterSeconds: 0,
  estimatedImpact: {
    documents: 3,
    basis: 'estimatedDocumentCount'
  },
  maxDeletion: 3
};

const waiverItem = {
  collection: 'users',
  type: 'nameMismatch',
  index: 'email_1_unique',
  actualIndex: 'email_1',
  reason: 'Indice equivalente heredado',
  riskAccepted: 'Se reviso el inventario manualmente'
};

const makeDocument = (kind, items, overrides = {}) => ({
  version: 1,
  kind,
  cluster: {
    identity: context.clusterIdentity,
    environment: context.environment,
    database: context.database
  },
  catalogFingerprint: context.catalogFingerprint,
  planFingerprint,
  approvedBy: 'responsable-operaciones',
  approvedAt: '2025-12-31T00:00:00.000Z',
  expiresAt: '2026-12-31T00:00:00.000Z',
  [kind === 'ttl-approval' ? 'ttlApprovals' : 'waivers']: items,
  ...overrides
});

const validate = (document, kind = document.kind) => validateApprovalDocument(
  document,
  { kind, context, now: NOW }
);

test('acepta una aprobacion TTL valida', () => {
  const document = makeDocument('ttl-approval', [ttlItem]);
  assert.equal(validate(document), document);
});

test('acepta un waiver valido para nameMismatch', () => {
  const document = makeDocument('index-waiver', [waiverItem]);
  assert.equal(validate(document), document);
});

test('acepta un waiver valido para extraIndex', () => {
  const document = makeDocument('index-waiver', [{
    collection: 'users',
    type: 'extraIndex',
    index: 'legacy_1',
    reason: 'Indice heredado',
    riskAccepted: 'Se reviso el inventario manualmente'
  }]);
  assert.equal(validate(document), document);
});

test('rechaza propiedades desconocidas en el documento y en los items', () => {
  assert.throws(
    () => validate(makeDocument('ttl-approval', [ttlItem], { extra: true })),
    /Propiedad desconocida/
  );

  assert.throws(
    () => validate(makeDocument('ttl-approval', [
      { ...ttlItem, extra: true }
    ])),
    /Propiedad desconocida/
  );

  assert.throws(
    () => validate(makeDocument('index-waiver', [
      { ...waiverItem, extra: true }
    ])),
    /Propiedad desconocida/
  );
});

test('rechaza propiedades faltantes', () => {
  const missing = makeDocument('ttl-approval', [ttlItem]);
  delete missing.approvedBy;

  assert.throws(() => validate(missing), /Falta la propiedad document.approvedBy/);
});

test('rechaza JSON con propiedades duplicadas', () => {
  assert.throws(
    () => parseStrictJson('{"version":1,"version":1}'),
    /propiedad duplicada/
  );

  assert.throws(
    () => parseStrictJson('{"cluster":{"identity":"a","identity":"b"}}'),
    /propiedad duplicada/
  );
});

test('rechaza JSON mal formado, escapes invalidos y contenido extra', () => {
  for (const text of [
    '{"version":1,}',
    '{"a":"\\x"}',
    '{"a":01}',
    '{"a":1} extra'
  ]) {
    assert.throws(() => parseStrictJson(text), /JSON invalido/);
  }
});

test('rechaza aprobaciones expiradas o con fechas invalidas', () => {
  assert.throws(
    () => validate(makeDocument('ttl-approval', [ttlItem], {
      expiresAt: '2025-12-31T23:59:59.000Z'
    })),
    /expirada/
  );

  assert.throws(
    () => validate(makeDocument('ttl-approval', [ttlItem], {
      approvedAt: 'no-es-fecha'
    })),
    /fecha valida/
  );

  assert.throws(
    () => validate(makeDocument('ttl-approval', [ttlItem], {
      approvedAt: '2027-01-01T00:00:00.000Z'
    })),
    /posterior a expiresAt/
  );
});

test('rechaza cluster, entorno, base y catalogFingerprint incorrectos', () => {
  const cases = [
    ['identity', HASH('d')],
    ['environment', 'development'],
    ['database', 'virgo_dev']
  ];

  for (const [field, value] of cases) {
    const document = makeDocument('ttl-approval', [ttlItem]);
    document.cluster[field] = value;
    assert.throws(() => validate(document), /otro cluster|otro entorno|otra base/);
  }

  assert.throws(
    () => validate(makeDocument('ttl-approval', [ttlItem], {
      catalogFingerprint: HASH('e')
    })),
    /catalogFingerprint distinto/
  );
});

test('rechaza huellas con formato invalido y listas vacias', () => {
  assert.throws(
    () => validate(makeDocument('ttl-approval', [ttlItem], {
      planFingerprint: 'abc'
    })),
    /sha256/
  );

  assert.throws(
    () => validate(makeDocument('ttl-approval', [])),
    /arreglo no vacio/
  );
});

test('rechaza limite de impacto menor al impacto estimado', () => {
  assert.throws(
    () => validate(makeDocument('ttl-approval', [{
      ...ttlItem,
      maxDeletion: 2
    }])),
    /maxDeletion.*menor que el impacto/
  );
});

test('rechaza items duplicados', () => {
  assert.throws(
    () => validate(makeDocument('ttl-approval', [ttlItem, ttlItem])),
    /duplicado/
  );

  assert.throws(
    () => validate(makeDocument('index-waiver', [waiverItem, waiverItem])),
    /duplicado/
  );
});

test('exige actualIndex distinto para nameMismatch', () => {
  assert.throws(
    () => validate(makeDocument('index-waiver', [{
      ...waiverItem,
      actualIndex: undefined
    }])),
    /actualIndex/
  );

  assert.throws(
    () => validate(makeDocument('index-waiver', [{
      ...waiverItem,
      actualIndex: waiverItem.index
    }])),
    /distinto/
  );
});

test('deriva una identidad de cluster estable sin credenciales ni query', () => {
  const first = clusterIdentityFromUri(
    'mongodb+srv://user:secret@Cluster.Example.net/db?retryWrites=true'
  );
  const second = clusterIdentityFromUri(
    'mongodb+srv://other:other@cluster.example.net/otra?tls=true'
  );
  const third = clusterIdentityFromUri(
    'mongodb+srv://user:secret@otro.example.net/db'
  );

  assert.equal(first, second);
  assert.notEqual(first, third);
  assert.match(first, /^sha256:[0-9a-f]{64}$/);
  assert.equal(first.includes('secret'), false);
});

test('lee archivos JSON estrictos desde disco', () => {
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'virgo-approval-'));

  try {
    const file = path.join(directory, 'ttl.json');
    const document = makeDocument('ttl-approval', [ttlItem]);
    fs.writeFileSync(file, JSON.stringify(document, null, 2));

    assert.deepEqual(readJsonFile(file), document);
    assert.deepEqual(
      loadApprovalFile(file, 'ttl-approval', context, { now: NOW }),
      document
    );

    fs.writeFileSync(file, '{"version":1,"version":1}');
    assert.throws(
      () => loadApprovalFile(file, 'ttl-approval', context, { now: NOW }),
      /propiedad duplicada/
    );
  } finally {
    fs.rmSync(directory, { recursive: true, force: true });
  }
});

test('catalogFingerprint es determinista para el catalogo', () => {
  assert.equal(catalogFingerprint(), catalogFingerprint());
  assert.match(catalogFingerprint(), /^sha256:[0-9a-f]{64}$/);
});
