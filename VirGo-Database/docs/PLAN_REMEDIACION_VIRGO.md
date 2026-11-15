# Plan de remediación de la base `virgo`

| Campo | Valor |
|---|---|
| Fecha | 2026-11-13 |
| Estado | **No-Go condicional**; guardas implementadas y probadas localmente; producción no ejecutada |
| Base objetivo | `virgo` |
| Fuente de verdad | `VirGo-Database/src/catalog/` |
| Evidencia de origen | [`VirGo-API/specs/010-contrato-datos/audit-live-atlas-2026-11-13.md`](../../VirGo-API/specs/010-contrato-datos/audit-live-atlas-2026-11-13.md) |
| Autorización requerida | Responsable de producción, credencial DDL, credencial de recuperación y ventana de cambio |
| Alcance de esta versión | Guardas locales implementadas y probadas; no modifica datos, credenciales, red ni la base de producción |

## 1. Resumen y veredicto

La auditoría live del 2026-11-13 encontró que `virgo` solo tiene `users` y `sessions`. Faltan 20 de las 22 colecciones, los validadores de `users` y `sessions` no coinciden con el catálogo y faltan 4 índices en esas dos colecciones.

El veredicto es **No-Go condicional**. No se debe ejecutar `db:init:prod` hasta cumplir las precondiciones, completar la auditoría read-only, obtener un Go/No-Go firmado y resolver o aceptar formalmente los hallazgos de la revisión.

El apply es idempotente y el inicializador no inserta, actualiza ni elimina documentos directamente. Sin embargo, **no es libre de efectos secundarios**:

- Crear un índice TTL puede provocar que MongoDB elimine documentos cuando el monitor TTL los procese.
- Crear un índice único puede fallar si existen duplicados, valores nulos o campos ausentes en conflicto.
- `collMod` reemplaza el validador completo y puede endurecer o debilitar restricciones.
- El apply puede competir con escrituras de la API si no existe un write-freeze demostrado.

Las **60 acciones son una estimación, no una cantidad fija**. Solo se consideran válidas si el plan recalculado coincide con el estado auditado y no existen índices adicionales, nombres diferentes, opciones incompatibles, conflictos ni acciones no previstas. Cualquier excepción de índices requiere un waiver autorizado conforme a la sección 5.4.

En las guardas implementadas, el waiver se registra en JSON con aprobador, vigencia, motivo y riesgo aceptado, y se vincula al `planFingerprint`. No hay firma criptográfica; la autorización sigue siendo un control humano.

Las guardas de código en `plan.js`, `verify.js`, `apply.js` e `init.js` están implementadas localmente y cubiertas por pruebas. La ejecución en producción sigue fuera de alcance hasta cumplir las precondiciones y obtener el Go/No-Go.

## 2. Estado actual según la auditoría

| Elemento | Estado |
|---|---:|
| Colecciones esperadas | 22 |
| Colecciones presentes | 2 (`users`, `sessions`) |
| Colecciones faltantes | 20 |
| Validadores diferentes | 2 (`users`, `sessions`) |
| Índices faltantes en colecciones existentes | 4 |
| Índices adicionales | 0 |
| Diferencias totales de `check_schema.py` | 26 |
| Errores de conexión, permisos o TLS en la auditoría | Ninguno |

La comparación de `check_schema.py` no verificó `validationLevel`, `validationAction` ni documentos. Tampoco demuestra que los validadores actuales y deseados sean equivalentes ni que los documentos existentes sean compatibles.

`db:verify:prod` tampoco es una prueba completa: su resultado `OK` depende de `plan.drift` y no convierte los índices adicionales en un fallo. Por eso la verificación de este plan exige una comparación manual y exacta de validadores, nombres de índices y opciones.

## 3. Riesgos reales del apply

