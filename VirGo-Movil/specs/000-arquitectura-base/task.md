# Tareas — 000 Arquitectura base del Móvil

| Campo | Valor |
|---|---|
| Módulo | `000-arquitectura-base` |
| Versión | 1.1.0 |
| Estado | Aprobado. Tareas habilitadas. |
| Fecha | 2026-10-29 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.1.0 |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**[P]** = puede ejecutarse en paralelo con otras tareas [P] de la misma fase.

**🧑 Requiere confirmación humana** = cambia la configuración de GitHub, los secretos o publica un release (Art. XI.5).

---

## F0 · Repositorio y gobernanza

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T001 | Confirmar que `VirGo-Movil/` entró a `main` del repositorio `virgorent/virGO_App` con su `.gitignore` DevSecOps (Art. V.8) y su `README.md` en inglés | — | `seguridad-ci` | `git remote -v` correcto; `git check-ignore local.properties` lo ignora | ⏳ |
| T002 | 🧑 Coordinar con `VirGo-API` T207 el ruleset único `main-protection` del repositorio (PR, 1 revisión, checks requeridos, sin *force push* ni borrado) y la protección de push contra secretos | T001 | `seguridad-ci` | Reglas visibles en GitHub (captura o `gh api`) | ⏳ |
| T003 | [P] Agregar `.editorconfig` (ktlint) y, en la raíz del repositorio, el bloque de `VirGo-Movil` en `.github/dependabot.yml`; la plantilla de PR es común (Gradle agrupado por familia y Actions) | T001 | `seguridad-ci` | Archivos en el repositorio; Dependabot activo | ⏳ |
| T004 | [P] Revisar la alineación de la gobernanza con `verificar-constitucion` | — | `arquitecto-movil` | Resultado «Alineado» en `memoria.md` | ⏳ |
| T037 | [P] Instalar y probar los MCP `figma` (lectura del *canvas* de «Pantallas VirGo») y `github` de `opencode.json`, con tokens en variables de entorno | — | `arquitecto-movil` | `opencode mcp list` los muestra conectados; captura en `memoria.md` | ⏳ |

## F1 · Proyecto Gradle y módulos

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T005 | Crear el proyecto (wrapper de Gradle, `settings.gradle.kts`, `gradle/libs.versions.toml`) con versiones **estables y compatibles** de Kotlin, AGP, KSP, Compose BOM y Hilt; registrar las versiones en `memoria.md` | T001 | `dev-android` | `./gradlew help` sin errores; tabla de versiones en la memoria | ⏳ |
| T006 | *Convention plugins* en `build-logic/convention`: `virgo.android.application`, `virgo.android.library`, `virgo.android.compose`, `virgo.hilt` y `virgo.jvm.library` (SDK 26/36, JDK 17) | T005 | `dev-android` | Un módulo de prueba aplica los plugins y compila | ⏳ |
| T007 | Crear `:app` (`mx.virgo.app`) y `:core:designsystem`, `:core:common`, `:core:domain` (JVM puro), `:core:network`, `:core:data`, `:core:datastore` y `:core:testing`, respetando las dependencias permitidas del plan (sección 4) | T006 | `dev-android` | `./gradlew assembleDebug` OK; `:core:domain` sin dependencias de Android | ⏳ |
| T008 | [P] Configurar ktlint y detekt (`config/detekt.yml`) sin *baseline* de errores | T007 | `dev-android` | `./gradlew ktlintCheck detekt` en verde | ⏳ |
| T009 | [P] Configurar Hilt: `VirGoApplication` (`@HiltAndroidApp`), `MainActivity` (`@AndroidEntryPoint`) y un módulo de *dispatchers* en `:core:common` | T007 | `dev-android` | La app arranca en el emulador sin errores de Hilt | ⏳ |

## F2 · Sistema de diseño

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T010 | Tokens de color claro y oscuro según el plan (sección 6.1), en `Color.kt` y `ColorScheme` | T007 | `ui-designsystem` | Los *previews* muestran la paleta en los dos temas | ⏳ |
| T011 | [P] Tipografía: Manrope y DM Sans en `res/font` con su licencia OFL y `VirGoTypography` con la escala de Material 3 | T007 | `ui-designsystem` | Los *previews* de la escala tipográfica usan las fuentes correctas | ⏳ |
| T012 | Formas, espaciado y `VirGoTheme` (claro, oscuro y el color dinámico desactivado para respetar la marca) | T010, T011 | `ui-designsystem` | `VirGoTheme` se aplica en `MainActivity` | ⏳ |
| T013 | Componentes base: `VirGoButton`, `VirGoTextField`, `VirGoCard`, `VirGoBadge` (incluye «Estimación IA»), `VirGoTopBar`, `VirGoBottomBar`, `EmptyState`, `LoadingState` y `ErrorBanner`, cada uno con `@Preview` en claro y oscuro | T012 | `ui-designsystem` | Catálogo de *previews* completo (CA-10) | ⏳ |
| T014 | Revisión de accesibilidad: contraste AA en los dos temas, objetivos táctiles de 48 dp o más, `contentDescription` y escala de fuente 2.0 | T013 | `ui-designsystem` + `qa-movil` | Tabla de contrastes en `memoria.md`; sin cortes con letra grande (CA-03) | ⏳ |

