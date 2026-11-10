# Tareas — 001 Inicio para visitantes

| Campo | Valor |
|---|---|
| Módulo | `001-inicio-visitante` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T101 | Pantalla A1 Inicio para visitantes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T102 | Pantalla A3 Vista previa del salón y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T103 | Repositorio y casos de uso | T101 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T104 | Pruebas unitarias, de UI y de seguridad | T102 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T105 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
