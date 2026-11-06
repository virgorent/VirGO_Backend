# Tareas — 006 Detalle del salón

| Campo | Valor |
|---|---|
| Módulo | `006-detalle-salon` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T601 | Pantalla B13 Detalle del salón y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T602 | Pantalla C13 Tour 360° y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T603 | Pantalla C14 Reseñas del salón y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T604 | Repositorio y casos de uso | T601 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T605 | Pruebas unitarias, de UI y de seguridad | T603 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T606 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
