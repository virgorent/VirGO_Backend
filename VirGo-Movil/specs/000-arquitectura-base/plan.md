# Plan — 000 Arquitectura base del Móvil

| Campo | Valor |
|---|---|
| Módulo | `000-arquitectura-base` |
| Versión | 1.1.0 |
| Estado | En revisión |
| Fecha | 2026-10-29 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) |
| Constitución | v1.1.0 |

## 1. Resumen técnico

Se crea un proyecto Android multi-módulo en Kotlin con Jetpack Compose (Material 3), MVVM/UDF y Hilt. Lo componen:

- Un módulo `:app`, que contiene el arranque, la navegación, la pantalla de inicio provisional y el diagnóstico solo en *debug*.
- Siete módulos `:core:*`: sistema de diseño, red, datos, dominio, almacenamiento cifrado, utilidades comunes y apoyo a pruebas.
- Convenciones de compilación centralizadas en `build-logic`.

La red usa Retrofit + OkHttp + kotlinx.serialization contra `API_BASE_URL`. El CI/CD corre en GitHub Actions; el release es **manual** (*Run workflow*) y publica el APK firmado en GitHub Releases, a costo cero.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje | Kotlin (última versión estable; se fija en T005) |
| Compilación | Gradle Kotlin DSL, *version catalog* (`gradle/libs.versions.toml`), *convention plugins* en `build-logic/` |
| Android | AGP estable más reciente, `compileSdk` y `targetSdk` 36, `minSdk` 26, JDK 17 |
| UI | Jetpack Compose (BOM), Material 3, `core-splashscreen` |
| Arquitectura | MVVM + UDF, `ViewModel` + `StateFlow`, `collectAsStateWithLifecycle` |
| Inyección de dependencias | Hilt (KSP) y `hilt-navigation-compose` |
| Navegación | Navigation Compose con rutas tipadas (`@Serializable`) |
| Red | Retrofit, OkHttp (interceptores) y kotlinx.serialization |
| Almacenamiento | DataStore (Preferences) + Tink (AEAD) con una llave maestra en el Android Keystore |
| Concurrencia | Corrutinas y `Flow`, con *dispatchers* inyectados |
| Logging | Timber (solo en *debug*) |
| Pruebas | JUnit 4, MockK, Turbine, kotlinx-coroutines-test, MockWebServer, Compose UI Test y Robolectric para las pruebas de UI en JVM |
| Análisis estático | ktlint (plugin de Gradle), detekt, Android Lint y CodeQL (`java-kotlin`) |
| CI/CD | GitHub Actions: `movil-ci.yml`, `movil-codeql.yml` y `movil-release.yml`; distribución por GitHub Releases |

> Las versiones exactas de las librerías se fijan en `libs.versions.toml` en T005 (solo estables) y se registran en `memoria.md`.

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| I | SDD | ✅ Cumple | El plan deriva del spec y no se implementa nada antes de aprobar `task.md`. |
| II | Módulo base primero | ✅ Cumple | Solo un inicio provisional y un diagnóstico en *debug*; sin pantallas del negocio. |
| III | Arquitectura | ✅ Cumple | Kotlin + Compose, MVVM/UDF, capas en módulos `:core:*`, Hilt y corrutinas; `:core:domain` es Kotlin puro. |
| IV | Contrato con la API | ✅ Cumple | `/api/v1`, Problem Details convertido a `AppError`, `camelCase`, fechas UTC y Bearer por interceptor. |
| V | Seguridad | ✅ Cumple | `network_security_config` (en *release* solo HTTPS), token cifrado con Tink y Keystore, R8, sin secretos y keystore en Secrets. |
| VI | Pruebas | ✅ Cumple | Unitarias, de red con MockWebServer y de UI con Compose Test; ktlint, detekt y Lint en el CI. |
| VII | Diseño | ✅ Cumple | `:core:designsystem` con los tokens de la maqueta, Manrope y DM Sans, tema claro y oscuro, accesibilidad y `strings.xml`. |
| VIII | CI/CD a costo cero | ✅ Cumple | Ruleset en `main`, GitHub Actions y release manual a GitHub Releases; `API_BASE_URL` de *release* desde una variable de GitHub. |
| IX | Observabilidad | ✅ Cumple | Timber solo en *debug* y `X-Correlation-Id` por solicitud. |
| X | Memoria | ✅ Cumple | Entrada en `memoria.md` por tarea; commits con el ID. |
| XI | Agentes | ✅ Cumple | Cada tarea tiene un subagente responsable definido en `AGENTS.md`. |
| XII | Idioma | ✅ Cumple | Código en inglés y textos en `strings.xml` en español; `.editorconfig` y ktlint. |

