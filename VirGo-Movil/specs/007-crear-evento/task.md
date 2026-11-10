# Tareas — 007 Crear evento

| Campo | Valor |
|---|---|
| Módulo | `007-crear-evento` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T701 | Pantalla C12 Crear evento y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T702 | Repositorio y casos de uso | T701 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T703 | Pruebas unitarias, de UI y de seguridad | T701 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T704 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
