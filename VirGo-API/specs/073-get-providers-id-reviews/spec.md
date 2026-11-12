# Spec — 073 GET /providers/:id/reviews

| Campo | Valor |
|---|---|
| Módulo | `073-get-providers-id-reviews` |
| Endpoint | `GET /api/v1/providers/:id/reviews` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Reseñas |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.10 |
| Consumido por Móvil | `011-proveedores-cliente`, `029-proveedor-resenas-estadisticas` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Publicar y responder reseñas. Este spec cubre solo `GET /api/v1/providers/:id/reviews`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Reseñas: reseñas públicas de un proveedor. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `GET /api/v1/providers/:id/reviews` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Reseñas.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
