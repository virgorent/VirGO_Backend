# Plan — 025 GET /venues/mine

| Campo | Valor |
|---|---|
| Módulo | `025-get-venues-mine` |
| Endpoint | `GET /api/v1/venues/mine` |
| Versión | 1.0.0 |
| Estado | Implementación local; despliegue pendiente |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `025-propietario-herramientas` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/venues/mine` | Sí | 200 | 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.5. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Solo el rol `owner` y solo sobre sus salones.
- La respuesta incluye el estado del salón; el `ownerId` no se expone.
- La consulta filtra por el `sub` del token y nunca acepta un propietario desde la URL o el cuerpo.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

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

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
