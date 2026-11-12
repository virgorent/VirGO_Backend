# Tareas — 017 GET /users/:id/public

| Campo | Valor |
|---|---|
| Módulo | `017-get-users-id-public` |
| Endpoint | `GET /api/v1/users/:id/public` |
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
| T1701 | Implementar `GET /api/v1/users/:id/public` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T1702 | Pruebas de repositorio, integración y seguridad | T1701 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T1703 | PR, despliegue, humo y entrega del contrato a Móvil | T1702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
