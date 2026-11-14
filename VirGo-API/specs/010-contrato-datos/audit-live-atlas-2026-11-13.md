# Auditoría live de Atlas — 010 contrato de datos

**Fecha de ejecución:** 2026-11-13
**Alcance:** validación de solo lectura de `virgo_dev` y `virgo` contra `VirGo-API/db/schema/`, para T1004.
**Restricciones respetadas:** sin inicializadores, sin DDL, sin seeds, sin CRUD, sin usuarios, sin Network Access, sin commits ni push.

## 1. Seguridad de la ejecución

- Script: `VirGo-API/db/tools/check_schema.py`.
- Operaciones live observadas:
  - `listCollections`
  - `listIndexes`
- No se detectaron operaciones de escritura, creación, modificación ni eliminación.
- La selección de base se hizo con `--db virgo_dev` y `--db virgo`.
- La URI no se incluye en este informe ni se guardó en archivos.
- Se usó una identidad de solo lectura; no se usó usuario administrador ni usuario DDL.
- TLS:
  - URI `mongodb+srv://` con `tls=true`.
  - Sin `tlsInsecure`, `tlsAllowInvalidCertificates`, `tlsAllowInvalidHostnames` ni equivalentes.
  - PyMongo 4.11.2 construyó el cliente con `tls=True`.
  - `tlsInsecure`, `tlsAllowInvalidCertificates` y `tlsAllowInvalidHostnames` quedaron sin override (`None`), por lo que aplican los valores seguros por defecto del driver.
- El script no genera archivos de resultados; solo imprime por `stdout` y devuelve código de salida.
- No se ejecutaron operaciones remotas distintas de las dos validaciones solicitadas.

## 2. Resultado `virgo_dev`

| Campo | Valor |
|---|---|
| Fecha y hora | `2026-11-13T00:15:41.9979530-06:00` |
| Base | `virgo_dev` |
| Colecciones esperadas | 22 |
| Colecciones encontradas | 22 |
| Diferencias de validador | 0 |
| Índices faltantes | 0 |
| Índices adicionales | 0 |
| Opciones de índice diferentes | 0 reportadas |
| Errores de conexión/permisos/TLS | Ninguno |
| Código de salida | 0 |
| Resultado final | **22 colecciones, 0 diferencias** |

Salida:

```text
22 colecciones, 0 diferencias
```

## 3. Resultado `virgo`

| Campo | Valor |
|---|---|
| Fecha y hora | `2026-11-13T00:16:02.3247525-06:00` |
| Base | `virgo` |
| Colecciones esperadas | 22 |
| Colecciones encontradas | 2 (`users`, `sessions`) |
| Colecciones faltantes | 20 |
| Diferencias de validador | 2 (`users`, `sessions`) |
| Índices faltantes | 4 |
| Índices adicionales | 0 |
| Opciones de índice diferentes | No se reportaron diferencias separadas; las opciones aparecen dentro de los índices faltantes |
| Errores de conexión/permisos/TLS | Ninguno |
| Código de salida | 1 |
| Resultado final | **22 colecciones, 26 diferencias** |

Salida:

```text
assistantMessages: falta en la base
bookings: falta en la base
contactMessages: falta en la base
conversations: falta en la base
counters: falta en la base
eventGuests: falta en la base
eventTasks: falta en la base
events: falta en la base
favorites: falta en la base
gigs: falta en la base
messages: falta en la base
monthlyStats: falta en la base
notices: falta en la base
passwordResets: falta en la base
paymentCards: falta en la base
providers: falta en la base
quotes: falta en la base
reviews: falta en la base
sessions: el validador difiere
sessions: falta el índice {"expireAfterSeconds": 0, "key": {"purgeAt": 1}}
sessions: falta el índice {"key": {"tokenHash": 1}, "unique": true}
sessions: falta el índice {"key": {"userId": 1}}
users: el validador difiere
users: falta el índice {"key": {"email": 1}, "unique": true}
venueDates: falta en la base
venues: falta en la base
22 colecciones, 26 diferencias
```

## 4. Comparación contra el catálogo local

La comprobación se hizo contra `VirGo-API/db/schema/`, que es la copia generada desde `VirGo-Database/src/catalog/`.

Evidencia local:

