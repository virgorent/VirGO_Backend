# Spec — 025 GET /venues/mine

| Campo | Valor |
|---|---|
| Módulo | `025-get-venues-mine` |
| Endpoint | `GET /api/v1/venues/mine` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `025-propietario-herramientas` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que el propietario cree y mantenga sus salones. Este spec cubre solo `GET /api/v1/venues/mine`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Gestión de salones: salones propios. |
| RF-02 | Solo el rol `owner` y solo sobre sus salones. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/venues/mine` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Gestión de salones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
