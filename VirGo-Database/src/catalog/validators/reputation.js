import { TARGET_TYPES } from '../enums.js';
import {
  date,
  enumOf,
  int,
  jsonSchema,
  monthString,
  object,
  string,
  uuid
} from '../helpers.js';

export const reviewsValidator = {
  $and: [
    jsonSchema('reviews', {
      _id: uuid(),
      targetType: enumOf(TARGET_TYPES),
      targetId: uuid(),
      authorId: uuid(),
      authorName: string(100),
      stars: int(1, 5),
      text: string(1000),
      bookingId: uuid(),
      reply: object({
        text: string(1000),
        at: date()
      }, ['text', 'at']),
      createdAt: date()
    }, [
      '_id',
      'targetType',
      'targetId',
      'authorId',
      'authorName',
      'stars',
      'text',
      'createdAt'
    ]),
    {
      $or: [
        {
          targetType: 'venue',
          bookingId: {
            $exists: true,
            $type: 'string'
          }
        },
        {
          targetType: 'provider'
        }
      ]
    }
  ]
};

export const monthlyStatsValidator = jsonSchema('monthlyStats', {
  _id: {
    bsonType: 'string',
    pattern: '^(venue|provider):[0-9a-fA-F-]{36}:[0-9]{4}-[0-9]{2}$',
    description: 'Determinista: <targetType>:<targetId>:<AAAA-MM>, para que el upsert con $inc funcione'
  },
  targetType: enumOf(TARGET_TYPES),
  targetId: uuid(),
  month: monthString(),
  visits: int(0),
  requests: int(0),
  bookings: int(0)
}, ['_id', 'targetType', 'targetId', 'month', 'visits', 'requests', 'bookings']);
