# Tareas — 058 PATCH /events/:id/collaborators/:userId

| Campo | Valor |
|---|---|
| Módulo | `058-patch-events-id-collaborators-userid` |
| Endpoint | `PATCH /api/v1/events/:id/collaborators/:userId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Colaboradores del evento |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `019-compartir-calificar` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T5801 | Implementar `PATCH /api/v1/events/:id/collaborators/:userId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T5802 | Pruebas de repositorio, integración y seguridad | T5801 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T5803 | PR, despliegue, humo y entrega del contrato a Móvil | T5802 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
