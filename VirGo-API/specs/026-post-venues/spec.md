# Spec — 026 POST /venues

| Campo | Valor |
|---|---|
| Módulo | `026-post-venues` |
| Endpoint | `POST /api/v1/venues` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Gestión de salones |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.5 |
| Consumido por Móvil | `022-propietario-registrar-salon` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que el propietario cree y mantenga sus salones. Este spec cubre solo `POST /api/v1/venues`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Gestión de salones: crear un salón en `draft`. |
| RF-02 | Solo el rol `owner` y solo sobre sus salones. |
| RF-03 | Ignora `rating`, `verified`, `createdAt` y `updatedAt` enviados por el cliente. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/venues` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Gestión de salones.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | No hay endpoint de carga de archivos para fotos y planos. | Equipo |
