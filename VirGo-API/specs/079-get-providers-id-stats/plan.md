# Plan — 079 GET /providers/:id/stats

| Campo | Valor |
|---|---|
| Módulo | `079-get-providers-id-stats` |
| Endpoint | `GET /api/v1/providers/:id/stats` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Estadísticas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.11 |
| Consumido por Móvil | `029-proveedor-resenas-estadisticas` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/providers/:id/stats` | Sí | 200 | 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.11. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Solo el dueño las consulta.
- `visits` queda fuera del MVP.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `monthlyStats`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `targetType` | string, enum: venue, provider | ✅ | — |
| `targetId` | string | ✅ | — |
| `month` | string | ✅ | — |
| `visits` | int | ✅ | — |
| `requests` | int | ✅ | — |
| `bookings` | int | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
