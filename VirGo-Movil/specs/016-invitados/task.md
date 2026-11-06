# Tareas — 016 Invitados e invitación

| Campo | Valor |
|---|---|
| Módulo | `016-invitados` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1601 | Pantalla D32 Invitados y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1602 | Pantalla C31 Invitación digital y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1603 | Repositorio y casos de uso | T1601 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1604 | Pruebas unitarias, de UI y de seguridad | T1602 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1605 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
