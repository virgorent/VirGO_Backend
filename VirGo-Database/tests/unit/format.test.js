import test from 'node:test';
import assert from 'node:assert/strict';
import { formatPlan, formatVerification } from '../../src/init/format.js';

const plan = {
  database: 'virgo_dev',
  hasChanges: true,
  summary: {
    collections: 22,
    createCollections: 1,
    updateValidators: 1,
    createIndexes: 1,
    indexConflicts: 0,
    extraIndexes: 1
  },
  collections: [
    {
      name: 'users',
      actions: [
        { type: 'createCollection', collection: 'users' },
        {
          type: 'createIndex',
          collection: 'users',
          index: {
            name: 'email_1_unique',
            key: { email: 1 },
            options: { unique: true }
          }
        }
      ]
    },
    {
      name: 'venues',
      actions: [
        { type: 'collMod', collection: 'venues' }
      ]
    }
  ],
  warnings: [
    {
      collection: 'users',
      message: 'Existe un indice no declarado.'
    }
  ],
  errors: []
};

test('formatPlan muestra las operaciones exactas', () => {
  const text = formatPlan(plan);
  assert.match(text, /virgo_dev/);
  assert.match(text, /createCollection users/);
  assert.match(text, /createIndex users email_1_unique/);
  assert.match(text, /collMod venues/);
  assert.match(text, /Advertencias/);
});

test('formatVerification resume el estado', () => {
  const text = formatVerification({
    ok: false,
    plan: { database: 'virgo_dev' },
    missingCollections: ['users'],
    validatorDrift: ['venues'],
    missingIndexes: [{ collection: 'bookings', indexes: ['folio_1_unique'] }],
    mismatchedIndexes: [],
    extraIndexes: []
  });

  assert.match(text, /DRIFT/);
  assert.match(text, /Colecciones faltantes: users/);
  assert.match(text, /Validadores desactualizados: venues/);
  assert.match(text, /Indices faltantes en bookings: folio_1_unique/);
});
