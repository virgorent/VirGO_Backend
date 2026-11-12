# Plan — 004 Proveedores y agenda

| Campo | Valor |
|---|---|
| Módulo | `004-proveedores` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `providers`

**Specs de API que la usan:** `034`, `035`, `036`, `037`, `038`, `072`, `073`, `074`, `075`, `076`, `077`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `businessName` | string | ✅ | 100 |
| `category` | string, enum: Banquete, Fotografía, Video, Música, Flores, Pastelería, Mobiliario, Animación, Maquillaje | ✅ | — |
| `description` | string | ✅ | 2000 |
| `basePrice` | decimal | ✅ | — |
| `zones` | array | ✅ | — |
| `packages` | array | ✅ | — |
| `gallery` | array | ✅ | — |
| `coverUrl` | string | ➖ | 500 |
| `rating` | object | ✅ | — |
| `status` | string, enum: draft, published | ✅ | — |
| `createdAt` | date | ✅ | — |
| `updatedAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `userId_1_unique` | `{"userId": 1}` | `{"unique": true}` |
| `status_1_category_1_businessName_1` | `{"status": 1, "category": 1, "businessName": 1}` | — |
### `gigs`

**Specs de API que la usan:** `039`, `040`, `041`, `042`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `providerId` | string | ✅ | — |
| `eventName` | string | ✅ | 100 |
| `clientName` | string | ✅ | 100 |
| `place` | string | ✅ | 200 |
| `date` | string | ✅ | — |
| `start` | string | ✅ | 5 |
| `end` | string | ✅ | 5 |
| `amount` | decimal | ➖ | — |
| `quoteId` | string | ➖ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `providerId_1_date_1_start_1` | `{"providerId": 1, "date": 1, "start": 1}` | — |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
