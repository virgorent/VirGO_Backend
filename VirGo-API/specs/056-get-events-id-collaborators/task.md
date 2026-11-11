# Tareas — 056 GET /events/:id/collaborators

| Campo | Valor |
|---|---|
| Módulo | `056-get-events-id-collaborators` |
| Endpoint | `GET /api/v1/events/:id/collaborators` |
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
| T5601 | Implementar `GET /api/v1/events/:id/collaborators` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T5602 | Pruebas de repositorio, integración y seguridad | T5601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5603 | PR, despliegue, humo y entrega del contrato a Móvil | T5602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
