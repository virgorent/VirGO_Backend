# Plan — 008 Reseñas y estadísticas

| Campo | Valor |
|---|---|
| Módulo | `008-reputacion` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `reviews`

**Specs de API que la usan:** `072`, `073`, `074`, `075`, `076`, `077`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `targetType` | string, enum: venue, provider | ✅ | — |
| `targetId` | string | ✅ | — |
| `authorId` | string | ✅ | — |
| `authorName` | string | ✅ | 100 |
| `stars` | int | ✅ | — |
| `text` | string | ✅ | 1000 |
| `bookingId` | string | ➖ | — |
| `reply` | object | ➖ | — |
| `createdAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `targetType_1_targetId_1_createdAt_-1` | `{"targetType": 1, "targetId": 1, "createdAt": -1}` | — |
| `targetType_1_targetId_1_authorId_1_unique` | `{"targetType": 1, "targetId": 1, "authorId": 1}` | `{"unique": true}` |
### `monthlyStats`

**Specs de API que la usan:** `078`, `079`, `080`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `targetType` | string, enum: venue, provider | ✅ | — |
| `targetId` | string | ✅ | — |
| `month` | string | ✅ | — |
| `visits` | int | ✅ | — |
| `requests` | int | ✅ | — |
| `bookings` | int | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `targetType_1_targetId_1_month_1_unique` | `{"targetType": 1, "targetId": 1, "month": 1}` | `{"unique": true}` |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
