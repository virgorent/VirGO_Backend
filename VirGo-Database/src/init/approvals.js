import { createHash } from 'node:crypto';
import fs from 'node:fs';
import { COLLECTIONS } from '../catalog/index.js';

export class ApprovalError extends Error {
  constructor(message) {
    super(message);
    this.name = 'ApprovalError';
  }
}

const INDEX_METADATA = new Set([
  'v',
  'key',
  'name',
  'ns',
  'background',
  '2dsphereIndexVersion',
  'textIndexVersion',
  'buildUUID'
]);
const KINDS = new Set(['ttl-approval', 'index-waiver']);
const TTL_TYPES = new Set(['ttlMissing', 'ttlExtra']);
const WAIVER_TYPES = new Set(['extraIndex', 'nameMismatch']);
const SHA256_PATTERN = /^sha256:[0-9a-f]{64}$/;

const isObject = (value) => (
  value !== null && typeof value === 'object' && !Array.isArray(value)
);

const cleanOptions = (options) => Object.fromEntries(
  Object.entries(options ?? {}).filter(([, value]) => value !== undefined)
);

const normalizeIndex = (index) => {
  if (!index) return null;
  const inferred = Object.fromEntries(
    Object.entries(index).filter(([key]) => !INDEX_METADATA.has(key))
  );
  return {
    name: index.name,
    key: index.key,
    options: cleanOptions(index.options ?? inferred)
  };
};

const normalizePair = (pair) => ({
  desired: normalizeIndex(pair.desired),
  existing: normalizeIndex(pair.existing)
});

const differenceFingerprint = (difference) => ({
  collection: difference.collection,
  type: difference.type,
  ttl: Boolean(difference.ttl),
  index: normalizeIndex(difference.index),
  desired: normalizeIndex(difference.desired),
  existing: normalizeIndex(difference.existing),
  impact: difference.impact
    ? {
      documents: difference.impact.documents,
      basis: difference.impact.basis
    }
    : null
});

const collectionFingerprint = (collection) => ({
  name: collection.name,
  exists: collection.exists,
  validatorMatches: collection.validatorMatches,
  validationLevelMatches: collection.validationLevelMatches,
  validationActionMatches: collection.validationActionMatches,
  needsValidatorUpdate: collection.needsValidatorUpdate,
  missingIndexes: collection.missingIndexes.map(normalizeIndex),
  mismatchedIndexes: collection.mismatchedIndexes.map(normalizePair),
  satisfiedIndexes: collection.satisfiedIndexes.map(normalizeIndex),
  extraIndexes: collection.extraIndexes.map(normalizeIndex),
  nameMismatches: (collection.nameMismatches ?? []).map(normalizePair),
  keyMismatches: (collection.keyMismatches ?? []).map(normalizePair),
  optionsMismatches: (collection.optionsMismatches ?? []).map(normalizePair),
  differences: (collection.differences ?? []).map(differenceFingerprint)
});

const actionFingerprint = (action) => ({
  type: action.type,
  collection: action.collection,
  index: normalizeIndex(action.index)
});

export const stableStringify = (value) => {
  if (value === undefined) return 'null';
  if (value === null || typeof value !== 'object') {
    return JSON.stringify(value);
  }

  if (Array.isArray(value)) {
    return `[${value.map(stableStringify).join(',')}]`;
  }

  const keys = Object.keys(value)
    .filter((key) => value[key] !== undefined)
    .sort();

  return `{${keys.map((key) => (
    `${JSON.stringify(key)}:${stableStringify(value[key])}`
  )).join(',')}}`;
};

export const sha256 = (value) => createHash('sha256')
  .update(typeof value === 'string' ? value : stableStringify(value))
  .digest('hex');

export const fingerprint = (value) => `sha256:${sha256(value)}`;

export const catalogFingerprint = (collections = COLLECTIONS) => fingerprint(
  collections.map((collection) => ({
    name: collection.name,
    validator: collection.validator,
    indexes: collection.indexes.map(normalizeIndex)
  }))
);

export const computePlanFingerprint = (plan) => fingerprint({
  database: plan.database,
  environment: plan.environment,
  clusterIdentity: plan.clusterIdentity,
  catalogFingerprint: plan.catalogFingerprint,
  collections: plan.collections.map(collectionFingerprint),
  actions: plan.actions.map(actionFingerprint),
  differences: plan.differences.map(differenceFingerprint),
  drift: plan.drift,
  hasChanges: plan.hasChanges
});

