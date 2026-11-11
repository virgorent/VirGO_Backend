# Tareas — 065 POST /quotes/:id/expire

| Campo | Valor |
|---|---|
| Módulo | `065-post-quotes-id-expire` |
| Endpoint | `POST /api/v1/quotes/:id/expire` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Respuesta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | proceso interno |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T6501 | Implementar `POST /api/v1/quotes/:id/expire` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6502 | Pruebas de repositorio, integración y seguridad | T6501 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6503 | PR, despliegue, humo y entrega del contrato a Móvil | T6502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