| Riesgo | Qué puede ocurrir | Control obligatorio |
|---|---|---|
| TTL en `sessions.purgeAt` | El monitor TTL puede borrar sesiones con `purgeAt <= ahora`; la pérdida no se revierte con un rollback DDL | Contar fechas TTL, aprobar el impacto por escrito y no crear el TTL sin autorización expresa |
| Índice único de `users.email` | El build falla si hay duplicados, nulos o ausentes en conflicto | Auditar duplicados y valores ausentes/nulos; exigir cero conflictos o plan de datos aprobado |
| Índice único de `sessions.tokenHash` | El build falla si hay duplicados, nulos o ausentes en conflicto | Auditar duplicados y valores ausentes/nulos; exigir cero conflictos o plan de datos aprobado |
| `collMod` de validadores | Reemplaza el validador completo; puede debilitar restricciones o rechazar escrituras futuras | Comparar actual vs. deseado, aprobar el diff y demostrar compatibilidad de la API |
| Documentos existentes inválidos | `collMod` no valida documentos existentes; pueden quedar inválidos y fallar en futuras actualizaciones | Auditar documentos contra el `$jsonSchema`; no afirmar que `collMod` los valida |
| Escrituras concurrentes | La API puede escribir mientras se crean índices o se cambia el validador | Write-freeze demostrado y conteos antes/después |
| Índices adicionales, nombres u opciones incompatibles | `verify` podía dar `OK` con índices extra; `plan.js` podía aceptar un índice equivalente con otro nombre | Nombres y opciones exactos obligatorios; índices adicionales, nombres diferentes u opciones incompatibles bloquean por defecto. Solo un waiver autorizado del responsable puede aceptar una excepción de nombre o índice extra |
| Rollback insuficiente | No se pueden eliminar índices/colecciones nuevas sin privilegios de recuperación | Credencial break-glass separada y procedimiento de rollback probado en staging |
| Incompatibilidad de API | La versión desplegada puede escribir campos rechazados por el validador nuevo | La API debe demostrar compatibilidad en staging antes del apply |
| Cantidad de acciones asumida | El plan real puede tener más o menos de 60 acciones | Recalcular el plan y detenerse ante cualquier diferencia no explicada |

## 4. Precondiciones bloqueantes

No ejecutar `db:init:prod` hasta cumplir todo esto y registrar la evidencia correspondiente.

1. Go/No-Go firmado por el responsable autorizado, con nombre, rol, fecha y hora, ventana, alcance y enlaces a la evidencia. Sin ese registro no hay aprobación formal.
2. Confirmación de que el destino es el clúster correcto y la base `virgo`.
3. Credencial DDL de producción con permisos mínimos para `listCollections`, `listIndexes`, `createCollection`, `createIndexes` y `collMod`.
4. Credencial de recuperación separada, con permisos para `dropIndex`, `dropCollection` y restauración de datos; no se usa en runtime ni en el apply normal. La recuperación de documentos eliminados por TTL requiere un procedimiento de restauración probado y autorización separada.
5. Write-freeze aprobado y demostrado: definir el mecanismo concreto (pausar la API, bloquear endpoints de escritura o congelar escrituras a nivel de base), el responsable, la hora de inicio/fin, si se permiten lecturas y cómo se verifica la ausencia de escrituras. La evidencia debe demostrar que no hubo escrituras durante el apply.
6. **El respaldo debe tener una restauración probada** en un entorno aislado y autorizado. La prueba debe comparar colecciones, validadores, `validationLevel`, `validationAction`, índices y conteos, y registrar la ubicación y el checksum del respaldo. Si Atlas no ofrece snapshot en el tier actual, usar `mongodump` con credencial read-only. El respaldo debe tomarse con la API congelada o con un snapshot consistente.
7. Snapshot de los validadores actuales, `validationLevel`, `validationAction`, opciones de colección y definiciones completas de índices de `users` y `sessions`.
8. Confirmación de que la API desplegada o candidata escribe documentos compatibles con los validadores del catálogo. **La API debe demostrar compatibilidad en staging** con un entorno equivalente, datos de prueba, versión/commit identificados, pruebas de registro/login/logout/refresh/lockout y evidencia de cero errores de validación.
9. Revisión de Network Access y TLS; este plan no cambia ninguno de los dos.
10. Operador, aprobador, responsable de rollback y ventana de comunicación definidos.
11. Confirmación de que `virgo-api-prod` sigue siendo el usuario runtime con `readWrite` solo sobre `virgo`; el usuario DDL no se usa en runtime.
12. Confirmación de que **la base `virgo` no debe recibir datos DEMO en esta fase**.
13. Decisión TTL registrada: aprobación expresa del impacto, con número máximo aceptable de documentos a eliminar (o cero), responsable, fecha y evidencia. Sin esta decisión, `db:init:prod` queda bloqueado.

La credencial de solo lectura usada en la auditoría no sirve para aplicar y no debe reutilizarse para DDL. No crear `.env.production` con secretos dentro del repositorio; preferir inyección en el proceso o un archivo ignorado y eliminado al terminar.

## 5. Auditoría read-only previa

Toda esta fase es de solo lectura. No modificar documentos, validadores, índices, credenciales ni Network Access. No imprimir la URI ni secretos.

### 5.1 Inventario y plan recalculado

Desde `VirGo-Database/`:

```powershell
npm run db:plan:prod
npm run db:validate:prod
node src/cli/inspect.js --env-file .env.production --json
```

Para capturar el plan en JSON sin depender del formato visual:

```powershell
node src/cli/init.js --env-file .env.production --mode plan --json
```

`db:validate:prod` puede fallar antes del apply porque hay deriva; eso es esperado. El resultado debe registrarse y explicarse, no interpretarse como un fallo de conexión.

