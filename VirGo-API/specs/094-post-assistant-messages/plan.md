# Plan — 094 POST /assistant/messages

| Campo | Valor |
|---|---|
| Módulo | `094-post-assistant-messages` |
| Endpoint | `POST /api/v1/assistant/messages` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Asistente IA |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.15 |
| Consumido por Móvil | `013-asistente-ia` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/assistant/messages` | Sí | 201 | 400, 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.15. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- El historial se borra solo a los 30 días.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `assistantMessages`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `role` | string, enum: user, assistant | ✅ | — |
| `text` | string | ✅ | 4000 |
| `createdAt` | date | ✅ | — |
| `purgeAt` | date | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
