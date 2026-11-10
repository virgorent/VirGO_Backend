# Tareas — 068 GET /bookings/by-folio/:folio

| Campo | Valor |
|---|---|
| Módulo | `068-get-bookings-by-folio-folio` |
| Endpoint | `GET /api/v1/bookings/by-folio/:folio` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Consulta de reservaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `010-reservaciones` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T6801 | Implementar `GET /api/v1/bookings/by-folio/:folio` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6802 | Pruebas de repositorio, integración y seguridad | T6801 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6803 | PR, despliegue, humo y entrega del contrato a Móvil | T6802 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
