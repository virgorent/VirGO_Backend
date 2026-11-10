# Tareas — 004 Recuperar contraseña

| Campo | Valor |
|---|---|
| Módulo | `004-recuperar-contrasena` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T401 | Pantalla A6 Recuperar contraseña y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T402 | Pantalla A7 Nueva contraseña y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T403 | Repositorio y casos de uso | T401 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T404 | Pruebas unitarias, de UI y de seguridad | T402 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T405 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
