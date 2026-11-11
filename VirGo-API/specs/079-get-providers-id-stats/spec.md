# Spec — 079 GET /providers/:id/stats

| Campo | Valor |
|---|---|
| Módulo | `079-get-providers-id-stats` |
| Endpoint | `GET /api/v1/providers/:id/stats` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Estadísticas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.11 |
| Consumido por Móvil | `029-proveedor-resenas-estadisticas` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Mostrar métricas al dueño de cada salón o perfil. Este spec cubre solo `GET /api/v1/providers/:id/stats`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Estadísticas: métricas del proveedor propio. |
| RF-02 | Solo el dueño las consulta. |
| RF-03 | `visits` queda fuera del MVP. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/providers/:id/stats` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Estadísticas.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
