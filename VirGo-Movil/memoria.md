# Memoria — VirGo Móvil (Android)

Bitácora oficial del proyecto (Art. X de [`constitution.md`](constitution.md)). Cada sesión agrega una entrada al final de «Bitácora» con la skill [`registrar-memoria`](.opencode/skills/registrar-memoria/SKILL.md).

## Estado actual

| Campo | Valor |
|---|---|
| Constitución | v1.1.0 **propuesta** (pendiente de aprobación del equipo) |
| Módulo en curso | `000-arquitectura-base`: spec, plan y tareas **en revisión**; tareas **sin ejecutar** |
| Tareas | 0 / 39 hechas |
| `API_BASE_URL` de *release* | Pendiente (la entrega `VirGo-API` en `002`: T210) |
| Última versión publicada | Ninguna |
| Siguiente paso | Evaluación del 2026-11-04: aprobar la enmienda v1.1.0 y `000-arquitectura-base` |

## Módulos

| Módulo | Estado | Notas |
|---|---|---|
| `000-arquitectura-base` | 📝 En revisión | Base técnica sin pantallas del negocio |
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
| ADR-15 | 2026-11-03 | **Repositorio único** `Karasu0001/virGO_App` con carpetas por proyecto | Decisión del equipo |

## Cambios de contrato (compartidos con Backend)

| Fecha | Cambio | Registrado en VirGo-API |
|---|---|---|
| 2026-10-29 | Contrato base acordado: `/api/v1`, Problem Details, `camelCase`, fechas UTC, JWT Bearer, `GET /health`, `GET /openapi/v1.json`, `X-Correlation-Id` | ✅ |
| 2026-11-03 | **Propuesto:** `POST /api/v1/auth/login` (devuelve `accessToken` de 15 min, `refreshToken` de 30 días y `user` con `role`) y `POST /api/v1/auth/logout` (204; ante 204 o 401 la app borra sus tokens). JSON en `VirGo-API/specs/003-auth-login` y `004-auth-logout` | ✅ |

## Versiones de librerías

Se llena en T005 con las versiones estables verificadas.

| Librería | Versión |
|---|---|
| — | — |

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
