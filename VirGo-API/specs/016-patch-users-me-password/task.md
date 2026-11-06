# Tareas — 016 PATCH /users/me/password

| Campo | Valor |
|---|---|
| Módulo | `016-patch-users-me-password` |
| Endpoint | `PATCH /api/v1/users/me/password` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Perfil de usuario |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.3 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1601 | Implementar `PATCH /api/v1/users/me/password` | `000`, `010` | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⏳ |
| T1602 | Pruebas de repositorio, integración y seguridad | T1601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T1603 | PR, despliegue, humo y entrega del contrato a Móvil | T1602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
