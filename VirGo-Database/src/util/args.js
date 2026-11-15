import { parseArgs } from 'node:util';

export const parseCliArgs = (argv) => {
  const { values } = parseArgs({
    args: argv,
    allowPositionals: false,
    options: {
      'env-file': { type: 'string' },
      mode: { type: 'string', default: 'plan' },
      yes: { type: 'boolean', default: false },
      'confirm-production': { type: 'boolean', default: false },
      'ttl-approval-file': { type: 'string' },
      'waiver-file': { type: 'string' },
      'plan-file': { type: 'string' },
      json: { type: 'boolean', default: false },
      help: { type: 'boolean', short: 'h', default: false }
    }
  });

  return values;
};

export const applyEnvFileArg = (args) => {
  if (args['env-file']) {
    process.env.MONGO_ENV_FILE = args['env-file'];
  }
};
