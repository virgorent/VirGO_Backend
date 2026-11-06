# Tareas — 075 POST /reviews/:id/reply

| Campo | Valor |
|---|---|
| Módulo | `075-post-reviews-id-reply` |
| Endpoint | `POST /api/v1/reviews/:id/reply` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `025-propietario-herramientas`, `029-proveedor-resenas-estadisticas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7501 | Implementar `POST /api/v1/reviews/:id/reply` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T7502 | Pruebas de repositorio, integración y seguridad | T7501 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7503 | PR, despliegue, humo y entrega del contrato a Móvil | T7502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
