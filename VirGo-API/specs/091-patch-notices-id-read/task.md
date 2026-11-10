# Tareas — 091 PATCH /notices/:id/read

| Campo | Valor |
|---|---|
| Módulo | `091-patch-notices-id-read` |
| Endpoint | `PATCH /api/v1/notices/:id/read` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Notificaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.14 |
| Consumido por Móvil | `020-comunicacion` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9101 | Implementar `PATCH /api/v1/notices/:id/read` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T9102 | Pruebas de repositorio, integración y seguridad | T9101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T9103 | PR, despliegue, humo y entrega del contrato a Móvil | T9102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
