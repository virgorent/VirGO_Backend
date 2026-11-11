# Plan — 055 DELETE /events/:id/tasks/:taskId

| Campo | Valor |
|---|---|
| Módulo | `055-delete-events-id-tasks-taskid` |
| Endpoint | `DELETE /api/v1/events/:id/tasks/:taskId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Pendientes del evento |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `017-pendientes` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `DELETE` | `/api/v1/events/:id/tasks/:taskId` | Sí | 204 | 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.7. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Requiere permiso sobre el evento.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `eventTasks`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `eventId` | string | ✅ | — |
| `title` | string | ✅ | 150 |
| `dueDate` | string | ✅ | — |
| `done` | bool | ✅ | — |
| `suggested` | bool | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
