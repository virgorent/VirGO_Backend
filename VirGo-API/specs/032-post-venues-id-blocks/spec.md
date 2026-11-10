# Spec — 032 POST /venues/:id/blocks

| Campo | Valor |
|---|---|
| Módulo | `032-post-venues-id-blocks` |
| Endpoint | `POST /api/v1/venues/:id/blocks` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Disponibilidad de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `023-propietario-inicio-calendario` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Mostrar y administrar los días ocupados de cada salón. Este spec cubre solo `POST /api/v1/venues/:id/blocks`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Disponibilidad de salones: crear un bloqueo manual. |
| RF-02 | El bloqueo crea `kind: blocked` y no admite `bookingId`. |
| RF-03 | Un día ya ocupado responde 409. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/venues/:id/blocks` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |
| CA-04 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Disponibilidad de salones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
