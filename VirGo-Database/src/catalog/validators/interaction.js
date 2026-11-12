import { NOTICE_KINDS, TARGET_TYPES } from '../enums.js';
import {
  bool,
  date,
  enumOf,
  jsonSchema,
  object,
  string,
  uuid
} from '../helpers.js';

export const conversationsValidator = jsonSchema('conversations', {
  _id: uuid(),
  clientId: uuid(),
  target: object({
    type: enumOf(TARGET_TYPES),
    id: uuid(),
    name: string(100)
  }, ['type', 'id', 'name']),
  targetUserId: uuid(),
  quoteId: uuid(),
  lastMessage: string(200),
  lastAt: date()
}, ['_id', 'clientId', 'target', 'targetUserId', 'lastMessage', 'lastAt']);

export const messagesValidator = jsonSchema('messages', {
  _id: uuid(),
  conversationId: uuid(),
  senderId: uuid(),
  text: string(2000),
  sentAt: date(),
  readAt: date()
}, ['_id', 'conversationId', 'senderId', 'text', 'sentAt']);

export const noticesValidator = jsonSchema('notices', {
  _id: uuid(),
  userId: uuid(),
  kind: enumOf(NOTICE_KINDS),
  title: string(150),
  body: string(500),
  link: string(500),
  read: bool(),
  createdAt: date(),
  purgeAt: date()
}, ['_id', 'userId', 'kind', 'title', 'body', 'link', 'read', 'createdAt', 'purgeAt']);
