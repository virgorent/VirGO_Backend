# Preparacion de produccion

## Objetivo

Preparar la revision de la base `virgo` sin conectarse, sin aplicar cambios y sin
crear usuarios, redes ni documentos. Este documento no sustituye la ventana de
cambio ni la autorizacion del responsable de produccion.

## Estado actual

- Base objetivo: `virgo`.
- Cluster y URI: no registrados en el repositorio.
- `.env.production` real: no existe en el entorno local.
- Credenciales de produccion: no disponibles para esta entrega.
- Usuario runtime `virgo-api-prod`: documentado, no verificado en Atlas.
- Plan remoto: **bloqueado** por falta de credenciales y autorizacion.
- Operaciones ejecutadas sobre `virgo`: ninguna.

No se debe inventar ni inferir el inventario remoto. El plan real solo se obtiene
al ejecutar el CLI con una credencial autorizada.

## Inventario esperado desde el catalogo

El catalogo local declara, como objetivo:

- 22 colecciones.
- 38 indices secundarios.
- 60 indices totales incluido `_id_`.
- 12 indices unicos.
- 2 indices parciales.
- 4 indices TTL.
- 1 indice geoespacial.

Este es el contrato esperado, no el inventario real de `virgo`.

## Autorizacion necesaria

Antes de continuar se requiere:

1. Aprobacion explicita del responsable de produccion.
2. Credencial DDL de produccion con permisos de lectura, creacion y `collMod`.
3. Confirmacion del cluster y de la base `virgo`.
4. Respaldo o snapshot reciente.
5. Ventana de cambio y responsable de verificacion.
6. Confirmacion de que la API productiva usa el usuario runtime, no el DDL.

## Secuencia propuesta

Ejecutar solo despues de la autorizacion:

```powershell
Copy-Item .env.production.example .env.production
```

Completar la URI con la credencial DDL autorizada y `MONGODB_DB=virgo`.

```powershell
npm run db:plan:prod
npm run db:validate:prod
```

Revisar el plan y el inventario antes de escribir:

```powershell
node src/cli/inspect.js --env-file .env.production --json
```

Si el plan es el esperado y no hay conflictos:

```powershell
npm run db:init:prod -- --yes
npm run db:verify:prod
```

`db:init:prod` incluye `--confirm-production`. No agregar `--yes` sin revisar el
plan. No ejecutar `seed` de DEMO en produccion. El seed solo esta permitido en
development/test y rechaza la base `virgo`; su limpieza es selectiva por `_id`.

## Revision del plan

| Control | Criterio |
| --- | --- |
| Colecciones | 22 esperadas; cualquier diferencia requiere explicacion |
| Indices secundarios | 38 esperados; conflictos bloquean el apply |
| Indices extra | Se reportan; no se eliminan automaticamente |
| Validadores | `validationLevel: strict` y `validationAction: error` |
| Datos | El apply no inserta, actualiza ni elimina documentos |
| Acciones destructivas | Deben ser cero; el inicializador las rechaza |
| Fallo parcial | Informa lo aplicado; se reanuda con otro plan idempotente |
| TLS | Obligatorio; production rechaza el override inseguro |

## API runtime

La API debe conectarse con `virgo-api-prod` y `Persistence:DatabaseName=virgo`.
No debe usar el usuario DDL, una cuenta administrativa ni la base `virgo_dev`.
La implementacion de la API debe validarse en la rama que contiene
`VirGo-API/src`; en `rama-backend` solo existe configuracion de ejemplo.

## Rotacion y respuesta

La rotacion y la respuesta a incidentes se describen en
[SECURITY.md](SECURITY.md). Ninguna credencial se versiona ni se imprime.

## Criterio de cierre

La preparacion se considera completa solo cuando exista evidencia de:

- `db:plan:prod` revisado y aprobado.
- `db:init:prod -- --yes` aplicado en la ventana autorizada.
- `db:verify:prod` sin deriva.
- Usuario runtime verificado sin permisos DDL.
- Red de produccion revisada y documentada.
- Respaldo y plan de contencion confirmados.
