# Tareas — 022 DELETE /payment-cards/:id

| Campo | Valor |
|---|---|
| Módulo | `022-delete-payment-cards-id` |
| Endpoint | `DELETE /api/v1/payment-cards/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Tarjetas de pago |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.4 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2201 | Implementar `DELETE /api/v1/payment-cards/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 204 (CA-01) | ⏳ |
| T2202 | Pruebas de repositorio, integración y seguridad | T2201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2203 | PR, despliegue, humo y entrega del contrato a Móvil | T2202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
