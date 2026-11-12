# Plan — 009 Conversaciones y avisos

| Campo | Valor |
|---|---|
| Módulo | `009-interaccion` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `conversations`

**Specs de API que la usan:** `084`, `085`, `086`, `087`, `088`, `089`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `clientId` | string | ✅ | — |
| `target` | object | ✅ | — |
| `targetUserId` | string | ✅ | — |
| `quoteId` | string | ➖ | — |
| `lastMessage` | string | ✅ | 200 |
| `lastAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `clientId_1_lastAt_-1` | `{"clientId": 1, "lastAt": -1}` | — |
| `targetUserId_1_lastAt_-1` | `{"targetUserId": 1, "lastAt": -1}` | — |
| `clientId_1_targetType_1_targetId_1_unique` | `{"clientId": 1, "target.type": 1, "target.id": 1}` | `{"unique": true}` |
### `messages`

**Specs de API que la usan:** `084`, `085`, `086`, `087`, `088`, `089`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `conversationId` | string | ✅ | — |
| `senderId` | string | ✅ | — |
| `text` | string | ✅ | 2000 |
| `sentAt` | date | ✅ | — |
| `readAt` | date | ➖ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `conversationId_1_sentAt_1` | `{"conversationId": 1, "sentAt": 1}` | — |
### `notices`

**Specs de API que la usan:** `090`, `091`, `092`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `kind` | string, enum: booking, payment, cancellation, quote, message, reminder | ✅ | — |
| `title` | string | ✅ | 150 |
| `body` | string | ✅ | 500 |
| `link` | string | ✅ | 500 |
| `read` | bool | ✅ | — |
| `createdAt` | date | ✅ | — |
| `purgeAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `userId_1_createdAt_-1` | `{"userId": 1, "createdAt": -1}` | — |
| `purgeAt_ttl` | `{"purgeAt": 1}` | `{"expireAfterSeconds": 0}` |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
