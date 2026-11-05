# Modelo de datos NoSQL de VirGo (MongoDB)

> **Nota de autoridad:** este archivo es contexto histórico. Para implementar la API, la fuente de verdad vigente es `API-HANDOFF.md` y el contrato ejecutable del repositorio `VirGo-Database`.

Campos necesarios para crear la base de datos **no relacional** de VirGo. Se dedujeron del `Checklist VirGo.md` (73 pantallas) y de los datos que ya guarda la maqueta funcional.

- **Versión:** borrador 3 · 2026-11-03 (reemplaza el borrador 2 para DynamoDB)
- **Motor:** MongoDB Atlas M0, según la decisión ADR-23 del backend (`VirGo-API`)
- **Base de datos:** `virgo` (producción) y `virgo_dev` (desarrollo)
- **Uso:** insumo para los specs funcionales del backend (`003-auth-login` en adelante)

## 1. Cómo se diseña una base de documentos

| Relacional (SQL) | Documentos (MongoDB) — lo que usamos |
|---|---|
| Una tabla por entidad y llaves foráneas | Una **colección** por entidad principal; las referencias son campos `xxxId` |
| Se normaliza y se une con `JOIN` | Se **incrusta** lo que se lee junto (fotos de un salón, pagos de una reservación) |
| Listas en tablas hijas | Listas pequeñas y acotadas **dentro del documento**; listas que crecen sin límite (invitados, mensajes) en su propia colección |
| El dato vive en un solo lugar | Se **duplica** a propósito lo que se muestra en listados (nombre, foto) |
| Índices para acelerar | Los índices hacen además el trabajo de las reglas: **únicos** (correo, folio, día ocupado), **TTL** (borrado automático) y **geoespaciales** (salones cerca) |

**Regla para incrustar o separar:** se incrusta si la lista es pequeña, acotada y siempre se lee con el documento padre. Se separa si puede crecer a cientos, se edita de uno en uno o se consulta sola. Un documento de MongoDB no puede pasar de 16 MB.

## 2. Convenciones

| Elemento | Regla |
|---|---|
| `_id` | UUID (representación estándar), salvo en `counters` |
| Nombres | Colecciones y campos en inglés y `camelCase` (Art. XII), igual que el JSON de la API |
| Fechas con hora | Tipo `Date` en UTC (`createdAt`, `updatedAt`, `expiresAt`) |
| Fechas de calendario | Texto `AAAA-MM-DD` (fecha del evento, día ocupado) |
| Dinero | `Decimal128` en MXN |
| Ubicación | GeoJSON `{ type: "Point", coordinates: [lng, lat] }` con índice `2dsphere` |
| Auditoría | Las colecciones principales tienen `createdAt` y `updatedAt` |
| Validación | Cada colección tiene un `$jsonSchema` con los campos obligatorios; `VirGo-Database` administra el esquema y los índices antes de que la API se conecte |
| Espacio | M0 da **512 MB**. Las colecciones temporales tienen índice TTL y las imágenes van a un almacenamiento de archivos (solo se guarda su URL) |

**Columna «Req.»:** ✅ obligatorio · ➖ opcional.

---

## 3. Consultas de la app (patrones de acceso)

Cada consulta se resuelve con **una** lectura que usa un índice.

