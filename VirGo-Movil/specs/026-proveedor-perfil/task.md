# Tareas — 026 Perfil del proveedor

| Campo | Valor |
|---|---|
| Módulo | `026-proveedor-perfil` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Proveedor |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2601 | Pantalla S2 Crear o editar el perfil y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2602 | Pantalla S6 Galería de trabajos y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2603 | Pantalla S7 Zonas de cobertura y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2604 | Repositorio y casos de uso | T2601 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2605 | Pruebas unitarias, de UI y de seguridad | T2603 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2606 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
