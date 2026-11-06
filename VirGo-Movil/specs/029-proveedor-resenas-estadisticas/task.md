# Tareas — 029 Reseñas y estadísticas

| Campo | Valor |
|---|---|
| Módulo | `029-proveedor-resenas-estadisticas` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Proveedor |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2901 | Pantalla S8 Reseñas y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2902 | Pantalla S9 Estadísticas y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2903 | Repositorio y casos de uso | T2901 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2904 | Pruebas unitarias, de UI y de seguridad | T2902 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2905 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
