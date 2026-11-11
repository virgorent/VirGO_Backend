# Tareas — 096 POST /contact

| Campo | Valor |
|---|---|
| Módulo | `096-post-contact` |
| Endpoint | `POST /api/v1/contact` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Contacto |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.16 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9601 | Implementar `POST /api/v1/contact` | `000`, `010` | `dev-dotnet` | Prueba de integración: 201 (CA-01) | ⏳ |
| T9602 | Pruebas de repositorio, integración y seguridad | T9601 | `qa` + `seguridad` | Todos los CA en verde | ⏳ |
| T9603 | PR, despliegue, humo y entrega del contrato a Móvil | T9602 | `devops-aws` + `arquitecto` | Humo por HTTPS; `check.md` completo | ⏳ |
