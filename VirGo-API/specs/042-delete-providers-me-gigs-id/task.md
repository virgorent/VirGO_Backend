# Tareas — 042 DELETE /providers/me/gigs/:id

| Campo | Valor |
|---|---|
| Módulo | `042-delete-providers-me-gigs-id` |
| Endpoint | `DELETE /api/v1/providers/me/gigs/:id` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Agenda del proveedor |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `027-proveedor-inicio-agenda` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4201 | Implementar `DELETE /api/v1/providers/me/gigs/:id` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⛔ |
| T4202 | Pruebas de repositorio, integración y seguridad | T4201 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T4203 | PR, despliegue, humo y entrega del contrato a Móvil | T4202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
