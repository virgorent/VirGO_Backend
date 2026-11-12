import { createHash } from 'node:crypto';
import { Decimal128 } from 'mongodb';
import { COLLECTION_NAMES } from '../catalog/index.js';

const DEMO_NAMESPACE = 'virgo-demo:';
const CREATED_AT = new Date('2026-11-01T12:00:00.000Z');
const UPDATED_AT = new Date('2026-11-02T12:00:00.000Z');
const EXPIRES_AT = new Date('2030-01-01T00:00:00.000Z');

const money = (value) => Decimal128.fromString(String(value));

export const demoId = (key) => {
  const hex = createHash('sha256')
    .update(`${DEMO_NAMESPACE}${key}`)
    .digest('hex')
    .slice(0, 32);

  return [
    hex.slice(0, 8),
    hex.slice(8, 12),
    hex.slice(12, 16),
    hex.slice(16, 20),
    hex.slice(20, 32)
  ].join('-');
};

export const DEMO_IDS = {
  client: demoId('user:client'),
  owner: demoId('user:owner'),
  providerUser: demoId('user:provider'),
  venue1: demoId('venue:centro'),
  venue2: demoId('venue:jardin'),
  provider: demoId('provider:catering'),
  quote: demoId('quote:venue'),
  booking: demoId('booking:confirmed'),
  event: demoId('event:boda'),
  conversation: demoId('conversation:venue'),
  session: demoId('session:client'),
  card: demoId('card:client'),
  passwordReset: demoId('password-reset:client'),
  favorite: demoId('favorite:venue'),
  monthlyStat: demoId('monthly-stat:venue'),
  reviewVenue: demoId('review:venue'),
  reviewProvider: demoId('review:provider'),
  gig: demoId('gig:catering'),
  venueDateBooked: demoId('venue-date:booked'),
  venueDateBlocked: demoId('venue-date:blocked'),
  guest: demoId('event-guest:one'),
  task: demoId('event-task:one'),
  message: demoId('message:one'),
  notice: demoId('notice:one'),
  assistantUser: demoId('assistant-message:user'),
  assistantReply: demoId('assistant-message:assistant'),
  contact: demoId('contact-message:one'),
  counter: 'folio-2026'
};

const buildUsers = () => [
  {
    _id: DEMO_IDS.client,
    name: 'Cliente Demo',
    email: 'cliente.demo@virgo.local',
    passwordHash: 'demo-password-hash-no-real',
    role: 'client',
    phone: '5550000001',
    isVerified: true,
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT,
    failedLoginCount: 0,
    unreadNotices: 1,
    city: 'Queretaro',
    defaultGuests: 100
  },
  {
    _id: DEMO_IDS.owner,
    name: 'Propietario Demo',
    email: 'propietario.demo@virgo.local',
    passwordHash: 'demo-password-hash-no-real',
    role: 'owner',
    phone: '5550000002',
    isVerified: true,
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT,
    failedLoginCount: 0,
    unreadNotices: 0
  },
  {
    _id: DEMO_IDS.providerUser,
    name: 'Proveedor Demo',
    email: 'proveedor.demo@virgo.local',
    passwordHash: 'demo-password-hash-no-real',
    role: 'provider',
    phone: '5550000003',
    providerId: DEMO_IDS.provider,
    isVerified: true,
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT,
    failedLoginCount: 0,
    unreadNotices: 0
  }
];