## F3 · Arranque y navegación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T015 | *Splash* con `core-splashscreen` y la marca VirGo; `strings.xml` base en español | T012 | `dev-android` | Al abrir la app se ve el *splash* (CA-02) | ⏳ |
| T016 | `VirGoNavHost` con rutas tipadas `@Serializable`, extensiones `NavGraphBuilder` por destino, `TopLevelDestination` y `HomePlaceholder` | T009, T013 | `dev-android` | La navegación abre el inicio provisional | ⏳ |
| T017 | `VirGoApp()` raíz: tema, `Scaffold`, barra inferior configurable (oculta en la base) y `SnackbarHost` | T016 | `dev-android` | La app muestra el inicio provisional con el tema de la maqueta (CA-02) | ⏳ |

## F4 · Red, datos y almacenamiento seguro

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T018 | `AppError` y `AppResult` en `:core:common` | T007 | `dev-android` | Compila; las pruebas de T028 cubren cada variante | ⏳ |
| T019 | `:core:network`: OkHttp (timeouts de 10 y 20 s), Retrofit con kotlinx.serialization y `API_BASE_URL` por variante; *release* falla al compilar si la URL no es `https://` | T018 | `dev-android` | `assembleDebug` OK; `assembleRelease` con una URL `http://` falla con un mensaje claro | ⏳ |
| T020 | [P] Interceptores: `CorrelationIdInterceptor`, `AuthInterceptor` (con `TokenStore`) y logging solo en *debug* | T019, T022 | `dev-android` | Las pruebas de MockWebServer verifican los encabezados (T028) | ⏳ |
| T021 | [P] `ProblemDetailsDto`, conversión de error de HTTP o excepción a `AppError`, y `HealthApi` (`GET health`) | T019 | `dev-android` | Pruebas de conversión en verde (T028) | ⏳ |
| T022 | `:core:datastore`: `TokenStore` con DataStore y Tink AEAD con una llave en el Android Keystore; revisión de `seguridad-ci` | T007 | `dev-android` + `seguridad-ci` | Guardar, leer y borrar funcionan; el archivo no tiene el token en claro (CA-07) | ⏳ |
| T023 | `CheckApiHealthUseCase` en `:core:domain` y `HealthRepository` en `:core:data` (mide la latencia y devuelve `AppResult`) | T021 | `dev-android` | Pruebas unitarias en verde (T028) | ⏳ |

## F5 · Diagnóstico de integración

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T024 | `DiagnosticsViewModel` y `DiagnosticsScreen` en el *source set* `debug`: versión de la app, `API_BASE_URL`, estado y latencia de `/health`, y errores convertidos a textos en español | T017, T023 | `dev-android` | Con la API local en marcha muestra «API disponible» (CA-04); con la API apagada, un mensaje claro sin cierre (CA-05) | ⏳ |
| T025 | Menú de desarrollo en la barra superior (solo *debug*) para abrir el diagnóstico | T024 | `dev-android` | El menú no existe en el APK de *release* | ⏳ |

## F6 · Seguridad de la compilación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T026 | `network_security_config` de *debug* (solo `10.0.2.2` en claro) y de *release* (sin tráfico en claro); R8 con sus reglas; firma de *release* desde variables de entorno sin respaldo con la llave de *debug* | T019 | `seguridad-ci` | `assembleRelease` minificado; en *release* una solicitud `http://` se bloquea (CA-06) | ⏳ |
| T027 | Revisión de seguridad de la app: permisos, secretos, logs, almacenamiento y R8 | T022, T026 | `seguridad-ci` | Reporte sin hallazgos críticos ni altos | ⏳ |