Revisar que el plan:

- No contenga acciones distintas de `createCollection`, `collMod` o `createIndex`.
- No contenga `indexConflict`.
- No contenga colecciones fuera del catálogo.
- No elimine colecciones, documentos ni índices.
- No cree índices TTL sin aprobación de impacto.
- Coincida con el inventario recalculado, no con una cifra asumida.

Matriz de referencia, condicionada al estado exacto:

| Acción | Estimación inicial |
|---|---:|
| `createCollection` | 20 |
| `collMod` | 2 |
| `createIndex` | 38 |
| Acciones totales | 60 |
| Conflictos de índices | 0 |
| Índices adicionales | 0 |

**Las 60 acciones son una estimación, no una cantidad fija.** Si el plan real difiere, detenerse, explicar la diferencia y recalcular antes de escribir.

### 5.2 Validadores actuales y deseados

Capturar la definición completa de cada colección existente, no solo el resultado de `inspect`:

```javascript
db.runCommand({
  listCollections: 1,
  filter: { name: { $in: ["users", "sessions"] } },
  nameOnly: false
})
```

Para cada colección, guardar:

- `options.validator`
- `options.validationLevel`
- `options.validationAction`
- opciones de colección relevantes

Comparar el validador actual con el deseado en `src/catalog/validators/accounts.js` y registrar:

| Colección | Validador actual | Validador deseado | Nivel/acción actual | Nivel/acción deseado | Restricciones que se agregan | Restricciones que se eliminan | Decisión |
|---|---|---|---|---|---|---|---|
| `users` | Capturar | Catálogo | Capturar | `strict`/`error` | Determinar | Determinar | Aprobar o bloquear |
| `sessions` | Capturar | Catálogo | Capturar | `strict`/`error` | Determinar | Determinar | Aprobar o bloquear |

Reglas:

- **No afirmar que `collMod` valida documentos existentes.** `collMod` cambia la regla de validación para escrituras futuras; no revalida ni repara documentos ya almacenados.
- Si el validador deseado elimina una restricción, agrega campos permitidos o elimina `additionalProperties: false`, tratarlo como posible debilitamiento y exigir aprobación explícita.
- `sessions` no declara `additionalProperties: false` en el catálogo. Si el validador actual sí lo tiene, el `collMod` lo eliminaría; no describir el cambio como un endurecimiento sin revisar el diff.
- Si el validador deseado es más estricto, auditar documentos existentes y demostrar que las escrituras de la API siguen siendo válidas.
- No aplicar `collMod` si el diff no fue revisado y aprobado.

### 5.3 Auditoría de documentos y datos

Ejecutar con una credencial read-only y guardar salidas sin secretos.

Conteos básicos:

```javascript
db.users.countDocuments({})
db.sessions.countDocuments({})
db.sessions.countDocuments({ purgeAt: { $lte: new Date() } })
db.sessions.countDocuments({ purgeAt: { $exists: false } })
db.sessions.countDocuments({ purgeAt: { $not: { $type: "date" } } })
```

Los conteos TTL anteriores no son mutuamente excluyentes. Si se suman, usar una consulta por categorías disjuntas y registrar la definición exacta de cada categoría.

Duplicados de `users.email`:

```javascript
db.users.aggregate([
  { $group: { _id: "$email", count: { $sum: 1 } } },
  { $match: { count: { $gt: 1 } } }
])
```

Duplicados case-insensitive de `users.email`:

```javascript
db.users.aggregate([
  { $group: { _id: { $toLower: "$email" }, count: { $sum: 1 } } },
  { $match: { count: { $gt: 1 } } }
])
```

El índice único de `users.email` no bloquea por sí solo los duplicados case-insensitive. Si existen, se consideran un conflicto de identidad y requieren una decisión de datos aprobada antes del Go/No-Go.

Valores ausentes o nulos que pueden conflictuar con el índice único:

```javascript
db.users.countDocuments({ $or: [{ email: { $exists: false } }, { email: null }] })
db.sessions.countDocuments({
  $or: [{ tokenHash: { $exists: false } }, { tokenHash: null }]
})
```

Duplicados de `sessions.tokenHash`:

```javascript
db.sessions.aggregate([
  { $group: { _id: "$tokenHash", count: { $sum: 1 } } },
  { $match: { count: { $gt: 1 } } }
])
```

Documentos que no cumplen el `$jsonSchema` del catálogo:

```javascript
db.users.countDocuments({
  $nor: [{ $jsonSchema: /* validator.$jsonSchema exacto de users */ }]
})

db.sessions.countDocuments({
  $nor: [{ $jsonSchema: /* validator.$jsonSchema exacto de sessions */ }]
})
```

