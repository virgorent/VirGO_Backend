import { INDEXES } from './indexes.js';
import {
  passwordResetsValidator,
  paymentCardsValidator,
  sessionsValidator,
  usersValidator
} from './validators/accounts.js';
import {
  bookingsValidator,
  countersValidator
} from './validators/bookings.js';
import {
  conversationsValidator,
  messagesValidator,
  noticesValidator
} from './validators/interaction.js';
import {
  eventGuestsValidator,
  eventTasksValidator,
  eventsValidator,
  favoritesValidator
} from './validators/planning.js';
import {
  gigsValidator,
  providersValidator
} from './validators/providers.js';
import { quotesValidator } from './validators/quotes.js';
import {
  monthlyStatsValidator,
  reviewsValidator
} from './validators/reputation.js';
import {
  assistantMessagesValidator,
  contactMessagesValidator
} from './validators/support.js';
import {
  venueDatesValidator,
  venuesValidator
} from './validators/venues.js';

const definitions = [
  ['users', usersValidator],
  ['passwordResets', passwordResetsValidator],
  ['sessions', sessionsValidator],
  ['paymentCards', paymentCardsValidator],
  ['venues', venuesValidator],
  ['venueDates', venueDatesValidator],
  ['providers', providersValidator],
  ['gigs', gigsValidator],
  ['reviews', reviewsValidator],
  ['monthlyStats', monthlyStatsValidator],
  ['events', eventsValidator],
  ['eventGuests', eventGuestsValidator],
  ['eventTasks', eventTasksValidator],
  ['quotes', quotesValidator],
  ['bookings', bookingsValidator],
  ['counters', countersValidator],
  ['favorites', favoritesValidator],
  ['conversations', conversationsValidator],
  ['messages', messagesValidator],
  ['notices', noticesValidator],
  ['assistantMessages', assistantMessagesValidator],
  ['contactMessages', contactMessagesValidator]
];

export const COLLECTIONS = definitions.map(([name, validator]) => ({
  name,
  validator,
  indexes: INDEXES[name] ?? []
}));

export const COLLECTION_NAMES = COLLECTIONS.map((collection) => collection.name);

export const COLLECTIONS_BY_NAME = new Map(
  COLLECTIONS.map((collection) => [collection.name, collection])
);

export { INDEXES };
