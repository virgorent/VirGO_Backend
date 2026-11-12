# Plan — 005 Planeación de eventos

| Campo | Valor |
|---|---|
| Módulo | `005-planeacion` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `events`

**Specs de API que la usan:** `043`, `044`, `045`, `046`, `047`, `056`, `057`, `058`, `059`.

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

| Índice | Llave | Opciones |
|---|---|---|
| `ownerId_1_date_1` | `{"ownerId": 1, "date": 1}` | — |
| `collaborators.userId_1_date_1` | `{"collaborators.userId": 1, "date": 1}` | — |
### `eventGuests`

**Specs de API que la usan:** `048`, `049`, `050`, `051`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `eventId` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `phone` | string | ✅ | 16 |
| `companions` | int | ✅ | — |
| `status` | string, enum: confirmed, pending, declined | ✅ | — |
| `table` | string | ➖ | 20 |

| Índice | Llave | Opciones |
|---|---|---|
| `eventId_1_status_1` | `{"eventId": 1, "status": 1}` | — |
### `eventTasks`

**Specs de API que la usan:** `052`, `053`, `054`, `055`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `eventId` | string | ✅ | — |
| `title` | string | ✅ | 150 |
| `dueDate` | string | ✅ | — |
| `done` | bool | ✅ | — |
| `suggested` | bool | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `eventId_1_dueDate_1` | `{"eventId": 1, "dueDate": 1}` | — |
### `favorites`

**Specs de API que la usan:** `081`, `082`, `083`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `targetType` | string, enum: venue, provider | ✅ | — |
| `targetId` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `photoUrl` | string | ✅ | 500 |
| `rating` | number | ✅ | — |
| `createdAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `userId_1_targetType_1_targetId_1_unique` | `{"userId": 1, "targetType": 1, "targetId": 1}` | `{"unique": true}` |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
