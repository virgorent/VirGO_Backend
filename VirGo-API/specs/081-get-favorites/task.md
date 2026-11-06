# Tareas — 081 GET /favorites

| Campo | Valor |
|---|---|
| Módulo | `081-get-favorites` |
| Endpoint | `GET /api/v1/favorites` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Favoritos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.12 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T8101 | Implementar `GET /api/v1/favorites` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T8102 | Pruebas de repositorio, integración y seguridad | T8101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8103 | PR, despliegue, humo y entrega del contrato a Móvil | T8102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
