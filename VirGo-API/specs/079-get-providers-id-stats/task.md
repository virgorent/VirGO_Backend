# Tareas — 079 GET /providers/:id/stats

| Campo | Valor |
|---|---|
| Módulo | `079-get-providers-id-stats` |
| Endpoint | `GET /api/v1/providers/:id/stats` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Estadísticas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.11 |
| Consumido por Móvil | `029-proveedor-resenas-estadisticas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7901 | Implementar `GET /api/v1/providers/:id/stats` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T7902 | Pruebas de repositorio, integración y seguridad | T7901 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7903 | PR, despliegue, humo y entrega del contrato a Móvil | T7902 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
