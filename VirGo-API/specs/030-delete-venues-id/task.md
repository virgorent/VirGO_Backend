# Tareas — 030 DELETE /venues/:id

| Campo | Valor |
|---|---|
| Módulo | `030-delete-venues-id` |
| Endpoint | `DELETE /api/v1/venues/:id` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `025-propietario-herramientas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T3001 | Implementar `DELETE /api/v1/venues/:id` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⛔ |
| T3002 | Pruebas de repositorio, integración y seguridad | T3001 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T3003 | PR, despliegue, humo y entrega del contrato a Móvil | T3002 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
