import {
  BOOKING_STATUSES,
  PAYMENT_METHODS,
  PAYMENT_STATUSES
} from '../enums.js';
import {
  arrayOf,
  date,
  dateString,
  enumOf,
  folioString,
  int,
  jsonSchema,
  money,
  object,
  string,
  uuid
} from '../helpers.js';

export const bookingsValidator = jsonSchema('bookings', {
  _id: uuid(),
  folio: folioString(),
  clientId: uuid(),
  clientName: string(),
  eventId: uuid(),
  quoteId: uuid(),
  venue: object({
    venueId: uuid(),
    name: string(),
    photoUrl: string()
  }, ['venueId', 'name', 'photoUrl']),
  date: dateString(),
  hours: int(1),
  total: money(),
  deposit: money(),
  status: enumOf(BOOKING_STATUSES),
  extras: arrayOf(object({
    name: string(),
    price: money()
  }, ['name', 'price'])),
  payments: arrayOf(object({
    paymentId: string(),
    concept: string(),
    amount: money(),
    method: enumOf(PAYMENT_METHODS),
    reference: string(),
    status: enumOf(PAYMENT_STATUSES),
    paidAt: date()
  }, ['paymentId', 'concept', 'amount', 'method', 'status'])),
  cancellation: object({
    at: date(),
    refund: money()
  }, ['at', 'refund']),
  rescheduledFrom: string(),
  createdAt: date(),
  updatedAt: date()
}, [
  '_id',
  'folio',
  'clientId',
  'clientName',
  'venue',
  'date',
  'hours',
  'total',
  'deposit',
  'status',
  'payments',
  'createdAt',
  'updatedAt'
]);

export const countersValidator = jsonSchema('counters', {
  _id: {
    bsonType: 'string',
    pattern: '^folio-\\d{4}$'
  },
  next: int(1)
}, ['_id', 'next']);
