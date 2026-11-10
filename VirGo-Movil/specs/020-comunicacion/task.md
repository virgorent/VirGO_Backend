# Tareas — 020 Mensajes y notificaciones

| Campo | Valor |
|---|---|
| Módulo | `020-comunicacion` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2001 | Pantalla C35 Chat y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2002 | Pantalla C36 Mensajes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2003 | Pantalla C37 Notificaciones y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2004 | Repositorio y casos de uso | T2001 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2005 | Pruebas unitarias, de UI y de seguridad | T2003 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2006 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
