# Tareas — 032 POST /venues/:id/blocks

| Campo | Valor |
|---|---|
| Módulo | `032-post-venues-id-blocks` |
| Endpoint | `POST /api/v1/venues/:id/blocks` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Disponibilidad de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `023-propietario-inicio-calendario` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T3201 | Implementar `POST /api/v1/venues/:id/blocks` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T3202 | Pruebas de repositorio, integración y seguridad | T3201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3203 | PR, despliegue, humo y entrega del contrato a Móvil | T3202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
