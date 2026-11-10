# Spec — 024 GET /venues/:id

| Campo | Valor |
|---|---|
| Módulo | `024-get-venues-id` |
| Endpoint | `GET /api/v1/venues/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Detalle de salón |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `001-inicio-visitante`, `006-detalle-salon` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Mostrar la información completa de un salón publicado. Este spec cubre solo `GET /api/v1/venues/:id`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Detalle de salón: detalle de un salón publicado. |
| RF-02 | Un salón sin publicar responde 404 a quien no es su dueño. |
| RF-03 | Sin cuenta se ocultan precios y fechas (A3). |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `GET /api/v1/venues/:id` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Detalle de salón.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Dónde se calcula el precio de fin de semana y temporada alta: API o app? | Equipo |
