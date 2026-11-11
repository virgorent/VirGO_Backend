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
      authorName: string(),
      stars: int(1, 5),
      text: string(),
      bookingId: uuid(),
      reply: object({
        text: string(),
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
  _id: uuid(),
  targetType: enumOf(TARGET_TYPES),
  targetId: uuid(),
  month: monthString(),
  visits: int(0),
  requests: int(0),
  bookings: int(0)
}, ['_id', 'targetType', 'targetId', 'month', 'visits', 'requests', 'bookings']);
