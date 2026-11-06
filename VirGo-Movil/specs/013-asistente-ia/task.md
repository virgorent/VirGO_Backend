# Tareas — 013 Asistente IA

| Campo | Valor |
|---|---|
| Módulo | `013-asistente-ia` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1301 | Pantalla C18 Asistente IA y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1302 | Repositorio y casos de uso | T1301 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1303 | Pruebas unitarias, de UI y de seguridad | T1301 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1304 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