## F7 · Pruebas y verificación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T028 | [P] Pruebas unitarias y de red: conversión a `AppError`, interceptores (MockWebServer), `HealthRepository`, `CheckApiHealthUseCase` y `TokenStore` (Robolectric) | T020–T023 | `qa-movil` | `./gradlew testDebugUnitTest` en verde | ⏳ |
| T029 | [P] Pruebas de UI (Compose Test + Robolectric): componentes base, navegación al inicio provisional, `DiagnosticsViewModel` con Turbine y escala de fuente 2.0 | T013, T017, T024 | `qa-movil` | Pruebas en verde (CA-10) | ⏳ |
| T038 | [P] Pruebas de mutación con `gradle-pitest-plugin` en `:core:domain` y `:core:common`; medir la línea base | T028 | `qa-movil` | `./gradlew pitest` termina; puntaje base en `memoria.md` | ⏳ |
| T030 | Verificación manual en el emulador: arranque en frío medido, modo oscuro, diagnóstico contra la API local y APK de *release* sin tráfico en claro | T026, T029 | `qa-movil` | Tiempos y capturas en `memoria.md` (CA-02, CA-03, CA-04, CA-06; RNF-03) | ⏳ |

## F8 · CI/CD y publicación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T031 | `.github/workflows/movil-ci.yml`: JDK 17, `setup-gradle`, jobs `changes` y `movil-build-test` con `ktlintCheck detekt lint testDebugUnitTest pitest assembleDebug`, artefactos (APK de *debug* y reporte de Pitest) y `concurrency` | T028, T029, T038 | `seguridad-ci` | PR de prueba con todos los checks en verde (CA-08) | ⏳ |
| T032 | [P] `.github/workflows/movil-codeql.yml` (`java-kotlin`) | T031 | `seguridad-ci` | Check de CodeQL en el PR sin alertas altas | ⏳ |
| T033 | 🧑 Generar el keystore de *release* y guardarlo en GitHub Secrets (`SIGNING_*`); crear la variable `VIRGO_API_BASE_URL` con la URL HTTPS que entrega T210 del backend | T026 | `seguridad-ci` | Secretos y variable visibles en GitHub; respaldo del keystore confirmado (P-02) | ⏳ |
| T034 | `.github/workflows/movil-release.yml`: **solo `workflow_dispatch`** desde `main` con la versión como entrada → `assembleRelease`, `apksigner verify` y `gh release create vX.Y.Z` con el APK | T031, T033 | `seguridad-ci` | YAML válido; sin secretos impresos en los logs | ⏳ |
| T035 | 🧑 Primer release `v0.1.0` con *Run workflow*: descargar, instalar y hacer la prueba de humo (la app abre y el diagnóstico llega a la API de producción por HTTPS) | T034 | `seguridad-ci` + `qa-movil` | Release publicado con el APK firmado, la versión y el commit (CA-09) | ⏳ |

## F9 · Cierre

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T039 | Agregar `movil-build-test` y `movil-codeql` como checks requeridos del ruleset único y actualizar `.github/rulesets/main-protection.json` en la raíz | T002, T031 | `seguridad-ci` | *Push* directo a `main` rechazado; JSON en el repositorio | ⏳ |
| T036 | Completar `check.md`; verificación final con `verificar-constitucion`; marcar el spec, el plan y las tareas como *Cerrado*; entrada de cierre en `memoria.md` con lo aprendido | T001–T039 | `qa-movil` + `arquitecto-movil` | Resultado «Alineado» y todos los criterios de aceptación con evidencia | ⏳ |

---

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 Módulos | T005, T006, T007 |
| RF-02 Arranque e inicio provisional | T015, T016, T017 |
| RF-03 Sistema de diseño | T010–T014 |
| RF-04 Navegación extensible | T016, T017 |
| RF-05 Cliente de red por variante | T019, T020 |
| RF-06 Errores a `AppError` | T018, T021, T028 |
| RF-07 Almacenamiento cifrado | T022, T028 |
| RF-08 Diagnóstico en *debug* | T023, T024, T025 |
| RF-09 Textos en recursos | T015, T024 |
| RF-10 CI en PR | T031, T032, T038, T039 |
| RF-11 APK firmado publicado | T033, T034, T035 |
| RNF-01 Android 8.0 o superior | T006 |
| RNF-02 Seguridad | T002, T022, T026, T027, T033, T039 |
| RNF-03 Arranque < 2 s | T030 |
| RNF-04 Accesibilidad | T014, T029 |
| RNF-05 Calidad | T008, T031 |
| RNF-06 Costo $0 | T031, T034 |
| RNF-07 Trazabilidad | T005, T035, T036 |
| CA-01 … CA-10 | T007/T031 · T015/T017/T030 · T014/T030 · T024/T030 · T024 · T026/T030 · T022 · T031 · T035 · T013/T029 |
