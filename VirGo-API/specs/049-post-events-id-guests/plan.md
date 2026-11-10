# Plan — 049 POST /events/:id/guests

| Campo | Valor |
|---|---|
| Módulo | `049-post-events-id-guests` |
| Endpoint | `POST /api/v1/events/:id/guests` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Invitados |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `016-invitados` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/events/:id/guests` | Sí | 201 | 400, 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.7. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Requiere permiso sobre el evento: `view` para leer y `edit` para escribir.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `eventGuests`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `eventId` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `phone` | string | ✅ | 16 |
| `companions` | int | ✅ | — |
| `status` | string, enum: confirmed, pending, declined | ✅ | — |
| `table` | string | ➖ | 20 |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
