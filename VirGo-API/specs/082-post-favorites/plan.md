# Plan — 082 POST /favorites

| Campo | Valor |
|---|---|
| Módulo | `082-post-favorites` |
| Endpoint | `POST /api/v1/favorites` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Favoritos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.12 |
| Consumido por Móvil | `005-explorar-salones`, `006-detalle-salon` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/favorites` | Sí | 201 | 400, 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.12. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Un destino se guarda una sola vez.
- `name`, `photoUrl` y `rating` son copias.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `favorites`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `targetType` | string, enum: venue, provider | ✅ | — |
| `targetId` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `photoUrl` | string | ✅ | 500 |
| `rating` | number | ✅ | — |
| `createdAt` | date | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
