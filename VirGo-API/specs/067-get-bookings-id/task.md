# Tareas — 067 GET /bookings/:id

| Campo | Valor |
|---|---|
| Módulo | `067-get-bookings-id` |
| Endpoint | `GET /api/v1/bookings/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Consulta de reservaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `009-reservar-pagar`, `010-reservaciones` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T6701 | Implementar `GET /api/v1/bookings/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6702 | Pruebas de repositorio, integración y seguridad | T6701 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6703 | PR, despliegue, humo y entrega del contrato a Móvil | T6702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
