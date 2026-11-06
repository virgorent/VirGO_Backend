# Tareas — 028 Solicitudes y mensajes

| Campo | Valor |
|---|---|
| Módulo | `028-proveedor-solicitudes` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Proveedor |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2801 | Pantalla S4 Solicitudes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2802 | Pantalla S5 Crear cotización y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2803 | Pantalla S10 Mensajes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2804 | Repositorio y casos de uso | T2801 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2805 | Pruebas unitarias, de UI y de seguridad | T2803 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2806 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
