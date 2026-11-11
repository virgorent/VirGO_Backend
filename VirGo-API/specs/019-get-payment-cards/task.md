# Tareas — 019 GET /payment-cards

| Campo | Valor |
|---|---|
| Módulo | `019-get-payment-cards` |
| Endpoint | `GET /api/v1/payment-cards` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Tarjetas de pago |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.4 |
| Consumido por Móvil | `009-reservar-pagar`, `021-favoritos-perfil` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1901 | Implementar `GET /api/v1/payment-cards` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T1902 | Pruebas de repositorio, integración y seguridad | T1901 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T1903 | PR, despliegue, humo y entrega del contrato a Móvil | T1902 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
