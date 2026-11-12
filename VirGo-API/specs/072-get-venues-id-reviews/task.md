# Tareas — 072 GET /venues/:id/reviews

| Campo | Valor |
|---|---|
| Módulo | `072-get-venues-id-reviews` |
| Endpoint | `GET /api/v1/venues/:id/reviews` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `006-detalle-salon`, `025-propietario-herramientas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T7201 | Implementar `GET /api/v1/venues/:id/reviews` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T7202 | Pruebas de repositorio, integración y seguridad | T7201 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T7203 | PR, despliegue, humo y entrega del contrato a Móvil | T7202 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
