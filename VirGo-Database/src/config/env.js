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

const readPositiveInt = (name, fallback) => {
  const raw = process.env[name];
  if (raw === undefined || raw === '') return fallback;
  const value = Number.parseInt(raw, 10);
  if (!Number.isInteger(value) || value <= 0) {
    throw new ConfigError(`${name} debe ser un entero positivo.`);
  }
  return value;
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
    )
  };
};

export const safeConfigSummary = (config) => ({
  envFile: config.envFile,
  nodeEnv: config.nodeEnv,
  dbName: config.dbName,
  appName: config.appName,
  maxPoolSize: config.maxPoolSize,
  serverSelectionTimeoutMS: config.serverSelectionTimeoutMS
});
