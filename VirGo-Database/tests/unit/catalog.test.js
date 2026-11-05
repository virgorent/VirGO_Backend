import test from 'node:test';
import assert from 'node:assert/strict';
import {
  COLLECTIONS,
  COLLECTION_NAMES,
  INDEXES
} from '../../src/catalog/index.js';

const expectedCollections = [
  'users',
  'passwordResets',
  'sessions',
  'paymentCards',
  'venues',
  'venueDates',
  'providers',
  'gigs',
  'reviews',
  'monthlyStats',
  'events',
  'eventGuests',
  'eventTasks',
  'quotes',
  'bookings',
  'counters',
  'favorites',
  'conversations',
  'messages',
  'notices',
  'assistantMessages',
  'contactMessages'
];

const rootSchema = (validator) => {
  if (validator.$jsonSchema) return validator.$jsonSchema;
  return validator.$and.find((item) => item.$jsonSchema).$jsonSchema;
};

const findNamedProperties = (node, name, found = []) => {
  if (!node || typeof node !== 'object') return found;

  if (node.properties && node.properties[name]) {
    found.push(node.properties[name]);
  }

  for (const value of Object.values(node)) {
    if (Array.isArray(value)) {
      for (const item of value) findNamedProperties(item, name, found);
    } else if (value && typeof value === 'object') {
      findNamedProperties(value, name, found);
    }
  }

  return found;
};

test('declara exactamente las 22 colecciones aprobadas', () => {
  assert.deepEqual(COLLECTION_NAMES, expectedCollections);
  assert.equal(COLLECTIONS.length, 22);
});

test('no declara config, migraciones ni locks', () => {
  for (const forbidden of ['config', 'migrations', 'locks']) {
    assert.equal(COLLECTION_NAMES.includes(forbidden), false);
  }
});

test('todas las colecciones tienen un validador raiz', () => {
  for (const collection of COLLECTIONS) {
    const schema = rootSchema(collection.validator);
    assert.equal(schema.bsonType, 'object', collection.name);
    assert.ok(Array.isArray(schema.required), collection.name);
  }
});

test('los nombres de indices son unicos por coleccion', () => {
  for (const collection of COLLECTIONS) {
    const names = collection.indexes.map((index) => index.name);
    assert.equal(new Set(names).size, names.length, collection.name);
  }
});

test('users y paymentCards no permiten campos adicionales', () => {
  const users = rootSchema(COLLECTIONS.find((item) => item.name === 'users').validator);
  const cards = rootSchema(
    COLLECTIONS.find((item) => item.name === 'paymentCards').validator
  );

  assert.equal(users.additionalProperties, false);
  assert.equal(cards.additionalProperties, false);
  assert.ok(users.required.includes('_id'));
  assert.ok(cards.required.includes('_id'));
});

test('quotes exige updatedAt', () => {
  const quotes = rootSchema(
    COLLECTIONS.find((item) => item.name === 'quotes').validator
  );
  assert.ok(quotes.required.includes('updatedAt'));
  assert.equal(quotes.properties.updatedAt.bsonType, 'date');
});

test('reviews exige bookingId solo para venues', () => {
  const reviews = COLLECTIONS.find((item) => item.name === 'reviews').validator;
  const conditional = reviews.$and.find((item) => item.$or);
  assert.ok(conditional);
  assert.equal(conditional.$or.length, 2);
  assert.equal(conditional.$or[0].targetType, 'venue');
  assert.equal(conditional.$or[0].bookingId.$exists, true);
  assert.equal(conditional.$or[1].targetType, 'provider');
});

test('los indices criticos existen', () => {
  const byName = (name) => INDEXES[name];
  assert.ok(byName('reviews').some((index) => (
    index.name === 'targetType_1_targetId_1_authorId_1_unique'
    && index.options.unique === true
  )));
  assert.ok(byName('conversations').some((index) => (
    index.name === 'clientId_1_targetType_1_targetId_1_unique'
    && index.options.unique === true
  )));
  assert.ok(byName('paymentCards').some((index) => (
    index.name === 'userId_1_default_unique'
    && index.options.unique === true
    && index.options.partialFilterExpression.isDefault === true
  )));
  assert.ok(byName('venues').some((index) => (
    index.key.location === '2dsphere' && index.key.status === 1
  )));
  assert.ok(byName('bookings').some((index) => (
    index.name === 'folio_1_unique' && index.options.unique === true
  )));
});

test('los TTL solo se aplican a las colecciones aprobadas', () => {
  const ttlCollections = [];

  for (const collection of COLLECTIONS) {
    for (const index of collection.indexes) {
      if (index.options.expireAfterSeconds !== undefined) {
        ttlCollections.push(collection.name);
        assert.equal(index.options.expireAfterSeconds, 0, collection.name);
      }
    }
  }

  assert.deepEqual(
    [...new Set(ttlCollections)].sort(),
    ['assistantMessages', 'notices', 'passwordResets', 'sessions']
  );
  assert.equal(INDEXES.venueDates.some((index) => index.options.expireAfterSeconds !== undefined), false);
  assert.equal(INDEXES.gigs.some((index) => index.options.expireAfterSeconds !== undefined), false);
  assert.equal(INDEXES.contactMessages.some((index) => index.options.expireAfterSeconds !== undefined), false);
});

test('no agrega maxItems arbitrarios a arreglos variables', () => {
  const arrays = [
    'photos',
    'gallery',
    'collaborators',
    'vendors',
    'reminders',
    'payments',
    'extras',
    'externalPayments',
    'requestedServices',
    'zones',
    'amenities',
    'services',
    'packages',
    'allies'
  ];

  for (const collection of COLLECTIONS) {
    for (const arrayName of arrays) {
      for (const schema of findNamedProperties(collection.validator, arrayName)) {
        assert.equal(schema.maxItems, undefined, `${collection.name}.${arrayName}`);
      }
    }
  }
});

test('counters no tiene indices adicionales', () => {
  assert.deepEqual(INDEXES.counters, []);
});
