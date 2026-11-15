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
- Enums de zonas, categorias de proveedor y tipos de evento.
- Campos nuevos de `users`, `passwordResets`, `sessions` y `providers`.
- Idempotencia y conteos de indices.
- Exportacion determinista, faltantes, diferencias y archivos extra.
- Comparacion de indices y formato de planes.
- TLS implicito en `mongodb+srv://` y explicito en `mongodb://`.
- Rechazo de TLS desactivado, parametros inseguros y valores contradictorios.
- Override inseguro solo en development/test y rechazo absoluto en production.
- El resumen de configuracion no expone la URI.
- Guardas del apply contra acciones destructivas, colecciones ajenas e indices manipulados.
- Rechazo de planes sin huellas, con `planFingerprint` obsoleto o con
  `catalogFingerprint` de otro catalogo.
- Aprobaciones TTL por plan, impacto y `maxDeletion`; TTL como advertencia fuera
  de produccion.
- Waivers de `extraIndex` y `nameMismatch`; rechazo de waivers para
  `keyMismatch`, `optionsMismatch` y TTL.
- JSON estricto: propiedades faltantes, desconocidas y duplicadas; fechas
  invalidas o expiradas; contexto y huellas incorrectas.
- Identidad de cluster estable sin credenciales ni query.
- `verify.ok` frente a `verify.exactMatch` con excepciones aprobadas.
- Fallo parcial con operaciones aplicadas, accion fallida e invalidacion del plan.
- Seed DEMO con las 22 colecciones, IDs deterministas y bloqueo en production.
- Conteo de documentos DEMO y rechazo de la base `virgo`.

Resultado verificado: 82 pruebas aprobadas.

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

- Creacion de las 22 colecciones y 38 indices.
- Verificacion sin deriva.
- Idempotencia de la segunda ejecucion.
- Email unico, `maxLength` y campos extra rechazados en `users`.
- `passwordResets` con `codeHash`, `attempts` y `userId` unico.
- `sessions` con `device` y `revokedAt`.
- Tarjeta predeterminada unica y campos extra rechazados.
- GeoJSON, estado y descripcion de venues.
- `updatedAt` de quotes.
- `bookingId` condicional en reviews y resena duplicada.
- Folio unico, estado y `Decimal128` en bookings.
- Conversaciones duplicadas.
- Fechas duplicadas por venue.
- `budgetPlan` dinamico con `Decimal128`.
- `userId` unico de providers, `pricing`, `responseTime` y galeria sin `maxItems`.
- `bookings.idempotencyKey` unico por cliente y opcional.
- Tipos de evento `XV años` y rechazo de `XV Años`.
- Ausencia de TTL en contactMessages.
- TTL aprobados.
- Deteccion de coleccion faltante sin modificarla.
- Restauracion idempotente despues de la deriva.
- Bloqueo de un indice equivalente con otro nombre hasta aprobar un waiver.
- Bloqueo de un indice con el mismo nombre y distinta definicion.
- TTL faltante en production bloqueado hasta aprobar la excepcion; luego se
  restaura el indice.
- Seed DEMO idempotente sobre las 22 colecciones.
- Limpieza DEMO selectiva que conserva documentos ajenos.
- Repeticion del seed despues de la limpieza.

Resultado verificado: 29 pruebas aprobadas contra MongoDB 8 local, incluida la
creacion del esquema, las nuevas guardas, la aprobacion TTL y el waiver de nombre
distinto. El MongoDB local se detuvo al terminar.

## CLI verificado

- `plan` sobre base vacia: 22 colecciones y 38 indices.
- `apply --yes`: 60 operaciones y verificacion OK.
- Segunda ejecucion: sin cambios.
- `seed-demo --mode plan`: conteos DEMO sin escribir.
- `seed-demo --mode seed --yes`: seed idempotente en development.
- `seed-demo --mode purge --yes`: limpieza selectiva por `_id`.
- `verify`: OK.
- `inspect`: inventario correcto.
- `apply` en produccion sin `--confirm-production`: rechazado antes de conectar.

## Exportador verificado

```powershell
npm run db:export:api:check
python ..\VirGo-API\db\tools\check_schema.py
```

Resultado verificado: 22 archivos sin diferencias y `22 colecciones, 0 diferencias`.

## MongoDB Atlas DEV verificado

Fecha de ejecución: 2026-11-09 01:08:51 -06:00.

- Clúster verificado: `virgo-dev`.
- Base verificada: `virgo_dev`.
- Producción `virgo`: no modificada ni consultada.
- `db:plan:dev` previo: 0 colecciones por crear, 21 validadores por actualizar, 3 índices por crear.
- `db:init:dev -- --yes`: 24 operaciones aplicadas y verificación OK.
- Índices obsoletos eliminados con autorización explícita: `passwordResets.userId_1` y `bookings.clientId_1_createdAt_-1`.
- `db:verify:dev`: OK.
- `db:plan:dev` posterior: 0 cambios pendientes.
- Inventario final: 22 colecciones, 38 índices secundarios, 60 índices totales, 12 únicos, 2 parciales, 4 TTL, 1 geoespacial.
- Documentos: 0 antes y después; no hubo pérdida de datos.

No se ejecutaron operaciones sobre la base de producción.
