# Tareas — 047 DELETE /events/:id

| Campo | Valor |
|---|---|
| Módulo | `047-delete-events-id` |
| Endpoint | `DELETE /api/v1/events/:id` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `014-mi-evento` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4701 | Implementar `DELETE /api/v1/events/:id` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⛔ |
| T4702 | Pruebas de repositorio, integración y seguridad | T4701 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T4703 | PR, despliegue, humo y entrega del contrato a Móvil | T4702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
