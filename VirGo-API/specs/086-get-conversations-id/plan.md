# Plan — 086 GET /conversations/:id

| Campo | Valor |
|---|---|
| Módulo | `086-get-conversations-id` |
| Endpoint | `GET /api/v1/conversations/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Mensajes |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.13 |
| Consumido por Móvil | `020-comunicacion` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/conversations/:id` | Sí | 200 | 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.13. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Solo participan el cliente y el destino.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `conversations`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `clientId` | string | ✅ | — |
| `target` | object | ✅ | — |
| `targetUserId` | string | ✅ | — |
| `quoteId` | string | ➖ | — |
| `lastMessage` | string | ✅ | 200 |
| `lastAt` | date | ✅ | — |

### `messages`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `conversationId` | string | ✅ | — |
| `senderId` | string | ✅ | — |
| `text` | string | ✅ | 2000 |
| `sentAt` | date | ✅ | — |
| `readAt` | date | ➖ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
