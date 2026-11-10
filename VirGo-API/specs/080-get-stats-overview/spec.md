# Spec — 080 GET /stats/overview

| Campo | Valor |
|---|---|
| Módulo | `080-get-stats-overview` |
| Endpoint | `GET /api/v1/stats/overview` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Estadísticas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.11 |
| Consumido por Móvil | `023-propietario-inicio-calendario`, `027-proveedor-inicio-agenda` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Mostrar métricas al dueño de cada salón o perfil. Este spec cubre solo `GET /api/v1/stats/overview`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Estadísticas: resumen según el rol. |
| RF-02 | `visits` queda fuera del MVP. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/stats/overview` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Estadísticas.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