La consulta anterior debe usar el `$jsonSchema` exacto del catálogo. No sustituirlo por una comprobación parcial de campos.

También registrar:

- Sesiones activas, expiradas y con `purgeAt` fuera de rango.
- `purgeAt` inválido, nulo o ausente.
- Campos que la API escribe y que no estén permitidos por el validador deseado.
- Cualquier documento que requiera una decisión de datos.

### 5.4 Inventario exacto de índices

Listar todos los índices de todas las colecciones existentes, con nombre, clave, `unique`, `expireAfterSeconds`, `partialFilterExpression` y opciones relevantes.

Reglas:

- Los nombres y opciones de los índices deben coincidir exactamente con el catálogo.
- Los índices adicionales, nombres diferentes u opciones incompatibles bloquean el Go/No-Go por defecto.
- La única excepción es un **waiver autorizado por el responsable**. El waiver debe identificar colección, índice, diferencia exacta (nombre o índice extra), justificación, riesgo, aprobador y vigencia; se vincula al `planFingerprint`.
- Un waiver solo cubre `extraIndex` o `nameMismatch`. No puede usarse para aceptar colecciones faltantes, validadores incorrectos, conflictos de clave u opciones, TTL ni diferencias no identificadas.
- `db:verify:prod` devuelve `ok: false` si hay índices extra o nombres distintos sin waiver. Con un waiver válido puede devolver `ok: true` y `exactMatch: false`; no es una coincidencia exacta.
- `plan.js` bloquea un nombre distinto hasta que exista un waiver que coincida exactamente con la diferencia; no lo acepta silenciosamente.
- `check_schema.py` compara clave y opciones, cuenta índices extra como diferencias y **no verifica los nombres exactos**. La revisión con `listIndexes` y el plan siguen siendo obligatorias.
- Si existe un waiver de índice adicional, `check_schema.py` puede seguir mostrando una diferencia por “sobra el índice”. Esa diferencia se registra como cubierta por el waiver solo si coincide exactamente con la excepción aprobada; no se interpreta como coincidencia exacta ni se convierte en un Go implícito.
- Si el waiver cubre un nombre distinto con la misma clave y opciones, `check_schema.py` puede no mostrar ninguna diferencia. El cierre debe apoyarse en la revisión de `listIndexes` y en el waiver, nunca en un “0 diferencias” que no evalúa nombres.
- Sin waiver, cualquier índice extra o nombre distinto es bloqueante.
- No eliminar índices adicionales en esta fase.

### 5.5 Compatibilidad de API en staging

Antes del apply, la API candidata o desplegada debe demostrar en staging que escribe documentos compatibles con los validadores deseados:

- Registrar, iniciar sesión, fallar login, bloquear cuenta, hacer logout/refresh y crear sesión.
- Verificar que `users` y `sessions` cumplen el `$jsonSchema`.
- Verificar que los campos escritos por `MongoAuthRepositories` y `LoginHandler` están permitidos.
- Confirmar que no hay errores de validación ni campos adicionales rechazados.
- Registrar versión, commit y resultados.

No usar producción para estas pruebas de escritura.

### 5.6 Criterios de parada de datos

Detenerse y no aplicar si:

- Hay duplicados que impedirían el índice único de `users.email` o `sessions.tokenHash`.
- Hay duplicados case-insensitive de `users.email` sin decisión de datos aprobada; el índice único no los bloquea necesariamente, pero afectan la identidad de usuario.
- Hay valores ausentes o nulos que conflictúan con un índice único y no existe decisión aprobada.
- Hay documentos existentes que no cumplen el validador y no existe un plan de datos aprobado.
- Hay documentos que la API escribe y el validador rechaza.
- Hay índices adicionales o nombres diferentes sin waiver autorizado, u opciones incompatibles.
- Existe un waiver, pero no cubre exactamente todas las diferencias detectadas.
- El conteo de documentos no se puede capturar.
- El respaldo no está verificado o su restauración no fue probada.
- El impacto del TTL no fue aprobado y no se puede excluir el TTL del apply.

## 6. Comparación explícita de validadores

Antes del Go/No-Go, guardar fuera del repositorio:

- Validador actual completo de `users` y `sessions`.
- `validationLevel` y `validationAction` actuales.
- Validador deseado del catálogo.
- Diff de propiedades, `required` y `additionalProperties`.
- Definiciones actuales de índices.
- Conteos de documentos.
- Ubicación del respaldo, sin credenciales.

La comparación debe responder por escrito:

1. ¿El validador deseado es igual, más estricto o más laxo?
2. ¿Se elimina alguna restricción existente?
3. ¿Se permite algún campo adicional que antes se rechazaba?
4. ¿Los documentos existentes cumplen el validador deseado?
5. ¿La API demuestra en staging que puede escribir con el validador deseado?
6. ¿El `collMod` conserva `validationLevel` y `validationAction` requeridos?
7. ¿El resultado incluye el valor actual y el deseado de `validationLevel` y `validationAction`?

