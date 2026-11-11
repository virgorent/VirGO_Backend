# Tareas — 039 GET /providers/me/gigs

| Campo | Valor |
|---|---|
| Módulo | `039-get-providers-me-gigs` |
| Endpoint | `GET /api/v1/providers/me/gigs` |
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
| T3901 | Implementar `GET /api/v1/providers/me/gigs` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T3902 | Pruebas de repositorio, integración y seguridad | T3901 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3903 | PR, despliegue, humo y entrega del contrato a Móvil | T3902 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
