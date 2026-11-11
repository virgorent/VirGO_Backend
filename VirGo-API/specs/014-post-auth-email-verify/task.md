# Tareas — 014 POST /auth/email/verify

| Campo | Valor |
|---|---|
| Módulo | `014-post-auth-email-verify` |
| Endpoint | `POST /api/v1/auth/email/verify` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Verificar correo |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `002-registro` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1401 | Implementar `POST /api/v1/auth/email/verify` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⛔ |
| T1402 | Pruebas de repositorio, integración y seguridad | T1401 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T1403 | PR, despliegue, humo y entrega del contrato a Móvil | T1402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
