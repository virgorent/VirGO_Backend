# Spec — 074 POST /reviews

| Campo | Valor |
|---|---|
| Módulo | `074-post-reviews` |
| Endpoint | `POST /api/v1/reviews` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `019-compartir-calificar` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Publicar y responder reseñas. Este spec cubre solo `POST /api/v1/reviews`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Reseñas: crear una reseña elegible. |
| RF-02 | Una reseña por persona y destino. |
| RF-03 | La reseña de un salón exige `bookingId`. |
| RF-04 | Guardar la reseña actualiza `rating` en una transacción. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/reviews` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Reseñas.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
