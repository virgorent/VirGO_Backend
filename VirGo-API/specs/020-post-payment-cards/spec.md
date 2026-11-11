# Spec — 020 POST /payment-cards

| Campo | Valor |
|---|---|
| Módulo | `020-post-payment-cards` |
| Endpoint | `POST /api/v1/payment-cards` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Tarjetas de pago |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.4 |
| Consumido por Móvil | `009-reservar-pagar` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Guardar referencias tokenizadas de tarjetas, sin datos sensibles. Este spec cubre solo `POST /api/v1/payment-cards`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Tarjetas de pago: registrar una referencia tokenizada. |
| RF-02 | La API nunca recibe ni guarda el número completo ni el CVV. |
| RF-03 | Nunca devuelve `gatewayToken`. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/payment-cards` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Tarjetas de pago.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | La pasarela de pago no está definida. | Equipo |
