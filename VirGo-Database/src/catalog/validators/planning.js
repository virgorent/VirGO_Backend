import {
  COLLABORATOR_PERMISSIONS,
  DESIGN_LAYOUTS,
  DESIGN_THEMES,
  EVENT_TYPES,
  FAVORITE_TARGET_TYPES,
  GUEST_STATUSES,
  PAYMENT_STATUSES,
  REMINDER_KINDS,
  VENDOR_PAYMENT_STATUSES
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

export const eventsValidator = jsonSchema('events', {
  _id: uuid(),
  ownerId: uuid(),
  name: string(),
  eventType: enumOf(EVENT_TYPES),
  date: dateString(),
  guests: int(1),
  budget: money(),
  hours: int(1),
  venue: object({
    venueId: uuid(),
    name: string(),
    photoUrl: string()
  }, ['venueId', 'name', 'photoUrl']),
  design: object({
    theme: enumOf(DESIGN_THEMES),
    layout: enumOf(DESIGN_LAYOUTS),
    staff: object({
      waiters: int(0),
      security: int(0),
      cleaning: int(0)
    }, ['waiters', 'security', 'cleaning'])
  }, ['theme', 'layout', 'staff']),
  budgetPlan: {
    bsonType: 'object',
    additionalProperties: money()
  },
  invitation: object({
    template: string(),
    message: string()
  }, ['template', 'message']),
  reminders: arrayOf(object({
    id: string(),
    title: string(),
    date: dateString(),
    kind: enumOf(REMINDER_KINDS),
    auto: bool()
  }, ['id', 'title', 'date', 'kind', 'auto'])),
  vendors: arrayOf(object({
    id: string(),
    providerId: uuid(),
    name: string(),
    category: string(),
    package: string(),
    price: money(),
    paymentStatus: enumOf(VENDOR_PAYMENT_STATUSES)
  }, ['id', 'name', 'category', 'price', 'paymentStatus'])),
  externalPayments: arrayOf(object({
    id: string(),
    concept: string(),
    amount: money(),
    status: enumOf(PAYMENT_STATUSES),
    paidAt: date()
  }, ['id', 'concept', 'amount', 'status'])),
  collaborators: arrayOf(object({
    userId: uuid(),
    name: string(),
    email: string(),
    permission: enumOf(COLLABORATOR_PERMISSIONS)
  }, ['userId', 'name', 'email', 'permission'])),
  createdAt: date(),
  updatedAt: date()
}, [
  '_id',
  'ownerId',
  'name',
  'eventType',
  'date',
  'guests',
  'budget',
  'hours',
  'createdAt',
  'updatedAt'
]);

export const eventGuestsValidator = jsonSchema('eventGuests', {
  _id: uuid(),
  eventId: uuid(),
  name: string(),
  phone: string(),
  companions: int(0),
  status: enumOf(GUEST_STATUSES),
  table: string()
}, ['_id', 'eventId', 'name', 'phone', 'companions', 'status']);

export const eventTasksValidator = jsonSchema('eventTasks', {
  _id: uuid(),
  eventId: uuid(),
  title: string(),
  dueDate: dateString(),
  done: bool(),
  suggested: bool()
}, ['_id', 'eventId', 'title', 'dueDate', 'done', 'suggested']);

export const favoritesValidator = jsonSchema('favorites', {
  _id: uuid(),
  userId: uuid(),
  targetType: enumOf(FAVORITE_TARGET_TYPES),
  targetId: uuid(),
  name: string(),
  photoUrl: string(),
  rating: number(0, 5),
  createdAt: date()
}, [
  '_id',
  'userId',
  'targetType',
  'targetId',
  'name',
  'photoUrl',
  'rating',
  'createdAt'
]);
