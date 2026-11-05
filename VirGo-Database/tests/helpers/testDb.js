import { randomUUID } from 'node:crypto';
import { MongoClient } from 'mongodb';

export const TEST_URI = process.env.MONGODB_TEST_URI ?? 'mongodb://127.0.0.1:27018';

export const createTestDb = async () => {
  const client = new MongoClient(TEST_URI, {
    serverSelectionTimeoutMS: 3000
  });

  try {
    await client.connect();
  } catch {
    await client.close().catch(() => {});
    return null;
  }

  const dbName = `virgo_test_${process.pid}_${Date.now()}_${randomUUID().slice(0, 8)}`;
  return {
    client,
    db: client.db(dbName),
    dbName
  };
};

export const dropTestDb = async (connection) => {
  if (!connection) return;
  if (!connection.dbName.startsWith('virgo_test_')) {
    throw new Error('Se intento eliminar una base que no es de prueba.');
  }
  await connection.client.db(connection.dbName).dropDatabase();
  await connection.client.close();
};
