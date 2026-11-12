# Operacion

## Archivos de entorno

- `.env.development`: desarrollo, base `virgo_dev`.
- `.env.production`: produccion, base `virgo`.
- `.env.example`: plantilla generica.

Los archivos reales estan en `.gitignore`. No los comparta ni los confirme.

`MONGODB_URI` debe usar TLS. `mongodb+srv://` lo habilita por defecto; con
`mongodb://` agregue `tls=true`. No desactive la validacion TLS. Para MongoDB
local en development/test puede usar `MONGO_ALLOW_INSECURE_TLS=true`; production
siempre rechaza esa variable.

## Flujo recomendado

1. `plan`: muestra las operaciones exactas sin escribir.
2. `validate`: falla si hay deriva.
3. `apply`: exige `--yes`; en produccion exige `--confirm-production`.
4. `verify`: confirma el estado final.
5. `inspect`: muestra el inventario real de colecciones e indices.

## Comandos

```powershell
npm run db:plan:dev
npm run db:validate:dev
npm run db:init:dev -- --yes
npm run db:verify:dev
npm run db:inspect:dev
```

```powershell
npm run db:plan:prod
npm run db:validate:prod
npm run db:init:prod -- --yes
npm run db:verify:prod
npm run db:inspect:prod
```

## Seed DEMO

Solo para development/test. El seed usa IDs deterministas y no se ejecuta en
production ni sobre `virgo`.

```powershell
npm run db:seed:demo:plan
npm run db:seed:demo
npm run db:seed:demo:purge
```

`db:seed:demo` y `db:seed:demo:purge` incluyen `--yes`; el modo `plan` no
escribe. `purge` elimina unicamente los `_id` del DEMO y conserva cualquier otro
documento. No se ejecuta automaticamente en `apply` ni en `verify`.

## Exportacion a la API

Despues de cambiar el catalogo:

```powershell
npm run db:export:api
npm run db:export:api:check
python ..\VirGo-API\db\tools\check_schema.py
```

`db:export:api` escribe los 22 archivos de `VirGo-API/db/schema`. `db:export:api:check` no escribe y falla si hay diferencias. No edite esos JSON a mano.

## MongoDB local para pruebas

Inicio:

```powershell
.\scripts\dev\start-local-mongo.ps1
$env:MONGODB_TEST_URI = 'mongodb://127.0.0.1:27018/?replicaSet=rs0'
```

Detencion:

```powershell
.\scripts\dev\stop-local-mongo.ps1
```

El script usa `%TEMP%\virgo-mongo-dev` y el puerto `27018`. No toca Atlas.

## Folios

`counters` se crea vacia. La generacion es responsabilidad de la API:

```js
const year = new Date().getUTCFullYear();
const counter = await db.collection('counters').findOneAndUpdate(
  { _id: `folio-${year}` },
  { $inc: { next: 1 } },
  { upsert: true, returnDocument: 'after' }
);
const folio = `VG-${year}-${String(counter.next).padStart(4, '0')}`;
```

El indice unico de `bookings.folio` protege contra duplicados. La secuencia no garantiza ausencia de huecos.

## Idempotencia de reservas

La API puede enviar `bookings.idempotencyKey` (maximo 100 caracteres). El indice unico parcial `clientId_1_idempotencyKey_1_unique` rechaza repetir la misma clave para el mismo cliente. Si el documento no tiene clave, el indice no lo considera.

## Cancelacion de reservas

La regla aprobada es transaccional:

1. Iniciar transaccion.
2. Eliminar el `venueDates` que coincida con `venueId`, `date` y `bookingId`.
3. Si no se elimino exactamente un documento, abortar.
4. Actualizar la reserva a `cancelled` y registrar la cancelacion.
5. Confirmar.

La anomalia debe registrarse fuera de la transaccion. El repositorio no implementa esta logica; solo define el contrato de datos.

## TTL

- `passwordResets.expiresAt`: fecha de expiracion del codigo de recuperacion.
- `sessions.purgeAt`: fecha de purga de la sesion.
- `notices.purgeAt`: `createdAt + 90 dias`.
- `assistantMessages.purgeAt`: `createdAt + 30 dias`.

MongoDB elimina documentos vencidos de forma periodica; no es inmediato. No se agregan TTL a `venueDates`, `gigs` ni `contactMessages`.

## Fallos parciales y reanudacion

`apply` valida el plan antes de escribir y nunca ejecuta `drop`, `delete`,
`rename` ni acciones fuera del catalogo. Si una operacion falla a mitad del
proceso, el CLI informa cuantas operaciones ya se aplicaron. Las operaciones son
idempotentes: corrija la causa, ejecute de nuevo `plan` y luego `apply --yes`.
No intente "deshacer" manualmente una coleccion o un indice ya creado.

## Problemas comunes

### No existe el archivo de entorno

Copie el ejemplo correspondiente y complete la URI.

### TLS rechazado

Use `mongodb+srv://` o agregue `tls=true` a `MONGODB_URI`. Retire
`tlsInsecure`, `tlsAllowInvalidCertificates`, `tlsAllowInvalidHostnames` y
`sslInvalidHostNameAllowed`. Si usa MongoDB local, habilite
`MONGO_ALLOW_INSECURE_TLS=true` solo en development/test.

### Base de datos rechazada

`development` solo acepta `virgo_dev`; `production` solo acepta `virgo`. Corrija `MONGODB_DB` o el entorno.

### Conflicto de indice

Existe un indice con el mismo nombre pero distinta clave u opciones. El plan se bloquea. Use `inspect` y resuelva manualmente. El inicializador no elimina indices.

### DuplicateKey al crear un indice unico

Hay documentos duplicados. Limpie o consolide los datos de forma revisada antes de reintentar. No hay borrado automatico.

### DocumentValidationFailure

El documento no cumple `$jsonSchema`. Revise campos obligatorios, enums, tipos y `additionalProperties` de `users` y `paymentCards`.

### Timeout de conexion

Revise la URI, el estado del cluster y la lista de acceso de red de Atlas.

## Prohibiciones

- No ejecutar `dropDatabase`, `dropCollection`, `dropIndexes` ni `deleteMany` desde este proyecto.
- No aplicar en produccion sin plan revisado y respaldo.
- No usar credenciales DDL en la API.
- No ejecutar el seed DEMO en produccion ni sobre `virgo`.
- No desactivar TLS ni usar el override inseguro fuera de development/test.
- No versionar archivos `.env` reales.
