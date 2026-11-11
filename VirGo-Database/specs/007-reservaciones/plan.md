# Plan — 007 Reservaciones y folios

| Campo | Valor |
|---|---|
| Módulo | `007-reservaciones` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `bookings`

**Specs de API que la usan:** `009`, `066`, `067`, `068`, `069`, `070`, `071`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `folio` | string | ✅ | — |
| `clientId` | string | ✅ | — |
| `clientName` | string | ✅ | 100 |
| `eventId` | string | ➖ | — |
| `quoteId` | string | ➖ | — |
| `venue` | object | ✅ | — |
| `date` | string | ✅ | — |
| `hours` | int | ✅ | — |
| `total` | decimal | ✅ | — |
| `deposit` | decimal | ✅ | — |
| `status` | string, enum: pending_payment, confirmed, cancelled | ✅ | — |
| `extras` | array | ➖ | — |
| `payments` | array | ✅ | — |
| `cancellation` | object | ➖ | — |
| `rescheduledFrom` | string | ➖ | 10 |
| `createdAt` | date | ✅ | — |
| `updatedAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `folio_1_unique` | `{"folio": 1}` | `{"unique": true}` |
| `clientId_1_createdAt_-1` | `{"clientId": 1, "createdAt": -1}` | — |
| `venueId_1_date_1` | `{"venue.venueId": 1, "date": 1}` | — |
### `counters`

**Specs de API que la usan:** `009`, `070`, `071`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `next` | int | ✅ | — |

Solo el índice por defecto de `_id`.

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
