# Tareas — 040 POST /providers/me/gigs

| Campo | Valor |
|---|---|
| Módulo | `040-post-providers-me-gigs` |
| Endpoint | `POST /api/v1/providers/me/gigs` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Agenda del proveedor |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `027-proveedor-inicio-agenda` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4001 | Implementar `POST /api/v1/providers/me/gigs` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T4002 | Pruebas de repositorio, integración y seguridad | T4001 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T4003 | PR, despliegue, humo y entrega del contrato a Móvil | T4002 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