| # | Necesidad (pantalla) | Colección | Filtro → índice |
|---|---|---|---|
| AP-01 | Iniciar sesión con correo (A5) | `users` | `{ email }` → único `email` |
| AP-02 | Ver mi perfil (C39) | `users` | `{ _id }` |
| AP-03 | Validar el código de recuperación (A7) | `passwordResets` | `{ userId }` → único `userId` |
| AP-04 | Renovar la sesión (móvil) | `sessions` | `{ tokenHash }` → único `tokenHash` |
| AP-05 | Mis métodos de pago (C8, C39) | `paymentCards` | `{ userId }` |
| AP-06 | Salones publicados cerca (A1, B8, C9, C10) | `venues` | `{ status: "published", location: { $near … } }` → `2dsphere` |
| AP-07 | Detalle del salón (B13) | `venues` | `{ _id }` |
| AP-08 | Fechas ocupadas de un salón en un mes (B13, G58, P9) | `venueDates` | `{ venueId, date: { $gte, $lte } }` → único `venueId + date` |
| AP-09 | Reseñas de un salón o proveedor (C14, P14, S8) | `reviews` | `{ targetType, targetId }` ordenado por `createdAt` |
| AP-10 | Mis salones (P6) | `venues` | `{ ownerId }` |
| AP-11 | Catálogo de proveedores por categoría (C19) | `providers` | `{ status: "published", category }` |
| AP-12 | Perfil del proveedor (C20, S2) | `providers` | `{ _id }` |
| AP-13 | Agenda semanal del proveedor (H74) | `gigs` | `{ providerId, date: { $gte, $lte } }` |
| AP-14 | Mis eventos y los compartidos conmigo (D27, C30) | `events` | `{ $or: [{ ownerId }, { "collaborators.userId": id }] }` |
| AP-15 | Mi Evento completo (D27 y sus 12 accesos) | `events` + `eventGuests` + `eventTasks` | `{ _id }` y `{ eventId }` |
| AP-16 | Mis cotizaciones (C22) | `quotes` | `{ clientId }` ordenado por `createdAt` |
| AP-17 | Solicitudes recibidas por estado (P7, S4) | `quotes` | `{ "target.type", "target.id", status }` |
| AP-18 | Detalle de una cotización (C7, P8, S5) | `quotes` | `{ _id }` |
| AP-19 | Mis reservaciones (C32) | `bookings` | `{ clientId }` ordenado por `date` |
| AP-20 | Reservaciones y pagos recibidos de un salón (G58, P12) | `bookings` | `{ "venue.venueId", date }` |
| AP-21 | Comprobante por folio (C23) | `bookings` | `{ folio }` → único `folio` |
| AP-22 | Mis favoritos (C38) | `favorites` | `{ userId }` |
| AP-23 | Mi bandeja de mensajes (C36, P16, S10) | `conversations` | Cliente: `{ clientId }`. Salón o proveedor: `{ targetUserId }`. Ordenado por `lastAt` |
| AP-24 | Mensajes de un chat (C35) | `messages` | `{ conversationId }` ordenado por `sentAt` |
| AP-25 | Mis notificaciones (C37) | `notices` | `{ userId }` ordenado por `createdAt` descendente |
| AP-26 | Estadísticas por mes (P13, S9) | `monthlyStats` | `{ targetType, targetId, month }` |
| AP-27 | Historial del asistente IA (C18) | `assistantMessages` | `{ userId }` ordenado por `createdAt` |

---

## 4. Colecciones y sus campos

### 4.1 Cuentas

#### `users`

| Campo | Tipo | Req. | Descripción |
|---|---|---|---|
| `_id` | UUID | ✅ | |
| `name` | string | ✅ | Nombre |
| `email` | string | ✅ | Correo en minúsculas; **índice único** (A4: no hay dos cuentas con el mismo correo) |
| `phone` | string | ✅ | `+52` y 10 dígitos |
| `passwordHash` | string | ✅ | Hash seguro (PBKDF2); **nunca** la contraseña |
| `role` | string | ✅ | `client`, `owner` o `provider` |
| `city` | string | ➖ | Perfil |
| `defaultGuests` | int | ➖ | Preferencia de invitados |
| `providerId` | UUID | ➖ | Si el rol es proveedor, el id de su perfil |
| `unreadNotices` | int | ✅ | Contador de la campana (`$inc` atómico) |
| `createdAt` / `updatedAt` | Date | ✅ | |

**Índices:** `{ email: 1 }` único.

#### `passwordResets`

| Campo | Tipo | Descripción |
|---|---|---|
| `_id` | UUID | |
| `userId` | UUID | **Índice único**: un código vigente por persona |
| `codeHash` | string | Código de 6 dígitos en hash |
| `attempts` | int | Intentos fallidos (máximo 5) |
| `expiresAt` | Date | Ahora + 15 min; **índice TTL**: MongoDB lo borra solo |

#### `sessions`

| Campo | Tipo | Descripción |
|---|---|---|
| `_id` | UUID | Es el `sid` del JWT (spec `003`) |
| `userId` | UUID | Dueño de la sesión |
| `tokenHash` | string | SHA-256 del token de sesión; **índice único** |
| `expiresAt` | Date | 30 días |
| `purgeAt` | Date | `expiresAt` + 7 días; **índice TTL** |
| `createdAt` | Date | |

