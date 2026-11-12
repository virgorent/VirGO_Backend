# Spec — 017 GET /users/:id/public

| Campo | Valor |
|---|---|
| Módulo | `017-get-users-id-public` |
| Endpoint | `GET /api/v1/users/:id/public` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Perfil de usuario |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.3 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que cada persona administre su cuenta. Este spec cubre solo `GET /api/v1/users/:id/public`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Perfil de usuario: perfil público mínimo. |
| RF-02 | El perfil público no expone datos privados. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona con o sin sesión, **cuando** llama a `GET /api/v1/users/:id/public` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Perfil de usuario.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
