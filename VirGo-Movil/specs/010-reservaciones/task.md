# Tareas — 010 Mis reservaciones

| Campo | Valor |
|---|---|
| Módulo | `010-reservaciones` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1001 | Pantalla C32 Reservaciones y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1002 | Pantalla C33 Gestionar reservación y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1003 | Repositorio y casos de uso | T1001 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1004 | Pruebas unitarias, de UI y de seguridad | T1002 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1005 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
