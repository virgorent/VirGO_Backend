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
    name: string(),
    email: email(),
    passwordHash: string(),
    role: enumOf(ROLES),
    phone: string(),
    providerId: uuid(),
    isVerified: bool(),
    createdAt: date(),
    updatedAt: date()
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
    'updatedAt'
  ],
  { additionalProperties: false }
);

export const passwordResetsValidator = jsonSchema('passwordResets', {
  _id: uuid(),
  userId: uuid(),
  tokenHash: string(),
  expiresAt: date(),
  used: bool()
}, ['_id', 'userId', 'tokenHash', 'expiresAt', 'used']);

export const sessionsValidator = jsonSchema('sessions', {
  _id: uuid(),
  userId: uuid(),
  tokenHash: string(),
  createdAt: date(),
  expiresAt: date(),
  purgeAt: date()
}, ['_id', 'userId', 'tokenHash', 'createdAt', 'expiresAt', 'purgeAt']);

export const paymentCardsValidator = jsonSchema(
  'paymentCards',
  {
    _id: uuid(),
    userId: uuid(),
    brand: string(),
    last4: {
      bsonType: 'string',
      pattern: '^\\d{4}$'
    },
    expMonth: int(1, 12),
    expYear: int(2000),
    gatewayToken: string(),
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
