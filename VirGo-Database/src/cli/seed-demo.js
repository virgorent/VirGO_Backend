import { applyEnvFileArg, parseCliArgs } from '../util/args.js';

const usage = `Uso: node src/cli/seed-demo.js --env-file .env.development --mode <plan|seed|purge> [opciones]

Opciones:
  --env-file <archivo>       Archivo de variables de entorno.
  --mode <plan|seed|purge>   plan no escribe; seed y purge requieren --yes.
  --yes                      Confirma la escritura o la limpieza selectiva.
  --json                     Imprime el resultado en JSON.
  --help                     Muestra esta ayuda.

El seed solo se permite en development o test. Nunca se ejecuta en production.
La limpieza elimina unicamente los _id deterministas del DEMO.
`;

const args = parseCliArgs(process.argv.slice(2));
applyEnvFileArg(args);

if (args.help) {
  process.stdout.write(`${usage}\n`);
  process.exit(0);
}

const modes = new Set(['plan', 'seed', 'purge']);
if (!modes.has(args.mode)) {
  process.stderr.write(`Modo invalido: ${args.mode}\n`);
  process.exit(2);
}

if (args.mode !== 'plan' && !args.yes) {
  process.stderr.write(
    'No se aplicaron cambios. Ejecute de nuevo con --yes para confirmar.\n'
  );
  process.exit(2);
}

const { loadConfig } = await import('../config/env.js');
const {
  assertDemoEnvironment,
  buildDemoDocuments,
  countDemoDocuments,
  purgeDemo,
  seedDemo
} = await import('../seed/demo.js');
const { closeDatabase, connectDatabase } = await import('../connection/client.js');

let client;
let exitCode = 0;

try {
  const config = loadConfig();
  assertDemoEnvironment(config);

  const documents = buildDemoDocuments();

  if (args.mode === 'plan') {
    const details = [...documents.entries()]
      .filter(([, docs]) => docs.length > 0)
      .map(([collection, docs]) => ({
        collection,
        documents: docs.length
      }));
    const result = {
      environment: config.nodeEnv,
      database: config.dbName,
      collections: details.length,
      documents: countDemoDocuments(documents),
      details
    };

    if (args.json) {
      process.stdout.write(`${JSON.stringify(result, null, 2)}\n`);
    } else {
      process.stdout.write(
        `Entorno: ${config.nodeEnv}\n`
        + `Base de datos: ${config.dbName}\n`
        + `Colecciones DEMO: ${result.collections}\n`
        + `Documentos DEMO: ${result.documents}\n`
      );
      for (const item of details) {
        process.stdout.write(`  ${item.collection}: ${item.documents}\n`);
      }
    }
  } else {
    const connection = await connectDatabase(config);
    client = connection.client;

    const result = args.mode === 'seed'
      ? await seedDemo(connection.db, documents)
      : await purgeDemo(connection.db, documents);

    if (args.json) {
      process.stdout.write(`${JSON.stringify(result, null, 2)}\n`);
    } else if (args.mode === 'seed') {
      process.stdout.write(
        `Entorno: ${config.nodeEnv}\n`
        + `Base de datos: ${config.dbName}\n`
        + `Colecciones sembradas: ${result.collections}\n`
        + `Documentos sembrados: ${result.documents}\n`
      );
    } else {
      process.stdout.write(
        `Entorno: ${config.nodeEnv}\n`
        + `Base de datos: ${config.dbName}\n`
        + `Colecciones revisadas: ${result.collections}\n`
        + `Documentos DEMO eliminados: ${result.deleted}\n`
      );
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
