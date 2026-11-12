# Spec — 058 PATCH /events/:id/collaborators/:userId

| Campo | Valor |
|---|---|
| Módulo | `058-patch-events-id-collaborators-userid` |
| Endpoint | `PATCH /api/v1/events/:id/collaborators/:userId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Colaboradores del evento |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `019-compartir-calificar` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Compartir un evento con otras personas. Este spec cubre solo `PATCH /api/v1/events/:id/collaborators/:userId`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Colaboradores del evento: cambiar el permiso. |
| RF-02 | Solo el dueño administra colaboradores. |
| RF-03 | Permisos: `view` o `edit`. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `PATCH /api/v1/events/:id/collaborators/:userId` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |
| CA-04 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Colaboradores del evento.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
