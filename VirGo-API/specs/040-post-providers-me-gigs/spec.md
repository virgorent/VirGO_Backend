# Spec — 040 POST /providers/me/gigs

| Campo | Valor |
|---|---|
| Módulo | `040-post-providers-me-gigs` |
| Endpoint | `POST /api/v1/providers/me/gigs` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Agenda del proveedor |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `027-proveedor-inicio-agenda` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que el proveedor organice sus servicios por semana. Este spec cubre solo `POST /api/v1/providers/me/gigs`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Agenda del proveedor: crear un servicio. |
| RF-02 | Solo sobre la agenda propia. |
| RF-03 | Avisa de empalmes en la semana (H74). |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/providers/me/gigs` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Agenda del proveedor.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
