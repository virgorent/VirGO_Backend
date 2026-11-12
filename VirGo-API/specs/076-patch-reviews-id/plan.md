# Plan — 076 PATCH /reviews/:id

| Campo | Valor |
|---|---|
| Módulo | `076-patch-reviews-id` |
| Endpoint | `PATCH /api/v1/reviews/:id` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `019-compartir-calificar` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `PATCH` | `/api/v1/reviews/:id` | Sí | 200 | 400, 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.10. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Sin reglas adicionales a las del Art. IV.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `reviews`

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

### `venues`

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

### `providers`

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

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
