# Spec — 090 GET /notices

| Campo | Valor |
|---|---|
| Módulo | `090-get-notices` |
| Endpoint | `GET /api/v1/notices` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Notificaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.14 |
| Consumido por Móvil | `005-explorar-salones`, `020-comunicacion` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Avisar a cada persona lo que requiere su atención. Este spec cubre solo `GET /api/v1/notices`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Notificaciones: listar avisos propios. |
| RF-02 | Se borran solos a los 90 días. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/notices` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Notificaciones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | Notificaciones push: fuera de alcance. | Equipo |
