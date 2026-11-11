# Tareas — 086 GET /conversations/:id

| Campo | Valor |
|---|---|
| Módulo | `086-get-conversations-id` |
| Endpoint | `GET /api/v1/conversations/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Mensajes |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.13 |
| Consumido por Móvil | `020-comunicacion` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T8601 | Implementar `GET /api/v1/conversations/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T8602 | Pruebas de repositorio, integración y seguridad | T8601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8603 | PR, despliegue, humo y entrega del contrato a Móvil | T8602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
