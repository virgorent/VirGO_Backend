# AGENTS.md — VirGo Móvil (Android)

Guía base para los agentes de IA y las personas que trabajan en este proyecto (carpeta `VirGo-Movil/` del repositorio `virgorent/virGO_App`). Sigue lo que dice [`constitution.md`](constitution.md); si algo aquí lo contradice, gana la constitución.

## Contexto del proyecto

- **Qué es:** la app Android de VirGo para organizar eventos y conectar con salones y proveedores.
- **Stack:** Kotlin, Jetpack Compose (Material 3), MVVM/UDF, Hilt, Retrofit + OkHttp + kotlinx.serialization, DataStore, Gradle con *version catalog* y GitHub Actions.
- **Backend:** `VirGo-API` (.NET 10 en AWS). Su contrato está en el Art. IV.
- **Diseño de referencia:** la maqueta funcional de VirGo y el archivo de Figma «Pantallas VirGo».
- **Estado actual:** `000-arquitectura-base` aprobado el 2026-11-05; tareas habilitadas. Todavía **no hay código** de la app. Los specs de pantallas (`001-…`) se crean uno por pantalla o flujo, al vuelo, cuando el backend entregue la URL y el JSON de sus endpoints.

## Estructura del proyecto

La estructura objetivo la define el plan de la arquitectura base:

```text
VirGo-Movil/
├── README.md                  # Portada en inglés (Art. XII.1)
├── .gitignore                 # Exclusiones DevSecOps (Art. V.8)
├── constitution.md
├── AGENTS.md
├── memoria.md
├── opencode.json              # Servidores MCP permitidos
├── .opencode/
│   ├── agents/
│   └── skills/
├── specs/000-arquitectura-base/{spec.md, plan.md, task.md, check.md}
├── app/                       # (pendiente) Aplicación, navegación y tema raíz
├── core/                      # (pendiente) designsystem, network, data, datastore, common, testing
├── gradle/libs.versions.toml  # (pendiente) Versiones centralizadas
└── (sin .github aquí)         # GitHub solo lee `.github/` de la raíz del repositorio
```

En la raíz del repositorio están `.github/` (pendiente: `movil-ci.yml`, `movil-codeql.yml`, `movil-release.yml`, `security.yml`, `rulesets/`) y la gobernanza global. Todos los comandos de abajo se ejecutan dentro de `VirGo-Movil/`.

## Comandos

Estos comandos estarán disponibles al terminar la arquitectura base:

```powershell
./gradlew ktlintCheck detekt lint     # Análisis estático
./gradlew testDebugUnitTest           # Unitarias, integración (MockWebServer), UI y seguridad (Robolectric)
./gradlew pitest                      # Pruebas de mutación en :core:domain y :core:common
./gradlew assembleDebug               # APK de desarrollo
./gradlew assembleRelease             # APK de release (requiere keystore)
```

Orden del pipeline de CI: `ktlint → detekt → lint → pruebas → pitest → assembleDebug`, en paralelo con `Gitleaks`, `dependency review` y `CodeQL`. El release es **manual** con *Run workflow* en `movil-release.yml`.

## Flujo de trabajo SDD (obligatorio)

