# Tareas — 071 GET /bookings/:id/payments

| Campo | Valor |
|---|---|
| Módulo | `071-get-bookings-id-payments` |
| Endpoint | `GET /api/v1/bookings/:id/payments` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Pagos de reservación |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `018-pagos-evento` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7101 | Implementar `GET /api/v1/bookings/:id/payments` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T7102 | Pruebas de repositorio, integración y seguridad | T7101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7103 | PR, despliegue, humo y entrega del contrato a Móvil | T7102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