El resultado de esta comparación debe incorporarse al Go/No-Go como evidencia verificable. `collMod` no valida documentos existentes. Si el validador es más estricto, los documentos inválidos pueden permanecer almacenados y fallar en futuras actualizaciones; eso requiere una decisión de datos separada.

No aplicar el `collMod` si el diff no fue aprobado.

## 7. Criterios de Go/No-Go

El apply solo puede comenzar con **Go** firmado. Un solo criterio en **No** bloquea la ejecución.

| Criterio | Evidencia requerida | Go/No-Go |
|---|---|---|
| Go/No-Go firmado | Nombre, rol, fecha/hora, ventana, alcance y enlaces a evidencia | |
| Destino correcto | Clúster y base `virgo` confirmados | |
| Credencial DDL | Permisos exactos y prueba de conexión | |
| Credencial de recuperación | `dropIndex`, `dropCollection` y restauración de datos disponibles | |
| Write-freeze | Mecanismo concreto, responsable y evidencia de ausencia de escrituras | |
| Respaldo | Backup verificado, checksum y restauración probada con comparación | |
| Validadores | Snapshot actual, diff aprobado y `validationLevel`/`validationAction` registrados | |
| Documentos existentes | Conteos y compatibilidad aprobados | |
| Duplicados | Cero conflictos o plan de datos aprobado | |
| TTL | Aprobación expresa con máximo de documentos a eliminar, responsable y fecha; o TTL excluido | |
| Índices | Nombres/opciones exactos; diferencias bloqueantes por defecto | |
| Waiver de índices | Firmado, con colección, índice, diferencia, justificación, riesgo, responsable y fecha; cubre exactamente todas las diferencias | |
| Plan recalculado | Coincide con el estado real y no hay acciones inesperadas | |
| API en staging | Entorno, versión/commit, pruebas de escritura y validación en verde | |
| DEMO | Confirmado que no se cargarán datos DEMO en `virgo` | |
| Rollback | Procedimiento probado y responsables definidos | |

Reglas del Go/No-Go:

- **Las 60 acciones son una estimación, no una cantidad fija.** Recalcular y explicar cualquier diferencia.
- **Los índices adicionales, nombres diferentes u opciones incompatibles bloquean por defecto.** Solo un waiver autorizado del responsable puede aceptar una excepción de nombre o índice extra.
- **No afirmar que existe coincidencia exacta si se está aceptando un waiver.** El cierre debe indicar “coincide salvo la excepción documentada en el waiver X”.
- **No ejecutar índices TTL hasta tener aprobación expresa del impacto.**
- **No ejecutar `db:init:prod` si no se ha autorizado el comportamiento TTL.**
- No ejecutar `db:init:prod` si existe cualquier bloqueante pendiente.
- No ejecutar `db:init:prod` si el plan contiene `indexConflict`, acciones no previstas o colecciones fuera del catálogo.
- No ejecutar `db:init:prod` sin write-freeze demostrado.
- No ejecutar `db:init:prod` si el respaldo no tiene restauración probada.
- No ejecutar `db:init:prod` si la API no demostró compatibilidad en staging.
- Un waiver de índices no autoriza TTL, no sustituye el Go/No-Go y no permite ignorar diferencias no listadas.

## 8. Secuencia de aplicación

### Fase 0 · Preparación

1. Obtener autorización para preparación y auditoría read-only; el Go/No-Go final se firma al cerrar la Fase 1.
2. Confirmar `NODE_ENV=production`, `MONGODB_DB=virgo` y TLS.
3. Inyectar la URI y credenciales sin imprimirlas ni versionarlas.
4. Activar el write-freeze con el mecanismo aprobado y verificar que no hay escrituras.
5. Ejecutar el respaldo con la API congelada y probar la restauración en un entorno autorizado.
6. Capturar el snapshot de validadores, índices y conteos después del write-freeze.

Mantener el write-freeze desde antes del respaldo hasta después de la verificación, salvo decisión expresa y documentada del responsable autorizado.

### Fase 1 · Auditoría read-only

1. Ejecutar el inventario y el plan recalculado.
2. Capturar validadores actuales y deseados.
3. Auditar documentos, duplicados, valores ausentes/nulos y fechas TTL.
4. Auditar nombres y opciones exactas de índices.
5. Ejecutar las pruebas de compatibilidad de API en staging.
6. Revisar y firmar el Go/No-Go.

### Fase 2 · Apply autorizado

Antes de cualquier comando, confirmar que no existe ningún bloqueante pendiente:

