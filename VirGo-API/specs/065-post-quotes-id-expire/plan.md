# Plan — 065 POST /quotes/:id/expire

| Campo | Valor |
|---|---|
| Módulo | `065-post-quotes-id-expire` |
| Endpoint | `POST /api/v1/quotes/:id/expire` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Respuesta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | proceso interno |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/quotes/:id/expire` | Interno | 200 | 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.8. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Vigencia de 15 días (C7).

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `quotes`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `clientId` | string | ✅ | — |
| `clientName` | string | ✅ | 100 |
| `target` | object | ✅ | — |
| `eventId` | string | ➖ | — |
| `eventType` | string, enum: Boda, XV Años, Corporativo, Cumpleaños, Otro | ✅ | — |
| `eventDate` | string | ✅ | — |
| `guests` | int | ✅ | — |
| `requestedServices` | array | ➖ | — |
| `message` | string | ➖ | 2000 |
| `status` | string, enum: sent, answered, accepted, rejected, expired | ✅ | — |
| `response` | object | ➖ | — |
| `createdAt` | date | ✅ | — |
| `updatedAt` | date | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
