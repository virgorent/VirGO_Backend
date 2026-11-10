# Tareas — 070 POST /bookings/:id/payments

| Campo | Valor |
|---|---|
| Módulo | `070-post-bookings-id-payments` |
| Endpoint | `POST /api/v1/bookings/:id/payments` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Pagos de reservación |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `009-reservar-pagar` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7001 | Implementar `POST /api/v1/bookings/:id/payments` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T7002 | Pruebas de repositorio, integración y seguridad | T7001 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7003 | PR, despliegue, humo y entrega del contrato a Móvil | T7002 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
