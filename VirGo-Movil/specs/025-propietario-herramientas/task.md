# Tareas — 025 Herramientas del propietario

| Campo | Valor |
|---|---|
| Módulo | `025-propietario-herramientas` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T2501 | Pantalla P6 Mis salones y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2502 | Pantalla P10 Paquetes y precios y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2503 | Pantalla P11 Planos y montajes y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2504 | Pantalla P12 Pagos recibidos y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2505 | Pantalla P13 Estadísticas y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2506 | Pantalla P14 Reseñas y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2507 | Pantalla P15 Proveedores aliados y su `ViewModel` | `000`: T017 | `dev-android` | Coincide con la maqueta; textos en `strings.xml` | ⏳ |
| T2508 | Repositorio y casos de uso | T2501 | `dev-android` | Pruebas con MockWebServer en verde | ⏳ |
| T2509 | Pruebas unitarias, de UI y de seguridad | T2507 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T2510 | Integrar con la URL real y hacer la prueba de humo | Specs de API | `qa-movil` | `check.md` completo | ⏳ |
