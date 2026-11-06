# Spec — 043 GET /events

| Campo | Valor |
|---|---|
| Módulo | `043-get-events` |
| Endpoint | `GET /api/v1/events` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Eventos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.7 |
| Consumido por Móvil | `014-mi-evento` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir crear y planear un evento en un solo lugar. Este spec cubre solo `GET /api/v1/events`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Eventos: eventos propios y compartidos. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/events` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Eventos.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
