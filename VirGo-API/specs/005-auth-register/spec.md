# Spec — 005 Registro de cuenta

| Campo | Valor |
|---|---|
| Endpoint | `POST /api/v1/auth/register` |
| Fuente de datos | `API-HANDOFF.md` §4.4 `users` y §9.2 |
| Estado | Implementación local; despliegue pendiente |

## Objetivo

Crear una cuenta con los campos obligatorios del contrato (`name`, `email`, `phone`, `passwordHash`, `role`, `isVerified`, fechas). La persona puede elegir un rol público con el campo opcional `role`: `client` (por omisión), `owner` o `provider`. Nunca `admin`; `isVerified` siempre empieza en `false`.

> **Cambio propuesto (2026-11-25, Móvil):** antes el rol público siempre era `client`, así que no existía forma de crear una cuenta de propietario o proveedor y nadie podía publicar salones. La app ya pide el rol al registrarse («Organizo un evento», «Tengo un salón», «Soy proveedor»). Requiere aprobación del equipo de backend.

## Criterios de aceptación

- Datos válidos devuelven `201` sin contraseña ni hash.
- El correo se normaliza a minúsculas y espacios externos.
- Un correo duplicado devuelve `409`.
- `role` ausente crea `client`; `owner` y `provider` crean ese rol; cualquier otro valor (incluido `admin`) devuelve `400` con error en `role`.
- Datos inválidos devuelven `400` en Problem Details.
- La contraseña se persiste únicamente como hash.
