# Tareas — 055 DELETE /events/:id/tasks/:taskId

| Campo | Valor |
|---|---|
| Módulo | `055-delete-events-id-tasks-taskid` |
| Endpoint | `DELETE /api/v1/events/:id/tasks/:taskId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Pendientes del evento |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `017-pendientes` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T5501 | Implementar `DELETE /api/v1/events/:id/tasks/:taskId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⏳ |
| T5502 | Pruebas de repositorio, integración y seguridad | T5501 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5503 | PR, despliegue, humo y entrega del contrato a Móvil | T5502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
