import fs from 'node:fs';
import path from 'node:path';
import dotenv from 'dotenv';

export class ConfigError extends Error {
  constructor(message) {
    super(message);
    this.name = 'ConfigError';
  }
}

const ENVIRONMENTS = new Set(['development', 'production', 'test']);
const DB_NAME_PATTERN = /^[A-Za-z0-9_-]+$/;
const TLS_PARAMETERS = new Set(['tls', 'ssl']);
const INSECURE_TLS_PARAMETERS = new Set([
  'tlsinsecure',
  'tlsallowinvalidcertificates',
  'tlsallowinvalidhostnames',
  'sslinvalidhostnameallowed'
]);
const TRUE_VALUES = new Set(['true', '1']);
const FALSE_VALUES = new Set(['false', '0']);

const readPositiveInt = (name, fallback) => {
  const raw = process.env[name];
  if (raw === undefined || raw === '') return fallback;
  const value = Number.parseInt(raw, 10);
  if (!Number.isInteger(value) || value <= 0) {
    throw new ConfigError(`${name} debe ser un entero positivo.`);
  }
  return value;
};

const readUriParameters = (uri) => {
  const queryIndex = uri.indexOf('?');
  if (queryIndex === -1) return [];

  const fragmentIndex = uri.indexOf('#', queryIndex);
  const query = uri.slice(
    queryIndex + 1,
    fragmentIndex === -1 ? undefined : fragmentIndex
  );

  return [...new URLSearchParams(query).entries()].map(([key, value]) => [
    key.toLowerCase(),
    value.toLowerCase()
  ]);
};

const resolveTlsPolicy = (uri, nodeEnv) => {
  const parameters = readUriParameters(uri);
  const tlsValues = parameters
    .filter(([key]) => TLS_PARAMETERS.has(key))
    .map(([, value]) => value);
  const insecureParameters = parameters
    .filter(([key, value]) => INSECURE_TLS_PARAMETERS.has(key) && TRUE_VALUES.has(value))
    .map(([key]) => key);

  if (insecureParameters.length > 0) {
    throw new ConfigError(
      `MONGODB_URI no puede desactivar la validacion TLS: ${insecureParameters.join(', ')}.`
    );
  }

  const invalidTlsValue = tlsValues.find(
    (value) => !TRUE_VALUES.has(value) && !FALSE_VALUES.has(value)
  );
  if (invalidTlsValue !== undefined) {
    throw new ConfigError(
      'Los parametros tls/ssl de MONGODB_URI solo aceptan true o false.'
    );
  }

  const allowInsecureTls = process.env.MONGO_ALLOW_INSECURE_TLS === 'true';
  if (allowInsecureTls && nodeEnv === 'production') {
    throw new ConfigError('MONGO_ALLOW_INSECURE_TLS no se permite en production.');
  }

  const explicitTls = tlsValues.some((value) => TRUE_VALUES.has(value));
  const explicitNoTls = tlsValues.some((value) => FALSE_VALUES.has(value));

  if (explicitTls && explicitNoTls) {
    throw new ConfigError('MONGODB_URI contiene valores tls/ssl contradictorios.');
  }

  const usesSrv = uri.startsWith('mongodb+srv://');
  const tls = explicitTls || (usesSrv && !explicitNoTls);

  if (tls) {
    return {
      tls: true,
      allowInsecureTls: false
    };
  }

  if (!allowInsecureTls) {
    throw new ConfigError(
      'MONGODB_URI debe usar TLS. Use mongodb+srv:// o agregue tls=true; '
      + 'solo development/test pueden usar MONGO_ALLOW_INSECURE_TLS=true con un servidor local.'
    );
  }

  return {
    tls: false,
    allowInsecureTls: true
  };
};

export const loadConfig = (options = {}) => {
  const envFile = options.envFile ?? process.env.MONGO_ENV_FILE ?? '.env';
  const envPath = path.resolve(process.cwd(), envFile);

  if (!fs.existsSync(envPath)) {
    throw new ConfigError(
      `No existe el archivo de entorno ${envFile}. Copie .env.example y complete los valores.`
    );
  }

  dotenv.config({ path: envPath, quiet: true, override: true });

  const nodeEnv = process.env.NODE_ENV ?? 'development';
  if (!ENVIRONMENTS.has(nodeEnv)) {
    throw new ConfigError('NODE_ENV debe ser development, production o test.');
  }

  const uri = process.env.MONGODB_URI;
  if (!uri || (!uri.startsWith('mongodb://') && !uri.startsWith('mongodb+srv://'))) {
    throw new ConfigError('MONGODB_URI debe iniciar con mongodb:// o mongodb+srv://.');
  }

  const dbName = process.env.MONGODB_DB;
  if (!dbName || !DB_NAME_PATTERN.test(dbName)) {
    throw new ConfigError('MONGODB_DB es obligatorio y solo admite letras, numeros, _ y -.');
  }

  if (nodeEnv === 'development' && dbName !== 'virgo_dev') {
    throw new ConfigError('En development, MONGODB_DB debe ser virgo_dev.');
  }

  if (nodeEnv === 'production' && dbName !== 'virgo') {
    throw new ConfigError('En production, MONGODB_DB debe ser virgo.');
  }

  if (nodeEnv === 'test' && !dbName.startsWith('virgo_test_')) {
    throw new ConfigError('En test, MONGODB_DB debe iniciar con virgo_test_.');
  }

  const tlsPolicy = resolveTlsPolicy(uri, nodeEnv);

  return {
    envFile: envPath,
    nodeEnv,
    uri,
    dbName,
    appName: process.env.MONGO_APP_NAME ?? 'VirGo-Database-Init',
    maxPoolSize: readPositiveInt('MONGO_MAX_POOL_SIZE', 10),
    serverSelectionTimeoutMS: readPositiveInt(
      'MONGO_SERVER_SELECTION_TIMEOUT_MS',
      10000
    ),
    ...tlsPolicy
  };
};

export const safeConfigSummary = (config) => ({
  envFile: config.envFile,
  nodeEnv: config.nodeEnv,
  dbName: config.dbName,
  appName: config.appName,
  maxPoolSize: config.maxPoolSize,
  serverSelectionTimeoutMS: config.serverSelectionTimeoutMS,
  tls: config.tls,
  allowInsecureTls: config.allowInsecureTls
});
