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
    name: string()
  }, ['type', 'id', 'name']),
  targetUserId: uuid(),
  quoteId: uuid(),
  lastMessage: string(),
  lastAt: date()
}, ['_id', 'clientId', 'target', 'targetUserId', 'lastMessage', 'lastAt']);

export const messagesValidator = jsonSchema('messages', {
  _id: uuid(),
  conversationId: uuid(),
  senderId: uuid(),
  text: string(),
  sentAt: date(),
  readAt: date()
}, ['_id', 'conversationId', 'senderId', 'text', 'sentAt']);

export const noticesValidator = jsonSchema('notices', {
  _id: uuid(),
  userId: uuid(),
  kind: enumOf(NOTICE_KINDS),
  title: string(),
  body: string(),
  link: string(),
  read: bool(),
  createdAt: date(),
  purgeAt: date()
}, ['_id', 'userId', 'kind', 'title', 'body', 'link', 'read', 'createdAt', 'purgeAt']);
