# Plan — 015 PATCH /users/me

| Campo | Valor |
|---|---|
| Módulo | `015-patch-users-me` |
| Endpoint | `PATCH /api/v1/users/me` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Perfil de usuario |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.3 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `PATCH` | `/api/v1/users/me` | Sí | 200 | 400, 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.3. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- No permite cambiar `role`, `providerId`, `isVerified` ni `createdAt`.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `users`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `name` | string | ✅ | 100 |
| `email` | string | ✅ | 150 |
| `passwordHash` | string | ✅ | 255 |
| `role` | string, enum: client, owner, provider | ✅ | — |
| `phone` | string | ✅ | 16 |
| `providerId` | string | ➖ | — |
| `isVerified` | bool | ✅ | — |
| `createdAt` | date | ✅ | — |
| `updatedAt` | date | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
