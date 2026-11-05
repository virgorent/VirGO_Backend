import { applyEnvFileArg, parseCliArgs } from '../util/args.js';

const usage = `Uso: node src/cli/inspect.js --env-file <archivo> [--json]\n`;

const args = parseCliArgs(process.argv.slice(2));
applyEnvFileArg(args);

if (args.help) {
  process.stdout.write(`${usage}\n`);
  process.exit(0);
}

const { loadConfig } = await import('../config/env.js');
const { closeDatabase, connectDatabase } = await import('../connection/client.js');

let client;
let exitCode = 0;

try {
  const config = loadConfig();
  const connection = await connectDatabase(config);
  client = connection.client;

  const collections = await connection.db
    .listCollections({}, { nameOnly: true })
    .toArray();
  const names = collections.map((collection) => collection.name).sort();
  const result = [];

  for (const name of names) {
    const indexes = await connection.db.collection(name).listIndexes().toArray();
    result.push({
      name,
      indexes: indexes.map((index) => ({
        name: index.name,
        key: index.key,
        unique: index.unique === true,
        expireAfterSeconds: index.expireAfterSeconds,
        partialFilterExpression: index.partialFilterExpression
      }))
    });
  }

  if (args.json) {
    process.stdout.write(`${JSON.stringify({
      environment: config.nodeEnv,
      database: config.dbName,
      collections: result
    }, null, 2)}\n`);
  } else {
    process.stdout.write(
      `Entorno: ${config.nodeEnv}\nBase de datos: ${config.dbName}\n`
      + `Colecciones existentes: ${result.length}\n`
    );
    for (const collection of result) {
      process.stdout.write(`\n[${collection.name}]\n`);
      for (const index of collection.indexes) {
        process.stdout.write(
          `  ${index.name} key=${JSON.stringify(index.key)}`
          + `${index.unique ? ' unique' : ''}`
          + `${index.expireAfterSeconds !== undefined
            ? ` expireAfterSeconds=${index.expireAfterSeconds}`
            : ''}\n`
        );
      }
    }
  }
} catch (error) {
  const message = error instanceof Error ? error.message : String(error);
  process.stderr.write(`Error: ${message}\n`);
  exitCode = 1;
} finally {
  await closeDatabase(client);
}

process.exit(exitCode);
