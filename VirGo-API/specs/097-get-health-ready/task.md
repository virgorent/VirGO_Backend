# Tareas — 097 GET /health/ready

| Campo | Valor |
|---|---|
| Módulo | `097-get-health-ready` |
| Endpoint | `GET /api/v1/health/ready` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Salud y operación |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.17 |
| Consumido por Móvil | uso operativo |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9701 | Implementar `GET /api/v1/health/ready` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T9702 | Pruebas de repositorio, integración y seguridad | T9701 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T9703 | PR, despliegue, humo y entrega del contrato a Móvil | T9702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
