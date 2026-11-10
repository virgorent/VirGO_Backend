# Tareas — 087 GET /conversations/:id/messages

| Campo | Valor |
|---|---|
| Módulo | `087-get-conversations-id-messages` |
| Endpoint | `GET /api/v1/conversations/:id/messages` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Mensajes |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.13 |
| Consumido por Móvil | `020-comunicacion`, `024-propietario-solicitudes`, `028-proveedor-solicitudes` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T8701 | Implementar `GET /api/v1/conversations/:id/messages` | `000`, `010` | `dev-dotnet` | Prueba de integración: 200 (CA-01) | ⏳ |
| T8702 | Pruebas de repositorio, integración y seguridad | T8701 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8703 | PR, despliegue, humo y entrega del contrato a Móvil | T8702 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
