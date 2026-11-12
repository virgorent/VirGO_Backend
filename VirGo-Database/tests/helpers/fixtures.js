import { randomUUID } from 'node:crypto';
import { Decimal128 } from 'mongodb';

export const id = () => randomUUID();

export const now = () => new Date();

export const money = (value) => Decimal128.fromString(String(value));

export const user = (overrides = {}) => ({
  _id: id(),
  name: 'Ana Perez',
  email: `ana.${id()}@example.com`,
  passwordHash: 'hash',
  role: 'client',
  phone: '5551234567',
  isVerified: false,
  failedLoginCount: 0,
  unreadNotices: 0,
  createdAt: now(),
  updatedAt: now(),
  ...overrides
});

export const passwordReset = (overrides = {}) => ({
  _id: id(),
  userId: id(),
  expiresAt: now(),
  codeHash: 'hash',
  attempts: 0,
  ...overrides
});

export const session = (overrides = {}) => ({
  _id: id(),
  userId: id(),
  tokenHash: id(),
  createdAt: now(),
  expiresAt: now(),
  purgeAt: now(),
  ...overrides
});

export const venue = (overrides = {}) => ({
  _id: id(),
  ownerId: id(),
  name: 'Salon Central',
  shortName: 'Central',
  description: 'Un salon amplio y equipado para eventos sociales.',
  address: 'Calle Principal 123',
  zone: 'Centro',
  location: {
    type: 'Point',
    coordinates: [-99.1332, 19.4326]
  },
  capacity: 200,
  price: {
    base: money(1500),
    unit: 'hour',
    minHours: 2
  },
  amenities: ['Estacionamiento'],
  services: [
    {
      name: 'Sonido',
      included: true,
      price: money(0)
    }
  ],
  photos: [],
  categories: ['Salon'],
  rating: {
    average: 4.5,
    count: 10
  },
  verified: true,
  status: 'published',
  allies: [],
  createdAt: now(),
  updatedAt: now(),
  ...overrides
});

export const provider = (overrides = {}) => ({
  _id: id(),
  userId: id(),
  businessName: 'Fotografia VirGo',
  category: 'Fotografía',
  description: 'Cobertura profesional de foto y video para eventos.',
  pricing: 'per_event',
  responseTime: 'Menos de 2 h',
  zones: ['Centro'],
  packages: [
    {
      id: 'basico',
      name: 'Basico',
      price: money(2500),
      includes: ['Sesion', 'Edicion']
    }
  ],
  gallery: [],
  coverUrl: '',
  rating: {
    average: 4,
    count: 2
  },
  status: 'published',
  createdAt: now(),
  updatedAt: now(),
  ...overrides
});

export const quote = (overrides = {}) => ({
  _id: id(),
  clientId: id(),
  clientName: 'Ana Perez',
  target: {
    type: 'venue',
    id: id(),
    name: 'Salon Central'
  },
  eventType: 'Boda',
  eventDate: '2026-12-01',
  guests: 100,
  requestedServices: ['Sonido'],
  message: 'Quiero cotizar el salon.',
  status: 'sent',
  createdAt: now(),
  updatedAt: now(),
  ...overrides
});

export const booking = (overrides = {}) => ({
  _id: id(),
  folio: 'VG-2026-0001',
  clientId: id(),
  clientName: 'Ana Perez',
  venue: {
    venueId: id(),
    name: 'Salon Central',
    photoUrl: ''
  },
  date: '2026-12-01',
  hours: 4,
  total: money(6000),
  deposit: money(1500),
  status: 'pending_payment',
  payments: [],
  createdAt: now(),
  updatedAt: now(),
  ...overrides
});

export const review = (overrides = {}) => ({
  _id: id(),
  targetType: 'venue',
  targetId: id(),
  authorId: id(),
  authorName: 'Ana Perez',
  stars: 5,
  text: 'Excelente servicio.',
  bookingId: id(),
  createdAt: now(),
  ...overrides
});

export const conversation = (overrides = {}) => ({
  _id: id(),
  clientId: id(),
  target: {
    type: 'venue',
    id: id(),
    name: 'Salon Central'
  },
  targetUserId: id(),
  lastMessage: 'Hola, tengo una pregunta.',
  lastAt: now(),
  ...overrides
});

export const event = (overrides = {}) => ({
  _id: id(),
  ownerId: id(),
  name: 'Boda de Ana',
  eventType: 'Boda',
  date: '2026-12-01',
  guests: 100,
  budget: money(100000),
  hours: 8,
  budgetPlan: {
    salon: money(50000),
    catering: money(50000)
  },
  createdAt: now(),
  updatedAt: now(),
  ...overrides
});

export const venueDate = (overrides = {}) => ({
  _id: id(),
  venueId: id(),
  date: '2026-12-01',
  kind: 'booked',
  bookingId: id(),
  ...overrides
});

export const paymentCard = (overrides = {}) => ({
  _id: id(),
  userId: id(),
  brand: 'Visa',
  last4: '4242',
  expMonth: 12,
  expYear: 2030,
  gatewayToken: 'token',
  isDefault: true,
  ...overrides
});

export const contactMessage = (overrides = {}) => ({
  _id: id(),
  name: 'Ana Perez',
  email: 'ana@example.com',
  message: 'Hola, necesito informacion.',
  createdAt: now(),
  ...overrides
});
