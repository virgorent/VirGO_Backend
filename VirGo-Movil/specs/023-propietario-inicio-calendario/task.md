# Tareas — 023 Inicio y calendario

| Campo | Valor |
|---|---|
| Módulo | `023-propietario-inicio-calendario` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2301 | Pantalla G54 Inicio y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2302 | Pantalla G58 Calendario y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2303 | Pantalla P9 Bloquear fecha y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2304 | Repositorio y casos de uso | T2301 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2305 | Pruebas unitarias, de UI y de seguridad | T2303 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2306 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
