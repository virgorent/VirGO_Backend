# Tareas — 041 PATCH /providers/me/gigs/:id

| Campo | Valor |
|---|---|
| Módulo | `041-patch-providers-me-gigs-id` |
| Endpoint | `PATCH /api/v1/providers/me/gigs/:id` |
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
| T4101 | Implementar `PATCH /api/v1/providers/me/gigs/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T4102 | Pruebas de repositorio, integración y seguridad | T4101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T4103 | PR, despliegue, humo y entrega del contrato a Móvil | T4102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
