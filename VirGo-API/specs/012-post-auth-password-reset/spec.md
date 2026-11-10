# Spec — 012 POST /auth/password/reset

| Campo | Valor |
|---|---|
| Módulo | `012-post-auth-password-reset` |
| Endpoint | `POST /api/v1/auth/password/reset` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Recuperar contraseña |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `004-recuperar-contrasena` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que una persona recupere el acceso a su cuenta sin revelar si el correo existe. Este spec cubre solo `POST /api/v1/auth/password/reset`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Recuperar contraseña: cambiar la contraseña con el código. |
| RF-02 | El código vale 15 minutos y es de un solo uso. |
| RF-03 | Solo se guarda su hash. |
| RF-04 | Código incorrecto o vencido: 400 con mensaje genérico. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `POST /api/v1/auth/password/reset` con datos válidos, **entonces** recibe 200. |
| CA-02 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Recuperar contraseña.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | `passwordResets`: `tokenHash` y `used` o `codeHash` y `attempts` (`VirGo-Database` `001` P-03). | Equipo |
