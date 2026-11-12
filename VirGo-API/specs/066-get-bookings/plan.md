# Plan — 066 GET /bookings

| Campo | Valor |
|---|---|
| Módulo | `066-get-bookings` |
| Endpoint | `GET /api/v1/bookings` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Consulta de reservaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `010-reservaciones`, `023-propietario-inicio-calendario`, `025-propietario-herramientas` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/bookings` | Sí | 200 | 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.9. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Sin reglas adicionales a las del Art. IV.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `bookings`

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

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