const buildVenues = () => [
  {
    _id: DEMO_IDS.venue1,
    ownerId: DEMO_IDS.owner,
    name: 'Salon Demo Centro',
    shortName: 'Demo Centro',
    description: 'Espacio de demostracion para eventos sociales y corporativos.',
    address: 'Calle Demo 123, Centro',
    zone: 'Centro',
    location: {
      type: 'Point',
      coordinates: [-99.1332, 19.4326]
    },
    capacity: 150,
    price: {
      base: money(1500),
      unit: 'hour',
      minHours: 2
    },
    amenities: ['Estacionamiento', 'Sonido'],
    services: [
      {
        name: 'Sonido',
        included: true,
        price: money(0)
      }
    ],
    photos: [
      {
        url: 'https://example.com/demo-venue-centro.jpg',
        order: 0
      }
    ],
    categories: ['Salon'],
    rating: {
      average: 4.5,
      count: 10
    },
    verified: true,
    status: 'published',
    allies: [],
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT
  },
  {
    _id: DEMO_IDS.venue2,
    ownerId: DEMO_IDS.owner,
    name: 'Jardin Demo Juriquilla',
    shortName: 'Demo Jardin',
    description: 'Jardin de demostracion con area exterior para celebraciones.',
    address: 'Avenida Demo 456, Juriquilla',
    zone: 'Juriquilla',
    location: {
      type: 'Point',
      coordinates: [-99.2101, 19.5012]
    },
    capacity: 220,
    price: {
      base: money(2200),
      unit: 'day',
      minHours: 1
    },
    amenities: ['Estacionamiento', 'Pista de baile'],
    services: [],
    photos: [],
    categories: ['Jardin'],
    rating: {
      average: 4.2,
      count: 5
    },
    verified: false,
    status: 'published',
    allies: [],
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT
  }
];

const buildProviders = () => [
  {
    _id: DEMO_IDS.provider,
    userId: DEMO_IDS.providerUser,
    businessName: 'Catering Demo',
    category: 'Catering',
    description: 'Servicio de catering de demostracion para eventos sociales.',
    zones: ['Centro'],
    packages: [
      {
        id: 'paquete-demo',
        name: 'Paquete Demo',
        price: money(3500),
        includes: ['Menu demo', 'Servicio']
      }
    ],
    gallery: [],
    coverUrl: '',
    rating: {
      average: 4.6,
      count: 8
    },
    status: 'published',
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT,
    pricing: 'per_event',
    responseTime: 'Menos de 2 h'
  }
];

const buildReviews = () => [
  {
    _id: DEMO_IDS.reviewVenue,
    targetType: 'venue',
    targetId: DEMO_IDS.venue1,
    authorId: DEMO_IDS.client,
    authorName: 'Cliente Demo',
    stars: 5,
    text: 'Resena de demostracion para el salon.',
    bookingId: DEMO_IDS.booking,
    createdAt: CREATED_AT
  },
  {
    _id: DEMO_IDS.reviewProvider,
    targetType: 'provider',
    targetId: DEMO_IDS.provider,
    authorId: DEMO_IDS.client,
    authorName: 'Cliente Demo',
    stars: 4,
    text: 'Resena de demostracion para el proveedor.',
    createdAt: CREATED_AT
  }
];

const buildEvents = () => [
  {
    _id: DEMO_IDS.event,
    ownerId: DEMO_IDS.owner,
    name: 'Boda Demo',
    eventType: 'Boda',
    date: '2026-12-15',
    guests: 120,
    budget: money(180000),
    hours: 8,
    venue: {
      venueId: DEMO_IDS.venue1,
      name: 'Salon Demo Centro',
      photoUrl: 'https://example.com/demo-venue-centro.jpg'
    },
    design: {
      theme: 'Boda',
      layout: 'center_floor',
      staff: {
        waiters: 10,
        security: 2,
        cleaning: 3
      }
    },
    budgetPlan: {
      salon: money(90000),
      catering: money(90000)
    },
    invitation: {
      template: 'demo',
      message: 'Acompana a la familia Demo a celebrar esta boda.'
    },
    reminders: [
      {
        id: 'demo-reminder-1',
        title: 'Pago de anticipo',
        date: '2026-12-01',
        kind: 'payment',
        auto: true
      }
    ],
    vendors: [
      {
        id: 'demo-vendor-1',
        providerId: DEMO_IDS.provider,
        name: 'Catering Demo',
        category: 'Catering',
        package: 'Paquete Demo',
        price: money(3500),
        paymentStatus: 'deposit'
      }
    ],
    externalPayments: [
      {
        id: 'demo-external-payment-1',
        concept: 'Anticipo de salon',
        amount: money(3000),
        status: 'pending'
      }
    ],
    collaborators: [
      {
        userId: DEMO_IDS.client,
        name: 'Cliente Demo',
        email: 'cliente.demo@virgo.local',
        permission: 'edit'
      }
    ],
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT
  }
];

