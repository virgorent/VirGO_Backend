# Tareas — 017 Pendientes y recordatorios

| Campo | Valor |
|---|---|
| Módulo | `017-pendientes` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1701 | Pantalla C27 Pendientes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1702 | Pantalla C29 Recordatorios y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1703 | Pantalla C28 Línea de tiempo y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1704 | Repositorio y casos de uso | T1701 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1705 | Pruebas unitarias, de UI y de seguridad | T1703 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1706 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
