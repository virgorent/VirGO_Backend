# Tareas — 085 POST /conversations

| Campo | Valor |
|---|---|
| Módulo | `085-post-conversations` |
| Endpoint | `POST /api/v1/conversations` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Mensajes |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.13 |
| Consumido por Móvil | `006-detalle-salon`, `020-comunicacion` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T8501 | Implementar `POST /api/v1/conversations` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T8502 | Pruebas de repositorio, integración y seguridad | T8501 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T8503 | PR, despliegue, humo y entrega del contrato a Móvil | T8502 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
