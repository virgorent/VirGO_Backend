# Tareas — 024 Solicitudes y mensajes

| Campo | Valor |
|---|---|
| Módulo | `024-propietario-solicitudes` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2401 | Pantalla P7 Solicitudes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2402 | Pantalla P8 Responder cotización y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2403 | Pantalla P16 Mensajes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2404 | Repositorio y casos de uso | T2401 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2405 | Pruebas unitarias, de UI y de seguridad | T2403 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2406 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