- Go/No-Go firmado y vigente.
- Write-freeze activo y demostrado.
- Respaldo con restauración probada.
- Auditoría de datos y validadores aprobada.
- API compatible en staging.
- Índices exactos o waiver autorizado que cubra todas las diferencias.
- Aprobación TTL registrada o TTL excluido del alcance.

**Bloqueo TTL:** `db:init:prod` está bloqueado si no existe una aprobación TTL específica vinculada al plan. La CLI aplica todas las acciones en un solo comando, incluidos los índices TTL, y no permite excluirlos selectivamente.

- **No ejecutar índices TTL hasta tener aprobación expresa del impacto.**
- **No usar `db:init:prod` si no se ha autorizado el comportamiento TTL.**
- Si el TTL no está autorizado, el apply queda bloqueado; prepare una aprobación TTL válida o use un procedimiento manual aprobado y documentado.
- No improvisar un apply parcial con `mongosh` en producción sin autorización y procedimiento documentados.
- Un waiver de índices no autoriza TTL.

**Ejecución condicional:** solo dentro de la ventana, con el Go/No-Go firmado y sin bloqueantes pendientes:

```powershell
npm run db:validate:prod -- --ttl-approval-file .\approvals\ttl.json --waiver-file .\approvals\waiver.json
npm run db:init:prod -- --yes --ttl-approval-file .\approvals\ttl.json --waiver-file .\approvals\waiver.json --plan-file .\approvals\plan.json
```

`db:init:prod` ya incluye `--confirm-production`; `--yes` confirma la aplicación. `--yes` y `--confirm-production` no sustituyen las aprobaciones. El comando no crea usuarios, no cambia Network Access y no carga DEMO. No ejecutarlo si falta cualquiera de las condiciones anteriores.

Si falla a mitad:

- No repetir a ciegas.
- Leer `Operaciones aplicadas antes del fallo`.
- Mantener el write-freeze y evaluar el estado.
- Corregir la causa.
- Volver a ejecutar la auditoría y `db:plan:prod`; el apply es idempotente, pero la decisión de reintentar requiere revisión.

### Fase 3 · Verificación

```powershell
npm run db:verify:prod -- --ttl-approval-file .\approvals\ttl.json --waiver-file .\approvals\waiver.json
npm run db:validate:prod -- --ttl-approval-file .\approvals\ttl.json --waiver-file .\approvals\waiver.json
npm run db:plan:prod -- --ttl-approval-file .\approvals\ttl.json --waiver-file .\approvals\waiver.json
node src/cli/inspect.js --env-file .env.production --json
```

Desde `VirGo-API/`, con la credencial de solo lectura:

```powershell
python db/tools/check_schema.py --live --db virgo
```

Verificación adicional obligatoria:

- Comparar validadores exactos con `listCollections`, incluidos `validationLevel` y `validationAction`; no confiar solo en `db:verify:prod`.
- Comparar nombres y opciones exactas de índices; confirmar que no hay índices adicionales, nombres diferentes ni opciones incompatibles.
- Comparar conteos de `users` y `sessions` con el snapshot previo tomado después del write-freeze.
- Cuantificar cualquier diferencia por TTL y confirmar que está dentro del impacto aprobado.
- Confirmar que no hay documentos DEMO.
- Ejecutar smoke tests de `/health` y lecturas permitidas.
- No ejecutar login/registro en producción durante la verificación salvo autorización específica; las pruebas de escritura se hacen en staging.
- Interpretar `check_schema.py` según la sección 5.4: no verifica nombres exactos y puede reportar índices extra cubiertos por un waiver.

`db:verify:prod` devolviendo `OK` no es suficiente por sí solo: no prueba documentos, no valida `validationLevel`/`validationAction` y no bloquea índices adicionales.

### Fase 4 · Cierre

- Registrar plan, auditoría, apply y verificación con fecha y hora.
- Actualizar la documentación de estado solo después de la ejecución autorizada.
- Confirmar que `virgo-api-prod` sigue siendo el usuario runtime y no tiene permisos DDL.
- Confirmar que la API responde `/health` y lecturas permitidas.
- Confirmar que **la base `virgo` no recibió datos DEMO**.
- Registrar incidentes, desviaciones y waivers firmados.

## 9. Verificación exacta, conteos y evidencia

La remediación no se considera verificada solo porque `db:verify:prod` devuelva `OK`. Se debe conservar:

