# Tareas — 012 POST /auth/password/reset

| Campo | Valor |
|---|---|
| Módulo | `012-post-auth-password-reset` |
| Endpoint | `POST /api/v1/auth/password/reset` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Recuperar contraseña |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `004-recuperar-contrasena` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1201 | Implementar `POST /api/v1/auth/password/reset` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T1202 | Pruebas de repositorio, integración y seguridad | T1201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T1203 | PR, despliegue, humo y entrega del contrato a Móvil | T1202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
