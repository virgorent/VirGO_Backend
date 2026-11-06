# Spec — 037 POST /providers

| Campo | Valor |
|---|---|
| Módulo | `037-post-providers` |
| Endpoint | `POST /api/v1/providers` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Perfil del proveedor |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.6 |
| Consumido por Móvil | `026-proveedor-perfil` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Permitir que el proveedor publique y mantenga su perfil. Este spec cubre solo `POST /api/v1/providers`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Perfil del proveedor: crear el perfil en `draft`. |
| RF-02 | Solo el rol `provider`. |
| RF-03 | Un usuario tiene un solo perfil: el segundo responde 409. |
| RF-04 | Crear el perfil actualiza `users.providerId` de forma consistente. |

## 3. Requerimientos no funcionales

Aplican los del Art. IV (contrato), V (seguridad), VI.6 (longitudes) y VII (costo cero) de la constitución.

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una persona autenticada con permiso, **cuando** llama a `POST /api/v1/providers` con datos válidos, **entonces** recibe 201. |
| CA-02 | **Dado** una solicitud sin token o con uno inválido, **cuando** llega, **entonces** recibe 401 en Problem Details. |
| CA-03 | **Dado** un dato inválido o un texto de 1500 caracteres, **cuando** se envía, **entonces** recibe 400 en Problem Details, nunca 500. |

## 5. Fuera de alcance

- Otros endpoints del grupo Perfil del proveedor.
- Cambiar la estructura de MongoDB: la administra `VirGo-Database`.

## 6. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | `basePrice` o `pricing` y `responseTime` (`VirGo-Database` `001` P-04). | Equipo |
