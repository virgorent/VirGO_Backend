import { ASSISTANT_ROLES } from '../enums.js';
import {
  date,
  email,
  enumOf,
  jsonSchema,
  string,
  uuid
} from '../helpers.js';

export const assistantMessagesValidator = jsonSchema('assistantMessages', {
  _id: uuid(),
  userId: uuid(),
  role: enumOf(ASSISTANT_ROLES),
  text: string(4000),
  createdAt: date(),
  purgeAt: date()
}, ['_id', 'userId', 'role', 'text', 'createdAt', 'purgeAt']);

export const contactMessagesValidator = jsonSchema('contactMessages', {
  _id: uuid(),
  userId: uuid(),
  name: string(100),
  email: email(150),
  message: string(2000),
  createdAt: date()
}, ['_id', 'name', 'email', 'message', 'createdAt']);
