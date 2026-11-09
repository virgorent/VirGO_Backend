import { EVENT_TYPES, QUOTE_STATUSES, TARGET_TYPES } from '../enums.js';
import {
  arrayOf,
  date,
  dateString,
  enumOf,
  int,
  jsonSchema,
  money,
  object,
  string,
  uuid
} from '../helpers.js';

export const quotesValidator = jsonSchema('quotes', {
  _id: uuid(),
  clientId: uuid(),
  clientName: string(100),
  target: object({
    type: enumOf(TARGET_TYPES),
    id: uuid(),
    name: string(100)
  }, ['type', 'id', 'name']),
  eventId: uuid(),
  eventType: enumOf(EVENT_TYPES),
  eventDate: dateString(),
  guests: int(1),
  requestedServices: arrayOf(string(100)),
  message: string(2000),
  status: enumOf(QUOTE_STATUSES),
  response: object({
    items: arrayOf(object({
      concept: string(150),
      qty: int(1),
      unitPrice: money()
    }, ['concept', 'qty', 'unitPrice'])),
    note: string(1000),
    subtotal: money(),
    tax: money(),
    total: money(),
    validUntil: date(),
    answeredAt: date()
  }, ['items', 'note', 'subtotal', 'tax', 'total', 'validUntil', 'answeredAt']),
  createdAt: date(),
  updatedAt: date()
}, [
  '_id',
  'clientId',
  'clientName',
  'target',
  'eventType',
  'eventDate',
  'guests',
  'status',
  'createdAt',
  'updatedAt'
]);