const buildBookings = () => [
  {
    _id: DEMO_IDS.booking,
    folio: 'VG-2026-9001',
    clientId: DEMO_IDS.client,
    clientName: 'Cliente Demo',
    eventId: DEMO_IDS.event,
    quoteId: DEMO_IDS.quote,
    idempotencyKey: 'demo-booking-key-1',
    venue: {
      venueId: DEMO_IDS.venue1,
      name: 'Salon Demo Centro',
      photoUrl: 'https://example.com/demo-venue-centro.jpg'
    },
    date: '2026-12-15',
    hours: 4,
    total: money(6000),
    deposit: money(3000),
    status: 'confirmed',
    extras: [],
    payments: [
      {
        paymentId: 'demo-payment-1',
        concept: 'Anticipo',
        amount: money(3000),
        method: 'transfer',
        reference: 'DEMO-001',
        status: 'completed',
        paidAt: CREATED_AT
      }
    ],
    createdAt: CREATED_AT,
    updatedAt: UPDATED_AT
  }
];

const buildConversation = () => [
  {
    _id: DEMO_IDS.conversation,
    clientId: DEMO_IDS.client,
    target: {
      type: 'venue',
      id: DEMO_IDS.venue1,
      name: 'Salon Demo Centro'
    },
    targetUserId: DEMO_IDS.owner,
    quoteId: DEMO_IDS.quote,
    lastMessage: 'Hola, quiero confirmar la fecha demo.',
    lastAt: CREATED_AT
  }
];

const buildDocuments = () => ({
  users: buildUsers(),
  passwordResets: [
    {
      _id: DEMO_IDS.passwordReset,
      userId: DEMO_IDS.client,
      expiresAt: EXPIRES_AT,
      codeHash: 'demo-reset-code-hash-no-real',
      attempts: 0
    }
  ],
  sessions: [
    {
      _id: DEMO_IDS.session,
      userId: DEMO_IDS.client,
      tokenHash: 'demo-session-token-hash-no-real',
      createdAt: CREATED_AT,
      expiresAt: EXPIRES_AT,
      purgeAt: EXPIRES_AT,
      device: 'Demo Android',
      revokedAt: null
    }
  ],
  paymentCards: [
    {
      _id: DEMO_IDS.card,
      userId: DEMO_IDS.client,
      brand: 'Visa',
      last4: '4242',
      expMonth: 12,
      expYear: 2030,
      gatewayToken: 'demo-gateway-token-no-real',
      isDefault: true
    }
  ],
  venues: buildVenues(),
  venueDates: [
    {
      _id: DEMO_IDS.venueDateBooked,
      venueId: DEMO_IDS.venue1,
      date: '2026-12-15',
      kind: 'booked',
      bookingId: DEMO_IDS.booking
    },
    {
      _id: DEMO_IDS.venueDateBlocked,
      venueId: DEMO_IDS.venue2,
      date: '2026-12-20',
      kind: 'blocked',
      reason: 'maintenance',
      note: 'Bloqueo de demostracion.'
    }
  ],
  providers: buildProviders(),
  gigs: [
    {
      _id: DEMO_IDS.gig,
      providerId: DEMO_IDS.provider,
      eventName: 'Boda Demo',
      clientName: 'Cliente Demo',
      place: 'Salon Demo Centro',
      date: '2026-12-15',
      start: '18:00',
      end: '23:00',
      amount: money(3500),
      quoteId: DEMO_IDS.quote
    }
  ],
  reviews: buildReviews(),
  monthlyStats: [
    {
      _id: `venue:${DEMO_IDS.venue1}:2026-11`,
      targetType: 'venue',
      targetId: DEMO_IDS.venue1,
      month: '2026-11',
      visits: 120,
      requests: 8,
      bookings: 2
    }
  ],
  events: buildEvents(),
  eventGuests: [
    {
      _id: DEMO_IDS.guest,
      eventId: DEMO_IDS.event,
      name: 'Invitado Demo',
      phone: '5550000004',
      companions: 2,
      status: 'confirmed',
      table: 'Mesa 1'
    }
  ],
  eventTasks: [
    {
      _id: DEMO_IDS.task,
      eventId: DEMO_IDS.event,
      title: 'Confirmar menu demo',
      dueDate: '2026-12-01',
      done: false,
      suggested: true
    }
  ],
  quotes: [
    {
      _id: DEMO_IDS.quote,
      clientId: DEMO_IDS.client,
      clientName: 'Cliente Demo',
      target: {
        type: 'venue',
        id: DEMO_IDS.venue1,
        name: 'Salon Demo Centro'
      },
      eventType: 'Boda',
      eventDate: '2026-12-15',
      guests: 120,
      requestedServices: ['Sonido'],
      message: 'Cotizacion de demostracion para el salon.',
      status: 'sent',
      createdAt: CREATED_AT,
      updatedAt: UPDATED_AT
    }
  ],
  bookings: buildBookings(),
  counters: [
    {
      _id: DEMO_IDS.counter,
      next: 9002
    }
  ],
  favorites: [
    {
      _id: DEMO_IDS.favorite,
      userId: DEMO_IDS.client,
      targetType: 'venue',
      targetId: DEMO_IDS.venue1,
      name: 'Salon Demo Centro',
      photoUrl: 'https://example.com/demo-venue-centro.jpg',
      rating: 4.5,
      createdAt: CREATED_AT
    }
  ],
  conversations: buildConversation(),
  messages: [
    {
      _id: DEMO_IDS.message,
      conversationId: DEMO_IDS.conversation,
      senderId: DEMO_IDS.client,
      text: 'Mensaje de demostracion.',
      sentAt: CREATED_AT,
      readAt: UPDATED_AT
    }
  ],
  notices: [
    {
      _id: DEMO_IDS.notice,
      userId: DEMO_IDS.client,
      kind: 'booking',
      title: 'Reserva demo confirmada',
      body: 'La reserva de demostracion fue confirmada.',
      link: `/bookings/${DEMO_IDS.booking}`,
      read: false,
      createdAt: CREATED_AT,
      purgeAt: EXPIRES_AT
    }
  ],
  assistantMessages: [
    {
      _id: DEMO_IDS.assistantUser,
      userId: DEMO_IDS.client,
      role: 'user',
      text: 'Necesito ideas para mi boda demo.',
      createdAt: CREATED_AT,
      purgeAt: EXPIRES_AT
    },
    {
      _id: DEMO_IDS.assistantReply,
      userId: DEMO_IDS.client,
      role: 'assistant',
      text: 'Puedo ayudarte con un plan de demostracion.',
      createdAt: CREATED_AT,
      purgeAt: EXPIRES_AT
    }
  ],
  contactMessages: [
    {
      _id: DEMO_IDS.contact,
      userId: DEMO_IDS.client,
      name: 'Cliente Demo',
      email: 'cliente.demo@virgo.local',
      message: 'Mensaje de contacto de demostracion.',
      createdAt: CREATED_AT
    }
  ]
});

