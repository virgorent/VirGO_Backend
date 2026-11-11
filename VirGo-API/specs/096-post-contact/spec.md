# Spec — 096 POST /contact

| Campo | Valor |
|---|---|
| Módulo | `096-post-contact` |
| Endpoint | `POST /api/v1/contact` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Contacto |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.16 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Recibir mensajes de ayuda. Este spec cubre solo `POST /api/v1/contact`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Contacto: enviar un mensaje de contacto. |
| RF-02 | Tiene límite de solicitudes. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona con o sin sesión, **cuando** llama a `POST /api/v1/contact` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Contacto.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | Purga a los 12 meses, pendiente en `VirGo-Database`. | Equipo |
