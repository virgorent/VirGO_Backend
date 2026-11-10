# Tareas — 008 Cotizaciones del cliente

| Campo | Valor |
|---|---|
| Módulo | `008-cotizaciones-cliente` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T801 | Pantalla C21 Solicitar cotización y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T802 | Pantalla C22 Mis cotizaciones y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T803 | Pantalla C7 Detalle de la cotización y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T804 | Repositorio y casos de uso | T801 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T805 | Pruebas unitarias, de UI y de seguridad | T803 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T806 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
