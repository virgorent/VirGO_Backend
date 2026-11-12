# Tareas — 054 PATCH /events/:id/tasks/:taskId

| Campo | Valor |
|---|---|
| Módulo | `054-patch-events-id-tasks-taskid` |
| Endpoint | `PATCH /api/v1/events/:id/tasks/:taskId` |
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
| T5401 | Implementar `PATCH /api/v1/events/:id/tasks/:taskId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T5402 | Pruebas de repositorio, integración y seguridad | T5401 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5403 | PR, despliegue, humo y entrega del contrato a Móvil | T5402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
