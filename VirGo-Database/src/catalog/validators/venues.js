import {
  AMENITIES,
  PROVIDER_CATEGORIES,
  VENUE_CATEGORIES,
  VENUE_DATE_KINDS,
  VENUE_DATE_REASONS,
  VENUE_STATUSES,
  ZONES
} from '../enums.js';
import {
  arrayOf,
  bool,
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

export const venuesValidator = jsonSchema('venues', {
  _id: uuid(),
  ownerId: uuid(),
  name: string(),
  shortName: string(),
  description: {
    bsonType: 'string',
    minLength: 20
  },
  address: string(),
  zone: enumOf(ZONES),
  location: object({
    type: {
      bsonType: 'string',
      enum: ['Point']
    },
    coordinates: {
      bsonType: 'array',
      minItems: 2,
      maxItems: 2,
      items: {
        bsonType: 'number'
      }
    }
  }, ['type', 'coordinates']),
  capacity: int(1),
  price: object({
    base: money(),
    unit: enumOf(['hour', 'day']),
    minHours: int(1),
    weekendPct: number(0),
    highSeasonPct: number(0),
    highSeasonMonths: arrayOf(int(1, 12))
  }, ['base', 'unit', 'minHours']),
  amenities: arrayOf(enumOf(AMENITIES)),
  services: arrayOf(object({
    name: string(),
    included: bool(),
    price: money()
  }, ['name', 'included', 'price'])),
  photos: arrayOf(object({
    url: string(),
    order: int(0)
  }, ['url', 'order'])),
  setups: object({
    banquet: int(0),
    auditorium: int(0),
    cocktail: int(0)
  }, ['banquet', 'auditorium', 'cocktail']),
  floorPlanUrl: string(),
  tour360Url: string(),
  categories: arrayOf(enumOf(VENUE_CATEGORIES)),
  rating: object({
    average: number(0, 5),
    count: int(0)
  }, ['average', 'count']),
  verified: bool(),
  status: enumOf(VENUE_STATUSES),
  allies: arrayOf(object({
    providerId: uuid(),
    name: string(),
    category: enumOf(PROVIDER_CATEGORIES)
  }, ['providerId', 'name', 'category'])),
  createdAt: date(),
  updatedAt: date()
}, [
  '_id',
  'ownerId',
  'name',
  'shortName',
  'description',
  'address',
  'zone',
  'location',
  'capacity',
  'price',
  'amenities',
  'services',
  'photos',
  'categories',
  'rating',
  'verified',
  'status',
  'allies',
  'createdAt',
  'updatedAt'
]);

export const venueDatesValidator = jsonSchema('venueDates', {
  _id: uuid(),
  venueId: uuid(),
  date: dateString(),
  kind: enumOf(VENUE_DATE_KINDS),
  reason: enumOf(VENUE_DATE_REASONS),
  note: string(),
  bookingId: uuid()
}, ['_id', 'venueId', 'date', 'kind']);
