import { ROLES } from '../enums.js';
import {
  bool,
  date,
  email,
  enumOf,
  int,
  jsonSchema,
  string,
  uuid
} from '../helpers.js';

export const usersValidator = jsonSchema(
  'users',
  {
    _id: uuid(),
    name: string(100),
    email: email(150),
    passwordHash: string(255),
    role: enumOf(ROLES),
    phone: string(16),
    providerId: uuid(),
    isVerified: bool(),
    createdAt: date(),
    updatedAt: date(),
    failedLoginCount: int(0),
    lockoutEndsAt: {
      bsonType: ['date', 'null']
    },
    unreadNotices: int(0),
    city: string(80),
    defaultGuests: int(0)
  },
  [
    '_id',
    'name',
    'email',
    'passwordHash',
    'role',
    'phone',
    'isVerified',
    'createdAt',
    'updatedAt',
    'failedLoginCount',
    'unreadNotices'
  ],
  { additionalProperties: false }
);

export const passwordResetsValidator = jsonSchema('passwordResets', {
  _id: uuid(),
  userId: uuid(),
  expiresAt: date(),
  codeHash: string(255),
  attempts: int(0, 5)
}, ['_id', 'userId', 'expiresAt', 'codeHash', 'attempts']);

export const sessionsValidator = jsonSchema('sessions', {
  _id: uuid(),
  userId: uuid(),
  tokenHash: string(88),
  createdAt: date(),
  expiresAt: date(),
  purgeAt: date(),
  device: string(100),
  revokedAt: {
    bsonType: ['date', 'null']
  }
}, ['_id', 'userId', 'tokenHash', 'createdAt', 'expiresAt', 'purgeAt']);

export const paymentCardsValidator = jsonSchema(
  'paymentCards',
  {
    _id: uuid(),
    userId: uuid(),
    brand: string(20),
    last4: {
      bsonType: 'string',
      pattern: '^\\d{4}$'
    },
    expMonth: int(1, 12),
    expYear: int(2000),
    gatewayToken: string(255),
    isDefault: bool()
  },
  [
    '_id',
    'userId',
    'brand',
    'last4',
    'expMonth',
    'expYear',
    'gatewayToken',
    'isDefault'
  ],
  { additionalProperties: false }
);