1. **Spec:** `arquitecto-movil` lo redacta y una persona lo aprueba.
2. **Plan:** `arquitecto-movil` lo redacta con el «Constitution Check» y una persona lo aprueba.
3. **Tareas:** se generan en `task.md` con un ID con el número del módulo (`T001` en `000`, `T101` en `001`…), dependencias, el responsable y la verificación.
4. **Check:** se crea `check.md` con la evidencia que hará falta para cada CA, cada tipo de prueba, el PR, el release y la memoria.
5. **Implementación:** una tarea a la vez, en orden, en la rama `feat/NNN-modulo`. Si el CI falla, se corrige en la misma rama y sobre el mismo spec.
6. **Release:** una persona lo lanza con *Run workflow*; la prueba de humo del APK debe pasar.
7. **Cierre y aprendizaje:** `qa-movil` completa el `check.md`, el spec pasa a *Publicado*/*Cerrado* y se registra en `memoria.md` lo aprendido y las mejoras para la constitución, este archivo, las skills y los subagentes.

Un spec es **atómico**: una pantalla o un flujo corto (Art. I.5). Antes de especificar una pantalla, léela con el MCP `figma`.

Usa la skill [`sdd-flujo`](.opencode/skills/sdd-flujo/SKILL.md).

## Subagentes

Están definidos en `.opencode/agents/`.

| Subagente | Archivo | Responsabilidad | No puede |
|---|---|---|---|
| `arquitecto-movil` | [arquitecto-movil.md](.opencode/agents/arquitecto-movil.md) | Specs y planes, módulos de Gradle, capas, contrato con la API y alineación con la constitución | Escribir código de producción |
| `dev-android` | [dev-android.md](.opencode/agents/dev-android.md) | Implementar las tareas en Kotlin y Compose siguiendo el plan | Cambiar el spec o el plan sin aprobación |
| `ui-designsystem` | [ui-designsystem.md](.opencode/agents/ui-designsystem.md) | Tema, tokens y componentes del sistema de diseño fieles a la maqueta y accesibles | Crear pantallas del negocio en el módulo base |
| `qa-movil` | [qa-movil.md](.opencode/agents/qa-movil.md) | Pruebas unitarias y de UI, y verificación de criterios con evidencia | Marcar tareas como hechas sin evidencia |
| `seguridad-ci` | [seguridad-ci.md](.opencode/agents/seguridad-ci.md) | Seguridad de la app (red, tokens, R8, secretos) y workflows de CI/CD | Publicar releases sin confirmación |

**Reglas comunes a todos los subagentes:**

- Lee `constitution.md`, el spec, el plan y el `task.md` antes de actuar.
- Trabaja solo en las tareas que te asigna el `task.md`.
- Antes de cambiar un archivo, revisa su contenido actual.
- No publiques releases, no cambies secretos y no hagas `push --force` sin confirmación humana.
- Al terminar, deja evidencia y registra en `memoria.md` con [`registrar-memoria`](.opencode/skills/registrar-memoria/SKILL.md).

## Skills

### Internas, en `.opencode/skills/`

| Skill | Cuándo se usa |
|---|---|
| [`sdd-flujo`](.opencode/skills/sdd-flujo/SKILL.md) | Crear o revisar `spec.md`, `plan.md` y `task.md` |
| [`registrar-memoria`](.opencode/skills/registrar-memoria/SKILL.md) | Agregar una entrada a `memoria.md` |
| [`verificar-constitucion`](.opencode/skills/verificar-constitucion/SKILL.md) | Revisar la alineación con la constitución |
| [`compilar-publicar-apk`](.opencode/skills/compilar-publicar-apk/SKILL.md) | Compilar, firmar y publicar el APK en GitHub Releases |

### Externas: lista filtrada

Solo se usan las skills externas de esta lista (Art. XI). Cualquier otra se evalúa y se agrega aquí antes de usarse.

| Skill externa | Estado | Motivo |
|---|---|---|
| `figma:figma-design-to-code` y `figma:figma-use` (solo lectura) | ✅ Permitida | Tomar medidas, tokens y estructura de «Pantallas VirGo» para el sistema de diseño (Art. VII) |
| `code-review` / `security-review` | ✅ Permitida | Revisión de PRs (Art. V y VI) |
| `simplify` | ✅ Permitida | Limpieza de código ya funcional |
| `figma:figma-swiftui` | ⛔ No aplica | La app es Android, no iOS |
| `figma:figma-generate-*` (escritura en Figma) | ⛔ No aplica | El móvil consume el diseño, no lo produce |
| Skills de nube o despliegue de servidores | ⛔ No aplica | La infraestructura vive en `VirGo-API` |

## Servidores MCP

Se configuran en [`opencode.json`](opencode.json) y se instalan **antes** de codificar la pantalla que los necesita (Art. XI.6). Los tokens se leen de variables de entorno; nunca se escriben en el archivo.

| MCP | Estado | Para qué | Credencial | Quién lo usa |
|---|---|---|---|---|
| `figma` (servidor remoto oficial) | ✅ Permitido, activo | Leer el *canvas* de «Pantallas VirGo»: medidas, tokens y estructura de cada pantalla al especificarla | Inicio de sesión de Figma (OAuth) | `arquitecto-movil`, `ui-designsystem`, `dev-android` |
| `github` (servidor remoto oficial) | ✅ Permitido, se activa en T037 | Leer PRs, checks y ejecuciones del release | `GITHUB_MCP_PAT`: token de grano fino solo de este repositorio | Todos |
| Canva MCP | 🔍 En evaluación | Solo si el equipo diseña material en Canva; la maqueta oficial está en Figma | — | — |
| MCP de emulador o dispositivo (`mobile-mcp`) | 🔍 En evaluación | Automatizar la prueba de humo del APK en el emulador | — | `qa-movil` |
| Escritura en Figma (`use_figma`, `generate_figma_design`) | ⛔ No aplica | El móvil consume el diseño, no lo produce | — | — |

Un MCP nuevo se evalúa (necesidad real, mantenedor, permisos que pide) y se agrega a esta tabla antes de usarse.

## Convenciones

- **Código** en inglés y **textos y documentación** en español (Art. XII).
- **Paquete base:** `mx.virgo.app`; los módulos usan `mx.virgo.core.<nombre>`.
- **Commits:** `tipo(TNNN): descripción`. **Ramas:** Móvil se integra en `rama-movil`. Las ramas `feat/NNN-modulo` y `fix/…` salen de ahí, y `rama-movil` entra a `main` por PR.
- **Estado de pantalla:** `data class XxxUiState` inmutable; un solo `StateFlow` por ViewModel.
- **Errores:** los Problem Details de la API se convierten en `AppError` y se muestran con textos de `strings.xml`.

## Coordinación con el Backend

- `API_BASE_URL` de *release* = la URL HTTPS que publica el workflow de despliegue de `VirGo-API` (tarea T210 del backend).
- Cada pantalla que consume un endpoint usa el **JSON probado** que entrega el backend (sección «Contrato» del `plan.md` del endpoint). Por ejemplo, login y logout: `VirGo-API/specs/003-auth-login` y `004-auth-logout`.
- Cualquier cambio de contrato se registra en `memoria.md` de **ambos** proyectos.
