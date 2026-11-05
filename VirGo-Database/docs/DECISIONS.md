# Decisiones

## Aprobadas e implementadas

### Alcance

- Proyecto independiente de base de datos; no se modifica ni duplica `VirGo-API`.
- 22 colecciones aprobadas.
- Sin `config`, migraciones ni locks.
- Sin datos de negocio iniciales.
- Sin operaciones destructivas.

### Entornos

- `virgo_dev` para desarrollo y `virgo` para produccion.
- Preferencia por proyectos y clusters separados.
- Usuario DDL separado del usuario runtime.
- Guardas por `NODE_ENV` y nombre de base.

### Esquema

- `quotes.updatedAt` obligatorio y BSON `Date`.
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
- Conflictos de indices bloquean el apply.
- Indices extra solo generan advertencia.

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
