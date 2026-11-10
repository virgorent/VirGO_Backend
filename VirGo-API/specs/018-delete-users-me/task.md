# Tareas — 018 DELETE /users/me

| Campo | Valor |
|---|---|
| Módulo | `018-delete-users-me` |
| Endpoint | `DELETE /api/v1/users/me` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Perfil de usuario |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.3 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1801 | Implementar `DELETE /api/v1/users/me` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⛔ |
| T1802 | Pruebas de repositorio, integración y seguridad | T1801 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T1803 | PR, despliegue, humo y entrega del contrato a Móvil | T1802 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
