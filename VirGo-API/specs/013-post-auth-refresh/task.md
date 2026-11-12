# Tareas — 013 POST /auth/refresh

| Campo | Valor |
|---|---|
| Módulo | `013-post-auth-refresh` |
| Endpoint | `POST /api/v1/auth/refresh` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Renovar sesión |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `003-iniciar-sesion` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1301 | Implementar `POST /api/v1/auth/refresh` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T1302 | Pruebas de repositorio, integración y seguridad | T1301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T1303 | PR, despliegue, humo y entrega del contrato a Móvil | T1302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
