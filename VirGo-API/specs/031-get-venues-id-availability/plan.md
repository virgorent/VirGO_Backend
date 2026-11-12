# Plan — 031 GET /venues/:id/availability

| Campo | Valor |
|---|---|
| Módulo | `031-get-venues-id-availability` |
| Endpoint | `GET /api/v1/venues/:id/availability` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Disponibilidad de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `006-detalle-salon`, `023-propietario-inicio-calendario` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/venues/:id/availability` | No | 200 | 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.5. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- No expone notas ni `bookingId` a personas no autorizadas.

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
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
