import test from 'node:test';
import assert from 'node:assert/strict';
import {
  describeIndex,
  indexesEquivalent
} from '../../src/init/plan.js';

test('considera equivalentes indices con el mismo key y opciones', () => {
  const expected = {
    name: 'email_1_unique',
    key: { email: 1 },
    options: { unique: true }
  };
  const actual = {
    name: 'email_1',
    key: { email: 1 },
    unique: true
  };

  assert.equal(indexesEquivalent(expected, actual), true);
});

test('detecta diferencias en unique, TTL, parcial y orden de key', () => {
  const expected = {
    name: 'userId_1_default_unique',
    key: { userId: 1 },
    options: {
      unique: true,
      partialFilterExpression: { isDefault: true }
    }
  };

  assert.equal(indexesEquivalent(expected, {
    name: 'x',
    key: { userId: 1 },
    unique: false,
    partialFilterExpression: { isDefault: true }
  }), false);

  assert.equal(indexesEquivalent(expected, {
    name: 'x',
    key: { userId: 1 },
    unique: true,
    partialFilterExpression: { isDefault: false }
  }), false);

  const ttl = {
    name: 'purgeAt_ttl',
    key: { purgeAt: 1 },
    options: { expireAfterSeconds: 0 }
  };
  assert.equal(indexesEquivalent(ttl, {
    name: 'purgeAt_ttl',
    key: { purgeAt: 1 },
    expireAfterSeconds: 30
  }), false);

  const compound = {
    name: 'compound',
    key: { a: 1, b: -1 },
    options: {}
  };
  assert.equal(indexesEquivalent(compound, {
    name: 'compound',
    key: { b: -1, a: 1 }
  }), false);
});

test('ignora opciones administradas por MongoDB', () => {
  const expected = {
    name: 'location_2dsphere_status_1',
    key: { location: '2dsphere', status: 1 },
    options: {}
  };
  const actual = {
    name: 'location_2dsphere_status_1',
    key: { location: '2dsphere', status: 1 },
    v: 2,
    '2dsphereIndexVersion': 3
  };

  assert.equal(indexesEquivalent(expected, actual), true);
});

test('describeIndex incluye las opciones relevantes', () => {
  const text = describeIndex({
    name: 'userId_1_default_unique',
    key: { userId: 1 },
    options: {
      unique: true,
      partialFilterExpression: { isDefault: true }
    }
  });

  assert.match(text, /userId_1_default_unique/);
  assert.match(text, /unique/);
  assert.match(text, /partialFilterExpression/);
});
