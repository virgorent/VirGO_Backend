# Tareas — 028 POST /venues/:id/submit

| Campo | Valor |
|---|---|
| Módulo | `028-post-venues-id-submit` |
| Endpoint | `POST /api/v1/venues/:id/submit` |
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
| T2801 | Implementar `POST /api/v1/venues/:id/submit` | Decisión del equipo | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⛔ |
| T2802 | Pruebas de repositorio, integración y seguridad | T2801 | `qa` + `seguridad` | Todos los CA en verde | ⛔ |
| T2803 | PR, despliegue, humo y entrega del contrato a Móvil | T2802 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⛔ |