- Salida de `db:plan:prod` antes y después, en JSON si es posible.
- Salida de `db:validate:prod` antes y después.
- Salida de `inspect` completa.
- Snapshot de validadores actuales y diff contra el catálogo.
- Inventario exacto de índices antes y después.
- Conteos de `users` y `sessions` antes y después.
- Conteo de documentos afectados por TTL y aprobación del impacto.
- Resultado de `db:init:prod`.
- Resultado de `db:verify:prod`.
- Resultado de `check_schema.py --live --db virgo`.
- Resultado de las pruebas de API en staging.
- Fecha, hora, operador, aprobador, ventana y versión del catálogo/scripts.
- Ubicación del respaldo y resultado de la restauración probada, sin credenciales.
- Waivers firmados por índices adicionales, nombres diferentes u opciones incompatibles, con colección, índice, diferencia, justificación, riesgo, responsable y fecha.
- Interpretación de `check_schema.py` cuando exista un waiver: diferencia exacta cubierta y confirmación de que no hay otras diferencias.

Criterio de cierre:

- `db:verify:prod` devuelve `OK`.
- `db:plan:prod` no tiene cambios pendientes.
- El plan recalculado no propone acciones pendientes y cualquier diferencia de índices está cubierta por un waiver autorizado.
- Validadores e índices coinciden, y cualquier excepción de índices está cubierta por un waiver autorizado. No se afirma coincidencia exacta si existe un waiver.
- `check_schema.py --live --db virgo` devuelve `22 colecciones, 0 diferencias`. Si un waiver de índice produce una diferencia esperada, el cierre registra `22 colecciones, N diferencias cubiertas por el waiver X` y no afirma “0 diferencias” ni coincidencia exacta.
- Los conteos de `users` y `sessions` coinciden con el snapshot tomado después del write-freeze o la diferencia TTL está aprobada.
- No hay documentos DEMO.
- La API productiva funciona con el usuario runtime.
- La evidencia fue revisada por una persona.

## 10. Rollback, RTO y RPO

Antes del apply, guardar fuera del repositorio:

- Validadores actuales de `users` y `sessions`.
- `validationLevel` y `validationAction` actuales.
- Definiciones completas de índices actuales.
- Conteos de documentos.
- Respaldo verificado y restauración probada.

Si el apply falla o la API empieza a rechazar escrituras:

1. Mantener el write-freeze, pausar la API o bloquear escrituras según el procedimiento aprobado.
2. Restaurar el validador, nivel y acción anteriores con `collMod`.
3. Si se creó un índice nuevo y causa problemas, eliminarlo solo con la credencial break-glass y autorización explícita.
4. Si se crearon colecciones vacías y se decide revertirlas, eliminarlas solo con autorización explícita y confirmación de que están vacías.
5. Si el TTL eliminó documentos no aprobados o hubo pérdida/corrupción, restaurar el respaldo en un entorno aislado y ejecutar la recuperación de datos con la credencial de restauración, autorización separada y procedimiento probado. Eliminar el índice TTL no recupera documentos.
6. Documentar el incidente y el resultado.

La CLI normal no elimina índices ni colecciones. El rollback de DDL requiere la credencial de recuperación y un procedimiento probado en staging. Eliminar un índice no recupera documentos; los documentos borrados por TTL solo pueden recuperarse desde un respaldo o mediante un procedimiento de recuperación de datos probado.

**RPO 0 solo aplica a operaciones DDL que no provocan eliminación de documentos.** La creación de índices TTL queda expresamente excluida. Un índice TTL puede provocar la eliminación de documentos vencidos después de su creación, incluso si el índice se elimina después.

Objetivos propuestos, sujetos a aprobación:

| Escenario | RTO objetivo | RPO objetivo |
|---|---|---|
| Rollback de validador | 60 minutos | 0 documentos; `collMod` no elimina documentos |
| Rollback de índice no TTL | 60 minutos | 0 documentos; `dropIndex` no elimina documentos ni recupera los ya borrados |
| Creación de índice TTL | 60 minutos para retirar el índice | **No aplicar RPO 0**; el TTL puede borrar documentos vencidos. RPO = último respaldo verificado o procedimiento de recuperación probado |
| Restauración completa por pérdida de datos | 4 horas | Último respaldo verificado; la frecuencia debe fijarse antes del apply |
| Sesiones eliminadas por TTL aprobado | No aplica rollback de datos | Pérdida intencional dentro del impacto aprobado |
| Sesiones eliminadas por TTL no aprobado | Según el restore disponible | Último respaldo verificado; requiere incidente, autorización de datos y procedimiento probado |

El RTO y el RPO definitivos deben ser aprobados por el responsable de producción antes del apply. La recuperación de documentos eliminados por TTL requiere restauración desde backup o un procedimiento de recuperación de datos probado; eliminar el índice TTL no recupera documentos.

## 11. Trazabilidad de hallazgos H-01 a H-11

