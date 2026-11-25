# Spec — 091 PATCH /notices/:id/read

| Campo | Valor |
|---|---|
| Módulo | `091-patch-notices-id-read` |
| Endpoint | `PATCH /api/v1/notices/:id/read` |
| Versión | 1.0.0 |
| Estado | Implementación local; despliegue pendiente |
| Fecha | 2026-11-05 |
| Grupo | Notificaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.14 |
| Consumido por Móvil | `020-comunicacion` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Avisar a cada persona lo que requiere su atención. Este spec cubre solo `PATCH /api/v1/notices/:id/read`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Notificaciones: marcar uno como leído. |
| RF-02 | Solo se modifica un aviso cuyo `userId` coincide con el `sub` del token. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `PATCH /api/v1/notices/:id/read` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

La operación es idempotente: marcar un aviso ya leído vuelve a responder `200` con `read: true`.

## 5. Fuera de alcance

- Otros endpoints del grupo Notificaciones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
