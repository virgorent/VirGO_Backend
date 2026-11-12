# Tareas — 092 POST /notices/read-all

| Campo | Valor |
|---|---|
| Módulo | `092-post-notices-read-all` |
| Endpoint | `POST /api/v1/notices/read-all` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Notificaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.14 |
| Consumido por Móvil | `020-comunicacion` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9201 | Implementar `POST /api/v1/notices/read-all` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T9202 | Pruebas de repositorio, integración y seguridad | T9201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T9203 | PR, despliegue, humo y entrega del contrato a Móvil | T9202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
