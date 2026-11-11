# Plan — 001 Contrato de datos con la API

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Resumen técnico

Se modifica el catálogo (`src/catalog`), se aplica con el inicializador existente y se agrega un exportador que genera `VirGo-API/db/schema/`. No hace falta una herramienta nueva para aplicar: `plan`, `apply` y `verify` ya existen.

## 2. Diferencias medidas (2026-11-05)

Comparación entre el catálogo, que coincide 22 de 22 con `virgo_dev`, y `db/schema/` de la rama del PR #5.

| Colección | Solo en el catálogo | Solo en `db/schema/` | ¿Bloquea las 7 APIs? |
|---|---|---|---|
| `users` | — | `failedLoginCount`, `lockoutEndsAt`, `unreadNotices`, `city`, `defaultGuests` | **Sí:** con `additionalProperties: false`, se rechaza el registro |
| `sessions` | — | `device`, `revokedAt` | No, se aceptan sin validar; hay que declararlos |
| `bookings` | Índice `clientId + createdAt` | `idempotencyKey`, índices `clientId + date` y `clientId + idempotencyKey` | **Sí:** sin el índice no hay idempotencia real |
| `events`, `quotes` | `XV Años` | `XV años` | **Sí:** `POST /quotes` con ese tipo sería rechazado |
| `venues` | Zonas por punto cardinal; `amenities` y `allies` obligatorios | Zonas por colonia; ambos opcionales | Parcial: afecta los filtros de `GET /venues` |
| `passwordResets` | `tokenHash`, `used` | `codeHash`, `attempts` | No |
| `providers` | `basePrice`; `gallery` obligatorio; 9 categorías | `pricing`, `responseTime`; 5 categorías | No |
| Todas | 0 de 77 textos con `maxLength` | Límites en los textos | No bloquea, pero incumple la regla de longitudes |

## 3. Decisiones

| Decisión | Motivo |
|---|---|
| El catálogo es la fuente de verdad; `db/schema/` será una copia generada | Lo acordó el equipo en la constitución v1.2.0 de `VirGo-API` (PR #5), y ya coincide con la base |
| Se agregan al catálogo los campos que la API necesita | El login no funciona sin ellos |
| El índice `clientId + createdAt` se conserva | El inicializador no elimina índices; queda como advertencia |

## 4. Diseño

| Archivo | Cambio |
|---|---|
| `src/catalog/validators/accounts.js` | Campos nuevos de `users` y `sessions` |
| `src/catalog/validators/bookings.js` | `idempotencyKey` |
| `src/catalog/indexes.js` | `clientId_1_date_-1` y `clientId_1_idempotencyKey_1_unique` |
| `src/catalog/helpers.js` | `string(max)` con `maxLength` |
| `src/catalog/enums.js` | Resultado de P-01 y P-02 |
| `src/cli/export.js` | Nuevo: escribe `VirGo-API/db/schema/*.json` |

## 5. Costo

$0.

## 6. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Todo `string` libre tiene `maxLength` |
| Integración | Fixtures con los documentos de la API (CA-01), 1500 caracteres (CA-02) e idempotencia (CA-03) |
| Deriva | `db:verify:dev` y `check_schema.py --live` (CA-04, CA-05) |

## 7. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| El índice único de `idempotencyKey` rechaza reservaciones sin llave | Se declara `sparse`, como en `db/schema/` |
| Se aplica a `virgo_dev` antes de acordar los enumerados | T105 depende de T101 |

## 8. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Decisiones | T101 |
| F1 | Catálogo y pruebas | T102–T104 |
| F2 | Aplicación | T105–T106 |
