# Tareas — 090 GET /notices

| Campo | Valor |
|---|---|
| Módulo | `090-get-notices` |
| Endpoint | `GET /api/v1/notices` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Notificaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.14 |
| Consumido por Móvil | `005-explorar-salones`, `020-comunicacion` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9001 | Implementar `GET /api/v1/notices` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T9002 | Pruebas de repositorio, integración y seguridad | T9001 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T9003 | PR, despliegue, humo y entrega del contrato a Móvil | T9002 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
