import test from 'node:test';
import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const projectRoot = path.resolve(
  path.dirname(fileURLToPath(import.meta.url)),
  '../..'
);

const runCli = (file, args = []) => spawnSync(
  process.execPath,
  [`src/cli/${file}`, ...args],
  {
    cwd: projectRoot,
    encoding: 'utf8',
    timeout: 10000
  }
);

test('init --help muestra los flags de guardas sin conectar', () => {
  const result = runCli('init.js', ['--help']);
  assert.equal(result.status, 0);
  assert.match(result.stdout, /--ttl-approval-file/);
  assert.match(result.stdout, /--waiver-file/);
  assert.match(result.stdout, /--plan-file/);
  assert.equal(result.stderr, '');
});

test('init rechaza un modo invalido con exit code 2', () => {
  const result = runCli('init.js', ['--mode', 'inventado']);
  assert.equal(result.status, 2);
  assert.match(result.stderr, /Modo invalido/);
});

test('verify --help muestra los flags de guardas sin conectar', () => {
  const result = runCli('verify.js', ['--help']);
  assert.equal(result.status, 0);
  assert.match(result.stdout, /--ttl-approval-file/);
  assert.match(result.stdout, /--waiver-file/);
  assert.match(result.stdout, /--plan-file/);
  assert.equal(result.stderr, '');
});
