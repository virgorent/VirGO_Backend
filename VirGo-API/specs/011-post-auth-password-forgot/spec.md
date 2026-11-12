# Spec — 011 POST /auth/password/forgot

| Campo | Valor |
|---|---|
| Módulo | `011-post-auth-password-forgot` |
| Endpoint | `POST /api/v1/auth/password/forgot` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Recuperar contraseña |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `004-recuperar-contrasena` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que una persona recupere el acceso a su cuenta sin revelar si el correo existe. Este spec cubre solo `POST /api/v1/auth/password/forgot`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Recuperar contraseña: solicitar recuperación. |
| RF-02 | La respuesta no revela si el correo existe. |
| RF-03 | El código vale 15 minutos y es de un solo uso. |
| RF-04 | Solo se guarda su hash. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `POST /api/v1/auth/password/forgot` con datos válidos, **entonces** recibe 202. |
| CA-02 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Recuperar contraseña.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | No hay mecanismo de envío de correo aprobado; `API-HANDOFF.md` prohíbe inventarlo. | Equipo |
| P-02 | `passwordResets`: `tokenHash` y `used` o `codeHash` y `attempts` (`VirGo-Database` `001` P-03). | Equipo |
