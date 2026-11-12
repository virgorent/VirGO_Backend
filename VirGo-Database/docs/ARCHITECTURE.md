# Arquitectura

## Objetivo

Mantener el contrato de datos de VirGo en MongoDB de forma versionada, verificable e idempotente, sin acoplarse a la API.

`src/catalog` es la fuente de verdad de:

- Nombres de colecciones.
- Validadores `$jsonSchema`.
- Indices y sus opciones.
- Reglas de despliegue y verificacion.

`VirGo-API/db/schema` es una salida generada desde el catalogo; no se edita a mano.

## Capas

### Catalogo

`src/catalog` declara las 22 colecciones y sus validadores. Cada coleccion tiene:

- `name`.
- `validator`.
- `indexes`.

Los validadores se agrupan por dominio para mantenerlos revisables:

- `accounts.js`: usuarios, resets, sesiones y tarjetas.
- `venues.js`: venues y disponibilidad.
- `providers.js`: proveedores y gigs.
- `planning.js`: eventos, invitados, tareas y favoritos.
- `quotes.js`: cotizaciones.
- `bookings.js`: reservas y contadores.
- `reputation.js`: resenas y estadisticas mensuales.
- `interaction.js`: conversaciones, mensajes y avisos.
- `support.js`: asistente y contacto.

### Inicializacion

`src/init` separa el proceso en tres fases:

1. `buildPlan`: lee el estado actual y calcula operaciones.
2. `applyPlan`: ejecuta solo las operaciones del plan.
3. `verifyInitialization`: vuelve a leer y confirma el estado.

El plan incluye:

- Colecciones faltantes.
- Validadores desactualizados.
- Indices faltantes.
- Conflictos de indices.
- Indices adicionales como advertencia.

Si hay un conflicto de indices, no se aplica ningun cambio. Los indices adicionales no se eliminan.

Antes de escribir, `applyPlan` valida que cada accion sea `createCollection`, `collMod` o `createIndex`, que la coleccion pertenezca al catalogo y que el indice coincida exactamente con su definicion declarada. Asi se bloquean planes manipulados o acciones destructivas. Si una operacion falla a mitad del proceso, el error incluye las operaciones ya aplicadas y la accion fallida; el siguiente `plan`/`apply` reanuda de forma idempotente.

### CLI

- `init.js`: plan, validate y apply.
- `verify.js`: verificacion de solo lectura.
- `inspect.js`: inventario de colecciones e indices existentes.

`apply` imprime el plan antes de escribir. Sin `--yes` no escribe. En produccion requiere `--confirm-production`.

### Exportacion

`src/export/schema.js` transforma cada coleccion del catalogo al contrato de `VirGo-API`:

- `buildApiSchemas` produce los 22 documentos.
- `writeApiSchemas` escribe JSON determinista con LF y salto final.
- `checkApiSchemas` detecta archivos faltantes, distintos o extra sin escribir.

`src/cli/export.js` expone `db:export:api` y `db:export:api:check`.

### Seed DEMO

`src/seed/demo.js` construye un conjunto ficticio y determinista para las 22
colecciones. Los `_id` se derivan de un hash estable, de modo que `replaceOne`
con `upsert` no duplica documentos. `src/cli/seed-demo.js` expone:

- `plan`: muestra conteos sin conectar.
- `seed`: inserta o reemplaza solo los documentos DEMO; exige `--yes`.
- `purge`: elimina solo los `_id` DEMO; exige `--yes`.

El seed se rechaza en production y en la base `virgo`. La limpieza no usa
`drop`, no borra colecciones y no toca documentos con otros `_id`.

### Configuracion

`src/config/env.js` carga el archivo indicado con `dotenv`, valida las variables y aplica guardas de entorno:

- `development` solo permite `virgo_dev`.
- `production` solo permite `virgo`.
- `test` solo permite nombres que empiecen con `virgo_test_`.
- TLS es obligatorio; `mongodb+srv://` lo habilita por defecto y `mongodb://` requiere `tls=true`.
- Se rechazan `tls=false`, `tlsInsecure`, certificados invalidos y nombres de host invalidos.
- `MONGO_ALLOW_INSECURE_TLS=true` solo se acepta en development/test para MongoDB local; production siempre lo rechaza.

La URI nunca se incluye en la salida. `connectDatabase` vuelve a exigir TLS antes de crear el cliente.

## Idempotencia

El inicializador no usa locks, colecciones de configuracion ni migraciones.

- Si una coleccion no existe, se crea.
- Si existe, se actualiza el validador con `collMod`.
- Un indice se considera satisfecho si existe uno equivalente por clave y opciones, aunque su nombre sea distinto.
- El indice parcial de idempotencia solo aplica a documentos con `bookings.idempotencyKey` string.
- Si el mismo nombre existe con otra definicion, se reporta conflicto y no se escribe.
- Los indices adicionales se reportan como advertencia.

Esto permite ejecutar el proceso en cada despliegue y tambien convivir con indices creados previamente por la API.

## Convenciones de datos

- Fechas de calendario: string `YYYY-MM-DD`.
- Marcas temporales: BSON `Date` UTC.
- Dinero: `Decimal128`.
- Contadores enteros: `int`.
- `_id`: UUID string, excepto `counters._id` con formato `folio-AAAA` y `monthlyStats._id` con formato `<targetType>:<targetId>:<AAAA-MM>`.
- Meses estadisticos: string `YYYY-MM`.
- Folios: string `VG-AAAA-NNNN` con al menos cuatro digitos.

## Validacion

Todas las colecciones usan `validationLevel: strict` y `validationAction: error`.

`users` y `paymentCards` usan `additionalProperties: false` y declaran `_id` explicitamente. El resto de colecciones permite campos adicionales para no bloquear evoluciones controladas.

`reviews` usa una condicion adicional: las resenas de venues deben incluir `bookingId`; las de proveedores pueden omitirlo.

## Limites

- No implementa generacion de folios; documenta el patron de `counters`.
- No implementa transacciones de cancelacion; documenta la regla aprobada.
- No implementa pagos ni webhooks.
- No crea usuarios de Atlas ni redes.
- No ejecuta `drop` ni `dropIndexes`; el seed solo reemplaza IDs DEMO y el purge solo elimina esos IDs.
- No crea datos reales de negocio; el seed DEMO es ficticio y opcional.
