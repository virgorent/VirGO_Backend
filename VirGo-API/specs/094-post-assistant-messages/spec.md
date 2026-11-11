# Spec — 094 POST /assistant/messages

| Campo | Valor |
|---|---|
| Módulo | `094-post-assistant-messages` |
| Endpoint | `POST /api/v1/assistant/messages` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Asistente IA |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.15 |
| Consumido por Móvil | `013-asistente-ia` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Conversar con el asistente de planeación. Este spec cubre solo `POST /api/v1/assistant/messages`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Asistente IA: enviar un mensaje. |
| RF-02 | El historial se borra solo a los 30 días. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/assistant/messages` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Asistente IA.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | El proveedor de IA no está definido; `API-HANDOFF.md` prohíbe inventarlo. | Equipo |
| P-02 | Cómo mantener el costo en $0. | Equipo |
