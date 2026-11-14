import test from 'node:test';
import assert from 'node:assert/strict';
import { parseCliArgs } from '../../src/util/args.js';

test('parseCliArgs acepta las rutas de aprobacion y plan', () => {
  const args = parseCliArgs([
    '--env-file', '.env.production',
    '--mode', 'apply',
    '--ttl-approval-file', 'ttl.json',
    '--waiver-file', 'waiver.json',
    '--plan-file', 'plan.json',
    '--yes',
    '--confirm-production',
    '--json'
  ]);

  assert.equal(args['env-file'], '.env.production');
  assert.equal(args.mode, 'apply');
  assert.equal(args['ttl-approval-file'], 'ttl.json');
  assert.equal(args['waiver-file'], 'waiver.json');
  assert.equal(args['plan-file'], 'plan.json');
  assert.equal(args.yes, true);
  assert.equal(args['confirm-production'], true);
  assert.equal(args.json, true);
});

test('parseCliArgs conserva los valores por defecto', () => {
  const args = parseCliArgs([]);
  assert.equal(args.mode, 'plan');
  assert.equal(args.yes, false);
  assert.equal(args.json, false);
  assert.equal(args.help, false);
});
