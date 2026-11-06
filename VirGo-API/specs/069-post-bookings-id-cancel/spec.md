# Spec — 069 POST /bookings/:id/cancel

| Campo | Valor |
|---|---|
| Módulo | `069-post-bookings-id-cancel` |
| Endpoint | `POST /api/v1/bookings/:id/cancel` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Cancelar reservación |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `010-reservaciones` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Cancelar una reservación según la política. Este spec cubre solo `POST /api/v1/bookings/:id/cancel`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Cancelar reservación: cancelar según la política. |
| RF-02 | Con 30 días o más se reembolsa el 50% del anticipo; con menos, nada. |
| RF-03 | Elimina `venueDates` por `venueId`, `date` y `bookingId`; si no coincide, aborta. |
| RF-04 | Es una transacción. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/bookings/:id/cancel` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Cancelar reservación.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | Reprogramar (C33): `VirGo-Database` lo marca pendiente y no tiene endpoint. | Equipo |
