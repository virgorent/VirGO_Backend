# Spec — 046 PATCH /events/:id

| Campo | Valor |
|---|---|
| Módulo | `046-patch-events-id` |
| Endpoint | `PATCH /api/v1/events/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `012-ia-studio`, `014-mi-evento`, `015-presupuesto`, `016-invitados`, `017-pendientes`, `018-pagos-evento` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir crear y planear un evento en un solo lugar. Este spec cubre solo `PATCH /api/v1/events/:id`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Eventos: actualizar con permiso `edit`. |
| RF-02 | `venue` se guarda como copia validada contra un salón publicado. |
| RF-03 | `budgetPlan`, `reminders`, `vendors`, `externalPayments`, `design` e `invitation` viajan dentro del evento. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `PATCH /api/v1/events/:id` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |
| CA-04 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Eventos.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Sub-recursos propios o `PATCH` del evento para esas listas? | Equipo |
