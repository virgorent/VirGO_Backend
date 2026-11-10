# Tareas — 002 Registro

| Campo | Valor |
|---|---|
| Módulo | `002-registro` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T201 | Pantalla A2 Registro y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T202 | Pantalla A4 Registro con errores y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T203 | Repositorio y casos de uso | T201 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T204 | Pruebas unitarias, de UI y de seguridad | T202 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T205 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