| ID | Hallazgo | Control en este plan | Evidencia | Riesgo residual |
|---|---|---|---|---|
| H-01 | El TTL de `sessions` puede borrar documentos sin aprobación de impacto | Conteo de `purgeAt <= ahora`, aprobación expresa con máximo de documentos a eliminar y bloqueo del apply sin autorización TTL | Conteos, aprobación vinculada al plan y salida del plan | Pérdida irreversible salvo restauración desde backup o recuperación de datos probada |
| H-02 | No había write-freeze ni control de carreras con la API | Write-freeze obligatorio y conteos antes/después | Evidencia de pausa y ventana | Depende de la ejecución operativa |
| H-03 | Duplicados en índices únicos no estaban auditados | Auditoría de duplicados, nulos y ausentes; cero conflictos o plan de datos | Agregaciones y conteos | Los datos duplicados siguen requiriendo remediación separada |
| H-04 | `collMod` reemplaza el validador y podía debilitar restricciones; `sessions` no declara `additionalProperties: false` | Diff actual vs. deseado y aprobación explícita de debilitamientos | Snapshot, diff y aprobación | El validador deseado podría ser más laxo; requiere decisión humana |
| H-05 | No se contaban sesiones con `purgeAt <= ahora` ni fechas inválidas | Consultas específicas de TTL y aprobación de impacto | Conteos de fechas | El monitor TTL puede ejecutar después del apply |
| H-06 | `verify` ignoraba índices adicionales y `plan.js` podía aceptar nombres diferentes | Nombres/opciones exactos, extras bloqueantes por defecto, waiver autorizado con campos completos y `check_schema.py` interpretado según el waiver | Inventario exacto, waiver vinculado al plan y salida de `check_schema.py` | Guarda local implementada; la autorización del waiver sigue siendo un control humano |
| H-07 | `inspect` no muestra validadores y no permitía comparar nivel/acción | Captura completa con `listCollections` | Snapshot y diff | Control manual; no automatizado en esta fase |
| H-08 | El rollback no tenía credencial ni procedimiento exacto para eliminar índices/colecciones y recuperar datos | Credencial break-glass, rollback probado, RPO 0 excluido para TTL y recuperación de datos probada | Prueba de rollback y restore en staging | La pérdida por TTL solo se recupera desde backup o recuperación de datos probada; `dropIndex` no recupera documentos |
| H-09 | Las 60 acciones se trataban como cantidad fija | Recalcular el plan y detenerse ante diferencias | Plan JSON y explicación de diferencias | La cifra real depende del estado auditado |
| H-10 | No se demostraba compatibilidad de la API ni de documentos existentes | Pruebas de escritura en staging y auditoría contra `$jsonSchema` | Resultados de staging y conteos | Los documentos inválidos requieren decisión de datos |
| H-11 | `check_schema.py` no valida documentos, nivel/acción, nombres exactos ni opciones completas | Verificación exacta adicional, revisión manual de nombres y evidencia de validadores/índices | Snapshot, `listCollections`, `listIndexes`, conteos e interpretación del waiver | Cero diferencias en `check_schema.py` no es prueba total |

## 12. Fuera de alcance

- Modificar código de la API.
- Ejecutar cambios sobre la base de producción o Atlas; las guardas locales ya
  están implementadas.
- Crear usuarios de Atlas.
- Cambiar contraseñas o rotar credenciales.
- Cambiar Network Access.
- Cargar datos DEMO o datos de prueba en `virgo`.
- Migrar, reparar o borrar documentos.
- Eliminar índices adicionales.
- Desplegar la API.
- Hacer commit o push.

Las guardas locales ya detectan índices adicionales, nombres no exactos,
opciones incompatibles y TTL selectivo, y exigen aprobaciones vinculadas por
huella. Esto no cierra los hallazgos operativos: la auditoría read-only, el
respaldo con restore probado, el write-freeze, la compatibilidad en staging y el
Go/No-Go firmado siguen siendo obligatorios antes de producción.

## 13. Criterio final de autorización

El apply solo se autoriza cuando:

- El plan recalculado es comprensible y no contiene acciones inesperadas.
- La auditoría read-only está completa.
- El respaldo tiene restauración probada.
- El write-freeze está demostrado.
- La API demostró compatibilidad en staging.
- El impacto TTL está aprobado con `maxDeletion` suficiente y una aprobación
  TTL vinculada al `planFingerprint`, o el TTL está excluido.
- Los índices adicionales o nombres diferentes están cubiertos por un waiver
  vinculado al plan; las diferencias de clave u opciones están ausentes.
- `catalogFingerprint` y `planFingerprint` coinciden con el estado auditado.
- No queda ningún bloqueante pendiente.
- El Go/No-Go está firmado con nombre, rol, fecha/hora, ventana, alcance y evidencia.
- Se confirmó que `virgo` no recibirá datos DEMO.
- El cierre no afirma coincidencia exacta si existe un waiver; registra la excepción exacta y su aprobación.
