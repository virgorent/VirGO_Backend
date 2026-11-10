# Spec — 013 POST /auth/refresh

| Campo | Valor |
|---|---|
| Módulo | `013-post-auth-refresh` |
| Endpoint | `POST /api/v1/auth/refresh` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Renovar sesión |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `003-iniciar-sesion` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Mantener la sesión abierta en el dispositivo sin pedir la contraseña. Este spec cubre solo `POST /api/v1/auth/refresh`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Renovar sesión: renovar el token de acceso. |
| RF-02 | Rechaza sesiones revocadas o expiradas. |
| RF-03 | Rota el token de sesión en cada uso. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una sesión vigente, **cuando** llama a `POST /api/v1/auth/refresh` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Renovar sesión.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| — | Sin preguntas abiertas | — |
