# Spec — 061 GET /quotes/:id

| Campo | Valor |
|---|---|
| Módulo | `061-get-quotes-id` |
| Endpoint | `GET /api/v1/quotes/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Consulta de cotizaciones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.8 |
| Consumido por Móvil | `008-cotizaciones-cliente`, `024-propietario-solicitudes`, `028-proveedor-solicitudes` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Consultar cotizaciones según el rol. Este spec cubre solo `GET /api/v1/quotes/:id`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Consulta de cotizaciones: detalle con autorización. |
| RF-02 | Cada persona solo ve las suyas o las dirigidas a su salón o perfil. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/quotes/:id` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Consulta de cotizaciones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