export const clusterIdentityFromUri = (uri) => {
  if (typeof uri !== 'string') {
    throw new ApprovalError('MONGODB_URI no es valida para derivar el cluster.');
  }

  const match = /^(mongodb(?:\+srv)?):\/\/(?:[^@/?#]*@)?([^/?#]+)/i.exec(uri);
  if (!match) {
    throw new ApprovalError(
      'MONGODB_URI no permite derivar la identidad del cluster.'
    );
  }

  const scheme = match[1].toLowerCase();
  const hosts = match[2]
    .split(',')
    .map((host) => host.trim().toLowerCase())
    .filter(Boolean)
    .sort();

  if (hosts.length === 0) {
    throw new ApprovalError('MONGODB_URI no contiene hosts validos.');
  }

  return fingerprint(`${scheme}|${hosts.join(',')}`);
};

class StrictJsonParser {
  constructor(text) {
    this.text = text.replace(/^\uFEFF/, '');
    this.index = 0;
  }

  parse() {
    const value = this.parseValue('$');
    this.skipWhitespace();
    if (this.index !== this.text.length) {
      this.fail('contenido adicional', '$');
    }
    return value;
  }

  fail(message, path) {
    throw new ApprovalError(`JSON invalido en ${path}: ${message}.`);
  }

  skipWhitespace() {
    while (
      this.index < this.text.length
      && /[ \t\r\n]/.test(this.text[this.index])
    ) {
      this.index += 1;
    }
  }

  parseValue(path) {
    this.skipWhitespace();
    const char = this.text[this.index];

    if (char === undefined) this.fail('valor esperado', path);
    if (char === '{') return this.parseObject(path);
    if (char === '[') return this.parseArray(path);
    if (char === '"') return this.parseString(path);
    if (char === '-' || /[0-9]/.test(char)) return this.parseNumber(path);
    if (this.text.startsWith('true', this.index)) {
      this.index += 4;
      return true;
    }
    if (this.text.startsWith('false', this.index)) {
      this.index += 5;
      return false;
    }
    if (this.text.startsWith('null', this.index)) {
      this.index += 4;
      return null;
    }

    this.fail('valor no reconocido', path);
  }

  parseObject(path) {
    this.index += 1;
    this.skipWhitespace();
    const result = {};

    if (this.text[this.index] === '}') {
      this.index += 1;
      return result;
    }

    while (true) {
      this.skipWhitespace();
      if (this.text[this.index] !== '"') {
        this.fail('clave esperada', path);
      }

      const key = this.parseString(`${path}.clave`);
      this.skipWhitespace();

      if (this.text[this.index] !== ':') {
        this.fail('se esperaba ":"', path);
      }
      this.index += 1;

      if (Object.prototype.hasOwnProperty.call(result, key)) {
        this.fail(`propiedad duplicada "${key}"`, path);
      }

      result[key] = this.parseValue(`${path}.${key}`);
      this.skipWhitespace();

      const next = this.text[this.index];
      if (next === ',') {
        this.index += 1;
        continue;
      }
      if (next === '}') {
        this.index += 1;
        return result;
      }

      this.fail('se esperaba "," o "}"', path);
    }
  }

  parseArray(path) {
    this.index += 1;
    this.skipWhitespace();
    const result = [];

    if (this.text[this.index] === ']') {
      this.index += 1;
      return result;
    }

    while (true) {
      result.push(this.parseValue(`${path}[${result.length}]`));
      this.skipWhitespace();

      const next = this.text[this.index];
      if (next === ',') {
        this.index += 1;
        continue;
      }
      if (next === ']') {
        this.index += 1;
        return result;
      }

      this.fail('se esperaba "," o "]"', path);
    }
  }

  parseString(path) {
    if (this.text[this.index] !== '"') {
      this.fail('string esperado', path);
    }

    const start = this.index;
    this.index += 1;

    while (this.index < this.text.length) {
      const char = this.text[this.index];

      if (char === '"') {
        this.index += 1;
        try {
          return JSON.parse(this.text.slice(start, this.index));
        } catch {
          this.fail('string invalido', path);
        }
      }

      if (char === '\\') {
        this.index += 1;
        const escape = this.text[this.index];

        if (escape === 'u') {
          const hex = this.text.slice(this.index + 1, this.index + 5);
          if (!/^[0-9a-fA-F]{4}$/.test(hex)) {
            this.fail('escape unicode invalido', path);
          }
          this.index += 5;
          continue;
        }

        if (escape && '"\\/bfnrt'.includes(escape)) {
          this.index += 1;
          continue;
        }

        this.fail('escape invalido', path);
      }

      if (char.charCodeAt(0) < 0x20) {
        this.fail('caracter de control no permitido', path);
      }

      this.index += 1;
    }

    this.fail('string sin cerrar', path);
  }

  parseNumber(path) {
    const match = /^-?(?:0|[1-9]\d*)(?:\.\d+)?(?:[eE][+-]?\d+)?/.exec(
      this.text.slice(this.index)
    );

    if (!match) this.fail('numero invalido', path);

    this.index += match[0].length;
    return Number(match[0]);
  }
}

export const parseStrictJson = (text) => {
  if (typeof text !== 'string') {
    throw new ApprovalError('El JSON debe ser texto.');
  }
  return new StrictJsonParser(text).parse();
};

export const readJsonFile = (filePath) => {
  let text;

  try {
    text = fs.readFileSync(filePath, 'utf8');
  } catch (error) {
    const reason = error instanceof Error ? error.message : String(error);
    throw new ApprovalError(`No se pudo leer ${filePath}: ${reason}`);
  }

  return parseStrictJson(text);
};

const assertObject = (value, path) => {
  if (!isObject(value)) {
    throw new ApprovalError(`${path} debe ser un objeto.`);
  }
};

const assertExactKeys = (value, allowed, required, path) => {
  assertObject(value, path);

  for (const key of Object.keys(value)) {
    if (!allowed.has(key)) {
      throw new ApprovalError(`Propiedad desconocida en ${path}: ${key}.`);
    }
  }

  for (const key of required) {
    if (!Object.prototype.hasOwnProperty.call(value, key)) {
      throw new ApprovalError(`Falta la propiedad ${path}.${key}.`);
    }
  }
};

const assertString = (value, path) => {
  if (typeof value !== 'string' || value.trim() === '') {
    throw new ApprovalError(`${path} debe ser un string no vacio.`);
  }
};

const assertInteger = (value, path, minimum = 0) => {
  if (!Number.isInteger(value) || value < minimum) {
    throw new ApprovalError(`${path} debe ser un entero mayor o igual a ${minimum}.`);
  }
};

const assertSha256 = (value, path) => {
  if (typeof value !== 'string' || !SHA256_PATTERN.test(value)) {
    throw new ApprovalError(`${path} debe usar el formato sha256:<64 hex>.`);
  }
};

const parseDate = (value, path) => {
  if (typeof value !== 'string') {
    throw new ApprovalError(`${path} debe ser una fecha ISO-8601.`);
  }

  const time = Date.parse(value);
  if (!Number.isFinite(time)) {
    throw new ApprovalError(`${path} no es una fecha valida.`);
  }

  return time;
};

const validateCluster = (cluster) => {
  assertExactKeys(
    cluster,
    new Set(['identity', 'environment', 'database']),
    ['identity', 'environment', 'database'],
    'cluster'
  );
  assertSha256(cluster.identity, 'cluster.identity');
  assertString(cluster.environment, 'cluster.environment');
  assertString(cluster.database, 'cluster.database');
};

const validateImpact = (impact) => {
  assertExactKeys(
    impact,
    new Set(['documents', 'basis']),
    ['documents', 'basis'],
    'estimatedImpact'
  );
  assertInteger(impact.documents, 'estimatedImpact.documents');
  assertString(impact.basis, 'estimatedImpact.basis');
};

const validateTtlItem = (item, index) => {
  const path = `ttlApprovals[${index}]`;
  assertExactKeys(
    item,
    new Set([
      'collection',
      'index',
      'type',
      'expireAfterSeconds',
      'estimatedImpact',
      'maxDeletion'
    ]),
    [
      'collection',
      'index',
      'type',
      'expireAfterSeconds',
      'estimatedImpact',
      'maxDeletion'
    ],
    path
  );

  assertString(item.collection, `${path}.collection`);
  assertString(item.index, `${path}.index`);

  if (!TTL_TYPES.has(item.type)) {
    throw new ApprovalError(
      `${path}.type debe ser ttlMissing o ttlExtra.`
    );
  }

  assertInteger(item.expireAfterSeconds, `${path}.expireAfterSeconds`);
  validateImpact(item.estimatedImpact);
  assertInteger(item.maxDeletion, `${path}.maxDeletion`);

  if (item.maxDeletion < item.estimatedImpact.documents) {
    throw new ApprovalError(
      `${path}.maxDeletion (${item.maxDeletion}) es menor que el impacto `
      + `estimado (${item.estimatedImpact.documents}).`
    );
  }
};

const validateWaiverItem = (item, index) => {
  const path = `waivers[${index}]`;
  assertExactKeys(
    item,
    new Set([
      'collection',
      'type',
      'index',
      'actualIndex',
      'reason',
      'riskAccepted'
    ]),
    ['collection', 'type', 'index', 'reason', 'riskAccepted'],
    path
  );

  assertString(item.collection, `${path}.collection`);
  assertString(item.index, `${path}.index`);
  assertString(item.reason, `${path}.reason`);
  assertString(item.riskAccepted, `${path}.riskAccepted`);

  if (!WAIVER_TYPES.has(item.type)) {
    throw new ApprovalError(
      `${path}.type debe ser extraIndex o nameMismatch.`
    );
  }

  if (item.type === 'nameMismatch') {
    if (!Object.prototype.hasOwnProperty.call(item, 'actualIndex')) {
      throw new ApprovalError(`Falta la propiedad ${path}.actualIndex.`);
    }
    assertString(item.actualIndex, `${path}.actualIndex`);
    if (item.actualIndex === item.index) {
      throw new ApprovalError(
        `${path}.actualIndex debe ser distinto de ${path}.index.`
      );
    }
    return;
  }

  if (
    Object.prototype.hasOwnProperty.call(item, 'actualIndex')
    && item.actualIndex !== item.index
  ) {
    throw new ApprovalError(
      `${path}.actualIndex debe coincidir con ${path}.index para extraIndex.`
    );
  }
};

const validateContext = (context) => {
  if (!isObject(context)) {
    throw new ApprovalError('El contexto de la aprobacion es obligatorio.');
  }

  assertSha256(context.clusterIdentity, 'context.clusterIdentity');
  assertString(context.environment, 'context.environment');
  assertString(context.database, 'context.database');
  assertSha256(context.catalogFingerprint, 'context.catalogFingerprint');
};

export const validateApprovalDocument = (
  document,
  { kind, context, now = new Date() } = {}
) => {
  if (!KINDS.has(kind)) {
    throw new ApprovalError(`Tipo de documento de aprobacion invalido: ${kind}.`);
  }
  validateContext(context);

  const arrayKey = kind === 'ttl-approval' ? 'ttlApprovals' : 'waivers';
  const allowed = new Set([
    'version',
    'kind',
    'cluster',
    'catalogFingerprint',
    'planFingerprint',
    'approvedBy',
    'approvedAt',
    'expiresAt',
    arrayKey
  ]);

  assertExactKeys(
    document,
    allowed,
    [
      'version',
      'kind',
      'cluster',
      'catalogFingerprint',
      'planFingerprint',
      'approvedBy',
      'approvedAt',
      'expiresAt',
      arrayKey
    ],
    'document'
  );

  if (document.version !== 1) {
    throw new ApprovalError('document.version debe ser 1.');
  }

  if (document.kind !== kind) {
    throw new ApprovalError(`document.kind debe ser ${kind}.`);
  }

  validateCluster(document.cluster);
  assertSha256(document.catalogFingerprint, 'catalogFingerprint');
  assertSha256(document.planFingerprint, 'planFingerprint');
  assertString(document.approvedBy, 'approvedBy');

  const approvedAt = parseDate(document.approvedAt, 'approvedAt');
  const expiresAt = parseDate(document.expiresAt, 'expiresAt');
  const current = now instanceof Date ? now.getTime() : Number(now);

  if (!Number.isFinite(current)) {
    throw new ApprovalError('La fecha de validacion no es valida.');
  }

  if (expiresAt <= current) {
    throw new ApprovalError('La aprobacion esta expirada.');
  }

  if (approvedAt > expiresAt) {
    throw new ApprovalError('approvedAt no puede ser posterior a expiresAt.');
  }

  if (document.cluster.identity !== context.clusterIdentity) {
    throw new ApprovalError('La aprobacion pertenece a otro cluster.');
  }

  if (document.cluster.environment !== context.environment) {
    throw new ApprovalError('La aprobacion pertenece a otro entorno.');
  }

  if (document.cluster.database !== context.database) {
    throw new ApprovalError('La aprobacion pertenece a otra base de datos.');
  }

  if (document.catalogFingerprint !== context.catalogFingerprint) {
    throw new ApprovalError('La aprobacion usa un catalogFingerprint distinto.');
  }

  if (!Array.isArray(document[arrayKey]) || document[arrayKey].length === 0) {
    throw new ApprovalError(`document.${arrayKey} debe ser un arreglo no vacio.`);
  }

  const seen = new Set();
  document[arrayKey].forEach((item, index) => {
    if (kind === 'ttl-approval') {
      validateTtlItem(item, index);
    } else {
      validateWaiverItem(item, index);
    }

    const key = [
      item.collection,
      item.type,
      item.index,
      item.actualIndex ?? ''
    ].join('|');

    if (seen.has(key)) {
      throw new ApprovalError(`${arrayKey}[${index}] esta duplicado.`);
    }
    seen.add(key);
  });

  return document;
};

export const loadApprovalFile = (
  filePath,
  kind,
  context,
  { now = new Date() } = {}
) => {
  const document = readJsonFile(filePath);
  return validateApprovalDocument(document, { kind, context, now });
};
