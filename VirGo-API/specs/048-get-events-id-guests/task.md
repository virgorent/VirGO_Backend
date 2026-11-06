# Tareas — 048 GET /events/:id/guests

| Campo | Valor |
|---|---|
| Módulo | `048-get-events-id-guests` |
| Endpoint | `GET /api/v1/events/:id/guests` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Invitados |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `016-invitados` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T4801 | Implementar `GET /api/v1/events/:id/guests` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T4802 | Pruebas de repositorio, integración y seguridad | T4801 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T4803 | PR, despliegue, humo y entrega del contrato a Móvil | T4802 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
