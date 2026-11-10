# Tareas — 057 POST /events/:id/collaborators

| Campo | Valor |
|---|---|
| Módulo | `057-post-events-id-collaborators` |
| Endpoint | `POST /api/v1/events/:id/collaborators` |
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
| T5701 | Implementar `POST /api/v1/events/:id/collaborators` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T5702 | Pruebas de repositorio, integración y seguridad | T5701 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5703 | PR, despliegue, humo y entrega del contrato a Móvil | T5702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
