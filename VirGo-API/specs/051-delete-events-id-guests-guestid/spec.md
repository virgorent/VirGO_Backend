# Spec — 051 DELETE /events/:id/guests/:guestId

| Campo | Valor |
|---|---|
| Módulo | `051-delete-events-id-guests-guestid` |
| Endpoint | `DELETE /api/v1/events/:id/guests/:guestId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Invitados |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `016-invitados` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Administrar los invitados de un evento. Este spec cubre solo `DELETE /api/v1/events/:id/guests/:guestId`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Invitados: eliminar un invitado. |
| RF-02 | Requiere permiso sobre el evento: `view` para leer y `edit` para escribir. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `DELETE /api/v1/events/:id/guests/:guestId` con datos válidos, **entonces** recibe 204. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Invitados.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
