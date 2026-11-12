# Tareas — 034 GET /providers

| Campo | Valor |
|---|---|
| Módulo | `034-get-providers` |
| Endpoint | `GET /api/v1/providers` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Catálogo de proveedores |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `011-proveedores-cliente` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T3401 | Implementar `GET /api/v1/providers` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T3402 | Pruebas de repositorio, integración y seguridad | T3401 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3403 | PR, despliegue, humo y entrega del contrato a Móvil | T3402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
