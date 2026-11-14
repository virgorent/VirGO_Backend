# VirGo Database

Infraestructura independiente de MongoDB para VirGo: colecciones, validadores `$jsonSchema`, indices, inicializador idempotente, verificacion, CLI y pruebas.

Este repositorio no contiene la API de VirGo. No crea ni modifica `VirGo-API`, no implementa endpoints, pagos ni logica de negocio. Incluye un seed DEMO opcional para development/test con datos ficticios y limpieza selectiva.

## Estado

- 22 colecciones declaradas.
- Validadores estrictos con `validationLevel: strict` y `validationAction: error`.
- 38 indices secundarios: 12 unicos, 2 parciales y 4 TTL.
- Inicializador idempotente con modos `plan`, `validate` y `apply`.
- Huellas de catalogo y plan, waivers por indice y aprobaciones TTL.
- Verificador de deriva con `ok` y `exactMatch`, y exportador determinista de esquemas para `VirGo-API`.
- 82 pruebas unitarias y 29 de integracion verificadas contra MongoDB 8 local.
- Seed DEMO idempotente con IDs deterministas y limpieza selectiva; nunca se ejecuta en production.
- TLS obligatorio para Atlas; el override inseguro solo se permite en development/test local.
- Sin credenciales ni secretos en el repositorio.

## Requisitos

- Node.js 20.19 o superior.
- npm.
- Acceso a MongoDB Atlas para desarrollo y produccion.
- MongoDB 8 local y `mongosh` para pruebas de integracion opcionales.

## Instalacion

```powershell
npm install
```

## Configuracion

Copie el ejemplo correspondiente y complete los valores:

```powershell
Copy-Item .env.development.example .env.development
Copy-Item .env.production.example .env.production
```

Nunca confirme `.env`, `.env.development` ni `.env.production`. Solo los archivos `*.example` se versionan.

Variables:

| Variable | Obligatoria | Descripcion |
| --- | --- | --- |
| `NODE_ENV` | Si | `development`, `production` o `test`. |
| `MONGODB_URI` | Si | URI de Atlas. No se imprime en logs. |
| `MONGODB_DB` | Si | `virgo_dev` en development, `virgo` en production. |
| `MONGO_APP_NAME` | No | Nombre de la aplicacion en MongoDB. |
| `MONGO_MAX_POOL_SIZE` | No | Tamano del pool. Default `10`. |
| `MONGO_SERVER_SELECTION_TIMEOUT_MS` | No | Timeout de seleccion. Default `10000`. |
| `MONGO_ALLOW_INSECURE_TLS` | No | Solo development/test local. Nunca se acepta en production. |

`MONGODB_URI` debe usar TLS. `mongodb+srv://` lo habilita por defecto; con
`mongodb://` agregue `tls=true`. La aplicacion rechaza `tls=false`,
`tlsInsecure`, certificados invalidos y nombres de host invalidos.

## Uso

Desarrollo:

```powershell
npm run db:plan:dev
npm run db:validate:dev
npm run db:init:dev -- --yes
npm run db:verify:dev
npm run db:inspect:dev
```

Produccion:

```powershell
npm run db:plan:prod
npm run db:validate:prod
npm run db:init:prod -- --yes
npm run db:verify:prod
```

`db:init:prod` incluye `--confirm-production`. Aun asi, revise siempre el plan y tenga un respaldo antes de aplicar. El inicializador nunca elimina colecciones, indices ni documentos.

Si el plan reporta un indice extra, un nombre distinto o un TTL pendiente en
produccion, genere el archivo de waiver o de aprobacion TTL correspondiente y
vinculado al `planFingerprint`. Use `--waiver-file`, `--ttl-approval-file` y
`--plan-file`. `--yes` no sustituye estas aprobaciones. Consulte
[Operacion](docs/OPERATIONS.md).

Tambien puede invocar el CLI directamente:

```powershell
node src/cli/init.js --env-file .env.development --mode plan
node src/cli/init.js --env-file .env.development --mode apply --yes
node src/cli/verify.js --env-file .env.development
node src/cli/inspect.js --env-file .env.development --json
node src/cli/export.js
node src/cli/export.js --check
```

