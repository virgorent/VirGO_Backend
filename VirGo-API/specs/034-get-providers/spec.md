# Spec — 034 GET /providers

| Campo | Valor |
|---|---|
| Módulo | `034-get-providers` |
| Endpoint | `GET /api/v1/providers` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Catálogo de proveedores |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `011-proveedores-cliente` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir buscar y comparar proveedores publicados. Este spec cubre solo `GET /api/v1/providers`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Catálogo de proveedores: catálogo publicado. |
| RF-02 | Filtros: `category`, `zone`, `minPrice`, `maxPrice`, `rating`, `q`, `cursor`, `limit`. |
| RF-03 | La comparación (B18) se resuelve en la app. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `GET /api/v1/providers` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Catálogo de proveedores.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | Categorías: la maqueta usa Catering, Música, Fotografía, Decoración y Mobiliario; la base tiene 9 distintas. | Equipo |
