# Pruebas

## Unitarias

```powershell
npm test
```

Verifican, sin MongoDB:

- Las 22 colecciones exactas.
- Ausencia de `config`, migraciones y locks.
- Validadores raiz.
- `additionalProperties: false` en `users` y `paymentCards`.
- `quotes.updatedAt` obligatorio.
- Condicion de `bookingId` en resenas.
- Indices criticos y unicos.
- TTL solo en las colecciones aprobadas.
- Ausencia de `maxItems` arbitrarios.
- Comparacion de indices y formato de planes.

Resultado verificado: 16 pruebas aprobadas.

## Integracion

Requiere MongoDB local. Con MongoDB 8 instalado:

```powershell
.\scripts\dev\start-local-mongo.ps1
$env:MONGODB_TEST_URI = 'mongodb://127.0.0.1:27018/?replicaSet=rs0'
npm run test:integration
.\scripts\dev\stop-local-mongo.ps1
```

Tambien puede apuntar a otro servidor de prueba:

```powershell
$env:MONGODB_TEST_URI = 'mongodb://127.0.0.1:27017'
npm run test:integration
```

Las pruebas crean una base `virgo_test_*` y la eliminan al terminar. Nunca usan `virgo_dev` ni `virgo`.

Cobertura:

- Creacion de las 22 colecciones y 37 indices.
- Verificacion sin deriva.
- Idempotencia de la segunda ejecucion.
- Email unico y campos extra rechazados en `users`.
- Tarjeta predeterminada unica y campos extra rechazados.
- GeoJSON, estado y descripcion de venues.
- `updatedAt` de quotes.
- `bookingId` condicional en reviews y resena duplicada.
- Folio unico, estado y `Decimal128` en bookings.
- Conversaciones duplicadas.
- Fechas duplicadas por venue.
- `budgetPlan` dinamico con `Decimal128`.
- `userId` unico de providers y galeria sin `maxItems`.
- Ausencia de TTL en contactMessages.
- TTL aprobados.
- Deteccion de coleccion faltante sin modificarla.
- Restauracion idempotente despues de la deriva.
- Compatibilidad con un indice equivalente creado con otro nombre.
- Bloqueo de un indice con el mismo nombre y distinta definicion.

Resultado verificado: 20 pruebas aprobadas contra MongoDB 8 local con replica set.

## CLI verificado

- `plan` sobre base vacia: 22 colecciones y 37 indices.
- `apply --yes`: 59 operaciones y verificacion OK.
- Segunda ejecucion: sin cambios.
- `verify`: OK.
- `inspect`: inventario correcto.
- `apply` en produccion sin `--confirm-production`: rechazado antes de conectar.

No se ejecutaron operaciones sobre MongoDB Atlas.
