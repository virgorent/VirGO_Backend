# Spec — NNN <nombre-del-módulo>

| Campo | Valor |
|---|---|
| Módulo | `NNN-<nombre>` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | AAAA-MM-DD |
| Constitución | vX.Y.Z |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

<Qué problema resuelve y por qué ahora. Una o dos frases.>

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **<rol>**, quiero **<acción>** para **<beneficio>**. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | <Comportamiento observable y verificable.> |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** <autenticación, autorización, validación de entrada, límites de tasa.> |
| RNF-02 | **Rendimiento:** <umbral medible.> |
| RNF-03 | **Accesibilidad / compatibilidad:** <...> |
| RNF-04 | **Costo:** <...> |
| RNF-05 | **Trazabilidad:** <...> |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** <contexto>, **cuando** <acción>, **entonces** <resultado>. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Fuga de secretos | Sin secretos en código ni logs; variables de entorno o GitHub Secrets. |
| Acceso no autorizado | Denegar por defecto; mínimo privilegio; validar en la frontera. |
| Datos personales | Minimizar, cifrar en tránsito y en reposo; no registrar datos sensibles. |
| Abuso o fuerza bruta | Límites de tasa y bloqueo temporal. |

## 7. Fuera de alcance

- <Lo que este módulo NO hace.>

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- <...>

**Dependencias**

- <...>

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | <...> | <...> |
