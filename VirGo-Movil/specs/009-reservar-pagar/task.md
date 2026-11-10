# Tareas — 009 Reservar y pagar

| Campo | Valor |
|---|---|
| Módulo | `009-reservar-pagar` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T901 | Pantalla C8 Pago del anticipo y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T902 | Pantalla C23 Comprobante de pago y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T903 | Repositorio y casos de uso | T901 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T904 | Pruebas unitarias, de UI y de seguridad | T902 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T905 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
