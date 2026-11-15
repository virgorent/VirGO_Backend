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
  passwordReset,
  paymentCard,
  provider,
  quote,
  review,
  session,
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

    let secondaryIndexes = 0;
    for (const collectionName of COLLECTION_NAMES) {
      const indexes = await db.collection(collectionName).listIndexes().toArray();
      secondaryIndexes += indexes.filter((index) => index.name !== '_id_').length;
    }
    assert.equal(secondaryIndexes, 38);
  });

  await t.test('la segunda ejecucion es idempotente', async () => {
    const plan = await buildPlan(db);
    assert.equal(plan.hasChanges, false);
    assert.equal(plan.drift, false);
    assert.deepEqual(await applyPlan(db, plan), []);
  });

  await t.test('users valida campos, email unico y maxLength', async () => {
    const users = db.collection('users');
    const valid = user();
    await users.insertOne(valid);

    await validationFailure(users.insertOne(user({
      email: 'extra@example.com',
      cvv: '123'
    })));

    await validationFailure(users.insertOne(user({
      name: 'a'.repeat(101)
    })));

    await validationFailure(users.insertOne(user({
      failedLoginCount: -1
    })));

    await duplicateKey(users.insertOne(user({
      email: valid.email
    })));
  });

  await t.test('passwordResets usa codeHash, attempts y userId unico', async () => {
    const resets = db.collection('passwordResets');
    const userId = id();
    await resets.insertOne(passwordReset({ userId }));

    await duplicateKey(resets.insertOne(passwordReset({ userId })));
    await validationFailure(resets.insertOne(passwordReset({ attempts: 6 })));
  });

  await t.test('sessions acepta device y revokedAt', async () => {
    const sessions = db.collection('sessions');
    await sessions.insertOne(session({
      device: 'Android 14 · Pixel 7',
      revokedAt: null
    }));

    await validationFailure(sessions.insertOne(session({
      tokenHash: 'a'.repeat(89)
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

  await t.test('venues valida GeoJSON, estado, zona y maxLength', async () => {
    const venues = db.collection('venues');
    await venues.insertOne(venue());

    const withoutOptional = venue();
    delete withoutOptional.amenities;
    delete withoutOptional.allies;
    await venues.insertOne(withoutOptional);

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
      zone: 'Norte'
    })));

    await validationFailure(venues.insertOne(venue({
      name: 'a'.repeat(101)
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

    await quotes.insertOne(quote({ eventType: 'XV años' }));

    await validationFailure(quotes.insertOne(quote({
      target: {
        type: 'otro',
        id: id(),
        name: 'Destino'
      }
    })));

    await validationFailure(quotes.insertOne(quote({
      eventType: 'XV Años'
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

  await t.test('bookings idempotencyKey es unico por cliente y opcional', async () => {
    const bookings = db.collection('bookings');
    const clientId = id();

    await bookings.insertOne(booking({
      folio: 'VG-2026-0200',
      clientId,
      idempotencyKey: 'key-1'
    }));

    await duplicateKey(bookings.insertOne(booking({
      folio: 'VG-2026-0201',
      clientId,
      idempotencyKey: 'key-1'
    })));

    await bookings.insertOne(booking({
      folio: 'VG-2026-0202',
      clientId: id(),
      idempotencyKey: 'key-1'
    }));

    await bookings.insertOne(booking({
      folio: 'VG-2026-0203',
      clientId,
      idempotencyKey: 'key-2'
    }));

    await bookings.insertOne(booking({
      folio: 'VG-2026-0204',
      clientId
    }));

    await bookings.insertOne(booking({
      folio: 'VG-2026-0205',
      clientId
    }));

    await validationFailure(bookings.insertOne(booking({
      folio: 'VG-2026-0206',
      clientId,
      idempotencyKey: 'a'.repeat(101)
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

  await t.test('events acepta XV años y budgetPlan dinamico solo con Decimal128', async () => {
    const events = db.collection('events');
    await events.insertOne(event());
    await events.insertOne(event({ eventType: 'XV años' }));

    await validationFailure(events.insertOne(event({
      eventType: 'XV Años'
    })));

    await validationFailure(events.insertOne(event({
      budgetPlan: {
        salon: 50000
      }
    })));
  });

  await t.test('providers valida userId unico, pricing y gallery sin maxItems', async () => {
    const providers = db.collection('providers');
    const userId = id();
    await providers.insertOne(provider({ userId }));

    await duplicateKey(providers.insertOne(provider({ userId })));

    const missingPricing = provider({ userId: id() });
    delete missingPricing.pricing;
    await validationFailure(providers.insertOne(missingPricing));

    await validationFailure(providers.insertOne(provider({
      userId: id(),
      pricing: 'otro'
    })));

    await validationFailure(providers.insertOne(provider({
      userId: id(),
      businessName: 'a'.repeat(101)
    })));

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

  await t.test('exige aprobacion TTL explicita en production', async () => {
    const sessions = db.collection('sessions');
    await sessions.dropIndex('purgeAt_ttl');

    const blocked = await buildPlan(db, undefined, {
      environment: 'production'
    });
    assert.equal(blocked.errors.length, 1);
    assert.equal(blocked.errors[0].code, 'ttlMissing');

    const difference = blocked.differences.find(
      (item) => item.type === 'ttlMissing'
    );
    const ttlApprovalDocument = {
      planFingerprint: blocked.planFingerprint,
      approvedBy: 'integracion',
      expiresAt: '2030-01-01T00:00:00.000Z',
      ttlApprovals: [{
        collection: 'sessions',
        index: 'purgeAt_ttl',
        type: 'ttlMissing',
        expireAfterSeconds: 0,
        estimatedImpact: difference.impact,
        maxDeletion: difference.impact.documents
      }]
    };

    const verification = await verifyInitialization(db, undefined, {
      environment: 'production',
      ttlApprovalDocument
    });
    assert.equal(verification.ok, false);
    assert.equal(verification.exactMatch, false);
    assert.equal(verification.ttlApprovals.length, 1);

    const plan = await buildPlan(db, undefined, {
      environment: 'production',
      ttlApprovalDocument
    });
    await applyPlan(db, plan);
    assert.equal((await verifyInitialization(db)).ok, true);
  });

  await t.test('bloquea un indice equivalente con otro nombre hasta aprobar waiver', async () => {
    const users = db.collection('users');
    await users.dropIndex('email_1_unique');
    await users.createIndex({ email: 1 }, { unique: true, name: 'email_1' });

    const plan = await buildPlan(db);
    assert.equal(plan.errors.length, 1);
    assert.equal(plan.errors[0].code, 'nameMismatch');
    assert.equal(plan.actions.length, 0);
    await assert.rejects(
      () => applyPlan(db, plan),
      /conflictos o aprobaciones pendientes/
    );

    const waiverDocument = {
      planFingerprint: plan.planFingerprint,
      approvedBy: 'integracion',
      expiresAt: '2030-01-01T00:00:00.000Z',
      waivers: [{
        collection: 'users',
        type: 'nameMismatch',
        index: 'email_1_unique',
        actualIndex: 'email_1',
        reason: 'Indice equivalente heredado',
        riskAccepted: 'Revisado en integracion'
      }]
    };

    const verification = await verifyInitialization(db, undefined, {
      waiverDocument
    });
    assert.equal(verification.ok, true);
    assert.equal(verification.exactMatch, false);

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
      /conflictos o aprobaciones pendientes/
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