**Índices:** `{ tokenHash: 1 }` único · `{ userId: 1 }` · `{ purgeAt: 1 }` TTL.

#### `paymentCards`

| Campo | Tipo | Descripción |
|---|---|---|
| `_id`, `userId` | UUID | Índice `{ userId: 1 }` |
| `brand`, `last4`, `expMonth`, `expYear` | string/int | **Nunca** el número completo ni el CVV |
| `gatewayToken` | string | Referencia de la pasarela de pago |
| `isDefault` | bool | Predeterminada |

> Va separada de `users` para que el token de la pasarela no viaje en cada lectura del perfil ni del login.

### 4.2 Salones

#### `venues`

Es un **documento completo**: las fotos, los servicios, las categorías, los montajes y los precios van dentro porque siempre se leen juntos (B13).

| Campo | Tipo | Req. | Descripción |
|---|---|---|---|
| `_id` | UUID | ✅ | |
| `ownerId` | UUID | ✅ | Usuario propietario |
| `name` / `shortName` | string | ✅ | «Casa Oliva» / «C. Oliva» (pin del mapa) |
| `description` | string | ✅ | Mínimo 20 caracteres |
| `address` / `zone` | string | ✅ | Zona del catálogo |
| `location` | GeoJSON Point | ✅ | `{ type: "Point", coordinates: [lng, lat] }` para buscar en un radio de 2, 5 o 10 km |
| `capacity` | int | ✅ | Invitados máximos |
| `price` | objeto | ✅ | `{ base, unit: "hour"/"day", minHours, weekendPct, highSeasonPct, highSeasonMonths: [int] }` |
| `categories` | [string] | ✅ | Jardines, Alberca, Elegante, Haciendas, Infantil |
| `amenities` | [string] | ➖ | Parking, Pista, A/C… |
| `photos` | [objeto] | ✅ | `[{ url, order }]`, la primera es la portada |
| `services` | [objeto] | ✅ | `[{ name, included: bool, price }]` |
| `setups` | objeto | ➖ | `{ banquet, auditorium, cocktail }` (capacidad por montaje) |
| `floorPlanUrl` / `tour360Url` | string | ➖ | Plano y tour 360° |
| `allies` | [objeto] | ➖ | `[{ providerId, name, category }]`: aliados activos (desnormalizado) |
| `verified` | bool | ✅ | Insignia |
| `status` | string | ✅ | `draft`, `review` o `published` |
| `rating` | objeto | ✅ | `{ average, count }`: se actualiza al publicar cada reseña |
| `createdAt` / `updatedAt` | Date | ✅ | |

**Índices:** `{ location: "2dsphere", status: 1 }` (catálogo cerca) · `{ ownerId: 1 }` (mis salones) · `{ status: 1, zone: 1, categories: 1 }` (filtros).

#### `venueDates`

Un documento por día ocupado. Sirve para el calendario del cliente y del propietario, y evita reservar dos veces el mismo día.

| Campo | Tipo | Descripción |
|---|---|---|
| `_id` | UUID | |
| `venueId` | UUID | |
| `date` | string | `AAAA-MM-DD` |
| `kind` | string | `blocked` (lo bloqueó el propietario) o `booked` (hay reservación confirmada) |
| `reason` | string | `maintenance`, `own_event` u `other` (solo si está bloqueado) |
| `note` | string | ➖ |
| `bookingId` | UUID | ➖ Si está reservado |

**Índices:** `{ venueId: 1, date: 1 }` **único**: si el día ya existe, la reservación o el bloqueo falla con un error de duplicado. **Cero empalmes.**

### 4.3 Proveedores

#### `providers`

| Campo | Tipo | Req. | Descripción |
|---|---|---|---|
| `_id` / `userId` | UUID | ✅ | |
| `businessName` | string | ✅ | |
| `category` | string | ✅ | Catering, Música, Fotografía, Decoración o Mobiliario |
| `description` | string | ✅ | |
| `pricing` | string | ✅ | `per_person` o `per_event` |
| `responseTime` | string | ✅ | «Menos de 2 h» |
| `coverUrl` | string | ➖ | |
| `packages` | [objeto] | ✅ | `[{ id, name, price, includes }]` |
| `gallery` | [objeto] | ➖ | `[{ url, order }]` (JPG o PNG de hasta 10 MB; aquí solo la URL) |
| `zones` | [string] | ✅ | Zonas de cobertura |
| `status` | string | ✅ | `draft` o `published` |
| `rating` | objeto | ✅ | `{ average, count }` |

