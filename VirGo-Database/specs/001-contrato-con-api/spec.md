# Spec — 001 Contrato de datos con la API

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Gobierno | Constitución global (`../../../constitution.md`) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Origen | Comparación del 2026-11-05 entre la base, `VirGo-API/db/schema/` y lo que escribe la API del PR #5 |

## 1. Contexto y objetivo

La API del PR #5 se construyó contra `VirGo-API/db/schema/`, que difiere del catálogo en 8 colecciones. Como `users` no admite campos adicionales, la base rechazaría el registro de usuarios y el bloqueo de cuentas. Además, ningún campo de texto tiene longitud máxima.

El objetivo es que el catálogo admita lo que las 7 APIs necesitan, para que puedan funcionar contra Atlas.

## 2. Historias técnicas

| ID | Historia |
|---|---|
| HT-01 | Como **equipo Backend**, quiero que la base acepte los documentos de las 7 APIs, para desplegar sin errores de validación. |
| HT-02 | Como **responsable de seguridad**, quiero longitud máxima en todo texto, para que una entrada enorme no llegue a la base. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | `users` admite `failedLoginCount`, `lockoutEndsAt`, `unreadNotices`, `city` y `defaultGuests`. |
| RF-02 | `sessions` declara `device` y `revokedAt`. |
| RF-03 | `bookings` admite `idempotencyKey`, con unicidad por cliente, y un índice para listar por cliente y fecha. |
| RF-04 | Todo campo de texto libre tiene longitud máxima. |
| RF-05 | Los valores permitidos de `eventType`, zonas y categorías son los mismos en la base y en la API. |
| RF-06 | El catálogo puede exportar `VirGo-API/db/schema/`, para que ese directorio deje de editarse a mano. |
| RF-07 | Los cambios se aplican sin eliminar colecciones, índices ni documentos. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Compatibilidad:** `virgo_dev` no tiene documentos de negocio, así que los cambios no requieren migración. |
| RNF-02 | **Seguridad:** el cambio se aplica con el usuario DDL, no con el de la aplicación. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** un usuario con los campos que escribe la API, **cuando** lo inserto, **entonces** la base lo acepta. |
| CA-02 | **Dado** un texto de 1500 caracteres en un campo limitado, **cuando** lo inserto, **entonces** la base lo rechaza. |
| CA-03 | **Dado** dos reservaciones con el mismo cliente e `idempotencyKey`, **cuando** inserto la segunda, **entonces** la base la rechaza. |
| CA-04 | **Dado** el catálogo actualizado, **cuando** ejecuto `db:verify:dev`, **entonces** no hay diferencias. |
| CA-05 | **Dado** el catálogo y `VirGo-API/db/schema/`, **cuando** los comparo, **entonces** coinciden. |

## 6. Fuera de alcance

- `passwordResets` y `providers`: ningún endpoint de los 7 los usa; se deciden en sus specs (P-03 y P-04).
- Crear la base `virgo` de producción: se hace después, con `db:init:prod`.

## 7. Supuestos, dependencias y preguntas abiertas

**Dependencias:** `000-base-datos`. Lo consume `VirGo-API/specs/010-contrato-datos`.

| # | Pregunta | Recomendación | Responsable |
|---|---|---|---|
| P-01 | Zonas: ¿`Norte, Sur, Centro, Oriente, Poniente` (base) o `Juriquilla, Centro, El Refugio…` (modelo)? | Las que muestre la maqueta | Equipo |
| P-02 | `eventType`: ¿`XV Años` (base) o `XV años` (`db/schema`)? | `XV Años`, como en `design.theme`; ajustar la API | Esau y Gafas |
| P-03 | `passwordResets`: ¿`tokenHash` y `used`, o `codeHash` y `attempts`? | Conservar `tokenHash` y `used`, y agregar `attempts` para frenar la fuerza bruta | Equipo |
| P-04 | `providers`: ¿`basePrice`, o `pricing` y `responseTime`? | Decidir en el spec de proveedores | Equipo |
| P-05 | ¿Son correctas las longitudes de la sección 8 del modelo de datos? | Propuesta pendiente de confirmar | Esau |
