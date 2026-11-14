import { applyEnvFileArg, parseCliArgs } from '../util/args.js';

const usage = `Uso: node src/cli/verify.js --env-file <archivo> [opciones]

Opciones:
  --env-file <archivo>       Archivo de variables de entorno.
  --ttl-approval-file <ruta> Aprobacion TTL especifica para production.
  --waiver-file <ruta>       Waiver de extraIndex o nameMismatch.
  --plan-file <ruta>         Plan previo; se rechaza si la huella no coincide.
  --json                     Imprime el resultado en JSON.
  --help                     Muestra esta ayuda.
`;

class PlanFileError extends Error {
  constructor(message) {
    super(message);
    this.name = 'PlanFileError';
  }
}

const args = parseCliArgs(process.argv.slice(2));
applyEnvFileArg(args);

if (args.help) {
  process.stdout.write(`${usage}\n`);
  process.exit(0);
}

const { ConfigError, loadConfig } = await import('../config/env.js');
const {
  ApprovalError,
  catalogFingerprint,
  clusterIdentityFromUri,
  formatVerification,
  loadApprovalFile,
  readJsonFile,
  verifyInitialization
} = await import('../init/index.js');
const { closeDatabase, connectDatabase } = await import('../connection/client.js');

const assertPlanFileCurrent = (filePath, plan) => {
  const document = readJsonFile(filePath);

  if (!document || typeof document !== 'object' || Array.isArray(document)) {
    throw new PlanFileError('El plan-file no contiene un objeto JSON valido.');
  }

  if (typeof document.planFingerprint !== 'string') {
    throw new PlanFileError('El plan-file no contiene planFingerprint.');
  }

  if (document.planFingerprint !== plan.planFingerprint) {
    throw new PlanFileError(
      'El plan-file no coincide con el estado actual. '
      + 'Recalcule el plan antes de continuar.'
    );
  }
};

let client;
let exitCode = 0;

try {
  const config = loadConfig();
  const clusterIdentity = clusterIdentityFromUri(config.uri);
  const approvalContext = {
    environment: config.nodeEnv,
    database: config.dbName,
    clusterIdentity,
    catalogFingerprint: catalogFingerprint()
  };
  const ttlApprovalDocument = args['ttl-approval-file']
    ? loadApprovalFile(
      args['ttl-approval-file'],
      'ttl-approval',
      approvalContext
    )
    : undefined;
  const waiverDocument = args['waiver-file']
    ? loadApprovalFile(
      args['waiver-file'],
      'index-waiver',
      approvalContext
    )
    : undefined;

  const connection = await connectDatabase(config);
  client = connection.client;

  const result = await verifyInitialization(
    connection.db,
    undefined,
    {
      environment: config.nodeEnv,
      clusterIdentity,
      ttlApprovalDocument,
      waiverDocument
    }
  );

  if (args['plan-file']) {
    assertPlanFileCurrent(args['plan-file'], result.plan);
  }

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

  if (error instanceof PlanFileError) {
    exitCode = 4;
  } else if (error instanceof ApprovalError || error instanceof ConfigError) {
    exitCode = 2;
  } else {
    exitCode = 1;
  }
} finally {
  await closeDatabase(client);
}

process.exit(exitCode);
