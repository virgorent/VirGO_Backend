# Plan — 014 POST /auth/email/verify

| Campo | Valor |
|---|---|
| Módulo | `014-post-auth-email-verify` |
| Endpoint | `POST /api/v1/auth/email/verify` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Verificar correo |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `002-registro` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/auth/email/verify` | No | 200 | 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.2. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Actualiza `users.isVerified`.

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
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
