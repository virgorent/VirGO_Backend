# Seguridad

## Alcance

Este documento cubre la operacion de MongoDB para VirGo: identidades, TLS,
red, almacenamiento de secretos y rotacion. No contiene credenciales reales ni
sustituye la configuracion de Atlas.

## Identidades y minimo privilegio

| Identidad | Acceso | Uso |
| --- | --- | --- |
| `virgo-api-prod` | `readWrite` solo sobre `virgo` | Runtime de la API en produccion |
| `virgo-api-dev` | `readWrite` solo sobre `virgo_dev` | Runtime de la API en desarrollo |
| Usuario DDL | Listar, crear colecciones e indices y `collMod` | `plan`, `validate`, `apply`, `inspect` |
| Usuario de lectura | `read` sobre la base autorizada | Inspeccion y diagnostico |

Reglas:

- La API nunca usa el usuario DDL ni una cuenta administrativa.
- El usuario DDL no requiere `dropDatabase`, `dropCollection`, `dropIndexes` ni `delete`.
- Cada entorno usa una credencial distinta.
- No se comparten credenciales entre desarrollo y produccion.

La creacion de usuarios en Atlas es una operacion externa al repositorio y
requiere autorizacion explicita. No se ejecuta desde este proyecto.

## TLS

La aplicacion exige TLS en todas las conexiones:

- `mongodb+srv://` habilita TLS por defecto.
- `mongodb://` requiere `tls=true` o `ssl=true`.
- Se rechazan `tls=false`, `tlsInsecure`, certificados invalidos y nombres de host invalidos.
- `MONGO_ALLOW_INSECURE_TLS=true` solo se admite en development/test para MongoDB local; production siempre lo rechaza.

## Almacenamiento de secretos

- En produccion, la URI y las llaves se guardan en AWS SSM SecureString y se
  inyectan como variables de entorno de la Lambda.
- Los archivos `.env.*` reales estan ignorados por Git y solo se usan en local.
- No se imprimen URIs, usuarios, contrasenas, tokens ni contenido de `.env`.
- Los errores y logs solo muestran entorno, base y conteos.
- El seed DEMO usa datos ficticios, se bloquea en production y nunca se ejecuta sobre `virgo`.
- El purge DEMO solo elimina `_id` conocidos y no reemplaza la revision de respaldos.

## Red

`0.0.0.0/0` en Atlas es una excepcion temporal. Si la Lambda no esta en una VPC,
no tiene una IP de salida fija y la lista de acceso no puede acotarse por IP.
Antes de produccion se debe elegir una de estas rutas:

1. Lambda en VPC con NAT Gateway e IP elastica registrada en Atlas.
2. PrivateLink o peering.
3. Excepcion documentada con `0.0.0.0/0`, usuario de minimo privilegio,
   contrasena larga, TLS obligatorio, alertas y rotacion periodica.

La opcion 3 no se considera definitiva para produccion.

## Rotacion de credenciales

Procedimiento propuesto, sin ejecutar en esta entrega:

1. Crear una credencial nueva para la misma identidad o un usuario nuevo con el
   mismo rol.
2. Guardar el valor en el almacen de secretos autorizado.
3. Actualizar el consumidor: SSM para la Lambda; `.env` local para desarrollo.
4. Desplegar o reiniciar el consumidor y verificar conexion, lectura y escritura.
5. Confirmar que no hay trafico con la credencial anterior.
6. Revocar la credencial anterior.
7. Registrar fecha, responsable y motivo de la rotacion.

Para el usuario DDL, repetir el proceso en una ventana de mantenimiento y
verificar `plan` y `verify` antes de revocar. Para JWT, rotar la llave de firma
con una ventana que permita expirar las sesiones anteriores.

## Respuesta a incidentes

1. Revocar la credencial comprometida.
2. Rotar la URI y las llaves relacionadas.
3. Revisar actividad reciente en Atlas y logs de la Lambda.
4. Verificar que no se modificaron colecciones, validadores ni indices.
5. Documentar el incidente en la memoria del proyecto.

## Brechas actuales

- No se ha verificado la creacion real de `virgo-api-prod` ni su rol en Atlas.
- No se ha verificado la lista de acceso de red definitiva.
- Gitleaks y Dependency Review siguen pendientes de ejecucion en el PR.
- La implementacion de la API no esta en la rama `rama-backend`; sus cambios de
  codigo deben revisarse en la rama que contenga `VirGo-API/src`.
