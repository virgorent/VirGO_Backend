# Spec — 031 GET /venues/:id/availability

| Campo | Valor |
|---|---|
| Módulo | `031-get-venues-id-availability` |
| Endpoint | `GET /api/v1/venues/:id/availability` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Disponibilidad de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `006-detalle-salon`, `023-propietario-inicio-calendario` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Mostrar y administrar los días ocupados de cada salón. Este spec cubre solo `GET /api/v1/venues/:id/availability`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Disponibilidad de salones: días ocupados o bloqueados en un rango. |
| RF-02 | No expone notas ni `bookingId` a personas no autorizadas. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `GET /api/v1/venues/:id/availability` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Disponibilidad de salones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