**Índices:** `{ userId: 1 }` único · `{ status: 1, category: 1, businessName: 1 }` (catálogo).

#### `gigs` (agenda del proveedor)

| Campo | Tipo | Descripción |
|---|---|---|
| `_id`, `providerId` | UUID | |
| `eventName`, `clientName`, `place` | string | |
| `date`, `start`, `end` | string | Para detectar empalmes en la semana |
| `amount` | Decimal128 | ➖ |
| `quoteId` | UUID | ➖ Si viene de una cotización aceptada |

**Índices:** `{ providerId: 1, date: 1, start: 1 }`.

### 4.4 Reseñas y estadísticas (compartidas por salón y proveedor)

#### `reviews`

| Campo | Tipo | Descripción |
|---|---|---|
| `_id` | UUID | |
| `targetType` / `targetId` | string / UUID | `venue` o `provider` y su id |
| `authorId` / `authorName` | UUID / string | El nombre va duplicado para mostrarlo sin otra lectura |
| `stars` | int | 1–5 |
| `text` | string | |
| `bookingId` | UUID | ➖ Comprueba que sí contrató |
| `reply` | objeto | ➖ `{ text, at }`: respuesta editable del propietario o proveedor |
| `createdAt` | Date | |

**Índices:** `{ targetType: 1, targetId: 1, createdAt: -1 }` · `{ authorId: 1, targetId: 1 }` único (una reseña por persona y salón o proveedor).

> Al guardar una reseña, una **transacción** actualiza también `rating.average` y `rating.count` del salón o proveedor.

#### `monthlyStats`

| Campo | Tipo | Descripción |
|---|---|---|
| `targetType` / `targetId` / `month` | string / UUID / string | `month` = `AAAA-MM` |
| `visits`, `requests`, `bookings` | int | Contadores con `$inc` y `upsert` |

**Índices:** `{ targetType: 1, targetId: 1, month: 1 }` único.

### 4.5 Eventos del cliente (Mi Evento)

#### `events`

Las listas pequeñas y acotadas van **dentro** del evento; los invitados y las tareas, que pueden ser cientos y se editan de uno en uno, van en su propia colección.

| Campo | Tipo | Req. | Descripción |
|---|---|---|---|
| `_id` / `ownerId` | UUID | ✅ | |
| `name` | string | ✅ | «Boda Mar & Alex» |
| `eventType` | string | ✅ | Boda, XV años, Cumpleaños, Corporativo u Otro |
| `date` | string | ✅ | `AAAA-MM-DD` |
| `guests` / `budget` / `hours` | int / Decimal128 / int | ✅ | Invitados esperados, presupuesto y horas |
| `venue` | objeto | ➖ | `{ venueId, name, photoUrl }`: salón elegido (desnormalizado) |
| `design` | objeto | ➖ | `{ theme, layout, staff: { waiters, security, cleaning } }` (IA Studio) |
| `budgetPlan` | objeto | ➖ | `{ categoria: monto }` (Presupuesto IA) |
| `invitation` | objeto | ➖ | `{ template, message }` |
| `reminders` | [objeto] | ➖ | `[{ id, title, date, kind, auto }]`; `kind` = `payment`, `confirmation`, `delivery` u `other` |
| `vendors` | [objeto] | ➖ | Equipo: `[{ id, providerId?, name, category, package?, price, paymentStatus }]` |
| `externalPayments` | [objeto] | ➖ | Pagos registrados a mano: `[{ id, concept, amount, status, paidAt }]` |
| `collaborators` | [objeto] | ➖ | `[{ userId, name, email, permission: "view"/"edit" }]` |
| `createdAt` / `updatedAt` | Date | ✅ | |

**Índices:** `{ ownerId: 1, date: 1 }` · `{ "collaborators.userId": 1, date: 1 }` (eventos compartidos conmigo).

#### `eventGuests`

| Campo | Tipo | Descripción |
|---|---|---|
| `_id`, `eventId` | UUID | |
| `name`, `phone` | string | |
| `companions` | int | Acompañantes |
| `table` | string | ➖ |
| `status` | string | `confirmed`, `pending` o `declined` |

**Índices:** `{ eventId: 1, status: 1 }`.

#### `eventTasks`

| Campo | Tipo | Descripción |
|---|---|---|
| `_id`, `eventId` | UUID | |
| `title` | string | |
| `dueDate` | string | `AAAA-MM-DD` |
| `done` / `suggested` | bool | |

**Índices:** `{ eventId: 1, dueDate: 1 }`.

### 4.6 Cotizaciones

#### `quotes`

La solicitud del cliente y la respuesta viven en **el mismo documento**.

| Campo | Tipo | Req. | Descripción |
|---|---|---|---|
| `_id` / `clientId` / `clientName` | UUID / UUID / string | ✅ | |
| `eventId` | UUID | ➖ | |
| `target` | objeto | ✅ | `{ type: "venue"/"provider", id, name }` |
| `eventType` / `eventDate` | string | ✅ | |
| `guests` | int | ✅ | |
| `requestedServices` | [string] | ➖ | Servicios o paquetes marcados al pedir |
| `message` | string | ➖ | |
| `status` | string | ✅ | `sent`, `answered`, `accepted`, `rejected` o `expired` |
| `response` | objeto | ➖ | `{ items: [{ concept, qty, unitPrice }], note, subtotal, tax, total, validUntil, answeredAt }` |
| `createdAt` | Date | ✅ | |

**Índices:** `{ clientId: 1, createdAt: -1 }` (mis cotizaciones) · `{ "target.type": 1, "target.id": 1, status: 1, createdAt: -1 }` (solicitudes por pestaña).

> Al cambiar `status`, la cotización «se mueve» sola de la pestaña Nuevas a Respondidas: el filtro usa el índice por estado.

### 4.7 Reservaciones y pagos

#### `bookings`

| Campo | Tipo | Req. | Descripción |
|---|---|---|---|
| `_id` | UUID | ✅ | |
| `folio` | string | ✅ | `VG-2026-0142`; **índice único** |
| `clientId` / `clientName` | UUID / string | ✅ | |
| `venue` | objeto | ✅ | `{ venueId, name, photoUrl }` |
| `eventId` / `quoteId` | UUID | ➖ | |
| `date` | string | ✅ | |
| `hours` | int | ✅ | |
| `extras` | [objeto] | ➖ | `[{ name, price }]`: servicios adicionales contratados |
| `total` / `deposit` | Decimal128 | ✅ | El anticipo es el 30% |
| `status` | string | ✅ | `pending_payment`, `confirmed` o `cancelled` |
| `payments` | [objeto] | ✅ | `[{ paymentId, concept, amount, method, reference, status, paidAt }]` |
| `cancellation` | objeto | ➖ | `{ at, refund }` (50% si se cancela con 30 días o más) |
| `rescheduledFrom` | string | ➖ | Fecha original, si se reprogramó |
| `createdAt` / `updatedAt` | Date | ✅ | |

**Índices:** `{ folio: 1 }` único · `{ clientId: 1, date: -1 }` · `{ "venue.venueId": 1, date: 1 }`.

> **Los pagos van dentro de la reservación** porque son pocos (anticipo y liquidación) y siempre se muestran con ella.

#### `counters`

| Campo | Tipo | Descripción |
|---|---|---|
| `_id` | string | `folio-2026` |
| `next` | int | Consecutivo; se obtiene con `findOneAndUpdate({ _id }, { $inc: { next: 1 } }, { upsert: true })` |

> **Confirmar una reservación** es una **transacción** (Atlas M0 las admite): inserta el `venueDates` del día (falla si ya está ocupado), cambia la reservación a `confirmed` e inserta las notificaciones del cliente y del propietario. Todo se aplica o nada se aplica.

### 4.8 Interacción

| Colección | Campos | Índices |
|---|---|---|
| `favorites` | `_id`, `userId`, `targetType` (`venue` o `provider`), `targetId`, `name`, `photoUrl`, `rating` (copiados para la lista), `createdAt` | `{ userId: 1, targetType: 1, targetId: 1 }` único |
| `conversations` | `_id`, `clientId`, `target { type, id, name }`, `targetUserId`, `quoteId` ➖, `lastMessage`, `lastAt` | `{ clientId: 1, lastAt: -1 }` · `{ targetUserId: 1, lastAt: -1 }` |
| `messages` | `_id`, `conversationId`, `senderId`, `text`, `sentAt`, `readAt` ➖ | `{ conversationId: 1, sentAt: 1 }` |
| `notices` | `_id`, `userId`, `kind` (`booking`, `payment`, `cancellation`, `quote`, `message` o `reminder`), `title`, `body`, `link`, `read`, `createdAt`, `purgeAt` | `{ userId: 1, createdAt: -1 }` · `{ purgeAt: 1 }` TTL (90 días) |
| `assistantMessages` | `_id`, `userId`, `role` (`user` o `assistant`), `text`, `createdAt`, `purgeAt` | `{ userId: 1, createdAt: 1 }` · `{ purgeAt: 1 }` TTL (30 días) |
| `contactMessages` | `_id`, `userId` ➖, `name`, `email`, `message`, `createdAt` | `{ createdAt: -1 }` |

### 4.9 Contenido fijo

Las preguntas frecuentes (C40), las ideas de decoración por tema (C17) y los catálogos (zonas, categorías, amenidades) **no necesitan la base de datos**: van en archivos de configuración del backend. Si se quieren editar sin desplegar, van en una colección `config` con `_id` = `faq` o `ideas-<tema>`.

---

## 5. Valores fijos (enumeraciones)

| Campo | Valores |
|---|---|
| `role` | client, owner, provider |
| `venues.status` / `providers.status` | draft, review, published / draft, published |
| `zone` | Juriquilla, Centro, El Refugio, Santa Fe, Antea, Jurica, Corregidora, El Marqués |
| `categories` (salón) | Jardines, Alberca, Elegante, Haciendas, Infantil |
| `amenities` | Parking, Pista, A/C, Planta, Pet friendly, Alberca, Área infantil, Cocina |
| `category` (proveedor) | Catering, Música, Fotografía, Decoración, Mobiliario |
| `eventType` | Boda, XV años, Cumpleaños, Corporativo, Otro |
| `design.theme` / `design.layout` | Boda, XV Años, Neón Party, Mexicano / center_floor, max_capacity, imperial |
| `quotes.status` | sent, answered, accepted, rejected, expired |
| `bookings.status` | pending_payment, confirmed, cancelled |
| `payments[].status` / `method` | completed, pending, failed, refunded / card, transfer, external |
| `eventGuests.status` / `vendors[].paymentStatus` | confirmed, pending, declined / pending, deposit, paid |

Se validan con `enum` en el `$jsonSchema` de cada colección y en los DTO de la API.

## 6. Reglas de consistencia

| Regla | Cómo se garantiza en MongoDB |
|---|---|
| Un correo, una cuenta | Índice único en `users.email` |
| Un salón no se reserva dos veces el mismo día | Índice único `venueDates { venueId, date }` dentro de la transacción de confirmación |
| Folio único y consecutivo | `counters` con `$inc` atómico + índice único en `bookings.folio` |
| El promedio de estrellas siempre cuadra | Transacción: insertar la reseña + actualizar `rating` |
| Los datos copiados (nombre o foto de un salón en favoritos o reservaciones) | Son una foto del momento; si el salón cambia de nombre, se actualizan en segundo plano o se aceptan como históricos |
| Los códigos, sesiones, notificaciones y mensajes del asistente viejos | Se borran solos con índices TTL (`expiresAt` o `purgeAt`) |
| Campos obligatorios y valores válidos | `$jsonSchema` en cada colección y validación en la API |
| Inyección de operadores (`{"$ne": null}`) | La API solo usa filtros tipados del driver (`Builders<T>.Filter`), nunca JSON armado con texto del usuario |

## 7. Orden sugerido para crearla (por módulos funcionales)

Las colecciones, sus índices y su `$jsonSchema` los administra `VirGo-Database` mediante sus comandos de inicialización y verificación. La API solo consume el contrato existente; no crea ni modifica la estructura al arrancar.

1. **Cuentas:** `users`, `sessions` (specs `003` y `004`), `passwordResets` y `paymentCards`.
2. **Catálogo:** `venues`, `venueDates` y `providers`.
3. **Planeación:** `events`, `eventGuests`, `eventTasks` y `favorites`.
4. **Transacciones:** `quotes`, `bookings` y `counters`.
5. **Interacción:** `reviews`, `conversations`, `messages` y `notices`.
6. **Apoyo:** `gigs`, `monthlyStats`, `assistantMessages`, `contactMessages` y `config`.
