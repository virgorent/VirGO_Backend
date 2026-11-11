# Tareas — 050 PATCH /events/:id/guests/:guestId

| Campo | Valor |
|---|---|
| Módulo | `050-patch-events-id-guests-guestid` |
| Endpoint | `PATCH /api/v1/events/:id/guests/:guestId` |
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
| T5001 | Implementar `PATCH /api/v1/events/:id/guests/:guestId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T5002 | Pruebas de repositorio, integración y seguridad | T5001 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5003 | PR, despliegue, humo y entrega del contrato a Móvil | T5002 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
