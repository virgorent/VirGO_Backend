# Configuracion de MongoDB Atlas

## Separacion de entornos

Se recomienda un proyecto de Atlas por entorno:

- Desarrollo: proyecto y cluster con base `virgo_dev`.
- Produccion: proyecto y cluster con base `virgo`.

No use datos reales en desarrollo. Si se comparte un unico cluster M0 por costo, use bases, usuarios y credenciales independientes. Nunca apunte `.env.development` a la base de produccion.

## Cluster

- Desarrollo: M0 es suficiente para pruebas iniciales.
- Produccion: use un tier con backups, monitoreo y recursos adecuados.
- Version objetivo: MongoDB 8.
- Habilite backups y alertas antes de aplicar cambios en produccion.

## Usuarios de base de datos

Cree dos usuarios por entorno:

### Usuario DDL

Solo se usa para `plan`, `apply` e `inspect`. Requiere permisos sobre la base objetivo:

- `listCollections`
- `listIndexes`
- `createCollection`
- `createIndex`
- `collMod`

Puede implementarse con un rol personalizado de Atlas. No requiere `dropCollection`, `dropIndexes`, `delete` ni `dropDatabase`.

### Usuario runtime

Solo lo usa la API. Debe tener `readWrite` sobre la base objetivo y no permisos DDL.

No use el usuario DDL en la aplicacion.

## Red

- Agregue solo las IP de desarrollo o las salidas NAT/VPC necesarias.
- En produccion prefiera PrivateLink o peering.
- Evite `0.0.0.0/0` salvo en un entorno temporal y controlado.

## Cadena de conexion

Use TLS y la cadena que Atlas proporciona:

```text
mongodb+srv://USUARIO:PASSWORD@CLUSTER.mongodb.net/?retryWrites=true&w=majority&appName=VirGo-Database
```

No guarde la cadena real en el repositorio. Coloquela en `.env.development` o `.env.production`, que estan ignorados por Git.

## Primer despliegue

1. Copie el ejemplo:

```powershell
Copy-Item .env.development.example .env.development
```

2. Complete `MONGODB_URI` con el usuario DDL y confirme `MONGODB_DB=virgo_dev`.
3. Ejecute el plan de solo lectura:

```powershell
npm run db:plan:dev
```

4. Revise las colecciones e indices que se crearan.
5. Aplique:

```powershell
npm run db:init:dev -- --yes
```

6. Verifique:

```powershell
npm run db:verify:dev
```

## Produccion

1. Cree un respaldo o snapshot.
2. Configure `.env.production` con el usuario DDL de produccion y `MONGODB_DB=virgo`.
3. Ejecute y revise `npm run db:plan:prod`.
4. Aplique con `npm run db:init:prod -- --yes`.
5. Ejecute `npm run db:verify:prod`.
6. Revise indices y rendimiento en Atlas.

El CLI imprime el plan exacto antes de escribir. No aplique un plan sin revisarlo.

## Notas

- Los TTL dependen de `expireAfterSeconds: 0` y de que la aplicacion escriba `purgeAt` o `expiresAt`.
- Los indices unicos pueden fallar si ya existen datos duplicados. Resuelva los datos manualmente; el inicializador no elimina informacion.
- Los indices adicionales se reportan como advertencia y no se eliminan.
- Un conflicto de nombre o definicion de indice bloquea la aplicacion del plan.
