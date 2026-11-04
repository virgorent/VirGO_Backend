# Spec — 000 Arquitectura base del Móvil

| Campo | Valor |
|---|---|
| Módulo | `000-arquitectura-base` |
| Versión | 1.1.0 |
| Estado | En revisión |
| Fecha | 2026-10-29 |
| Constitución | v1.1.0. Aplica todos los artículos; sobre todo II, III, IV, V, VII y VIII |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

La app de VirGo tendrá muchas pantallas para tres tipos de usuarios. Antes de construir cualquiera, hace falta una **base común**:

- Estructura del proyecto y sistema de diseño fiel a la maqueta.
- Navegación, conexión con la API, manejo de errores y almacenamiento seguro.
- Pruebas y entrega automática del APK.

El objetivo es que, al terminar este módulo, exista una app **sin funcionalidad del negocio pero lista para crecer**:

- Arranca con la identidad visual de VirGo.
- Tiene la navegación base.
- Se conecta por HTTPS a la API de VirGo y lo demuestra con una pantalla de diagnóstico.
- Se compila, prueba y publica automáticamente.

## 2. Historias técnicas

| ID | Historia |
|---|---|
| HT-01 | Como **desarrollador**, quiero un proyecto organizado en módulos y capas para agregar funcionalidades sin acoplarlas. |
| HT-02 | Como **diseñador o desarrollador**, quiero un sistema de diseño con los colores, tipografías y componentes de la maqueta para que todas las pantallas se vean consistentes. |
| HT-03 | Como **desarrollador**, quiero un cliente de red configurado contra la API de VirGo que interprete sus errores para no repetir esa lógica en cada pantalla. |
| HT-04 | Como **equipo**, quiero comprobar desde la app que la API responde para validar la integración desde el primer día. |
| HT-05 | Como **responsable de seguridad**, quiero que la base ya exija HTTPS, guarde los tokens cifrados y ofusque la versión final para no agregar esos controles después. |
| HT-06 | Como **equipo**, quiero que cada cambio se revise y pruebe solo, y publicar cada versión con un botón como APK descargable sin costo. |
| HT-07 | Como **persona usuaria**, quiero que la app respete el tema claro u oscuro y el tamaño de letra de mi teléfono para usarla cómodamente. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | El proyecto se divide en un módulo de aplicación y módulos base reutilizables: diseño, red, datos, almacenamiento, utilidades comunes y apoyo a pruebas. |
| RF-02 | La app muestra una pantalla de arranque con la marca VirGo y abre un contenedor de navegación con una pantalla de inicio provisional, sin contenido del negocio. |
| RF-03 | Existe un sistema de diseño con los colores, tipografías, formas, espaciados y componentes base de la maqueta, en tema claro y oscuro. |
| RF-04 | La navegación base permite registrar destinos nuevos desde módulos futuros, incluyendo una barra inferior configurable. |
| RF-05 | El cliente de red usa una dirección base distinta por variante (desarrollo o versión final), agrega el identificador de correlación y, si existe, el token de acceso. |
| RF-06 | Los errores de la API (Problem Details), la falta de conexión y los tiempos de espera se convierten en un tipo de error propio con mensajes en español para el usuario. |
| RF-07 | Existe un almacenamiento local cifrado para el token de acceso, con operaciones de guardar, leer y borrar. Aún no hay pantallas de inicio de sesión. |
| RF-08 | En la variante de desarrollo, una pantalla de diagnóstico muestra la versión de la app, la dirección de la API y el resultado del chequeo de salud del backend. |
| RF-09 | Los textos visibles están centralizados en recursos en español. |
| RF-10 | Cada PR ejecuta automáticamente el análisis estático, las pruebas (unitarias, integración, UI, seguridad y mutación), la búsqueda de secretos y la compilación. |
| RF-11 | Un release lanzado a mano desde la rama principal genera un APK firmado, crea su etiqueta de versión y lo publica como descarga en el repositorio. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Compatibilidad:** Android 8.0 (API 26) o superior. |
| RNF-02 | **Seguridad:** solo HTTPS en la versión final, token cifrado con una llave del sistema, versión final ofuscada y ningún secreto en el código ni en el APK. |
| RNF-03 | **Rendimiento:** arranque en frío de la app vacía en menos de 2 s en un emulador de gama media; sin bloqueos del hilo principal. |
| RNF-04 | **Accesibilidad:** contraste AA, objetivos táctiles de 48 dp o más, descripciones en íconos con acción y soporte de letra grande. |
| RNF-05 | **Calidad:** análisis estático sin errores y pruebas en verde para fusionar. |
| RNF-06 | **Costo:** $0. Solo herramientas y servicios gratuitos (GitHub Actions y Releases). |
| RNF-07 | **Trazabilidad:** cada APK publicado se asocia a una versión, un commit y una entrada en `memoria.md`. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el repositorio clonado, **cuando** compilo la variante de desarrollo, **entonces** se genera el APK sin errores con un solo comando. |
| CA-02 | **Dado** la app instalada, **cuando** la abro, **entonces** veo la marca VirGo y después la pantalla de inicio provisional con el tema y las tipografías de la maqueta. |
| CA-03 | **Dado** el teléfono en modo oscuro o con letra grande, **cuando** abro la app, **entonces** los componentes base se adaptan sin cortes ni falta de contraste. |
| CA-04 | **Dado** la variante de desarrollo con la API disponible, **cuando** abro el diagnóstico, **entonces** veo «API disponible» con la URL y el tiempo de respuesta. |
| CA-05 | **Dado** la API caída, sin red o respondiendo con un error, **cuando** se consulta, **entonces** la app muestra un mensaje claro en español y no se cierra. |
| CA-06 | **Dado** la versión final, **cuando** se intenta una conexión HTTP sin cifrar, **entonces** el sistema la bloquea. |
| CA-07 | **Dado** un token guardado, **cuando** reviso el almacenamiento de la app, **entonces** el valor no aparece en texto plano. |
| CA-08 | **Dado** un PR, **cuando** se abre, **entonces** el pipeline ejecuta el análisis, las pruebas y la compilación, y bloquea la fusión si algo falla. |
| CA-09 | **Dado** `main` en verde, **cuando** lanzo el release con *Run workflow* y termina el pipeline de publicación, **entonces** existe una descarga del APK firmado con su versión y su commit. |
| CA-10 | **Dado** el sistema de diseño, **cuando** reviso los componentes base, **entonces** cada uno tiene vista previa en claro y oscuro y prueba de UI. |

## 6. Fuera de alcance

- **Cualquier pantalla o flujo del negocio:** registro, inicio de sesión, mapa de salones, detalle, cotizaciones, reservaciones, pagos, Mi Evento, IA Studio, proveedores y propietarios. Se especifican en módulos posteriores.
- Mapas, ubicación, cámara y notificaciones push.
- Publicación en Google Play u otras tiendas.
- Versión para iOS.

## 7. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- El equipo tiene Android Studio (última versión estable) y JDK 17 o superior.
- Hay un repositorio en GitHub con permisos de administración.

**Dependencias**

- La URL HTTPS de producción de la API la entrega la tarea T210 de `VirGo-API` (antes T033). Mientras no exista, la versión de desarrollo apunta a la API local (`http://10.0.2.2:5080`, solo en *debug*).

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿El identificador de la aplicación será `mx.virgo.app`? | Equipo |
| P-02 | ¿Quién custodia el keystore de firma y su respaldo? | Equipo |
