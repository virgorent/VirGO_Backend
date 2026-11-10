# Tareas — 005 Explorar salones

| Campo | Valor |
|---|---|
| Módulo | `005-explorar-salones` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T501 | Pantalla B8 Explorar (mapa) y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T502 | Pantalla C9 Explorar (lista) y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T503 | Pantalla C10 Filtros y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T504 | Pantalla C11 Recomendados para ti y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T505 | Repositorio y casos de uso | T501 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T506 | Pruebas unitarias, de UI y de seguridad | T504 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T507 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
