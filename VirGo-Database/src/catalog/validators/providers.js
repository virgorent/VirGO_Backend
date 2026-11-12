import { PROVIDER_CATEGORIES, PROVIDER_STATUSES } from '../enums.js';
import {
  arrayOf,
  date,
  dateString,
  enumOf,
  int,
  jsonSchema,
  money,
  number,
  object,
  string,
  uuid
} from '../helpers.js';

export const providersValidator = jsonSchema('providers', {
  _id: uuid(),
  userId: uuid(),
  businessName: string(100),
  category: enumOf(PROVIDER_CATEGORIES),
  description: {
    bsonType: 'string',
    minLength: 20,
    maxLength: 2000
  },
  zones: arrayOf(string(50)),
  packages: arrayOf(object({
    id: string(36),
    name: string(100),
    price: money(),
    includes: {
      bsonType: ['string', 'array'],
      items: {
        bsonType: 'string',
        maxLength: 200
      }
    }
  }, ['id', 'name', 'price', 'includes'])),
  gallery: arrayOf(object({
    url: string(500),
    order: int(0)
  }, ['url', 'order'])),
  coverUrl: string(500),
  rating: object({
    average: number(0, 5),
    count: int(0)
  }, ['average', 'count']),
  status: enumOf(PROVIDER_STATUSES),
  createdAt: date(),
  updatedAt: date(),
  pricing: enumOf(['per_person', 'per_event']),
  responseTime: string(50)
}, [
  '_id',
  'userId',
  'businessName',
  'category',
  'description',
  'zones',
  'packages',
  'rating',
  'status',
  'createdAt',
  'updatedAt',
  'pricing',
  'responseTime'
]);

export const gigsValidator = jsonSchema('gigs', {
  _id: uuid(),
  providerId: uuid(),
  eventName: string(100),
  clientName: string(100),
  place: string(200),
  date: dateString(),
  start: string(5),
  end: string(5),
  amount: money(),
  quoteId: uuid()
}, [
  '_id',
  'providerId',
  'eventName',
  'clientName',
  'place',
  'date',
  'start',
  'end'
]);
