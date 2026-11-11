# Tareas — 053 POST /events/:id/tasks

| Campo | Valor |
|---|---|
| Módulo | `053-post-events-id-tasks` |
| Endpoint | `POST /api/v1/events/:id/tasks` |
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
| T5301 | Implementar `POST /api/v1/events/:id/tasks` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T5302 | Pruebas de repositorio, integración y seguridad | T5301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5303 | PR, despliegue, humo y entrega del contrato a Móvil | T5302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
