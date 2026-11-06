# Tareas — 015 Presupuesto

| Campo | Valor |
|---|---|
| Módulo | `015-presupuesto` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1501 | Pantalla C25 Presupuesto y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1502 | Pantalla C24 Presupuesto IA y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T1503 | Repositorio y casos de uso | T1501 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T1504 | Pruebas unitarias, de UI y de seguridad | T1502 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T1505 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
