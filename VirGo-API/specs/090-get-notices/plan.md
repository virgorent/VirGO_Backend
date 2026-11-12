# Plan — 090 GET /notices

| Campo | Valor |
|---|---|
| Módulo | `090-get-notices` |
| Endpoint | `GET /api/v1/notices` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Notificaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.14 |
| Consumido por Móvil | `005-explorar-salones`, `020-comunicacion` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/notices` | Sí | 200 | 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.14. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Se borran solos a los 90 días.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `notices`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `kind` | string, enum: booking, payment, cancellation, quote, message, reminder | ✅ | — |
| `title` | string | ✅ | 150 |
| `body` | string | ✅ | 500 |
| `link` | string | ✅ | 500 |
| `read` | bool | ✅ | — |
| `createdAt` | date | ✅ | — |
| `purgeAt` | date | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
