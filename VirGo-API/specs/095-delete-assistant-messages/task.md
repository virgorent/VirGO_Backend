# Tareas — 095 DELETE /assistant/messages

| Campo | Valor |
|---|---|
| Módulo | `095-delete-assistant-messages` |
| Endpoint | `DELETE /api/v1/assistant/messages` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Asistente IA |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.15 |
| Consumido por Móvil | `013-asistente-ia` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9501 | Implementar `DELETE /api/v1/assistant/messages` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⛔ |
| T9502 | Pruebas de repositorio, integración y seguridad | T9501 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T9503 | PR, despliegue, humo y entrega del contrato a Móvil | T9502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
