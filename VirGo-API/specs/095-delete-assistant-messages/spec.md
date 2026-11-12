# Spec — 095 DELETE /assistant/messages

| Campo | Valor |
|---|---|
| Módulo | `095-delete-assistant-messages` |
| Endpoint | `DELETE /api/v1/assistant/messages` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Asistente IA |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.15 |
| Consumido por Móvil | `013-asistente-ia` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Conversar con el asistente de planeación. Este spec cubre solo `DELETE /api/v1/assistant/messages`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Asistente IA: limpiar el historial. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `DELETE /api/v1/assistant/messages` con datos válidos, **entonces** recibe 204. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Asistente IA.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
