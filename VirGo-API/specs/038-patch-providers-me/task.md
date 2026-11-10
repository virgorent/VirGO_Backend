# Tareas — 038 PATCH /providers/me

| Campo | Valor |
|---|---|
| Módulo | `038-patch-providers-me` |
| Endpoint | `PATCH /api/v1/providers/me` |
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
| T3801 | Implementar `PATCH /api/v1/providers/me` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T3802 | Pruebas de repositorio, integración y seguridad | T3801 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3803 | PR, despliegue, humo y entrega del contrato a Móvil | T3802 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
