# Tareas — 033 DELETE /venues/:id/blocks/:blockId

| Campo | Valor |
|---|---|
| Módulo | `033-delete-venues-id-blocks-blockid` |
| Endpoint | `DELETE /api/v1/venues/:id/blocks/:blockId` |
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
| T3301 | Implementar `DELETE /api/v1/venues/:id/blocks/:blockId` | `000`, `010` | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⏳ |
| T3302 | Pruebas de repositorio, integración y seguridad | T3301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3303 | PR, despliegue, humo y entrega del contrato a Móvil | T3302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
