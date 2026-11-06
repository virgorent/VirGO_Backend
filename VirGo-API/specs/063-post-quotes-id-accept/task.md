# Tareas — 063 POST /quotes/:id/accept

| Campo | Valor |
|---|---|
| Módulo | `063-post-quotes-id-accept` |
| Endpoint | `POST /api/v1/quotes/:id/accept` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Respuesta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | `008-cotizaciones-cliente` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T6301 | Implementar `POST /api/v1/quotes/:id/accept` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6302 | Pruebas de repositorio, integración y seguridad | T6301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6303 | PR, despliegue, humo y entrega del contrato a Móvil | T6302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
