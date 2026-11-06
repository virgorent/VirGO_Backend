# Spec — 019 GET /payment-cards

| Campo | Valor |
|---|---|
| Módulo | `019-get-payment-cards` |
| Endpoint | `GET /api/v1/payment-cards` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Tarjetas de pago |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.4 |
| Consumido por Móvil | `009-reservar-pagar`, `021-favoritos-perfil` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Guardar referencias tokenizadas de tarjetas, sin datos sensibles. Este spec cubre solo `GET /api/v1/payment-cards`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Tarjetas de pago: listar tarjetas propias. |
| RF-02 | Nunca devuelve `gatewayToken`. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/payment-cards` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Tarjetas de pago.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