## 4. Estructura de módulos y carpetas

```text
VirGo-Movil/
├── settings.gradle.kts
├── build.gradle.kts
├── gradle/libs.versions.toml
├── build-logic/convention/          # Plugins: virgo.android.application, .library, .compose, .hilt, .jvm
├── app/                             # mx.virgo.app
│   └── src/
│       ├── main/                    # VirGoApplication (@HiltAndroidApp), MainActivity, VirGoApp(), VirGoNavHost, HomePlaceholder
│       ├── debug/                   # DiagnosticsScreen + DiagnosticsViewModel, network_security_config de debug
│       └── release/                 # network_security_config de release (solo HTTPS)
├── core/
│   ├── designsystem/                # VirGoTheme, Color, Type, Shape, Spacing, componentes base, fuentes en res/font
│   ├── common/                      # AppError, AppResult, @Dispatcher, extensiones
│   ├── domain/                      # Kotlin/JVM puro: modelos base, CheckApiHealthUseCase
│   ├── network/                     # Retrofit, OkHttp, interceptores, ProblemDetailsDto, HealthApi, NetworkModule
│   ├── data/                        # Repositorios (HealthRepository), conversión de DTO a modelo y de error a AppError
│   ├── datastore/                   # TokenStore (DataStore + Tink AEAD)
│   └── testing/                     # MainDispatcherRule, fakes y fixtures
```

`.github/` no va aquí: vive en la raíz del repositorio `virgorent/virGO_App` (`movil-ci.yml`, `movil-codeql.yml`, `movil-release.yml`, `security.yml` y `dependabot.yml`).

**Dependencias permitidas:**

- `:app` → todos los `:core:*`.
- `:core:data` → `:core:network`, `:core:datastore`, `:core:domain` y `:core:common`.
- `:core:network` → `:core:common`. `:core:domain` → `:core:common`.
- `:core:designsystem` → nada del proyecto.
- Los módulos `:feature:*` futuros → `:core:*`, nunca otro `:feature:*`.

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-01 | Kotlin + Compose nativo | React Native, Flutter | Stack del curso y del proyecto de referencia; mejor integración con Android |
| ADR-02 | Multi-módulo `:core:*` y *convention plugins* | Un solo módulo | Capas aisladas, compilación incremental y base para `:feature:*` |
| ADR-03 | MVVM + UDF con `StateFlow` | MVI con una librería, MVP | Patrón recomendado por Android y sin dependencias extra |
| ADR-04 | Retrofit + kotlinx.serialization | Ktor client, Gson/Moshi | Estándar, tipado y sin reflexión |
| ADR-05 | DataStore + Tink con llave en el Keystore | `EncryptedSharedPreferences` | `security-crypto` está obsoleta; Tink es la recomendación vigente |
| ADR-06 | Fuentes Manrope y DM Sans empaquetadas en `res/font` (licencia OFL) | Fuentes descargables de Google | Funcionan sin conexión y sin depender de Google Play Services |
| ADR-07 | Distribución por **GitHub Releases** | Google Play, Firebase App Distribution | $0 y sin cuentas extra (Art. VIII.4) |
| ADR-08 | `API_BASE_URL` por variante con `buildConfigField` | Configuración remota | Simple; *release* valida que empiece con `https://` o falla la compilación |
| ADR-09 | Robolectric para las pruebas de UI en el CI | Emulador en GitHub Actions | Más rápido y gratis; el emulador queda para pruebas manuales |

