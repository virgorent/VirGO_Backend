# Spec — 064 POST /quotes/:id/reject

| Campo | Valor |
|---|---|
| Módulo | `064-post-quotes-id-reject` |
| Endpoint | `POST /api/v1/quotes/:id/reject` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Respuesta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | `008-cotizaciones-cliente` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Responder una cotización y decidir sobre ella. Este spec cubre solo `POST /api/v1/quotes/:id/reject`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Respuesta de cotizaciones: rechazar como cliente. |
| RF-02 | Solo el destino responde y solo el cliente acepta o rechaza. |
| RF-03 | Un estado incompatible responde 409. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/quotes/:id/reject` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Respuesta de cotizaciones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
