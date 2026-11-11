# Tareas — 073 GET /providers/:id/reviews

| Campo | Valor |
|---|---|
| Módulo | `073-get-providers-id-reviews` |
| Endpoint | `GET /api/v1/providers/:id/reviews` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `011-proveedores-cliente`, `029-proveedor-resenas-estadisticas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7301 | Implementar `GET /api/v1/providers/:id/reviews` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T7302 | Pruebas de repositorio, integración y seguridad | T7301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7303 | PR, despliegue, humo y entrega del contrato a Móvil | T7302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
