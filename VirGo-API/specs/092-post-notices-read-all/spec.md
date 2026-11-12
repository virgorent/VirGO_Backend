# Spec — 092 POST /notices/read-all

| Campo | Valor |
|---|---|
| Módulo | `092-post-notices-read-all` |
| Endpoint | `POST /api/v1/notices/read-all` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Notificaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.14 |
| Consumido por Móvil | `020-comunicacion` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Avisar a cada persona lo que requiere su atención. Este spec cubre solo `POST /api/v1/notices/read-all`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Notificaciones: marcar todos como leídos. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/notices/read-all` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Notificaciones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
