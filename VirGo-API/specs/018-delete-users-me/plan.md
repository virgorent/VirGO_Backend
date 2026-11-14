# Plan — 018 DELETE /users/me

| Campo | Valor |
|---|---|
| Módulo | `018-delete-users-me` |
| Endpoint | `DELETE /api/v1/users/me` |
| Versión | 1.0.0 |
| Estado | En implementación |
| Fecha | 2026-11-05 |
| Grupo | Perfil de usuario |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.3 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `DELETE` | `/api/v1/users/me` | Sí | 204 | 401, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.3. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Anonimizar `name`, `email`, `phone` y `passwordHash`.
- Usar un correo determinista `deleted+<userId>@invalid.virgo` para conservar la unicidad.
- Reemplazar el teléfono por un valor neutro válido para el schema.
- Desactivar verificación y preferencias personales; conservar `id`, `role` y fechas históricas.
- Revocar todas las sesiones antes de devolver 204.
- No borrar reservas ni reseñas que necesiten conservar referencias históricas.

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

La operación no cambia el schema de MongoDB; usa campos existentes y elimina referencias opcionales de proveedor, ubicación y preferencias.

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
