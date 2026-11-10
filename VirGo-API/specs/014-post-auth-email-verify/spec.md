# Spec — 014 POST /auth/email/verify

| Campo | Valor |
|---|---|
| Módulo | `014-post-auth-email-verify` |
| Endpoint | `POST /api/v1/auth/email/verify` |
| Versión | 1.0.0 |
| Estado | Borrador; **requiere decisión** |
| Fecha | 2026-11-05 |
| Grupo | Verificar correo |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.2 |
| Consumido por Móvil | `002-registro` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Confirmar que la persona controla el correo de su cuenta. Este spec cubre solo `POST /api/v1/auth/email/verify`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Verificar correo: verificar el correo. |
| RF-02 | Actualiza `users.isVerified`. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** cualquier persona, **cuando** llama a `POST /api/v1/auth/email/verify` con datos válidos, **entonces** recibe 200. |

## 5. Fuera de alcance

- Otros endpoints del grupo Verificar correo.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | `API-HANDOFF.md` lo marca «si se aprueba»: falta decidir si la verificación es obligatoria y cómo se envía el correo. | Equipo |
