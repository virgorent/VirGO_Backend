# Plan — 013 POST /auth/refresh

| Campo | Valor |
|---|---|
| Módulo | `013-post-auth-refresh` |
| Endpoint | `POST /api/v1/auth/refresh` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Renovar sesión |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `003-iniciar-sesion` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/auth/refresh` | Sesión | 200 | 400, 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.2. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Rechaza sesiones revocadas o expiradas.
- Rota el token de sesión en cada uso.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `sessions`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `tokenHash` | string | ✅ | 88 |
| `createdAt` | date | ✅ | — |
| `expiresAt` | date | ✅ | — |
| `purgeAt` | date | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
