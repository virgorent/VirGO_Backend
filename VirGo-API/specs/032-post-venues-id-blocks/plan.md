# Plan — 032 POST /venues/:id/blocks

| Campo | Valor |
|---|---|
| Módulo | `032-post-venues-id-blocks` |
| Endpoint | `POST /api/v1/venues/:id/blocks` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Disponibilidad de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `023-propietario-inicio-calendario` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/venues/:id/blocks` | Sí | 201 | 400, 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.5. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- El bloqueo crea `kind: blocked` y no admite `bookingId`.
- Un día ya ocupado responde 409.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `venueDates`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `venueId` | string | ✅ | — |
| `date` | string | ✅ | — |
| `kind` | string, enum: blocked, booked | ✅ | — |
| `reason` | string, enum: maintenance, own_event, other | ➖ | — |
| `note` | string | ➖ | 200 |
| `bookingId` | string | ➖ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
