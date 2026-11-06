# Tareas — 019 Compartir y calificar

| Campo | Valor |
|---|---|
| Módulo | `019-compartir-calificar` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1901 | Pantalla C30 Compartir evento y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1902 | Pantalla C34 Calificar y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1903 | Repositorio y casos de uso | T1901 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1904 | Pruebas unitarias, de UI y de seguridad | T1902 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1905 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
