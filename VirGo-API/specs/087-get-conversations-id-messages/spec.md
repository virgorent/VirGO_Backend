# Spec — 087 GET /conversations/:id/messages

| Campo | Valor |
|---|---|
| Módulo | `087-get-conversations-id-messages` |
| Endpoint | `GET /api/v1/conversations/:id/messages` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Mensajes |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.13 |
| Consumido por Móvil | `020-comunicacion`, `024-propietario-solicitudes`, `028-proveedor-solicitudes` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir la conversación entre clientes, salones y proveedores. Este spec cubre solo `GET /api/v1/conversations/:id/messages`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Mensajes: mensajes por cursor. |
| RF-02 | Solo participan el cliente y el destino. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/conversations/:id/messages` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un recurso de otra persona o inexistente, **cuando** se solicita, **entonces** recibe 403 o 404, sin revelar datos. |

## 5. Fuera de alcance

- Otros endpoints del grupo Mensajes.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
