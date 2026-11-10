# Tareas — 077 DELETE /reviews/:id

| Campo | Valor |
|---|---|
| Módulo | `077-delete-reviews-id` |
| Endpoint | `DELETE /api/v1/reviews/:id` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `019-compartir-calificar` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7701 | Implementar `DELETE /api/v1/reviews/:id` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⛔ |
| T7702 | Pruebas de repositorio, integración y seguridad | T7701 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T7703 | PR, despliegue, humo y entrega del contrato a Móvil | T7702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
