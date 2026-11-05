# VirGo Database

Infraestructura independiente de MongoDB para VirGo: colecciones, validadores `$jsonSchema`, indices, inicializador idempotente, verificacion, CLI y pruebas.

Este repositorio no contiene la API de VirGo. No crea ni modifica `VirGo-API`, no implementa endpoints, pagos ni logica de negocio, y no siembra datos.

## Estado

- 22 colecciones declaradas.
- Validadores estrictos con `validationLevel: strict` y `validationAction: error`.
- 37 indices, incluidos TTL, unicos y un indice unico parcial.
- Inicializador idempotente con modos `plan`, `validate` y `apply`.
- Verificador de deriva.
- 16 pruebas unitarias y 20 pruebas de integracion verificadas contra MongoDB 8 local.
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

## Uso

Desarrollo:

```powershell
npm run db:plan:dev
npm run db:init:dev -- --yes
npm run db:verify:dev
npm run db:inspect:dev
```

Produccion:

```powershell
npm run db:plan:prod
npm run db:init:prod -- --yes
npm run db:verify:prod
```

`db:init:prod` incluye `--confirm-production`. Aun asi, revise siempre el plan y tenga un respaldo antes de aplicar. El inicializador nunca elimina colecciones, indices ni documentos.

Tambien puede invocar el CLI directamente:

```powershell
node src/cli/init.js --env-file .env.development --mode plan
node src/cli/init.js --env-file .env.development --mode apply --yes
node src/cli/verify.js --env-file .env.development
node src/cli/inspect.js --env-file .env.development --json
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
  cli/                 plan/apply/verify/inspect
  config/              Carga y validacion de entorno
  connection/          Cliente MongoDB
  init/                Plan, aplicacion, verificacion y formato
  util/                Parametros de CLI
tests/
  unit/                Pruebas sin MongoDB
  integration/         Pruebas contra MongoDB local
  helpers/             Fixtures y base de prueba
scripts/dev/           MongoDB local para pruebas
docs/                  Arquitectura, Atlas, operaciones y decisiones
```

## Seguridad operativa

- `plan` y `validate` son de solo lectura.
- `apply` imprime el plan exacto antes de escribir y exige `--yes`.
- En `production`, `apply` exige ademas `--confirm-production`.
- El nombre de base se valida contra el entorno.
- No se imprimen URIs ni credenciales.
- No se eliminan colecciones, indices ni documentos.
- Se recomienda un usuario DDL separado del usuario runtime.

## Documentacion

- [Arquitectura](docs/ARCHITECTURE.md)
- [Colecciones e indices](docs/COLLECTIONS.md)
- [Configuracion de Atlas](docs/ATLAS_SETUP.md)
- [Operacion](docs/OPERATIONS.md)
- [Decisiones](docs/DECISIONS.md)
- [Pruebas](docs/TESTING.md)
