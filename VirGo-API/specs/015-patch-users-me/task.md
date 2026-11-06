# Tareas — 015 PATCH /users/me

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
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1501 | Implementar `PATCH /api/v1/users/me` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T1502 | Pruebas de repositorio, integración y seguridad | T1501 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T1503 | PR, despliegue, humo y entrega del contrato a Móvil | T1502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