## 6. Diseño

### 6.1 Sistema de diseño (`:core:designsystem`)

| Token | Valor en tema claro | Uso |
|---|---|---|
| `primary` | `#6366F1` (violeta) | Botones principales y elementos activos |
| `primaryDark` | `#4F46E5` | Estado presionado |
| `primaryContainer` | `#EFEFFF` | Insignias e IA |
| `onSurface` (tinta) | `#18191F` | Texto principal |
| `onSurfaceVariant` | `#747782` | Texto secundario |
| `outline` | `#E9E9ED` | Bordes |
| `surfaceVariant` | `#F7F7F9` | Fondos suaves |
| `tertiary` (coral) | `#FF6B6B` | Acentos y favoritos |
| `success` / `error` / `warning` | `#21A668` / `#EB5757` / `#E6A937` | Estados |

- **Tipografía:** Manrope (500–800) para *display*, *headline* y *title*; DM Sans (400–700) para *body* y *label*.
- **Formas:** radios de 12, 18 y 24 dp. **Espaciado:** escala de 4 dp.
- **Tema oscuro:** derivado con contraste AA verificado en T014.
- **Componentes base:** `VirGoButton` (primario, secundario y texto), `VirGoTextField`, `VirGoCard`, `VirGoBadge` (incluye la variante «Estimación IA»), `VirGoTopBar`, `VirGoBottomBar`, `EmptyState`, `LoadingState` y `ErrorBanner`.

### 6.2 Navegación

- `VirGoNavHost` registra los destinos con extensiones `NavGraphBuilder.xxxScreen()`. Cada módulo futuro aporta las suyas.
- `TopLevelDestination` es una lista configurable para la barra inferior. En la base solo existe `Home` provisional y la barra está oculta.
- En *debug*, el destino `Diagnostics` es accesible desde un menú de desarrollo en la barra superior.

### 6.3 Red y errores

| Elemento | Definición |
|---|---|
| `API_BASE_URL` | *debug*: `local.properties → virgo.apiBaseUrl` o, por defecto, `http://10.0.2.2:5080/`. *release*: variable de GitHub `VIRGO_API_BASE_URL` (debe ser `https://`) |
| Interceptores de OkHttp | `CorrelationIdInterceptor` (UUID en `X-Correlation-Id`), `AuthInterceptor` (lee `TokenStore`; si hay token, agrega `Authorization: Bearer`) y `HttpLoggingInterceptor` solo en *debug* con nivel `BASIC` |
| Tiempos de espera | Conexión 10 s, lectura 20 s |
| `HealthApi` | `GET health` → `HealthDto(status)` |
| `AppError` | `Network` (sin conexión), `Timeout`, `Unauthorized`, `NotFound`, `Validation(fields)`, `Server(traceId)` y `Unknown` |
| Conversión | La respuesta no exitosa se interpreta como `ProblemDetailsDto` y se convierte en `AppError`; `IOException` se convierte en `Network` y `SocketTimeoutException` en `Timeout` |
| Mensajes | Un `AppError` se convierte en `@StringRes` en `:core:designsystem` o `:app`; nunca se muestra un texto técnico |

### 6.4 Almacenamiento seguro (`:core:datastore`)

`TokenStore` ofrece `suspend fun save(token)`, `val token: Flow<String?>` y `suspend fun clear()`. El valor se cifra con un `Aead` de Tink cuya llave maestra está en el Android Keystore. En el archivo de DataStore solo queda el texto cifrado en Base64.

### 6.5 Seguridad de la compilación

- `release`: `isMinifyEnabled = true`, `isShrinkResources = true`, reglas de R8 para kotlinx.serialization y Retrofit, y `isDebuggable = false`.
- `network_security_config.xml` de *release*: `cleartextTrafficPermitted="false"`. El de *debug* permite solo `10.0.2.2`.
- La firma usa las variables de entorno del CI. Si faltan, la tarea `assembleRelease` falla con un mensaje claro, nunca usa la llave de *debug*.

