# Tareas — 062 POST /quotes/:id/response

| Campo | Valor |
|---|---|
| Módulo | `062-post-quotes-id-response` |
| Endpoint | `POST /api/v1/quotes/:id/response` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Respuesta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | `024-propietario-solicitudes`, `028-proveedor-solicitudes` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T6201 | Implementar `POST /api/v1/quotes/:id/response` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6202 | Pruebas de repositorio, integración y seguridad | T6201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6203 | PR, despliegue, humo y entrega del contrato a Móvil | T6202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
