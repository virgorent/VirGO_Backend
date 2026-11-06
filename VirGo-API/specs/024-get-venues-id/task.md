# Tareas — 024 GET /venues/:id

| Campo | Valor |
|---|---|
| Módulo | `024-get-venues-id` |
| Endpoint | `GET /api/v1/venues/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Detalle de salón |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `001-inicio-visitante`, `006-detalle-salon` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2401 | Implementar `GET /api/v1/venues/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T2402 | Pruebas de repositorio, integración y seguridad | T2401 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2403 | PR, despliegue, humo y entrega del contrato a Móvil | T2402 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
