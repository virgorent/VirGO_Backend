# Tareas — 023 POST /payment-cards/:id/default

| Campo | Valor |
|---|---|
| Módulo | `023-post-payment-cards-id-default` |
| Endpoint | `POST /api/v1/payment-cards/:id/default` |
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
| T2301 | Implementar `POST /api/v1/payment-cards/:id/default` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T2302 | Pruebas de repositorio, integración y seguridad | T2301 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2303 | PR, despliegue, humo y entrega del contrato a Móvil | T2302 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
