# Tareas — 026 POST /venues

| Campo | Valor |
|---|---|
| Módulo | `026-post-venues` |
| Endpoint | `POST /api/v1/venues` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `022-propietario-registrar-salon` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2601 | Implementar `POST /api/v1/venues` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T2602 | Pruebas de repositorio, integración y seguridad | T2601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2603 | PR, despliegue, humo y entrega del contrato a Móvil | T2602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
