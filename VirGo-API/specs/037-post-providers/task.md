# Tareas — 037 POST /providers

| Campo | Valor |
|---|---|
| Módulo | `037-post-providers` |
| Endpoint | `POST /api/v1/providers` |
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
| T3701 | Implementar `POST /api/v1/providers` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T3702 | Pruebas de repositorio, integración y seguridad | T3701 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3703 | PR, despliegue, humo y entrega del contrato a Móvil | T3702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
