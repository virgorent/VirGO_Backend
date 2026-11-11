import { applyEnvFileArg, parseCliArgs } from '../util/args.js';

const usage = `Uso: node src/cli/init.js --env-file .env.development --mode <plan|validate|apply> [opciones]

Opciones:
  --env-file <archivo>       Archivo de variables de entorno.
  --mode <plan|validate|apply>
  --yes                      Confirma la aplicacion de cambios.
  --confirm-production       Requerido para aplicar en production.
  --json                     Imprime el plan en JSON.
  --help                     Muestra esta ayuda.

Modos:
  plan       Solo muestra las operaciones que se realizarian.
  validate   Verifica el estado y falla si hay diferencias.
  apply      Aplica el plan, pero solo con --yes.
`;

const args = parseCliArgs(process.argv.slice(2));
applyEnvFileArg(args);

if (args.help) {
  process.stdout.write(`${usage}\n`);
  process.exit(0);
}

const modes = new Set(['plan', 'validate', 'apply']);
if (!modes.has(args.mode)) {
  process.stderr.write(`Modo invalido: ${args.mode}\n`);
  process.exit(2);
}

const {
  ConfigError,
  loadConfig
} = await import('../config/env.js');
const {
  applyPlan,
  buildPlan,
  formatPlan,
  formatVerification,
  verifyInitialization
} = await import('../init/index.js');
const {
  closeDatabase,
  connectDatabase
} = await import('../connection/client.js');

let client;
let exitCode = 0;

try {
  const config = loadConfig();

  if (
    args.mode === 'apply'
    && config.nodeEnv === 'production'
    && !args['confirm-production']
  ) {
    throw new ConfigError(
      'Para aplicar en production se requiere --confirm-production.'
    );
  }

  process.stdout.write(
    `Entorno: ${config.nodeEnv}\n`
    + `Base de datos objetivo: ${config.dbName}\n`
  );

  const connection = await connectDatabase(config);
  client = connection.client;

  if (args.mode === 'validate') {
    const result = await verifyInitialization(connection.db);
    if (args.json) {
      process.stdout.write(`${JSON.stringify(result, null, 2)}\n`);
    } else {
      process.stdout.write(`${formatVerification(result)}\n`);
    }
    exitCode = result.ok ? 0 : 1;
  } else {
    const plan = await buildPlan(connection.db);

    if (args.json) {
      process.stdout.write(`${JSON.stringify(plan, null, 2)}\n`);
    } else {
      process.stdout.write(`${formatPlan(plan)}\n`);
    }

    if (plan.errors.length > 0) {
      exitCode = 1;
    } else if (args.mode === 'apply' && plan.hasChanges && !args.yes) {
      process.stdout.write(
        '\nNo se aplicaron cambios. Ejecute de nuevo con --yes para confirmar.\n'
      );
      exitCode = 2;
    } else if (args.mode === 'apply' && !plan.hasChanges) {
      process.stdout.write('\nNo hay cambios por aplicar.\n');
    } else if (args.mode === 'apply') {
      const results = await applyPlan(connection.db, plan);
      process.stdout.write(`\nOperaciones aplicadas: ${results.length}\n`);

      const verification = await verifyInitialization(connection.db);
      process.stdout.write(`\n${formatVerification(verification)}\n`);
      exitCode = verification.ok ? 0 : 1;
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
