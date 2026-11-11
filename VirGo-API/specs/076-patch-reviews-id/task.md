# Tareas — 076 PATCH /reviews/:id

| Campo | Valor |
|---|---|
| Módulo | `076-patch-reviews-id` |
| Endpoint | `PATCH /api/v1/reviews/:id` |
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
| T7601 | Implementar `PATCH /api/v1/reviews/:id` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⛔ |
| T7602 | Pruebas de repositorio, integración y seguridad | T7601 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T7603 | PR, despliegue, humo y entrega del contrato a Móvil | T7602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
