import test from 'node:test';
import assert from 'node:assert/strict';
import { COLLECTION_NAMES } from '../../src/catalog/index.js';
import {
  applyPlan,
  buildPlan,
  verifyInitialization
} from '../../src/init/index.js';
import {
  booking,
  contactMessage,
  conversation,
  event,
  id,
  money,
  paymentCard,
  provider,
  quote,
  review,
  user,
  venue,
  venueDate
} from '../helpers/fixtures.js';
import { createTestDb, dropTestDb } from '../helpers/testDb.js';

const connection = await createTestDb();

const validationFailure = (promise) => assert.rejects(
  promise,
  (error) => error?.code === 121
);

const duplicateKey = (promise) => assert.rejects(
  promise,
  (error) => error?.code === 11000
);

test('integracion contra MongoDB local', {
  skip: connection ? false : 'MongoDB local no disponible en MONGODB_TEST_URI'
}, async (t) => {
  const db = connection.db;

  t.after(async () => {
    await dropTestDb(connection);
  });

  await t.test('crea las 22 colecciones y sus indices', async () => {
    const plan = await buildPlan(db);
    assert.equal(plan.errors.length, 0);
    assert.equal(plan.summary.collections, 22);

    await applyPlan(db, plan);

    const verification = await verifyInitialization(db);
    assert.equal(verification.ok, true, JSON.stringify(verification, null, 2));

    const existing = (await db.listCollections({}, { nameOnly: true }).toArray())
      .map((item) => item.name)
      .sort();
    assert.deepEqual(existing, [...COLLECTION_NAMES].sort());
  });

  await t.test('la segunda ejecucion es idempotente', async () => {
    const plan = await buildPlan(db);
    assert.equal(plan.hasChanges, false);
    assert.equal(plan.drift, false);
    assert.deepEqual(await applyPlan(db, plan), []);
  });

  await t.test('users valida campos y email unico', async () => {
    const users = db.collection('users');
    const valid = user();
    await users.insertOne(valid);

    await validationFailure(users.insertOne(user({
      email: 'extra@example.com',
      cvv: '123'
    })));

    await duplicateKey(users.insertOne(user({
      email: valid.email
    })));
  });

  await t.test('paymentCards valida campos y tarjeta predeterminada unica', async () => {
    const cards = db.collection('paymentCards');
    const userId = id();

    await cards.insertOne(paymentCard({ userId, isDefault: true }));
    await duplicateKey(cards.insertOne(paymentCard({ userId, isDefault: true })));
    await cards.insertOne(paymentCard({ userId, isDefault: false, last4: '1111' }));

    await validationFailure(cards.insertOne(paymentCard({
      userId,
      isDefault: false,
      cvv: '123'
    })));
  });

  await t.test('venues valida GeoJSON, estado y campos obligatorios', async () => {
    const venues = db.collection('venues');
    await venues.insertOne(venue());

    await validationFailure(venues.insertOne(venue({
      location: {
        type: 'Point',
        coordinates: [-99.1, 19.4, 0]
      }
    })));

    await validationFailure(venues.insertOne(venue({
      status: 'invalido'
    })));

    await validationFailure(venues.insertOne(venue({
      description: 'corta'
    })));
  });

  await t.test('quotes exige updatedAt y target valido', async () => {
    const quotes = db.collection('quotes');
    await quotes.insertOne(quote());

    const missingUpdatedAt = quote();
    delete missingUpdatedAt.updatedAt;
    await validationFailure(quotes.insertOne(missingUpdatedAt));

    await validationFailure(quotes.insertOne(quote({
      target: {
        type: 'otro',
        id: id(),
        name: 'Destino'
      }
    })));
  });

  await t.test('reviews exige bookingId solo para venues y evita duplicados', async () => {
    const reviews = db.collection('reviews');
    const venueId = id();
    const providerId = id();
    const authorId = id();

    const providerReview = review({
      targetType: 'provider',
      targetId: providerId,
      authorId
    });
    delete providerReview.bookingId;
    await reviews.insertOne(providerReview);

    const missingBooking = review({
      targetType: 'venue',
      targetId: venueId,
      authorId: id()
    });
    delete missingBooking.bookingId;
    await validationFailure(reviews.insertOne(missingBooking));

    const venueReview = review({
      targetType: 'venue',
      targetId: venueId,
      authorId
    });
    await reviews.insertOne(venueReview);

    await duplicateKey(reviews.insertOne(review({
      targetType: 'venue',
      targetId: venueId,
      authorId
    })));
  });

  await t.test('bookings exige folio unico, Decimal128 y estado valido', async () => {
    const bookings = db.collection('bookings');
    const valid = booking({ folio: 'VG-2026-0100' });
    await bookings.insertOne(valid);

    await duplicateKey(bookings.insertOne(booking({
      folio: valid.folio
    })));

    await validationFailure(bookings.insertOne(booking({
      folio: 'VG-2026-0101',
      status: 'otro'
    })));

    await validationFailure(bookings.insertOne(booking({
      folio: 'VG-2026-0102',
      total: 6000
    })));
  });

  await t.test('conversations evita duplicados por cliente y destino', async () => {
    const conversations = db.collection('conversations');
    const clientId = id();
    const target = {
      type: 'venue',
      id: id(),
      name: 'Salon Central'
    };

    await conversations.insertOne(conversation({ clientId, target }));
    await duplicateKey(conversations.insertOne(conversation({ clientId, target })));
  });

  await t.test('venueDates evita duplicar fecha por venue', async () => {
    const dates = db.collection('venueDates');
    const doc = venueDate();
    await dates.insertOne(doc);
    await duplicateKey(dates.insertOne(venueDate({
      venueId: doc.venueId,
      date: doc.date
    })));
  });

  await t.test('events acepta budgetPlan dinamico solo con Decimal128', async () => {
    const events = db.collection('events');
    await events.insertOne(event());

    await validationFailure(events.insertOne(event({
      budgetPlan: {
        salon: 50000
      }
    })));
  });

  await t.test('providers valida userId unico y gallery sin maxItems', async () => {
    const providers = db.collection('providers');
    const userId = id();
    await providers.insertOne(provider({ userId }));

    await duplicateKey(providers.insertOne(provider({ userId })));

    const manyPhotos = Array.from({ length: 30 }, (_, index) => ({
      url: `https://example.com/${index}.jpg`,
      order: index
    }));
    await providers.insertOne(provider({
      userId: id(),
      gallery: manyPhotos
    }));
  });

  await t.test('contactMessages no tiene TTL', async () => {
    const messages = db.collection('contactMessages');
    await messages.insertOne(contactMessage());

    const indexes = await messages.listIndexes().toArray();
    assert.equal(
      indexes.some((index) => index.expireAfterSeconds !== undefined),
      false
    );
  });

  await t.test('mantiene los TTL aprobados', async () => {
    for (const collectionName of ['passwordResets', 'sessions', 'notices', 'assistantMessages']) {
      const indexes = await db.collection(collectionName).listIndexes().toArray();
      assert.ok(
        indexes.some((index) => index.expireAfterSeconds === 0),
        collectionName
      );
    }

    for (const collectionName of ['venueDates', 'gigs']) {
      const indexes = await db.collection(collectionName).listIndexes().toArray();
      assert.equal(
        indexes.some((index) => index.expireAfterSeconds !== undefined),
        false,
        collectionName
      );
    }
  });

  await t.test('verify detecta una coleccion faltante sin modificarla', async () => {
    await db.collection('favorites').drop();

    const result = await verifyInitialization(db);
    assert.equal(result.ok, false);
    assert.ok(result.missingCollections.includes('favorites'));

    const plan = await buildPlan(db);
    assert.ok(plan.actions.some((action) => (
      action.type === 'createCollection' && action.collection === 'favorites'
    )));

    const exists = await db.listCollections({ name: 'favorites' }).toArray();
    assert.equal(exists.length, 0);
  });

  await t.test('restaura el estado esperado', async () => {
    const plan = await buildPlan(db);
    await applyPlan(db, plan);
    const result = await verifyInitialization(db);
    assert.equal(result.ok, true);
  });

  await t.test('acepta un indice equivalente creado con otro nombre', async () => {
    const users = db.collection('users');
    await users.dropIndex('email_1_unique');
    await users.createIndex({ email: 1 }, { unique: true, name: 'email_1' });

    const plan = await buildPlan(db);
    assert.equal(plan.errors.length, 0);
    assert.equal(
      plan.actions.some((action) => (
        action.type === 'createIndex' && action.collection === 'users'
      )),
      false
    );

    const verification = await verifyInitialization(db);
    assert.equal(verification.ok, true);

    await users.dropIndex('email_1');
    await applyPlan(db, await buildPlan(db));
    assert.equal((await verifyInitialization(db)).ok, true);
  });

  await t.test('bloquea un indice con el mismo nombre y distinta definicion', async () => {
    const users = db.collection('users');
    await users.dropIndex('email_1_unique');
    await users.createIndex({ email: 1 }, { name: 'email_1_unique' });

    const plan = await buildPlan(db);
    assert.equal(plan.errors.length, 1);
    assert.equal(plan.errors[0].type, 'indexConflict');
    await assert.rejects(
      () => applyPlan(db, plan),
      /conflictos de indices/
    );

    await users.dropIndex('email_1_unique');
    await applyPlan(db, await buildPlan(db));
    assert.equal((await verifyInitialization(db)).ok, true);
  });

  await t.test('mantiene el dinero como Decimal128', async () => {
    const bookingDoc = await db.collection('bookings').findOne({
      folio: 'VG-2026-0100'
    });
    assert.equal(bookingDoc.total._bsontype, 'Decimal128');
    assert.equal(bookingDoc.total.toString(), money(6000).toString());
  });
});
