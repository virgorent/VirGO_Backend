# Tareas — 020 POST /payment-cards

| Campo | Valor |
|---|---|
| Módulo | `020-post-payment-cards` |
| Endpoint | `POST /api/v1/payment-cards` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Tarjetas de pago |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.4 |
| Consumido por Móvil | `009-reservar-pagar` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2001 | Implementar `POST /api/v1/payment-cards` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T2002 | Pruebas de repositorio, integración y seguridad | T2001 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2003 | PR, despliegue, humo y entrega del contrato a Móvil | T2002 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
