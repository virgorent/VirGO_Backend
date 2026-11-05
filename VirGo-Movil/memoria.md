# Memoria — VirGo Móvil (Android)

Bitácora oficial del proyecto (Art. X de [`constitution.md`](constitution.md)). Cada sesión agrega una entrada al final de «Bitácora» con la skill [`registrar-memoria`](.opencode/skills/registrar-memoria/SKILL.md).

## Estado actual

| Campo | Valor |
|---|---|
| Constitución | v1.1.0 aprobada por el equipo el 2026-11-05 |
| Módulo en curso | `000-arquitectura-base`: aprobado; T001, T003 y T005 hechas |
| Tareas | 3 / 39 hechas (T037 bloqueada por MCP) |
| `API_BASE_URL` de *release* | Pendiente (la entrega `VirGo-API` en `002`: T210) |
| Última versión publicada | Ninguna |
| Siguiente paso | Ejecutar T006 y T007; desbloquear T037; resolver P-01/P-02 |

## Módulos

| Módulo | Estado | Notas |
|---|---|---|
| `000-arquitectura-base` | 🚧 Aprobado; T001/T003/T005 hechas | Base técnica sin pantallas del negocio |
| `001-…` pantallas | ⏸ No iniciados | Un spec atómico por pantalla o flujo, al vuelo; los primeros serán «Iniciar sesión» y «Cerrar sesión», con el JSON de `VirGo-API` `003` y `004` |

## Decisiones (ADR)

| ADR | Fecha | Decisión | Motivo |
|---|---|---|---|
| ADR-01 | 2026-10-29 | **Kotlin + Jetpack Compose** nativo | Stack del curso y del proyecto de referencia |
| ADR-02 | 2026-10-29 | Multi-módulo `:app` + `:core:*` con *convention plugins* | Capas aisladas y base para `:feature:*` |
| ADR-03 | 2026-10-29 | MVVM + UDF con `StateFlow` | Patrón recomendado y sin dependencias extra |
| ADR-04 | 2026-10-29 | Retrofit + OkHttp + kotlinx.serialization | Estándar y tipado |
| ADR-05 | 2026-10-29 | DataStore + Tink con llave en el Keystore | `EncryptedSharedPreferences` está obsoleta |
| ADR-06 | 2026-10-29 | Manrope y DM Sans empaquetadas | Fieles a la maqueta y funcionan sin conexión |
| ADR-07 | 2026-10-29 | Distribución por **GitHub Releases** | $0 |
| ADR-08 | 2026-10-29 | `API_BASE_URL` por variante; *release* solo `https://` | Simple y seguro |
| ADR-09 | 2026-10-29 | Robolectric para las pruebas de UI en el CI | Rápido y gratis |
| ADR-10 | 2026-11-03 | **Release manual** (`workflow_dispatch` desde `main`) | Indicación del curso (clase del 29/10) |
| ADR-11 | 2026-11-03 | **Ruleset** de GitHub en `main` | Mecanismo actual; exportable a JSON |
| ADR-12 | 2026-11-03 | **Pitest** (`gradle-pitest-plugin`) en módulos JVM puros | Mutación sin dispositivo; umbral tras medir |
| ADR-13 | 2026-11-03 | MCP permitidos: `figma` (lectura del *canvas*) y `github` | Pantallas al vuelo desde la maqueta oficial |
| ADR-14 | 2026-11-03 | Google Play como destino futuro, con enmienda | Tiene un pago único; por ahora GitHub Releases |
| ADR-15 | 2026-11-03 | **Repositorio único** `virgorent/virGO_App` con carpetas por proyecto | Decisión del equipo |

## Cambios de contrato (compartidos con Backend)

| Fecha | Cambio | Registrado en VirGo-API |
|---|---|---|
| 2026-10-29 | Contrato base acordado: `/api/v1`, Problem Details, `camelCase`, fechas UTC, JWT Bearer, `GET /health`, `GET /openapi/v1.json`, `X-Correlation-Id` | ✅ |
| 2026-11-03 | **Propuesto:** `POST /api/v1/auth/login` (devuelve `accessToken` de 15 min, `refreshToken` de 30 días y `user` con `role`) y `POST /api/v1/auth/logout` (204; ante 204 o 401 la app borra sus tokens). JSON en `VirGo-API/specs/003-auth-login` y `004-auth-logout` | ✅ |

## Versiones de librerías

Versiones fijadas en T005 y verificadas con `./gradlew help`.

| Librería / herramienta | Versión |
|---|---|
| Gradle | 9.8.2 |
| Android Gradle Plugin | 9.5.1 |
| Kotlin (KGP, Compose y serialización) | 2.4.10 |
| KSP | 2.3.13 |
| Hilt | 2.60.1 |
| Compose BOM | 2026.09.00 |
| JDK de ejecución local | 25 (el CI usará JDK 17) |

---

## Bitácora

### 2026-10-29 · Archivos de gobernanza y specs de la arquitectura base

