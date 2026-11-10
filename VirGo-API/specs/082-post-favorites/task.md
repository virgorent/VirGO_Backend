# Tareas — 082 POST /favorites

| Campo | Valor |
|---|---|
| Módulo | `082-post-favorites` |
| Endpoint | `POST /api/v1/favorites` |
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
| T8201 | Implementar `POST /api/v1/favorites` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T8202 | Pruebas de repositorio, integración y seguridad | T8201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8203 | PR, despliegue, humo y entrega del contrato a Móvil | T8202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
