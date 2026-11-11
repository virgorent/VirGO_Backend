# Spec — 070 POST /bookings/:id/payments

| Campo | Valor |
|---|---|
| Módulo | `070-post-bookings-id-payments` |
| Endpoint | `POST /api/v1/bookings/:id/payments` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Pagos de reservación |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `009-reservar-pagar` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Registrar los pagos y confirmar la reservación. Este spec cubre solo `POST /api/v1/bookings/:id/payments`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Pagos de reservación: registrar un pago. |
| RF-02 | Usa `Idempotency-Key`. |
| RF-03 | La fecha se aparta solo cuando el pago queda validado. |
| RF-04 | Confirmar es una transacción: inserta `venueDates`, y si el día está ocupado responde 409. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/bookings/:id/payments` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |
| CA-04 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Pagos de reservación.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | La pasarela y sus webhooks no están definidos; hoy el pago es simulado. | Equipo |
