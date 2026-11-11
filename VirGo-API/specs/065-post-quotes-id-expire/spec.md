# Spec — 065 POST /quotes/:id/expire

| Campo | Valor |
|---|---|
| Módulo | `065-post-quotes-id-expire` |
| Endpoint | `POST /api/v1/quotes/:id/expire` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Respuesta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | proceso interno |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Responder una cotización y decidir sobre ella. Este spec cubre solo `POST /api/v1/quotes/:id/expire`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Respuesta de cotizaciones: marcar como vencida. |
| RF-02 | Vigencia de 15 días (C7). |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** un proceso interno, **cuando** llama a `POST /api/v1/quotes/:id/expire` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Respuesta de cotizaciones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Quién ejecuta el vencimiento? No hay un job definido. | Equipo |
