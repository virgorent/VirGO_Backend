# Plan — 010 Asistente y contacto

| Campo | Valor |
|---|---|
| Módulo | `010-soporte` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Diseño

La columna «Máx. propuesto» está pendiente de `001-contrato-con-api` P-05.

### `assistantMessages`

**Specs de API que la usan:** `093`, `094`, `095`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `role` | string, enum: user, assistant | ✅ | — |
| `text` | string | ✅ | 4000 |
| `createdAt` | date | ✅ | — |
| `purgeAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `userId_1_createdAt_-1` | `{"userId": 1, "createdAt": -1}` | — |
| `purgeAt_ttl` | `{"purgeAt": 1}` | `{"expireAfterSeconds": 0}` |
### `contactMessages`

**Specs de API que la usan:** `096`.

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ➖ | — |
| `name` | string | ✅ | 100 |
| `email` | string | ✅ | 150 |
| `message` | string | ✅ | 2000 |
| `createdAt` | date | ✅ | — |

| Índice | Llave | Opciones |
|---|---|---|
| `createdAt_-1` | `{"createdAt": -1}` | — |

## 2. Costo

$0: Atlas M0.

## 3. Pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |
