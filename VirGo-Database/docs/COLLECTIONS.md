# Colecciones e indices

Todas las colecciones usan validacion estricta. Los importes monetarios son `Decimal128`, las fechas de calendario son `YYYY-MM-DD` y las marcas temporales son `Date`.

## Resumen

| Coleccion | Proposito | Indices principales | TTL |
| --- | --- | --- | --- |
| `users` | Cuentas y roles | `email` unico | No |
| `passwordResets` | Codigos de recuperacion | `userId` unico; `expiresAt` | `expiresAt` |
| `sessions` | Sesiones activas | `tokenHash` unico; `userId`; `purgeAt` | `purgeAt` |
| `paymentCards` | Tarjetas tokenizadas | `userId`; `userId` unico parcial | No |
| `venues` | Catalogo de venues | geo + estado; `ownerId`; estado/zona/categorias | No |
| `venueDates` | Fechas bloqueadas o reservadas | `venueId + date` unico | No |
| `providers` | Proveedores | `userId` unico; estado/categoria/nombre | No |
| `gigs` | Agenda de proveedores | `providerId + date + start` | No |
| `reviews` | Resenas | destino + fecha; destino/autor unico | No |
| `monthlyStats` | Metricas mensuales | destino + mes unico | No |
| `events` | Eventos del cliente | `ownerId + date`; colaborador + fecha | No |
| `eventGuests` | Invitados | `eventId + status` | No |
| `eventTasks` | Checklist | `eventId + dueDate` | No |
| `quotes` | Cotizaciones | cliente + fecha; destino/estado/fecha | No |
| `bookings` | Reservas | `folio` unico; cliente + fecha; cliente + idempotencia; venue + fecha | No |
| `counters` | Folios secuenciales | Solo `_id` | No |
| `favorites` | Favoritos | usuario/destino unico | No |
| `conversations` | Conversaciones | cliente/fecha; destino/fecha; cliente/destino unico | No |
| `messages` | Mensajes | conversacion + fecha | No |
| `notices` | Notificaciones | usuario + fecha; `purgeAt` | `purgeAt` 90 dias |
| `assistantMessages` | Historial del asistente | usuario + fecha; `purgeAt` | `purgeAt` 30 dias |
| `contactMessages` | Contacto | `createdAt` | No |

## Detalle

### users

- `_id`, `name`, `email`, `passwordHash`, `role`, `phone`, `providerId`, `isVerified`, `createdAt`, `updatedAt`, `failedLoginCount`, `lockoutEndsAt`, `unreadNotices`, `city`, `defaultGuests`.
- `providerId` es opcional y deriva de `providers.userId`.
- `failedLoginCount` y `unreadNotices` son obligatorios; `lockoutEndsAt` acepta `Date` o `null`.
- `additionalProperties: false`.
- Indice: `email_1_unique`.

### passwordResets

- `_id`, `userId`, `codeHash`, `expiresAt`, `attempts`.
- `attempts` va de 0 a 5. Un codigo por usuario: `userId` es unico.
- TTL sobre `expiresAt` con `expireAfterSeconds: 0`.
- Indices: `userId_1_unique`, `expiresAt_ttl`.

### sessions

- `_id`, `userId`, `tokenHash`, `createdAt`, `expiresAt`, `purgeAt`, `device`, `revokedAt`.
- `device` y `revokedAt` son opcionales; `revokedAt` acepta `Date` o `null`.
- TTL sobre `purgeAt`.
- Indices: `tokenHash_1_unique`, `userId_1`, `purgeAt_ttl`.

### paymentCards

- `_id`, `userId`, `brand`, `last4`, `expMonth`, `expYear`, `gatewayToken`, `isDefault`.
- No almacena PAN ni CVV. `additionalProperties: false`.
- Indices: `userId_1` y `userId_1_default_unique` con `partialFilterExpression: { isDefault: true }`.

### venues

- Incluye `ownerId`, datos de catalogo, `location` GeoJSON, capacidad, precio, servicios, fotos, categorias, rating, estado y aliados.
- `amenities` y `allies` son opcionales. `allies[].category` usa `PROVIDER_CATEGORIES`; `zone` usa `ZONES`.
- `price.base` es `Decimal128`.
- Indices: `location_2dsphere_status_1`, `ownerId_1`, `status_1_zone_1_categories_1`.

### venueDates

- `_id`, `venueId`, `date`, `kind`, `reason`, `note`, `bookingId`.
- La fecha es unica por venue.
- Indice: `venueId_1_date_1_unique`.
- No tiene TTL.

### providers

- `_id`, `userId`, `businessName`, `category`, `description`, `pricing`, `responseTime`, `zones`, `packages`, `gallery`, `coverUrl`, `rating`, `status`, timestamps.
- `pricing` (`per_person` o `per_event`) y `responseTime` son obligatorios; `basePrice` no existe.
- `gallery` es opcional.
- `userId` es la relacion fuente de verdad.
- Indices: `userId_1_unique`, `status_1_category_1_businessName_1`.
- No se elimina fisicamente; la desactivacion logica queda pendiente.

### gigs

- `_id`, `providerId`, `eventName`, `clientName`, `place`, `date`, `start`, `end`, `amount`, `quoteId`.
- Indice: `providerId_1_date_1_start_1`.
- No tiene TTL.

### reviews

- Destino `venue` o `provider`, autor, estrellas, texto, `bookingId`, respuesta y fecha.
- `bookingId` es obligatorio cuando `targetType` es `venue`; opcional para proveedores.
- Indices: `targetType_1_targetId_1_createdAt_-1` y `targetType_1_targetId_1_authorId_1_unique`.

### monthlyStats

- Destino, mes, visitas, solicitudes y reservas.
- `_id` es determinista: `<targetType>:<targetId>:<AAAA-MM>`.
- `visits` se conserva en el esquema aunque queda fuera del MVP funcional.
- Indice: `targetType_1_targetId_1_month_1_unique`.

### events

- Evento, fecha, invitados, presupuesto, horas, venue, diseno, `budgetPlan`, invitacion, recordatorios, proveedores, pagos externos y colaboradores.
- `budgetPlan` es un mapa dinamico de claves a `Decimal128`.
- Indices: `ownerId_1_date_1`, `collaborators.userId_1_date_1`.
- No se imponen `maxItems` arbitrarios.

### eventGuests

- `_id`, `eventId`, `name`, `phone`, `companions`, `status`, `table`.
- Indice: `eventId_1_status_1`.

### eventTasks

- `_id`, `eventId`, `title`, `dueDate`, `done`, `suggested`.
- Indice: `eventId_1_dueDate_1`.

### quotes

- Cliente, destino, evento, servicios, mensaje, estado, respuesta, `createdAt` y `updatedAt`.
- `updatedAt` es obligatorio.
- Indices: `clientId_1_createdAt_-1`, `targetType_1_targetId_1_status_1_createdAt_-1`.

### bookings

- Folio, cliente, venue, fecha, horas, total, deposito, estado, extras, pagos, cancelacion, `rescheduledFrom` e `idempotencyKey`.
- `folio` es unico. El folio se genera con `counters` y `$inc`.
- `idempotencyKey` es opcional y de maximo 100 caracteres.
- Indices: `folio_1_unique`, `clientId_1_date_-1`, `clientId_1_idempotencyKey_1_unique` (parcial), `venueId_1_date_1`.
- `pending_payment` no bloquea la fecha. `venueDates` se inserta al confirmar.
- Cancelacion aprobada: transaccion que elimina `venueDates` por `venueId`, `date` y `bookingId`; si no coincide exactamente, se aborta.

### counters

- `_id` con formato `folio-AAAA`, `next` entero.
- Sin indices adicionales.
- La API debe hacer `findOneAndUpdate` con `$inc` y `upsert: true`.

### favorites

- Copia de presentacion del destino favorito.
- Indice: `userId_1_targetType_1_targetId_1_unique`.

### conversations

- Cliente, destino, usuario destino, ultimo mensaje y fecha.
- Indices: `clientId_1_lastAt_-1`, `targetUserId_1_lastAt_-1`, `clientId_1_targetType_1_targetId_1_unique`.

### messages

- Conversacion, emisor, texto, `sentAt`, `readAt`.
- Indice: `conversationId_1_sentAt_1`.

### notices

- Aviso por usuario, tipo, titulo, cuerpo, enlace, lectura, `createdAt` y `purgeAt`.
- TTL sobre `purgeAt`; la aplicacion debe calcular `createdAt + 90 dias`.
- Indices: `userId_1_createdAt_-1`, `purgeAt_ttl`.

### assistantMessages

- Historial del asistente por usuario.
- TTL sobre `purgeAt`; la aplicacion debe calcular `createdAt + 30 dias`.
- Indices: `userId_1_createdAt_-1`, `purgeAt_ttl`.

### contactMessages

- `_id`, `userId` opcional, nombre, email, mensaje y `createdAt`.
- Indice: `createdAt_-1`.
- Sin TTL. La retencion futura de 12 meses queda definida como politica, no como TTL actual.
