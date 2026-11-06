# Tareas — 083 DELETE /favorites/:targetType/:targetId

| Campo | Valor |
|---|---|
| Módulo | `083-delete-favorites-targettype-targetid` |
| Endpoint | `DELETE /api/v1/favorites/:targetType/:targetId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Favoritos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.12 |
| Consumido por Móvil | `005-explorar-salones`, `006-detalle-salon` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T8301 | Implementar `DELETE /api/v1/favorites/:targetType/:targetId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⏳ |
| T8302 | Pruebas de repositorio, integración y seguridad | T8301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8303 | PR, despliegue, humo y entrega del contrato a Móvil | T8302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
