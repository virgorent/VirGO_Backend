# Plan — 002 Cuentas y acceso

| Campo | Valor |
|---|---|
| Módulo | `002-cuentas` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `users`

**Specs de API que la usan:** `003`, `005`, `006`, `011`, `012`, `014`, `015`, `016`, `017`, `018`, `036`, `037`, `038`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `email` | string | ✅ | 150 |
| `passwordHash` | string | ✅ | 255 |
| `role` | string, enum: client, owner, provider | ✅ | — |
| `phone` | string | ✅ | 16 |
| `providerId` | string | ➖ | — |
| `isVerified` | bool | ✅ | — |
| `createdAt` | date | ✅ | — |
| `updatedAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `email_1_unique` | `{"email": 1}` | `{"unique": true}` |
### `passwordResets`

**Specs de API que la usan:** `011`, `012`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `tokenHash` | string | ✅ | — |
| `expiresAt` | date | ✅ | — |
| `used` | bool | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `userId_1` | `{"userId": 1}` | — |
| `expiresAt_ttl` | `{"expiresAt": 1}` | `{"expireAfterSeconds": 0}` |
### `sessions`

**Specs de API que la usan:** `003`, `004`, `013`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `tokenHash` | string | ✅ | 88 |
| `createdAt` | date | ✅ | — |
| `expiresAt` | date | ✅ | — |
| `purgeAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `tokenHash_1_unique` | `{"tokenHash": 1}` | `{"unique": true}` |
| `userId_1` | `{"userId": 1}` | — |
| `purgeAt_ttl` | `{"purgeAt": 1}` | `{"expireAfterSeconds": 0}` |
### `paymentCards`

**Specs de API que la usan:** `019`, `020`, `021`, `022`, `023`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `brand` | string | ✅ | 20 |
| `last4` | string | ✅ | — |
| `expMonth` | int | ✅ | — |
| `expYear` | int | ✅ | — |
| `gatewayToken` | string | ✅ | 255 |
| `isDefault` | bool | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `userId_1` | `{"userId": 1}` | — |
| `userId_1_default_unique` | `{"userId": 1}` | `{"unique": true, "partialFilterExpression": {"isDefault": true}}` |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
