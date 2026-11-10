# Tareas — 012 IA Studio

| Campo | Valor |
|---|---|
| Módulo | `012-ia-studio` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1201 | Pantalla C15 IA Studio – Estilo y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1202 | Pantalla E43 IA Studio – Mesas y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1203 | Pantalla C16 IA Studio – Personal y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1204 | Pantalla C17 IA Studio – Ideas y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1205 | Repositorio y casos de uso | T1201 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1206 | Pruebas unitarias, de UI y de seguridad | T1204 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1207 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
