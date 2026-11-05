# Constitution — VirGo Móvil (Android)

**Versión:** 1.1.0 · **Ratificada:** 2026-10-29 · **Última enmienda:** 2026-11-03

Este documento norma el proyecto Móvil de VirGo. Todos los demás archivos (`AGENTS.md`, `memoria.md`, subagentes, skills y specs) deben estar alineados con él. Si un archivo lo contradice, prevalece este documento y el otro archivo se corrige.

VirGo conecta a quien organiza un evento con salones y proveedores de servicios. Este proyecto vive en la carpeta `VirGo-Movil/` del repositorio [`virgorent/virGO_App`](https://github.com/virgorent/virGO_App) y contiene la app Android, que consume la API del proyecto `VirGo-API/`.

Se subordina a la constitución global de la raíz (`../constitution.md`): esta desarrolla sus principios para el proyecto. En lo técnico del proyecto manda este documento; en los principios generales, la global. El diseño de referencia es la maqueta funcional de Figma «Pantallas VirGo».

---

## Artículo I. Desarrollo guiado por especificaciones (SDD)

1. **No se escribe código sin especificación.** Todo trabajo sigue el flujo `spec.md → plan.md → task.md → implementación`.
2. Cada etapa requiere aprobación antes de pasar a la siguiente:
   - El **spec** dice *qué* y *por qué*.
   - El **plan** dice *cómo*, e incluye el «Constitution Check».
   - Las **tareas** son pasos pequeños, verificables y con un responsable.
3. Si aparece algo no previsto, primero se actualiza el spec o el plan y después el código.
4. Las especificaciones viven en `specs/NNN-nombre-del-modulo/` con exactamente cuatro archivos: `spec.md`, `plan.md`, `task.md` y `check.md`. El `check.md` es la lista de verificación del módulo: reúne la evidencia de cada criterio de aceptación, de cada tipo de prueba, del PR, del release y de la memoria.
5. **Specs atómicos.** Cada spec cubre una sola capacidad: una pantalla o un flujo corto (por ejemplo, «Iniciar sesión» y «Cerrar sesión» van separados). Las pantallas se especifican conforme se necesitan, a partir de la arquitectura global y de la maqueta.
6. Los IDs de tarea llevan el número del módulo para no repetirse: el módulo `000` usa `T001…T099`, el `001` usa `T101…T199` y así sucesivamente.
7. Un spec pasa por los estados *Borrador → En revisión → Aprobado → Publicado → Cerrado*. Solo se marca *Cerrado* cuando su `check.md` está completo, la versión está publicada y quedó registrada en `memoria.md`.

## Artículo II. Modularidad e iteraciones

1. El módulo `000-arquitectura-base` se construye primero. No contiene pantallas ni flujos del negocio: solo la estructura, el sistema de diseño, la navegación, la red, el almacenamiento seguro, las pruebas y CI/CD.
2. Los specs funcionales se pueden **redactar** antes de terminar la base para dar contexto a los agentes, pero sus tareas **no se ejecutan** hasta que `000-arquitectura-base` esté cerrado y el backend haya entregado la URL y el JSON de los endpoints que consumen.
3. Cada módulo funcional define sus pantallas, los endpoints que consume y los subagentes y skills que necesita.
4. Un módulo se cierra solo con todas sus tareas verificadas y registradas en `memoria.md`.

## Artículo III. Arquitectura

1. **Plataforma:** Android nativo con **Kotlin** y **Jetpack Compose** (Material 3). `minSdk 26`, `targetSdk` y `compileSdk 36`.
2. **Patrón:** MVVM con flujo unidireccional de datos (UDF). La UI observa un `UiState` inmutable expuesto por el `ViewModel` con `StateFlow`; los eventos suben y el estado baja.
3. **Capas:**

   | Capa | Contenido | Regla |
   |---|---|---|
   | UI | Pantallas Compose y ViewModels | No conoce Retrofit ni DataStore |
   | Dominio | Modelos y casos de uso en Kotlin puro | Sin dependencias de Android |
   | Datos | Repositorios, fuentes remotas (API) y locales | Única capa que habla con la red y el almacenamiento |

4. **Multi-módulo de Gradle:** `:app` más módulos `:core:*`. Los módulos funcionales futuros serán `:feature:*` y no dependen entre sí.
5. **Inyección de dependencias** con Hilt. Concurrencia con corrutinas y `Flow`. Nada de `GlobalScope`.
6. **Una sola fuente de verdad** por dato: el repositorio.

## Artículo IV. Contrato con la API (acuerdo con Backend)

1. La app consume **solo** la API de `VirGo-API` en `API_BASE_URL`, bajo `/api/v1`.
2. El JSON va en `camelCase`, las fechas en ISO-8601 UTC (se muestran en la zona horaria del dispositivo) y los montos en MXN.
3. Los errores llegan como **Problem Details (RFC 9457)**. La app los convierte en un tipo de error propio y nunca muestra mensajes técnicos al usuario.
4. Se autentica con un token Bearer en el encabezado `Authorization`.
5. El contrato oficial es el OpenAPI que publica el backend. Cualquier cambio se registra en `memoria.md` de ambos proyectos.

## Artículo V. Seguridad

1. **Solo HTTPS.** El tráfico en claro está prohibido en *release*. Solo *debug* puede permitir `10.0.2.2` (el emulador) para pruebas locales.
2. **Ningún secreto en el código ni en el APK.** La app no guarda llaves de servicios; la URL de la API no es un secreto.
3. **Tokens** guardados en DataStore y cifrados con una llave del Android Keystore. Se borran al cerrar sesión.
4. Las versiones *release* se generan con R8 (minificación y ofuscación) y sin logs de red con cuerpos ni tokens.
5. Los permisos del sistema se piden solo cuando se usan y con explicación previa.
6. Las dependencias se revisan con Dependabot y *dependency review*, el código con Android Lint y CodeQL, y el repositorio con Gitleaks en cada PR.
7. La firma de *release* (keystore) vive solo en GitHub Secrets.
8. El `.gitignore` existe desde el primer commit y excluye secretos (`local.properties`, keystores, `keystore.properties`, `.env`, `google-services.json`), APK/AAB y artefactos de compilación. Agregar una exclusión de seguridad no requiere enmienda; quitarla, sí.

## Artículo VI. Calidad y pruebas

1. Cada tarea define cómo se verifica antes de empezarla.
2. Hay **pruebas unitarias** (JUnit, MockK, Turbine) de ViewModels, casos de uso y repositorios, y **pruebas de UI** con Compose Testing en los componentes del sistema de diseño y la navegación.
3. Las capas de red se prueban con un servidor simulado (MockWebServer), nunca contra producción.
4. El formato y análisis estático (ktlint + detekt + Android Lint) no deben dejar errores.
5. No se fusiona un PR con pruebas o análisis en rojo.
6. Cada pantalla o flujo tiene cinco tipos de prueba:

   | Tipo | Herramienta | Qué verifica |
   |---|---|---|
   | Unitarias | JUnit, MockK y Turbine | ViewModels, casos de uso y repositorios |
   | Integración | MockWebServer y Robolectric | Red, conversión de errores y almacenamiento con el contrato real del backend |
   | UI | Compose Testing | Componentes, navegación y accesibilidad |
   | Seguridad | Pruebas unitarias y Lint | Tokens cifrados y borrados al cerrar sesión, sin tráfico en claro, sin datos sensibles en logs |
   | Mutación | Pitest (`gradle-pitest-plugin`) en los módulos JVM puros | Que las pruebas detecten cambios en el dominio y los casos de uso |

   La prueba de **humo** se hace sobre el APK publicado: se instala, arranca y llega a la API de producción por HTTPS.
7. Los umbrales de cobertura y de mutación se fijan después de medirlos, nunca antes.

## Artículo VII. Diseño e interfaz

1. El sistema de diseño (`:core:designsystem`) replica los tokens de la maqueta:
   - Colores: violeta `#6366F1` (primario) y `#4F46E5` (oscuro), tinta `#18191F`, gris `#747782`, coral `#FF6B6B`, verde `#21A668`, rojo `#EB5757` y dorado `#E6A937`.
   - Tipografía: **Manrope** para títulos y **DM Sans** para textos.
2. Las pantallas solo usan componentes del sistema de diseño; no se crean estilos sueltos.
3. **Accesibilidad:** contraste AA, áreas táctiles de 48 dp o más, `contentDescription` en íconos con acción y soporte de texto escalable.
4. Hay tema claro y oscuro.
5. Los textos visibles están en `strings.xml` (español), nunca en el código.

## Artículo VIII. Integración y entrega continuas a costo cero

1. **GitHub** aloja el código. La rama `main` está protegida con un **ruleset** de GitHub: PR obligatorio con una revisión, *status checks* del CI requeridos, sin `push --force` y sin borrar la rama.
2. **CI** en cada PR: ktlint, detekt, Lint, pruebas unitarias, de integración, de UI, de seguridad y de mutación, CodeQL, Gitleaks y `assembleDebug`. Si el CI falla, se corrige en la misma rama y sobre el mismo spec antes de fusionar.
3. **CD manual:** el release se lanza a mano desde GitHub con el botón *Run workflow* (`workflow_dispatch`), solo desde `main` y con el CI en verde. Compila el APK *release* firmado y lo publica como **GitHub Release** con el número de versión y el commit.
4. Solo se usan herramientas gratuitas: GitHub Actions y GitHub Releases. Una tienda o servicio de pago requiere enmendar este documento. **Google Play** es el destino final del curso; su cuenta de desarrollador tiene un pago único, por lo que se agregará con una enmienda cuando el equipo la tenga.
5. `API_BASE_URL` de *release* es la URL HTTPS publicada por el despliegue del backend y se inyecta desde una variable de GitHub.
6. Antes de integrar un endpoint, el backend entrega su URL HTTPS y el JSON probado (petición y respuesta). Sin esa entrega, la pantalla se prueba solo contra MockWebServer.
7. **Repositorio único:** los workflows, el ruleset y Dependabot viven en `.github/` de la raíz del repositorio, compartidos con el backend; los de este proyecto llevan el prefijo `movil-`. Ningún workflow requerido usa filtros `paths`: cada uno decide con un job `changes`.
8. Después de un release exitoso se registra en `memoria.md` lo aprendido, el spec se marca como *Publicado* y se proponen las mejoras a la constitución, `AGENTS.md`, las skills y los subagentes.

## Artículo IX. Observabilidad

1. Logging con Timber: solo en *debug*, sin datos personales.
2. Cada solicitud lleva un `X-Correlation-Id`. Cuando hay un error, se registra para cruzarlo con los logs del backend.

## Artículo X. Memoria y trazabilidad

1. `memoria.md` es la bitácora oficial. Cada sesión agrega una entrada con la fecha, lo hecho, las decisiones, la evidencia y lo pendiente.
2. Los commits siguen *Conventional Commits* y citan el ID de la tarea (por ejemplo, `feat(T012): …`).
3. Las decisiones de arquitectura se registran como ADR breves en `memoria.md`.

## Artículo XI. Gobierno de agentes y skills

1. Los subagentes y sus responsabilidades se definen en `AGENTS.md` y en `.opencode/agents/`. Ningún agente actúa fuera de su rol.
2. Ningún agente modifica este documento. Las enmiendas las aprueba el equipo humano.
3. Solo se usan skills externas que estén en la lista permitida de `AGENTS.md`.
4. Los agentes no ejecutan tareas que no estén en un `task.md` aprobado.
5. Ningún agente publica *releases*, cambia secretos ni hace un push forzado sin confirmación humana.
6. **Servidores MCP.** Solo se usan los MCP de la lista permitida de `AGENTS.md`, configurados en `opencode.json` (por ejemplo, el MCP de Figma para leer el *canvas* de «Pantallas VirGo»). Se instalan antes de codificar la pantalla que los necesita, sus credenciales se leen de variables de entorno y no escriben en archivos de diseño ni en producción sin confirmación humana.

## Artículo XII. Idioma y estilo

1. La documentación, los specs, la memoria y los textos de la app van en **español**. La excepción es `README.md`, que va en **inglés** porque es la portada pública del repositorio.
2. El código y los identificadores van en **inglés**, con las convenciones oficiales de Kotlin.
3. El formato del código lo definen `.editorconfig` y ktlint, y es obligatorio.

---

## Enmiendas

- Una enmienda se propone con un PR que modifica este archivo e incluye el motivo, el impacto y una nueva versión (*SemVer*).
- En el mismo PR se ajustan `AGENTS.md`, los subagentes, las skills y los specs afectados.
- Cada enmienda se registra en `memoria.md`.

| Versión | Fecha | Cambio |
|---|---|---|
| 1.0.0 | 2026-10-29 | Versión inicial |
| 1.1.0 | 2026-11-03 | **Aprobada por el equipo el 2026-11-05.** Lineamientos de la clase del 28 y 29 de octubre: `check.md` y specs atómicos por pantalla (I.4–I.7), redacción anticipada (II.2), `.gitignore` y Gitleaks (V.6, V.8), cinco tipos de prueba con mutación y humo (VI.6–VI.7), ruleset, CD manual, Google Play como destino futuro, entrega de URL y JSON por el backend, repositorio único y aprendizaje tras el release (VIII), servidores MCP (XI.6) y README en inglés (XII.1). |
