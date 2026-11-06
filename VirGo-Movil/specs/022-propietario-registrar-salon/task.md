# Tareas — 022 Registrar un salón

| Campo | Valor |
|---|---|
| Módulo | `022-propietario-registrar-salon` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2201 | Pantalla P3 Paso 1: datos y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2202 | Pantalla P4 Paso 2: capacidad y precios y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2203 | Pantalla P5 Paso 3: revisar y publicar y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2204 | Repositorio y casos de uso | T2201 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2205 | Pruebas unitarias, de UI y de seguridad | T2203 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2206 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
