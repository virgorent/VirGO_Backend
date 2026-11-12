import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import {
  loadConfig,
  safeConfigSummary
} from '../../src/config/env.js';

const ENV_KEYS = [
  'NODE_ENV',
  'MONGODB_URI',
  'MONGODB_DB',
  'MONGO_APP_NAME',
  'MONGO_MAX_POOL_SIZE',
  'MONGO_SERVER_SELECTION_TIMEOUT_MS',
  'MONGO_ALLOW_INSECURE_TLS'
];

const withConfig = (lines, callback) => {
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'virgo-config-'));
  const envFile = path.join(directory, '.env');
  fs.writeFileSync(envFile, `${lines.join('\n')}\n`);

  const saved = Object.fromEntries(
    ENV_KEYS.map((key) => [key, process.env[key]])
  );

  try {
    return callback(envFile);
  } finally {
    for (const key of ENV_KEYS) {
      if (saved[key] === undefined) {
        delete process.env[key];
      } else {
        process.env[key] = saved[key];
      }
    }
    fs.rmSync(directory, { recursive: true, force: true });
  }
};

const developmentEnv = (uri, extra = []) => [
  'NODE_ENV=development',
  `MONGODB_URI=${uri}`,
  'MONGODB_DB=virgo_dev',
  ...extra
];

test('acepta mongodb+srv como TLS implicito', () => {
  const config = withConfig(
    developmentEnv(
      'mongodb+srv://USER:PASSWORD@cluster.mongodb.net/?retryWrites=true'
    ),
    (envFile) => loadConfig({ envFile })
  );

  assert.equal(config.tls, true);
  assert.equal(config.allowInsecureTls, false);
});

test('acepta tls=true y ssl=1 en mongodb://', () => {
  for (const uri of [
    'mongodb://USER:PASSWORD@127.0.0.1:27018/?tls=true',
    'mongodb://USER:PASSWORD@127.0.0.1:27018/?ssl=1'
  ]) {
    const config = withConfig(
      developmentEnv(uri),
      (envFile) => loadConfig({ envFile })
    );
    assert.equal(config.tls, true);
  }
});

test('rechaza mongodb:// sin TLS en development', () => {
  assert.throws(
    () => withConfig(
      developmentEnv('mongodb://127.0.0.1:27018/?replicaSet=rs0'),
      (envFile) => loadConfig({ envFile })
    ),
    /debe usar TLS/
  );
});

test('rechaza TLS desactivado o parametros inseguros', () => {
  const uris = [
    'mongodb://127.0.0.1:27018/?tls=false',
    'mongodb://127.0.0.1:27018/?ssl=false',
    'mongodb://127.0.0.1:27018/?tls=true&tlsInsecure=true',
    'mongodb://127.0.0.1:27018/?tls=true&tlsAllowInvalidCertificates=true',
    'mongodb://127.0.0.1:27018/?tls=true&tlsAllowInvalidHostnames=true',
    'mongodb://127.0.0.1:27018/?tls=true&sslInvalidHostNameAllowed=true'
  ];

  for (const uri of uris) {
    assert.throws(
      () => withConfig(
        developmentEnv(uri),
        (envFile) => loadConfig({ envFile })
      ),
      /TLS/
    );
  }
});

test('rechaza valores tls contradictorios o invalidos', () => {
  for (const uri of [
    'mongodb://127.0.0.1:27018/?tls=true&ssl=false',
    'mongodb://127.0.0.1:27018/?tls=maybe'
  ]) {
    assert.throws(
      () => withConfig(
        developmentEnv(uri),
        (envFile) => loadConfig({ envFile })
      ),
      /tls|ssl/
    );
  }
});

test('permite conexion local sin TLS solo con override explicito', () => {
  const config = withConfig(
    developmentEnv(
      'mongodb://127.0.0.1:27018/?replicaSet=rs0',
      ['MONGO_ALLOW_INSECURE_TLS=true']
    ),
    (envFile) => loadConfig({ envFile })
  );

  assert.equal(config.tls, false);
  assert.equal(config.allowInsecureTls, true);
});

test('rechaza el override inseguro en production', () => {
  assert.throws(
    () => withConfig(
      [
        'NODE_ENV=production',
        'MONGODB_URI=mongodb+srv://USER:PASSWORD@cluster.mongodb.net/',
        'MONGODB_DB=virgo',
        'MONGO_ALLOW_INSECURE_TLS=true'
      ],
      (envFile) => loadConfig({ envFile })
    ),
    /MONGO_ALLOW_INSECURE_TLS no se permite en production/
  );
});

test('el resumen seguro no expone la URI', () => {
  const config = withConfig(
    developmentEnv(
      'mongodb+srv://secret-user:secret-password@cluster.mongodb.net/'
    ),
    (envFile) => loadConfig({ envFile })
  );

  const summary = JSON.stringify(safeConfigSummary(config));
  assert.equal(summary.includes('secret-user'), false);
  assert.equal(summary.includes('secret-password'), false);
  assert.equal(summary.includes('mongodb'), false);
  assert.equal(summary.includes('"tls":true'), true);
});
