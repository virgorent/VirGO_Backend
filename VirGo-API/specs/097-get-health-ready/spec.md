# Spec — 097 GET /health/ready

| Campo | Valor |
|---|---|
| Módulo | `097-get-health-ready` |
| Endpoint | `GET /api/v1/health/ready` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Salud y operación |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.17 |
| Consumido por Móvil | uso operativo |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Saber si la API y sus dependencias están disponibles. Este spec cubre solo `GET /api/v1/health/ready`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Salud y operación: base disponible y configuración válida. |
| RF-02 | No expone datos sensibles. |
| RF-03 | Se usan en las pruebas de humo. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `GET /api/v1/health/ready` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Salud y operación.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | `000` define `/health`; hay que decidir si se conserva o se reemplaza. | Equipo |
