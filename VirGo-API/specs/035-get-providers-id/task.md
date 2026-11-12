# Tareas — 035 GET /providers/:id

| Campo | Valor |
|---|---|
| Módulo | `035-get-providers-id` |
| Endpoint | `GET /api/v1/providers/:id` |
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
| T3501 | Implementar `GET /api/v1/providers/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T3502 | Pruebas de repositorio, integración y seguridad | T3501 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3503 | PR, despliegue, humo y entrega del contrato a Móvil | T3502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
