# Plan — 039 GET /providers/me/gigs

| Campo | Valor |
|---|---|
| Módulo | `039-get-providers-me-gigs` |
| Endpoint | `GET /api/v1/providers/me/gigs` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Agenda del proveedor |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `027-proveedor-inicio-agenda` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/providers/me/gigs` | Sí | 200 | 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.6. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Solo sobre la agenda propia.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `gigs`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `providerId` | string | ✅ | — |
| `eventName` | string | ✅ | 100 |
| `clientName` | string | ✅ | 100 |
| `place` | string | ✅ | 200 |
| `date` | string | ✅ | — |
| `start` | string | ✅ | 5 |
| `end` | string | ✅ | 5 |
| `amount` | decimal | ➖ | — |
| `quoteId` | string | ➖ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
