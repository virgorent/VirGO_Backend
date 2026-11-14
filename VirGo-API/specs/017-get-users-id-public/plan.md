# Plan — 017 GET /users/:id/public

| Campo | Valor |
|---|---|
| Módulo | `017-get-users-id-public` |
| Endpoint | `GET /api/v1/users/:id/public` |
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
| `GET` | `/api/v1/users/:id/public` | Opcional | 200 | 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.3. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

Respuesta 200:

```json
{
  "id": "8f6b8b48-f5d0-4f65-8fb0-2a7f0f6ab4b1",
  "name": "VirGo Demo",
  "role": "client",
  "isVerified": false
}
```

## 2. Reglas

- El perfil público no expone datos privados.
- `id` debe ser un UUID; valores inválidos se tratan como inexistentes.
- La consulta usa igualdad sobre `_id`; nunca interpreta el parámetro como filtro MongoDB.

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