## 7. CI/CD, distribución y costo

| Workflow | Disparador | Pasos |
|---|---|---|
| `movil-ci.yml` | PR y push a `main` | JDK 17, `setup-gradle` (caché), job `changes` (detecta `VirGo-Movil/**`, sin filtros `paths`), `working-directory: VirGo-Movil`, `ktlintCheck detekt lint testDebugUnitTest pitest assembleDebug` en el job `movil-build-test` y subida del APK de *debug* y del reporte de Pitest como artefactos |
| `movil-codeql.yml` | PR, push a `main` y semanal | CodeQL `java-kotlin` |
| `security.yml` | Todos los PR (común con el backend) | Gitleaks y *dependency review* |
| `movil-release.yml` | **Solo `workflow_dispatch`** desde `main` (entrada: versión `X.Y.Z`) | Decodificar el keystore, `assembleRelease` con `VIRGO_API_BASE_URL`, `apksigner verify` y `gh release create vX.Y.Z` con el APK (crea la etiqueta) |

| Servicio | Costo |
|---|---|
| GitHub Actions | $0 (ilimitado en repositorio público; 2 000 min al mes en privado; una compilación tarda unos 6–8 min) |
| GitHub Releases | $0 |
| **Total** | **$0 USD** |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `:core:common`, `:core:domain`, `:core:data` | Conversión de Problem Details y excepciones a `AppError`; `CheckApiHealthUseCase` |
| Red | `:core:network` | MockWebServer: 200, 4xx/5xx con Problem Details, timeout y encabezados `X-Correlation-Id` y `Authorization` |
| Almacenamiento | `:core:datastore` | Guardar, leer y borrar; el archivo no contiene el token en texto plano (Robolectric) |
| ViewModel | `:app` | `DiagnosticsViewModel` con Turbine: carga → éxito y carga → error |
| UI | `:core:designsystem`, `:app` | Compose Test con Robolectric: componentes base, navegación al inicio provisional y textos con escala de fuente 2.0 |
| Seguridad | `:core:datastore`, `:core:network`, `:app` | Token cifrado y borrado al cerrar sesión, *release* sin tráfico en claro, sin cuerpos ni tokens en los logs, Lint de seguridad |
| Mutación | `:core:domain`, `:core:common` (JVM puros) | Pitest con `gradle-pitest-plugin`; primero se mide, después se fija el umbral |
| Humo | APK publicado | Instalar, abrir y ver la API de producción en verde |
| Manual | Emulador | Arranque en frío medido (RNF-03), modo oscuro y diagnóstico contra la API local y la de producción |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| La URL de producción del backend todavía no existe | *debug* apunta a la API local; *release* se compila hasta que T210 del backend (antes T033) entregue la URL |
| Llave de firma perdida | Respaldo cifrado del keystore fuera del repositorio; custodia definida en P-02 |
| Versiones incompatibles de AGP, Kotlin, KSP y Compose | Fijar un conjunto compatible en T005 y actualizar con Dependabot de forma agrupada |
| Las pruebas de UI en Robolectric difieren del dispositivo real | Verificación manual en el emulador en T030 |
| Minutos de Actions en un repositorio privado | Caché de Gradle y *concurrency* para cancelar ejecuciones obsoletas |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Repositorio y gobernanza | T001–T004 |
| F1 | Proyecto Gradle y módulos | T005–T009 |
| F2 | Sistema de diseño | T010–T014 |
| F3 | Arranque y navegación | T015–T017 |
| F4 | Red, datos y almacenamiento seguro | T018–T023 |
| F5 | Diagnóstico de integración | T024–T025 |
| F6 | Seguridad de la compilación | T026–T027 |
| F7 | Pruebas y verificación | T028–T030 |
| F8 | CI/CD y publicación | T031–T035 |
| F9 | Cierre | T036 |

> **Cambio v1.1.0:** se agregaron T037 (MCP), T038 (pruebas de mutación) y T039 (ruleset exportado), y el release pasó a ser manual.
