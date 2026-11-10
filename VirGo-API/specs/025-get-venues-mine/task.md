# Tareas — 025 GET /venues/mine

| Campo | Valor |
|---|---|
| Módulo | `025-get-venues-mine` |
| Endpoint | `GET /api/v1/venues/mine` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `025-propietario-herramientas` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2501 | Implementar `GET /api/v1/venues/mine` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T2502 | Pruebas de repositorio, integración y seguridad | T2501 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T2503 | PR, despliegue, humo y entrega del contrato a Móvil | T2502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
