import { applyEnvFileArg, parseCliArgs } from '../util/args.js';

const usage = `Uso: node src/cli/verify.js --env-file <archivo> [--json]\n`;

const args = parseCliArgs(process.argv.slice(2));
applyEnvFileArg(args);

if (args.help) {
  process.stdout.write(`${usage}\n`);
  process.exit(0);
}

const { loadConfig } = await import('../config/env.js');
const {
  formatVerification,
  verifyInitialization
} = await import('../init/index.js');
const { closeDatabase, connectDatabase } = await import('../connection/client.js');

let client;
let exitCode = 0;

try {
  const config = loadConfig();
  const connection = await connectDatabase(config);
  client = connection.client;

  const result = await verifyInitialization(connection.db);
  if (args.json) {
    process.stdout.write(`${JSON.stringify(result, null, 2)}\n`);
  } else {
    process.stdout.write(
      `Entorno: ${config.nodeEnv}\n${formatVerification(result)}\n`
    );
  }
  exitCode = result.ok ? 0 : 1;
} catch (error) {
  const message = error instanceof Error ? error.message : String(error);
  process.stderr.write(`Error: ${message}\n`);
  exitCode = 1;
} finally {
  await closeDatabase(client);
}

process.exit(exitCode);
