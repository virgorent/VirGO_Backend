# Tareas — 044 POST /events

| Campo | Valor |
|---|---|
| Módulo | `044-post-events` |
| Endpoint | `POST /api/v1/events` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `007-crear-evento` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4401 | Implementar `POST /api/v1/events` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T4402 | Pruebas de repositorio, integración y seguridad | T4401 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T4403 | PR, despliegue, humo y entrega del contrato a Móvil | T4402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