export const buildDemoDocuments = () => {
  const documents = buildDocuments();
  return new Map(
    COLLECTION_NAMES.map((name) => [name, documents[name] ?? []])
  );
};

export const countDemoDocuments = (documents = buildDemoDocuments()) => {
  let total = 0;
  for (const docs of documents.values()) total += docs.length;
  return total;
};

export const assertDemoEnvironment = (config) => {
  if (config.nodeEnv === 'production' || config.dbName === 'virgo') {
    throw new Error('El seed DEMO solo se permite en development o test.');
  }
};

export const seedDemo = async (db, documents = buildDemoDocuments()) => {
  const details = [];

  for (const [collection, docs] of documents) {
    for (const doc of docs) {
      await db.collection(collection).replaceOne(
        { _id: doc._id },
        doc,
        { upsert: true }
      );
    }
    if (docs.length > 0) {
      details.push({ collection, documents: docs.length });
    }
  }

  return {
    collections: details.length,
    documents: countDemoDocuments(documents),
    details
  };
};

export const purgeDemo = async (db, documents = buildDemoDocuments()) => {
  const details = [];

  for (const [collection, docs] of documents) {
    if (docs.length === 0) continue;
    const ids = docs.map((doc) => doc._id);
    const result = await db.collection(collection).deleteMany({
      _id: { $in: ids }
    });
    details.push({ collection, deleted: result.deletedCount });
  }

  return {
    collections: details.length,
    deleted: details.reduce((total, item) => total + item.deleted, 0),
    details
  };
};
