# Plan — 045 GET /events/:id

| Campo | Valor |
|---|---|
| Módulo | `045-get-events-id` |
| Endpoint | `GET /api/v1/events/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `014-mi-evento`, `015-presupuesto` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/events/:id` | Sí | 200 | 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.7. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Sin reglas adicionales a las del Art. IV.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `events`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `ownerId` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `eventType` | string, enum: Boda, XV Años, Corporativo, Cumpleaños, Otro | ✅ | — |
| `date` | string | ✅ | — |
| `guests` | int | ✅ | — |
| `budget` | decimal | ✅ | — |
| `hours` | int | ✅ | — |
| `venue` | object | ➖ | — |
| `design` | object | ➖ | — |
| `budgetPlan` | object | ➖ | — |
| `invitation` | object | ➖ | — |
| `reminders` | array | ➖ | — |
| `vendors` | array | ➖ | — |
| `externalPayments` | array | ➖ | — |
| `collaborators` | array | ➖ | — |
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
