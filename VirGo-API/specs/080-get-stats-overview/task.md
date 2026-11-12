# Tareas — 080 GET /stats/overview

| Campo | Valor |
|---|---|
| Módulo | `080-get-stats-overview` |
| Endpoint | `GET /api/v1/stats/overview` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Estadísticas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.11 |
| Consumido por Móvil | `023-propietario-inicio-calendario`, `027-proveedor-inicio-agenda` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T8001 | Implementar `GET /api/v1/stats/overview` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T8002 | Pruebas de repositorio, integración y seguridad | T8001 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8003 | PR, despliegue, humo y entrega del contrato a Móvil | T8002 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
