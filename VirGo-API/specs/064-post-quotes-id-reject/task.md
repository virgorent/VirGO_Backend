# Tareas — 064 POST /quotes/:id/reject

| Campo | Valor |
|---|---|
| Módulo | `064-post-quotes-id-reject` |
| Endpoint | `POST /api/v1/quotes/:id/reject` |
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
| T6401 | Implementar `POST /api/v1/quotes/:id/reject` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6402 | Pruebas de repositorio, integración y seguridad | T6401 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6403 | PR, despliegue, humo y entrega del contrato a Móvil | T6402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
