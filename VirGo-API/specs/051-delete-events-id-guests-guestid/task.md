# Tareas — 051 DELETE /events/:id/guests/:guestId

| Campo | Valor |
|---|---|
| Módulo | `051-delete-events-id-guests-guestid` |
| Endpoint | `DELETE /api/v1/events/:id/guests/:guestId` |
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
| T5101 | Implementar `DELETE /api/v1/events/:id/guests/:guestId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⏳ |
| T5102 | Pruebas de repositorio, integración y seguridad | T5101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5103 | PR, despliegue, humo y entrega del contrato a Móvil | T5102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
