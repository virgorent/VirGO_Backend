# Tareas — 027 Inicio y agenda

| Campo | Valor |
|---|---|
| Módulo | `027-proveedor-inicio-agenda` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Proveedor |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2701 | Pantalla S3 Inicio y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2702 | Pantalla H74 Agenda y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2703 | Repositorio y casos de uso | T2701 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2704 | Pruebas unitarias, de UI y de seguridad | T2702 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2705 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
