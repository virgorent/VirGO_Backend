# Tareas — 021 Favoritos, perfil y ayuda

| Campo | Valor |
|---|---|
| Módulo | `021-favoritos-perfil` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2101 | Pantalla C38 Favoritos y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2102 | Pantalla C39 Perfil y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2103 | Pantalla C40 Ayuda y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2104 | Repositorio y casos de uso | T2101 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2105 | Pruebas unitarias, de UI y de seguridad | T2103 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2106 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
