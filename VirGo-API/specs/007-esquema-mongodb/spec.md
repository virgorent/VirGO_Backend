# Spec — 007 Esquema de MongoDB

| Campo | Valor |
|---|---|
| Módulo | `007-esquema-mongodb` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-04 |
| Constitución | v1.1.0. Aplica sobre todo III, V y VI |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Origen | Revisión de `virgo_dev` y clase del 2026-11-04: la base, el modelo y los specs se contradecían, y ningún campo de texto tenía longitud máxima |

## 1. Contexto y objetivo

La base `virgo_dev` ya tenía 22 colecciones, pero no coincidía con el modelo de datos: el validador de `users`, por ejemplo, rechazaría el bloqueo de cuentas del login. Además, un campo pensado para 50 caracteres podía recibir 1500.

El objetivo es tener **una sola fuente de verdad** del esquema, versionada en el repositorio, y una comprobación automática que impida que la base y los documentos vuelvan a separarse.

## 2. Historias técnicas

| ID | Historia |
|---|---|
| HT-01 | Como **desarrollador**, quiero un solo lugar que defina colecciones, validadores e índices, para no adivinar cuál documento es correcto. |
| HT-02 | Como **responsable de seguridad**, quiero que todo texto tenga longitud máxima, para que una entrada enorme no llegue a la base. |
| HT-03 | Como **equipo**, quiero detectar automáticamente si la base difiere del esquema, para no descubrirlo cuando falle un endpoint. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Cada colección tiene un archivo versionado con su validador y sus índices. |
| RF-02 | Todo campo de texto libre tiene longitud máxima. |
| RF-03 | Una comprobación sin credenciales falla si algún campo de texto libre no tiene límite. |
| RF-04 | Una comprobación contra la base lista las diferencias de colecciones, validadores e índices. |
| RF-05 | El modelo de datos y los specs coinciden con el esquema versionado. |
| RF-06 | La base se puede recrear desde el repositorio, sin pasos manuales. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** ninguna credencial en el repositorio; las herramientas leen la conexión de una variable de entorno. |
| RNF-02 | **Costo:** $0. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el repositorio, **cuando** ejecuto la comprobación sin credenciales, **entonces** reporta 0 campos sin límite. |
| CA-02 | **Dado** `virgo_dev`, **cuando** ejecuto la comprobación contra la base, **entonces** reporta 0 diferencias. |
| CA-03 | **Dado** un documento con un campo que supera su límite, **cuando** intento insertarlo, **entonces** la base lo rechaza. |
| CA-04 | **Dado** un usuario con `failedLoginCount` y `lockoutEndsAt`, **cuando** lo guardo, **entonces** la base lo acepta. |
| CA-05 | **Dado** un campo nuevo sin `maxLength`, **cuando** abro un PR, **entonces** el CI falla. |

## 6. Fuera de alcance

- Crear la base `virgo` de producción: la crea la API en el primer despliegue.
- La validación en los DTO: la define cada endpoint, con estos mismos límites.

## 7. Supuestos, dependencias y preguntas abiertas

**Dependencias:** `001` T101 (clúster y usuarios). Este módulo debe cerrarse antes de empezar `003`.

**Preguntas abiertas**

| # | Pregunta | Valor provisional | Responsable |
|---|---|---|---|
| P-01 | ¿Las zonas y categorías reales son las del modelo o las que tenía la base? | Las del modelo, que salieron de la maqueta | Equipo |
| P-02 | ¿`_id` como texto UUID? | Sí, como ya estaba en la base | Equipo |
| P-03 | ¿Quién creó la estructura original y dónde está su script? | Se reemplaza por `db/schema/` | Equipo |
| P-04 | ¿Se conserva `isVerified`, y `passwordResets` usa `codeHash` y `attempts`? | Sí a ambas | Equipo |
| P-05 | ¿Son correctas las longitudes de la sección 8 del modelo? | Propuesta de Claude Code | Equipo |
