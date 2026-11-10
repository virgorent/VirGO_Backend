# Tareas — 031 GET /venues/:id/availability

| Campo | Valor |
|---|---|
| Módulo | `031-get-venues-id-availability` |
| Endpoint | `GET /api/v1/venues/:id/availability` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Disponibilidad de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `006-detalle-salon`, `023-propietario-inicio-calendario` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T3101 | Implementar `GET /api/v1/venues/:id/availability` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T3102 | Pruebas de repositorio, integración y seguridad | T3101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T3103 | PR, despliegue, humo y entrega del contrato a Móvil | T3102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