```text
npm run db:export:api:check
Esquemas actualizados: 22 archivos.

python db/tools/check_schema.py
22 colecciones, 0 diferencias
exit_code=0
```

Interpretación:

- `virgo_dev` coincide con el catálogo local según las comprobaciones del script.
- `virgo` no coincide: faltan 20 colecciones, `users` y `sessions` tienen validadores diferentes y faltan 4 índices.
- No se reportaron índices adicionales.
- No se reportaron diferencias separadas de opciones fuera de los índices faltantes.

## 5. Clasificación de diferencias en `virgo`

### Colecciones faltantes (20)

`assistantMessages`, `bookings`, `contactMessages`, `conversations`, `counters`, `eventGuests`, `eventTasks`, `events`, `favorites`, `gigs`, `messages`, `monthlyStats`, `notices`, `passwordResets`, `paymentCards`, `providers`, `quotes`, `reviews`, `venueDates`, `venues`.

### Validadores diferentes (2)

- `users`
- `sessions`

### Índices faltantes (4)

- `sessions`: `{ "purgeAt": 1 }` con `expireAfterSeconds: 0`
- `sessions`: `{ "tokenHash": 1 }` único
- `sessions`: `{ "userId": 1 }`
- `users`: `{ "email": 1 }` único

### Índices adicionales

Ninguno reportado.

### Opciones diferentes

No se reportaron como categoría separada. Las opciones conocidas por el script aparecen en los índices faltantes.

## 6. Limitaciones del script

`check_schema.py` no verifica:

- `validationLevel`
- `validationAction`
- documentos almacenados
- todas las opciones de colección
- todas las opciones avanzadas de índices, como `collation`, `hidden`, `2dsphereIndexVersion` o `wildcardProjection`
- el nombre real de los índices: compara clave y opciones, no exigir el mismo nombre
- el índice `_id_`
- que la URI use TLS o que el certificado sea válido; eso depende de PyMongo y de la URI

Por tanto:

- **Cero diferencias en `virgo_dev` no demuestra por sí solo** que `validationLevel`, `validationAction` o los documentos sean correctos.
- Para esas garantías siguen siendo necesarias las comprobaciones de `db:verify:dev` y las pruebas de integración correspondientes.

## 7. Recomendaciones para T1004

1. **T1004 para `virgo_dev`:** la validación live reportó 0 diferencias y código de salida 0. Esta es la evidencia de CA-02 solicitada por el spec 010, con la limitación indicada en la sección 6.
2. **Registrar la evidencia** en `VirGo-API/specs/010-contrato-datos/check.md` y actualizar el estado de T1004 cuando se autorice modificar la documentación del spec.
3. **No cerrar T1004 como verificación total del esquema:** complementar con `db:verify:dev` y con la evidencia de T1006 para validadores reales y límites de DTO.
4. **`virgo` no está listo para el catálogo:** faltan 20 colecciones, 2 validadores y 4 índices. No aplicar cambios sin autorización de producción, respaldo y ventana de cambio.
5. **No ejecutar seed DEMO en `virgo`.** La carga de datos demostrativos en producción sigue pendiente de aprobación expresa.
6. **T1005, T1006 y T1007 siguen pendientes.** T1006 requiere pruebas de repositorios con Testcontainers y validadores reales; no existe todavía esa infraestructura de pruebas.

## 8. Estado de comprobaciones pendientes

| Elemento | Estado |
|---|---|
| `virgo_dev` contra catálogo | 0 diferencias según `check_schema.py` |
| Validadores de `virgo_dev` (`validationLevel`/`validationAction`) | No verificado por este script |
| Documentos de `virgo_dev` | No verificados por este script |
| `virgo` contra catálogo | 26 diferencias |
| T1004 | Evidencia live de dev obtenida; cierre formal pendiente de registro |
| T1006 | Pendiente; sin Testcontainers ni pruebas de repositorios contra validadores reales |
| T1005 | Pendiente |
| T1007 | Pendiente |

## 9. Conclusión

- `virgo_dev` cumple la comparación de colecciones, validadores e índices que realiza `check_schema.py`.
- `virgo` no cumple: está incompleta frente al catálogo.
- No hubo errores de conexión, permisos ni TLS.
- No se realizó ninguna escritura remota.
- No se modificó ningún archivo de código.