## Seed DEMO

El seed usa IDs deterministas, documentos ficticios y `replaceOne` con `upsert`.
Se puede ejecutar varias veces sin duplicar. Solo se permite en `development` o
`test`; `production` y la base `virgo` se rechazan antes de conectar.

```powershell
npm run db:seed:demo:plan
npm run db:seed:demo
npm run db:seed:demo:purge
```

`purge` elimina solo los `_id` del DEMO y no toca documentos ajenos ni
colecciones. No se ejecuta automaticamente en `apply` ni en `verify`.

## Exportacion del contrato para la API

`VirGo-Database` es la fuente de verdad. Los esquemas consumidos por `VirGo-API` se generan, no se editan a mano:

```powershell
npm run db:export:api
npm run db:export:api:check
python ..\VirGo-API\db\tools\check_schema.py
```

## Pruebas

```powershell
npm test
```

Integracion contra MongoDB local:

```powershell
.\scripts\dev\start-local-mongo.ps1
$env:MONGODB_TEST_URI = 'mongodb://127.0.0.1:27018/?replicaSet=rs0'
npm run test:integration
.\scripts\dev\stop-local-mongo.ps1
```

Si no hay MongoDB disponible, las pruebas de integracion se omiten. Las pruebas usan bases `virgo_test_*` y eliminan solo esa base al terminar.

## Estructura

```text
src/
  catalog/
    validators/        Validadores por dominio
    enums.js           Valores permitidos
    helpers.js         Helpers de $jsonSchema
    indexes.js         Indices declarados
    index.js           Catalogo de las 22 colecciones
  cli/                 plan/apply/verify/inspect/export
  config/              Carga y validacion de entorno
  connection/          Cliente MongoDB
  export/              Generacion determinista para VirGo-API
  init/                Plan, aplicacion, verificacion y formato
  seed/                Documentos DEMO deterministas
  util/                Parametros de CLI
tests/
  unit/                Pruebas sin MongoDB
  integration/         Pruebas contra MongoDB local
  helpers/             Fixtures y base de prueba
specs/                 Specs 000-base-datos y 001-contrato-con-api
scripts/dev/           MongoDB local para pruebas
docs/                  Arquitectura, Atlas, operaciones y decisiones
```

## Seguridad operativa

- `plan` y `validate` son de solo lectura.
- `apply` imprime el plan exacto antes de escribir y exige `--yes`.
- En `production`, `apply` exige ademas `--confirm-production`.
- El nombre de base se valida contra el entorno.
- La conexion exige TLS y rechaza parametros que desactiven su validacion.
- No se imprimen URIs ni credenciales.
- No se eliminan colecciones, indices ni documentos.
- `apply` valida huellas, aprobaciones y que cada accion pertenezca al catalogo antes de escribir.
- Los waivers solo cubren indices extra o nombres distintos; no cubren conflictos de clave, opciones ni TTL.
- En produccion, TTL faltante o adicional exige aprobacion especifica con limite de impacto.
- Un fallo parcial informa las operaciones aplicadas, invalida el plan y exige una nueva inspeccion.
- El seed DEMO esta bloqueado en production y su limpieza solo borra IDs conocidos.
- Se recomienda un usuario DDL separado del usuario runtime.

## Documentacion

- [Arquitectura](docs/ARCHITECTURE.md)
- [Colecciones e indices](docs/COLLECTIONS.md)
- [Configuracion de Atlas](docs/ATLAS_SETUP.md)
- [Operacion](docs/OPERATIONS.md)
- [Decisiones](docs/DECISIONS.md)
- [Pruebas](docs/TESTING.md)
- [Seguridad](docs/SECURITY.md)
- [Preparacion de produccion](docs/PRODUCTION_READINESS.md)
- [Handoff para la API](docs/API-HANDOFF.md)
- [Spec 000: base de datos](specs/000-base-datos/spec.md)
- [Spec 001: contrato con la API](specs/001-contrato-con-api/spec.md)
