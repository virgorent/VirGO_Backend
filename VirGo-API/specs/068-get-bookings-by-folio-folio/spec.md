# Spec — 068 GET /bookings/by-folio/:folio

| Campo | Valor |
|---|---|
| Módulo | `068-get-bookings-by-folio-folio` |
| Endpoint | `GET /api/v1/bookings/by-folio/:folio` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Consulta de reservaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.9 |
| Consumido por Móvil | `010-reservaciones` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Consultar reservaciones y comprobantes. Este spec cubre solo `GET /api/v1/bookings/by-folio/:folio`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Consulta de reservaciones: buscar por folio. |
| RF-02 | La búsqueda por folio tiene límite de solicitudes. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/bookings/by-folio/:folio` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Consulta de reservaciones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
