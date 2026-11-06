# Tareas — 059 DELETE /events/:id/collaborators/:userId

| Campo | Valor |
|---|---|
| Módulo | `059-delete-events-id-collaborators-userid` |
| Endpoint | `DELETE /api/v1/events/:id/collaborators/:userId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Colaboradores del evento |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `019-compartir-calificar` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T5901 | Implementar `DELETE /api/v1/events/:id/collaborators/:userId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⏳ |
| T5902 | Pruebas de repositorio, integración y seguridad | T5901 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5903 | PR, despliegue, humo y entrega del contrato a Móvil | T5902 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
