# Spec — 018 DELETE /users/me

| Campo | Valor |
|---|---|
| Módulo | `018-delete-users-me` |
| Endpoint | `DELETE /api/v1/users/me` |
| Versión | 1.0.0 |
| Estado | En implementación |
| Fecha | 2026-11-05 |
| Grupo | Perfil de usuario |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.3 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que cada persona administre su cuenta. Este spec cubre solo `DELETE /api/v1/users/me`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Anonimizar el perfil del usuario y desactivar su acceso, conservando referencias históricas necesarias para reservas y reseñas. |
| RF-02 | Revocar todas las sesiones activas del usuario. |
| RF-03 | El correo, teléfono, contraseña y preferencias personales dejan de ser recuperables desde la cuenta. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `DELETE /api/v1/users/me` con datos válidos, **entonces** recibe 204. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** una cuenta eliminada, **cuando** intenta iniciar sesión con sus credenciales anteriores, **entonces** recibe 401. |
| CA-04 | **Dado** una cuenta con varias sesiones, **cuando** solicita su eliminación, **entonces** todas las sesiones quedan invalidadas. |

## 5. Fuera de alcance

- Otros endpoints del grupo Perfil de usuario.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Decisión aprobada: anonimización y desactivación, preservando referencias históricas. | Equipo |
