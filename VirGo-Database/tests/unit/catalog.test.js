import test from 'node:test';
import assert from 'node:assert/strict';
import {
  COLLECTIONS,
  COLLECTION_NAMES,
  INDEXES
} from '../../src/catalog/index.js';
import {
  EVENT_TYPES,
  PROVIDER_CATEGORIES,
  ZONES
} from '../../src/catalog/enums.js';

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

test('usa los enums aprobados para zonas, aliados y eventos', () => {
  assert.deepEqual(ZONES, [
    'Juriquilla',
    'Centro',
    'El Refugio',
    'Santa Fe',
    'Antea',
    'Jurica',
    'Corregidora',
    'El Marqués'
  ]);
  assert.deepEqual(PROVIDER_CATEGORIES, [
    'Catering',
    'Música',
    'Fotografía',
    'Decoración',
    'Mobiliario'
  ]);
  assert.deepEqual(EVENT_TYPES, [
    'Boda',
    'XV años',
    'Cumpleaños',
    'Corporativo',
    'Otro'
  ]);

  const venues = rootSchema(
    COLLECTIONS.find((item) => item.name === 'venues').validator
  );
  const providers = rootSchema(
    COLLECTIONS.find((item) => item.name === 'providers').validator
  );
  const events = rootSchema(
    COLLECTIONS.find((item) => item.name === 'events').validator
  );
  const allies = findNamedProperties(venues, 'allies')[0];

  assert.deepEqual(venues.properties.zone.enum, ZONES);
  assert.deepEqual(allies.items.properties.category.enum, PROVIDER_CATEGORIES);
  assert.deepEqual(providers.properties.category.enum, PROVIDER_CATEGORIES);
  assert.deepEqual(events.properties.eventType.enum, EVENT_TYPES);
});

test('users, passwordResets y sessions cumplen el contrato actual', () => {
  const users = rootSchema(
    COLLECTIONS.find((item) => item.name === 'users').validator
  );
  const resets = rootSchema(
    COLLECTIONS.find((item) => item.name === 'passwordResets').validator
  );
  const sessions = rootSchema(
    COLLECTIONS.find((item) => item.name === 'sessions').validator
  );

  assert.ok(users.required.includes('failedLoginCount'));
  assert.ok(users.required.includes('unreadNotices'));
  assert.equal(users.properties.name.maxLength, 100);
  assert.equal(users.properties.email.maxLength, 150);
  assert.equal(users.properties.failedLoginCount.minimum, 0);
  assert.deepEqual(users.properties.lockoutEndsAt.bsonType, ['date', 'null']);

  assert.ok(resets.required.includes('codeHash'));
  assert.ok(resets.required.includes('attempts'));
  assert.equal(resets.properties.attempts.maximum, 5);
  assert.equal(resets.properties.userId.bsonType, 'string');

  assert.equal(sessions.properties.tokenHash.maxLength, 88);
  assert.equal(sessions.properties.device.maxLength, 100);
  assert.deepEqual(sessions.properties.revokedAt.bsonType, ['date', 'null']);
});

test('providers y venues reflejan el contrato actual', () => {
  const providers = rootSchema(
    COLLECTIONS.find((item) => item.name === 'providers').validator
  );
  const venues = rootSchema(
    COLLECTIONS.find((item) => item.name === 'venues').validator
  );

  assert.ok(providers.required.includes('pricing'));
  assert.ok(providers.required.includes('responseTime'));
  assert.equal(providers.required.includes('gallery'), false);
  assert.equal(providers.properties.basePrice, undefined);
  assert.deepEqual(providers.properties.pricing.enum, ['per_person', 'per_event']);
  assert.equal(providers.properties.responseTime.maxLength, 50);
  assert.equal(venues.required.includes('amenities'), false);
  assert.equal(venues.required.includes('allies'), false);
});

test('bookings define idempotencia por cliente y maxLength', () => {
  const bookings = rootSchema(
    COLLECTIONS.find((item) => item.name === 'bookings').validator
  );
  const index = INDEXES.bookings.find((item) => (
    item.name === 'clientId_1_idempotencyKey_1_unique'
  ));

  assert.equal(bookings.required.includes('idempotencyKey'), false);
  assert.equal(bookings.properties.idempotencyKey.maxLength, 100);
  assert.deepEqual(index.key, { clientId: 1, idempotencyKey: 1 });
  assert.equal(index.options.unique, true);
  assert.deepEqual(index.options.partialFilterExpression, {
    idempotencyKey: { $type: 'string' }
  });
});

test('monthlyStats usa _id determinista', () => {
  const monthly = rootSchema(
    COLLECTIONS.find((item) => item.name === 'monthlyStats').validator
  );
  assert.equal(monthly.properties._id.bsonType, 'string');
  assert.equal(
    monthly.properties._id.pattern,
    '^(venue|provider):[0-9a-fA-F-]{36}:[0-9]{4}-[0-9]{2}$'
  );
});

test('declara los conteos de indices aprobados', () => {
  const allIndexes = COLLECTIONS.flatMap((collection) => collection.indexes);
  assert.equal(allIndexes.length, 38);
  assert.equal(
    allIndexes.filter((index) => index.options.unique === true).length,
    12
  );
  assert.equal(
    allIndexes.filter((index) => index.options.partialFilterExpression).length,
    2
  );
  assert.equal(
    allIndexes.filter((index) => index.options.expireAfterSeconds !== undefined).length,
    4
  );
});
