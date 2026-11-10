# Tareas — 066 GET /bookings

| Campo | Valor |
|---|---|
| Módulo | `066-get-bookings` |
| Endpoint | `GET /api/v1/bookings` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Consulta de reservaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `010-reservaciones`, `023-propietario-inicio-calendario`, `025-propietario-herramientas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T6601 | Implementar `GET /api/v1/bookings` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T6602 | Pruebas de repositorio, integración y seguridad | T6601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T6603 | PR, despliegue, humo y entrega del contrato a Móvil | T6602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
