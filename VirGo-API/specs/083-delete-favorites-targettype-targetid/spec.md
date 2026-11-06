# Spec — 083 DELETE /favorites/:targetType/:targetId

| Campo | Valor |
|---|---|
| Módulo | `083-delete-favorites-targettype-targetid` |
| Endpoint | `DELETE /api/v1/favorites/:targetType/:targetId` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Favoritos |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.12 |
| Consumido por Móvil | `005-explorar-salones`, `006-detalle-salon` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Guardar salones y proveedores. Este spec cubre solo `DELETE /api/v1/favorites/:targetType/:targetId`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Favoritos: quitar por destino. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `DELETE /api/v1/favorites/:targetType/:targetId` con datos válidos, **entonces** recibe 204. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Favoritos.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | `API-HANDOFF.md` propone dos formas de borrar; aquí se elige por destino. | Equipo |
