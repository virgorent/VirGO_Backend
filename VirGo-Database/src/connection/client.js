import { MongoClient } from 'mongodb';

export const connectDatabase = async (config) => {
  if (config.tls !== true && config.allowInsecureTls !== true) {
    throw new Error('La conexion MongoDB requiere TLS.');
  }

  const client = new MongoClient(config.uri, {
    appName: config.appName,
    maxPoolSize: config.maxPoolSize,
    serverSelectionTimeoutMS: config.serverSelectionTimeoutMS,
    tls: config.tls
  });

  await client.connect();
  return {
    client,
    db: client.db(config.dbName)
  };
};

export const closeDatabase = async (client) => {
  if (!client) return;
  try {
    await client.close();
  } catch {
    return;
  }
};
