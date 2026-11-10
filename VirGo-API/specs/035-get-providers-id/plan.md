# Plan — 035 GET /providers/:id

| Campo | Valor |
|---|---|
| Módulo | `035-get-providers-id` |
| Endpoint | `GET /api/v1/providers/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Catálogo de proveedores |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `011-proveedores-cliente` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/providers/:id` | No | 200 | 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.6. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Sin reglas adicionales a las del Art. IV.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `providers`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `businessName` | string | ✅ | 100 |
| `category` | string, enum: Banquete, Fotografía, Video, Música, Flores, Pastelería, Mobiliario, Animación, Maquillaje | ✅ | — |
| `description` | string | ✅ | 2000 |
| `basePrice` | decimal | ✅ | — |
| `zones` | array | ✅ | — |
| `packages` | array | ✅ | — |
| `gallery` | array | ✅ | — |
| `coverUrl` | string | ➖ | 500 |
| `rating` | object | ✅ | — |
| `status` | string, enum: draft, published | ✅ | — |
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
