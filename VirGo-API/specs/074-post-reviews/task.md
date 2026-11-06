# Tareas — 074 POST /reviews

| Campo | Valor |
|---|---|
| Módulo | `074-post-reviews` |
| Endpoint | `POST /api/v1/reviews` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `019-compartir-calificar` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7401 | Implementar `POST /api/v1/reviews` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T7402 | Pruebas de repositorio, integración y seguridad | T7401 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7403 | PR, despliegue, humo y entrega del contrato a Móvil | T7402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
