# Tareas — 036 GET /providers/me

| Campo | Valor |
|---|---|
| Módulo | `036-get-providers-me` |
| Endpoint | `GET /api/v1/providers/me` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Perfil del proveedor |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `026-proveedor-perfil` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T3601 | Implementar `GET /api/v1/providers/me` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T3602 | Pruebas de repositorio, integración y seguridad | T3601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3603 | PR, despliegue, humo y entrega del contrato a Móvil | T3602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
