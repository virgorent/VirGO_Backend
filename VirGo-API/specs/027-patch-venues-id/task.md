# Tareas — 027 PATCH /venues/:id

| Campo | Valor |
|---|---|
| Módulo | `027-patch-venues-id` |
| Endpoint | `PATCH /api/v1/venues/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `022-propietario-registrar-salon`, `025-propietario-herramientas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2701 | Implementar `PATCH /api/v1/venues/:id` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T2702 | Pruebas de repositorio, integración y seguridad | T2701 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2703 | PR, despliegue, humo y entrega del contrato a Móvil | T2702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
