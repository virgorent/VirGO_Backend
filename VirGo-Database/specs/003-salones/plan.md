# Plan — 003 Salones y disponibilidad

| Campo | Valor |
|---|---|
| Módulo | `003-salones` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `venues`

**Specs de API que la usan:** `007`, `024`, `025`, `026`, `027`, `028`, `029`, `030`, `072`, `073`, `074`, `075`, `076`, `077`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `ownerId` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `shortName` | string | ✅ | 20 |
| `description` | string | ✅ | 2000 |
| `address` | string | ✅ | 200 |
| `zone` | string, enum: Norte, Sur, Centro, Oriente, Poniente | ✅ | — |
| `location` | object | ✅ | — |
| `capacity` | int | ✅ | — |
| `price` | object | ✅ | — |
| `amenities` | array | ✅ | — |
| `services` | array | ✅ | — |
| `photos` | array | ✅ | — |
| `setups` | object | ➖ | — |
| `floorPlanUrl` | string | ➖ | 500 |
| `tour360Url` | string | ➖ | 500 |
| `categories` | array | ✅ | — |
| `rating` | object | ✅ | — |
| `verified` | bool | ✅ | — |
| `status` | string, enum: draft, review, published | ✅ | — |
| `allies` | array | ✅ | — |
| `createdAt` | date | ✅ | — |
| `updatedAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `location_2dsphere_status_1` | `{"location": "2dsphere", "status": 1}` | — |
| `ownerId_1` | `{"ownerId": 1}` | — |
| `status_1_zone_1_categories_1` | `{"status": 1, "zone": 1, "categories": 1}` | — |
### `venueDates`

**Specs de API que la usan:** `031`, `032`, `033`, `069`, `070`, `071`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `venueId` | string | ✅ | — |
| `date` | string | ✅ | — |
| `kind` | string, enum: blocked, booked | ✅ | — |
| `reason` | string, enum: maintenance, own_event, other | ➖ | — |
| `note` | string | ➖ | 200 |
| `bookingId` | string | ➖ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `venueId_1_date_1_unique` | `{"venueId": 1, "date": 1}` | `{"unique": true}` |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
