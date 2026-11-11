# Plan — 006 Cotizaciones

| Campo | Valor |
|---|---|
| Módulo | `006-cotizaciones` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `quotes`

**Specs de API que la usan:** `008`, `060`, `061`, `062`, `063`, `064`, `065`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `clientId` | string | ✅ | — |
| `clientName` | string | ✅ | 100 |
| `target` | object | ✅ | — |
| `eventId` | string | ➖ | — |
| `eventType` | string, enum: Boda, XV Años, Corporativo, Cumpleaños, Otro | ✅ | — |
| `eventDate` | string | ✅ | — |
| `guests` | int | ✅ | — |
| `requestedServices` | array | ➖ | — |
| `message` | string | ➖ | 2000 |
| `status` | string, enum: sent, answered, accepted, rejected, expired | ✅ | — |
| `response` | object | ➖ | — |
| `createdAt` | date | ✅ | — |
| `updatedAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `clientId_1_createdAt_-1` | `{"clientId": 1, "createdAt": -1}` | — |
| `targetType_1_targetId_1_status_1_createdAt_-1` | `{"target.type": 1, "target.id": 1, "status": 1, "createdAt": -1}` | — |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
