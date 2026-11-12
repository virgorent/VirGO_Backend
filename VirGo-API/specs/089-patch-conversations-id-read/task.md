# Tareas — 089 PATCH /conversations/:id/read

| Campo | Valor |
|---|---|
| Módulo | `089-patch-conversations-id-read` |
| Endpoint | `PATCH /api/v1/conversations/:id/read` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Mensajes |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.13 |
| Consumido por Móvil | `020-comunicacion` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T8901 | Implementar `PATCH /api/v1/conversations/:id/read` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T8902 | Pruebas de repositorio, integración y seguridad | T8901 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8903 | PR, despliegue, humo y entrega del contrato a Móvil | T8902 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
