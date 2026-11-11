# Tareas — 052 GET /events/:id/tasks

| Campo | Valor |
|---|---|
| Módulo | `052-get-events-id-tasks` |
| Endpoint | `GET /api/v1/events/:id/tasks` |
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
| T5201 | Implementar `GET /api/v1/events/:id/tasks` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T5202 | Pruebas de repositorio, integración y seguridad | T5201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5203 | PR, despliegue, humo y entrega del contrato a Móvil | T5202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
