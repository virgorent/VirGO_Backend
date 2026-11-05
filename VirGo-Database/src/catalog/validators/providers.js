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
  businessName: string(),
  category: enumOf(PROVIDER_CATEGORIES),
  description: {
    bsonType: 'string',
    minLength: 20
  },
  basePrice: money(),
  zones: arrayOf(string()),
  packages: arrayOf(object({
    id: string(),
    name: string(),
    price: money(),
    includes: {
      bsonType: ['string', 'array'],
      items: {
        bsonType: 'string'
      }
    }
  }, ['id', 'name', 'price', 'includes'])),
  gallery: arrayOf(object({
    url: string(),
    order: int(0)
  }, ['url', 'order'])),
  coverUrl: string(),
  rating: object({
    average: number(0, 5),
    count: int(0)
  }, ['average', 'count']),
  status: enumOf(PROVIDER_STATUSES),
  createdAt: date(),
  updatedAt: date()
}, [
  '_id',
  'userId',
  'businessName',
  'category',
  'description',
  'basePrice',
  'zones',
  'packages',
  'gallery',
  'rating',
  'status',
  'createdAt',
  'updatedAt'
]);

export const gigsValidator = jsonSchema('gigs', {
  _id: uuid(),
  providerId: uuid(),
  eventName: string(),
  clientName: string(),
  place: string(),
  date: dateString(),
  start: string(),
  end: string(),
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
