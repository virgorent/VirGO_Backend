# Tareas — 046 PATCH /events/:id

| Campo | Valor |
|---|---|
| Módulo | `046-patch-events-id` |
| Endpoint | `PATCH /api/v1/events/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `012-ia-studio`, `014-mi-evento`, `015-presupuesto`, `016-invitados`, `017-pendientes`, `018-pagos-evento` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4601 | Implementar `PATCH /api/v1/events/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T4602 | Pruebas de repositorio, integración y seguridad | T4601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T4603 | PR, despliegue, humo y entrega del contrato a Móvil | T4602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
