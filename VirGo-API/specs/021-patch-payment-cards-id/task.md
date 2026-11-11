# Tareas — 021 PATCH /payment-cards/:id

| Campo | Valor |
|---|---|
| Módulo | `021-patch-payment-cards-id` |
| Endpoint | `PATCH /api/v1/payment-cards/:id` |
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
| T2101 | Implementar `PATCH /api/v1/payment-cards/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T2102 | Pruebas de repositorio, integración y seguridad | T2101 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2103 | PR, despliegue, humo y entrega del contrato a Móvil | T2102 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
