# Plan — 044 POST /events

| Campo | Valor |
|---|---|
| Módulo | `044-post-events` |
| Endpoint | `POST /api/v1/events` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `007-crear-evento` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/events` | Sí | 201 | 400, 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.7. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- `venue` se guarda como copia validada contra un salón publicado.
- `budgetPlan`, `reminders`, `vendors`, `externalPayments`, `design` e `invitation` viajan dentro del evento.

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
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
