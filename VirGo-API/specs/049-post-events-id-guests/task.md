# Tareas — 049 POST /events/:id/guests

| Campo | Valor |
|---|---|
| Módulo | `049-post-events-id-guests` |
| Endpoint | `POST /api/v1/events/:id/guests` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Invitados |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `016-invitados` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4901 | Implementar `POST /api/v1/events/:id/guests` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T4902 | Pruebas de repositorio, integración y seguridad | T4901 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T4903 | PR, despliegue, humo y entrega del contrato a Móvil | T4902 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
