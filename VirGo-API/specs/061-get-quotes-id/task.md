# Tareas — 061 GET /quotes/:id

| Campo | Valor |
|---|---|
| Módulo | `061-get-quotes-id` |
| Endpoint | `GET /api/v1/quotes/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Consulta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | `008-cotizaciones-cliente`, `024-propietario-solicitudes`, `028-proveedor-solicitudes` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T6101 | Implementar `GET /api/v1/quotes/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6102 | Pruebas de repositorio, integración y seguridad | T6101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6103 | PR, despliegue, humo y entrega del contrato a Móvil | T6102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
