# Tareas — 029 POST /venues/:id/publish

| Campo | Valor |
|---|---|
| Módulo | `029-post-venues-id-publish` |
| Endpoint | `POST /api/v1/venues/:id/publish` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `022-propietario-registrar-salon` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2901 | Implementar `POST /api/v1/venues/:id/publish` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⛔ |
| T2902 | Pruebas de repositorio, integración y seguridad | T2901 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T2903 | PR, despliegue, humo y entrega del contrato a Móvil | T2902 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
