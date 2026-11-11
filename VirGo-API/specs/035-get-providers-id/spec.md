# Spec — 035 GET /providers/:id

| Campo | Valor |
|---|---|
| Módulo | `035-get-providers-id` |
| Endpoint | `GET /api/v1/providers/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Catálogo de proveedores |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `011-proveedores-cliente` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir buscar y comparar proveedores publicados. Este spec cubre solo `GET /api/v1/providers/:id`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Catálogo de proveedores: detalle publicado. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `GET /api/v1/providers/:id` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Catálogo de proveedores.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
