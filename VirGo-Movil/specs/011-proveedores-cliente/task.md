# Tareas — 011 Proveedores

| Campo | Valor |
|---|---|
| Módulo | `011-proveedores-cliente` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1101 | Pantalla C19 Catálogo de proveedores y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1102 | Pantalla B18 Comparar proveedores y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1103 | Pantalla C20 Perfil del proveedor y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1104 | Repositorio y casos de uso | T1101 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1105 | Pruebas unitarias, de UI y de seguridad | T1103 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1106 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