- **Autor:** equipo + Claude Code (`arquitecto-movil`)
- **Módulo / tareas:** `specs/000-arquitectura-base` · ninguna ejecutada
- **Qué se hizo:**
  - Se crearon `constitution.md` v1.0.0, `AGENTS.md` y esta `memoria.md`.
  - Se crearon 5 subagentes en `.opencode/agents/`: `arquitecto-movil`, `dev-android`, `ui-designsystem`, `qa-movil` y `seguridad-ci`.
  - Se crearon 4 skills internas: `sdd-flujo`, `registrar-memoria`, `verificar-constitucion` y `compilar-publicar-apk`, más la lista filtrada de skills externas en `AGENTS.md`.
  - Se redactaron `spec.md` (7 historias técnicas, 11 RF, 7 RNF y 10 CA), `plan.md` (Constitution Check, 9 ADR, tokens de diseño de la maqueta, red, seguridad y CI/CD a $0) y `task.md` (36 tareas en 10 fases, con matriz de trazabilidad).
  - El contrato con la API se alineó con `VirGo-API/constitution.md` (Art. IV).
- **Evidencia:** archivos en el repositorio. El Constitution Check del plan tiene los 12 artículos en ✅.
- **Decisiones:** ADR-01 a ADR-09.
- **Costo:** $0.
- **Pendiente:**
  - Revisión del profesor y del equipo.
  - Responder P-01 (ID de la app) y P-02 (custodia del keystore).
  - Al aprobarse, empezar por T001. **No se ejecutan tareas hasta la aprobación.**

### 2026-11-03 · Lineamientos de clase: specs atómicos, check.md, DevSecOps, release manual y MCP

- **Autor:** equipo + Claude Code (`arquitecto-movil`)
- **Módulo / tareas:** gobernanza · `specs/000-arquitectura-base` · ninguna tarea ejecutada
- **Qué se hizo:**
  - Se propuso la enmienda **v1.1.0** de `constitution.md`: `check.md` y specs atómicos por pantalla, redacción anticipada, `.gitignore` y Gitleaks, pruebas de seguridad, mutación y humo, ruleset, **release manual**, Google Play como destino futuro, entrega de URL y JSON por el backend, MCP y README en inglés.
  - Se crearon `README.md` (inglés), `.gitignore` DevSecOps y `opencode.json` (MCP `figma` y `github`).
  - `000` pasó a v1.1.0: release solo con `workflow_dispatch`, ruleset, nuevas tareas T037 (MCP), T038 (Pitest) y T039 (ruleset exportado), y `check.md`.
  - Se actualizaron `AGENTS.md`, los 5 subagentes y las skills `sdd-flujo`, `verificar-constitucion`, `compilar-publicar-apk` y `registrar-memoria`.
  - Se registró como propuesto el contrato de login y logout del backend.
- **Evidencia:** archivos en el repositorio.
- **Decisiones:** ADR-10 a ADR-14.
- **Costo:** $0.
- **Aprendizaje:** «Canvas» en la clase se interpretó como el *canvas* de Figma; Canva queda en evaluación.
- **Pendiente:**
  - Aprobar la enmienda v1.1.0 (Art. XI.2).
  - Responder P-01 (ID de la app) y P-02 (custodia del keystore).
  - Redactar los specs de pantalla «Iniciar sesión» y «Cerrar sesión» cuando el backend confirme el contrato.

### 2026-11-03 · Repositorio único `Karasu0001/virGO_App`

- **Autor:** equipo + Claude Code (`arquitecto-movil`)
- **Módulo / tareas:** gobernanza · `specs/000-arquitectura-base` · ninguna tarea ejecutada
- **Qué se hizo:** el proyecto pasa a la carpeta `VirGo-Movil/` del repositorio único; `.github/` va en la raíz con prefijo `movil-`, `security.yml` común y ruleset único coordinado con `VirGo-API` T207. Esta constitución se subordina a la global de la raíz en los principios generales.
- **Evidencia:** PR desde la rama `docs/sdd-virgo`.
- **Decisiones:** ADR-15.
- **Costo:** $0.
- **Aprendizaje:** ninguno nuevo; ver `VirGo-API/memoria.md`.
- **Pendiente:** la visibilidad del repositorio (`VirGo-API` `002` P-02).

### 2026-11-04 · El repositorio pasa a la organización `virgorent`

- **Autor:** equipo + Claude Code (`arquitecto-movil`)
- **Módulo / tareas:** gobernanza · `specs/000-arquitectura-base` · ninguna tarea ejecutada
- **Qué se hizo:** no se pudo transferir `Karasu0001/virGO_App`, así que se creó `virgorent/virGO_App` vacío y se subieron `main`, `rama-apis`, `rama-backend`, `rama-movil` y `docs/sdd-virgo` con su historial. Las referencias vigentes apuntan al repositorio nuevo; las entradas anteriores de esta bitácora conservan el nombre de entonces.
- **Evidencia:** `git ls-remote` muestra los mismos commits en ambos repositorios.
- **Decisiones:** `virgorent/virGO_App` es el repositorio oficial.
- **Costo:** $0.
- **Aprendizaje:** ninguno.
- **Pendiente:** archivar `Karasu0001/virGO_App` cuando el equipo confirme; `VirGo-API` `002` P-02 (el repositorio es privado y el ruleset no aplicaría).

