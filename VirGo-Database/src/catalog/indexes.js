const index = (name, key, options = {}) => ({ name, key, options });

export const INDEXES = {
  users: [
    index('email_1_unique', { email: 1 }, { unique: true })
  ],
  passwordResets: [
    index('userId_1_unique', { userId: 1 }, { unique: true }),
    index('expiresAt_ttl', { expiresAt: 1 }, { expireAfterSeconds: 0 })
  ],
  sessions: [
    index('tokenHash_1_unique', { tokenHash: 1 }, { unique: true }),
    index('userId_1', { userId: 1 }),
    index('purgeAt_ttl', { purgeAt: 1 }, { expireAfterSeconds: 0 })
  ],
  paymentCards: [
    index('userId_1', { userId: 1 }),
    index('userId_1_default_unique', { userId: 1 }, {
      unique: true,
      partialFilterExpression: { isDefault: true }
    })
  ],
  venues: [
    index('location_2dsphere_status_1', { location: '2dsphere', status: 1 }),
    index('ownerId_1', { ownerId: 1 }),
    index('status_1_zone_1_categories_1', { status: 1, zone: 1, categories: 1 })
  ],
  venueDates: [
    index('venueId_1_date_1_unique', { venueId: 1, date: 1 }, { unique: true })
  ],
  providers: [
    index('userId_1_unique', { userId: 1 }, { unique: true }),
    index('status_1_category_1_businessName_1', {
      status: 1,
      category: 1,
      businessName: 1
    })
  ],
  gigs: [
    index('providerId_1_date_1_start_1', { providerId: 1, date: 1, start: 1 })
  ],
  reviews: [
    index('targetType_1_targetId_1_createdAt_-1', {
      targetType: 1,
      targetId: 1,
      createdAt: -1
    }),
    index('targetType_1_targetId_1_authorId_1_unique', {
      targetType: 1,
      targetId: 1,
      authorId: 1
    }, { unique: true })
  ],
  monthlyStats: [
    index('targetType_1_targetId_1_month_1_unique', {
      targetType: 1,
      targetId: 1,
      month: 1
    }, { unique: true })
  ],
  events: [
    index('ownerId_1_date_1', { ownerId: 1, date: 1 }),
    index('collaborators.userId_1_date_1', {
      'collaborators.userId': 1,
      date: 1
    })
  ],
  eventGuests: [
    index('eventId_1_status_1', { eventId: 1, status: 1 })
  ],
  eventTasks: [
    index('eventId_1_dueDate_1', { eventId: 1, dueDate: 1 })
  ],
  quotes: [
    index('clientId_1_createdAt_-1', { clientId: 1, createdAt: -1 }),
    index('targetType_1_targetId_1_status_1_createdAt_-1', {
      'target.type': 1,
      'target.id': 1,
      status: 1,
      createdAt: -1
    })
  ],
  bookings: [
    index('folio_1_unique', { folio: 1 }, { unique: true }),
    index('clientId_1_date_-1', { clientId: 1, date: -1 }),
    index('clientId_1_idempotencyKey_1_unique', {
      clientId: 1,
      idempotencyKey: 1
    }, {
      unique: true,
      partialFilterExpression: { idempotencyKey: { $type: 'string' } }
    }),
    index('venueId_1_date_1', { 'venue.venueId': 1, date: 1 })
  ],
  counters: [],
  favorites: [
    index('userId_1_targetType_1_targetId_1_unique', {
      userId: 1,
      targetType: 1,
      targetId: 1
    }, { unique: true })
  ],
  conversations: [
    index('clientId_1_lastAt_-1', { clientId: 1, lastAt: -1 }),
    index('targetUserId_1_lastAt_-1', { targetUserId: 1, lastAt: -1 }),
    index('clientId_1_targetType_1_targetId_1_unique', {
      clientId: 1,
      'target.type': 1,
      'target.id': 1
    }, { unique: true })
  ],
  messages: [
    index('conversationId_1_sentAt_1', { conversationId: 1, sentAt: 1 })
  ],
  notices: [
    index('userId_1_createdAt_-1', { userId: 1, createdAt: -1 }),
    index('purgeAt_ttl', { purgeAt: 1 }, { expireAfterSeconds: 0 })
  ],
  assistantMessages: [
    index('userId_1_createdAt_-1', { userId: 1, createdAt: -1 }),
    index('purgeAt_ttl', { purgeAt: 1 }, { expireAfterSeconds: 0 })
  ],
  contactMessages: [
    index('createdAt_-1', { createdAt: -1 })
  ]
};
