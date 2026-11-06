# Tareas — 043 GET /events

| Campo | Valor |
|---|---|
| Módulo | `043-get-events` |
| Endpoint | `GET /api/v1/events` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `014-mi-evento` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4301 | Implementar `GET /api/v1/events` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T4302 | Pruebas de repositorio, integración y seguridad | T4301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T4303 | PR, despliegue, humo y entrega del contrato a Móvil | T4302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