### 2026-11-04 · El repositorio ya es público

- **Autor:** equipo + Claude Code (`arquitecto-movil`)
- **Módulo / tareas:** gobernanza · ninguna tarea ejecutada
- **Qué se hizo:** se comprobó que `virgorent/virGO_App` es público, con lo que el ruleset y los revisores de los *environments* ya están disponibles en el plan Free.
- **Evidencia:** la API pública de GitHub responde `visibility: public`. `Karasu0001/virGO_App` responde «not found».
- **Decisiones:** se resuelve `VirGo-API` `002` P-02.
- **Costo:** $0.
- **Aprendizaje:** al ser público, todo lo que se suba es visible; Gitleaks y la protección de push son prioritarios.
- **Pendiente:** coordinar el ruleset con `VirGo-API` T207.

### 2026-11-04 · Regla de longitudes tras la clase

- **Autor:** equipo + Claude Code (`arquitecto-movil`)
- **Módulo / tareas:** gobernanza · ninguna tarea ejecutada
- **Qué se hizo:** `sdd-flujo` y `verificar-constitucion` exigen ahora longitud máxima por campo y comprobar lo afirmado contra la realidad.
- **Evidencia:** archivos en el repositorio.
- **Decisiones:** ninguna.
- **Costo:** $0.
- **Aprendizaje:** ver `VirGo-API/memoria.md`.
- **Pendiente:** usar las longitudes de `Modelo de datos VirGo.md` (sección 8) en los formularios.

### 2026-11-05 · Aprobación de la gobernanza móvil y corrección del puerto local

- **Autor:** equipo + opencode (`arquitecto-movil`)
- **Módulo / tareas:** gobernanza · `000-arquitectura-base` · T001/T003/T037 habilitadas
- **Qué se hizo:** se aprobó la constitución v1.1.0 y el spec, plan y tareas de `000`; se corrigió el puerto local de la API de `5080` a `5122` en `spec.md` y `plan.md` para alinearlo con `VirGo-API/src/VirGo.Api/Properties/launchSettings.json`.
- **Evidencia:** `git diff --check` limpio; `launchSettings.json` usa `http://localhost:5122`.
- **Decisiones:** el equipo aprueba la gobernanza móvil el 2026-11-05; el visto bueno del profesor queda pendiente en `check.md`.
- **Costo:** $0.
- **Aprendizaje:** el puerto local del backend debe ser la fuente de verdad para la URL de debug del móvil.
- **Pendiente:** PR de aprobación/corrección; P-01 (ID de app), P-02 (keystore), MCP Figma/GitHub (T037) y ejecución de T001/T003.

### 2026-11-05 · T001, T003 y T005: repositorio, estilo y proyecto Gradle

- **Autor:** equipo + opencode (`seguridad-ci` / `dev-android`)
- **Módulo / tareas:** `000-arquitectura-base` · T001, T003 y T005 hechas; T037 bloqueada
- **Qué se hizo:**
  - T001: se confirmó el remoto `https://github.com/virgorent/virGO_App.git`, la presencia de `VirGo-Movil/.gitignore` y `README.md`, y que `git check-ignore VirGo-Movil/local.properties` lo ignora.
  - T003: se agregó `VirGo-Movil/.editorconfig` con `ktlint_official`, longitud máxima de 120 y estilo Kotlin; el bloque `gradle` de `VirGo-Movil` ya existía en `.github/dependabot.yml`.
  - T005: se creó el wrapper de Gradle 9.8.2, `settings.gradle.kts`, `build.gradle.kts`, `gradle.properties` y `gradle/libs.versions.toml` con AGP 9.5.1, Kotlin 2.4.10, KSP 2.3.13, Hilt 2.60.1 y Compose BOM 2026.09.00. Se usa Kotlin integrado de AGP 9 en módulos Android y KGP 2.4.10 para JVM.
- **Evidencia:** `./gradlew help` termina en `BUILD SUCCESSFUL`; `git check-ignore -v VirGo-Movil/local.properties` responde con la regla de `.gitignore`.
- **Decisiones:** AGP 9.5.1 activa Kotlin integrado; no se aplica `org.jetbrains.kotlin.android` en módulos Android. La versión de Kotlin del *catalog* se usa en módulos JVM y en los plugins de Compose y serialización.
- **Costo:** $0.
- **Aprendizaje:** con AGP 9 el proyecto no debe declarar `kotlin-android`; el wrapper de Gradle 9.8.2 funciona con el JDK 25 local, aunque el CI mantiene JDK 17.
- **Pendiente:** T006 (convention plugins), T007 (módulos), T037 (Figma devuelve HTTP 405 y GitHub sigue deshabilitado), P-01 y P-02.
