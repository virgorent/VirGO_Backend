# Decisiones

## Aprobadas e implementadas

### Alcance

- Proyecto independiente de base de datos; no se modifica ni duplica `VirGo-API`.
- 22 colecciones aprobadas.
- Sin `config`, migraciones ni locks.
- Sin datos reales de negocio; el seed DEMO es ficticio y opcional.
- El inicializador no ejecuta operaciones destructivas; el purge DEMO solo elimina sus propios `_id`.

### Entornos

- `virgo_dev` para desarrollo y `virgo` para produccion.
- Preferencia por proyectos y clusters separados.
- Usuario DDL separado del usuario runtime.
- Guardas por `NODE_ENV` y nombre de base.

### Fuente de verdad

- `VirGo-Database/src/catalog` es la fuente de verdad de colecciones, validadores e indices.
- `VirGo-API/db/schema` se genera con `npm run db:export:api`; no se edita a mano.
- El exportador es determinista y `db:export:api:check` bloquea la deriva.

### Esquema

- `quotes.updatedAt` obligatorio y BSON `Date`.
- Todos los strings libres tienen `maxLength`.
- `passwordResets` usa `codeHash` y `attempts`; `userId` es unico.
- `providers` usa `pricing` y `responseTime`; `basePrice` no existe.
- `monthlyStats._id` es determinista: `<targetType>:<targetId>:<AAAA-MM>`.
- `reviews.bookingId` obligatorio solo para `targetType: venue`.
- Indice unico de resenas por `targetType`, `targetId`, `authorId`.
- Indice unico de conversaciones por `clientId`, `target.type`, `target.id`.
- Indice unico parcial de tarjeta predeterminada por `userId` con `isDefault: true`.
- `providers.userId` es la relacion fuente de verdad; `users.providerId` es derivada.
- `budgetPlan` es un mapa dinamico de `Decimal128`.
- `monthlyStats.visits` se conserva en el esquema pero queda fuera del MVP.
- `users` y `paymentCards` usan `additionalProperties: false`.
- Sin `maxItems` arbitrarios en arreglos variables.
- Sin campos nuevos para pasarela, reprogramacion o estado eliminado de proveedor.

### Dinero y fechas

- Dinero: `Decimal128`.
- Fechas de calendario: string `YYYY-MM-DD`.
- Marcas temporales: BSON `Date`.
- Contadores: `int`.
- IDs: UUID string, excepto `counters._id`.

### Reservas y disponibilidad

- `pending_payment` no bloquea la fecha.
- `venueDates` se inserta al confirmar.
- La cancelacion elimina `venueDates` por `venueId`, `date` y `bookingId`; si no coincide exactamente, aborta.
- `venueDates` no tiene TTL.
- `bookings.idempotencyKey` es opcional, maximo 100 caracteres.
- La idempotencia de reservas se garantiza con el indice unico parcial `clientId_1_idempotencyKey_1_unique`.

### Folios

- `counters._id` con formato `folio-AAAA`.
- Incremento con `$inc` y `upsert`.
- Folio unico y creciente, sin garantia de consecutividad.
- Ano UTC al momento de creacion.

### TTL

- `passwordResets.expiresAt`.
- `sessions.purgeAt`.
- `notices.purgeAt` a 90 dias.
- `assistantMessages.purgeAt` a 30 dias.
- Sin TTL para `venueDates`, `gigs` ni `contactMessages`.

### Operacion

- Inicializador idempotente en cada despliegue.
- `plan` y `validate` de solo lectura.
- `apply` exige `--yes`.
- Produccion exige `--confirm-production`.
- Conflictos de clave u opciones bloquean el apply y no aceptan waiver.
- Indices extra y nombres distintos requieren waiver aprobado por indice.
- En produccion, TTL faltante o adicional requiere aprobacion TTL especifica;
  en development/test se registra como advertencia.
- `verify.ok` indica ausencia de bloqueos y acciones pendientes;
  `verify.exactMatch` indica coincidencia exacta sin excepciones.
- `apply` rechaza acciones fuera del catalogo, indices manipulados y cualquier operacion destructiva.
- Un fallo parcial conserva el detalle de lo aplicado, invalida el plan y exige
  una nueva inspeccion y un nuevo plan idempotente.
- No se usa firma criptografica por ahora; las aprobaciones se vinculan por
  `planFingerprint` y contexto de cluster, entorno y base.

### Seguridad

- TLS obligatorio para toda conexion a Atlas.
- Se rechazan parametros que desactiven la validacion TLS.
- `MONGO_ALLOW_INSECURE_TLS` solo se permite en development/test y nunca en production.
- El usuario runtime de la API tiene `readWrite` solo sobre su base; no usa el usuario DDL.
- El acceso de red amplio (`0.0.0.0/0`) es una excepcion temporal, no una configuracion final de produccion.

### Datos DEMO

- El seed usa IDs deterministas y `replaceOne` con `upsert`; es idempotente.
- Solo se permite en development/test; production y la base `virgo` se rechazan.
- El purge es selectivo por `_id` DEMO y no toca documentos ajenos.
- No se ejecuta automaticamente en `plan`, `apply` ni `verify`.

## Pendientes

Estos puntos requieren una decision posterior y no bloquean el MVP:

- Identificador de evento de la pasarela para idempotencia de webhooks.
- Relacion explicita entre reservas y proveedores.
- Estado o campo para proveedor desactivado o eliminado logicamente.
- Regla de reprogramacion y campos asociados.
- Politica de purga automatica de `contactMessages` a 12 meses.
- Limites de arreglos si el producto los requiere.
- Integracion de pagos.
- Mecanismo de migraciones si el esquema deja de ser compatible.
