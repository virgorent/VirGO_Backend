# Spec — 036 GET /providers/me

| Campo | Valor |
|---|---|
| Módulo | `036-get-providers-me` |
| Endpoint | `GET /api/v1/providers/me` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Perfil del proveedor |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `026-proveedor-perfil` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que el proveedor publique y mantenga su perfil. Este spec cubre solo `GET /api/v1/providers/me`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Perfil del proveedor: perfil propio. |
| RF-02 | Solo el rol `provider`. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `GET /api/v1/providers/me` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |

## 5. Fuera de alcance

- Otros endpoints del grupo Perfil del proveedor.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
