# Tareas — 078 GET /venues/:id/stats

| Campo | Valor |
|---|---|
| Módulo | `078-get-venues-id-stats` |
| Endpoint | `GET /api/v1/venues/:id/stats` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Estadísticas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.11 |
| Consumido por Móvil | `025-propietario-herramientas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7801 | Implementar `GET /api/v1/venues/:id/stats` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T7802 | Pruebas de repositorio, integración y seguridad | T7801 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7803 | PR, despliegue, humo y entrega del contrato a Móvil | T7802 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
