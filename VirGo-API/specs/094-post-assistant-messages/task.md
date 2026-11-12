# Tareas — 094 POST /assistant/messages

| Campo | Valor |
|---|---|
| Módulo | `094-post-assistant-messages` |
| Endpoint | `POST /api/v1/assistant/messages` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Asistente IA |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.15 |
| Consumido por Móvil | `013-asistente-ia` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9401 | Implementar `POST /api/v1/assistant/messages` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T9402 | Pruebas de repositorio, integración y seguridad | T9401 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T9403 | PR, despliegue, humo y entrega del contrato a Móvil | T9402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
