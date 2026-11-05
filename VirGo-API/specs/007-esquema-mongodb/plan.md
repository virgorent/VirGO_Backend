# Plan — 007 Esquema de MongoDB

| Campo | Valor |
|---|---|
| Módulo | `007-esquema-mongodb` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-04 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.1.0 |

## 1. Resumen técnico

`VirGo-API/db/schema/<coleccion>.json` guarda el validador y los índices de cada colección. Se exportó de `virgo_dev` y se corrigió contra el modelo. `db/tools/check_schema.py` comprueba los límites sin credenciales y, con `--live`, compara contra la base. `MongoIndexInitializer` (`000`: T019) aplica esos mismos archivos al arrancar la API.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Fuente de verdad | `VirGo-API/db/schema/*.json` |
| Formato | `collection`, `validationLevel`, `validationAction`, `validator`, `indexes` |
| Herramienta | Python 3 con `pymongo` solo para `--live` |
| Credencial | Variable `VIRGO_MONGO_URI`; nunca en archivos |
| Clúster | `virgo-dev`; bases `virgo_dev` y `virgo` |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| I | SDD | ✅ Cumple | Spec atómico con cuatro archivos y tareas `T7NN`. |
| II | Modularidad | ✅ Cumple | Sin endpoints; solo la estructura de datos. |
| III | Arquitectura | ✅ Cumple | La API crea e idempotentemente actualiza el esquema desde `db/schema/`. |
| IV | Contrato | ✅ Cumple | Los límites coinciden con los DTO. |
| V | Seguridad | ⚠️ Justificado | Las longitudes cumplen V.5. El usuario administrador queda solo para administrar; T700 crea usuarios de mínimo privilegio para el trabajo diario. |
| VI | Pruebas | ✅ Cumple | Comprobación en el CI y pruebas de rechazo. |
| VII | Costo cero | ✅ Cumple | Sin recursos nuevos. |
| VIII | CI/CD | ✅ Cumple | La comprobación sin credenciales corre en cada PR. |
| IX | Observabilidad | ➖ No aplica | |
| X | Memoria | ✅ Cumple | Las decisiones se registran en `memoria.md`. |
| XI | Agentes y MCP | ✅ Cumple | Se verifica con el MCP `mongodb` en solo lectura. |
| XII | Idioma | ✅ Cumple | Campos en inglés; documentación en español. |

## 4. Estructura

```text
VirGo-API/db/
├── schema/            # 22 archivos, uno por colección
└── tools/
    └── check_schema.py
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-27 | **`db/schema/` como fuente de verdad** | El modelo en Markdown; la base en vivo | Es lo único que una máquina puede comparar y aplicar |
| ADR-28 | **`_id` y referencias como texto UUID** | UUID binario | Ya estaba así; se lee directo en herramientas y JSON. En .NET se mapea con `BsonRepresentation(BsonType.String)` |
| ADR-29 | `monthlyStats._id` determinista: `<targetType>:<targetId>:<AAAA-MM>` | `_id` generado | El `upsert` con `$inc` no puede generar un UUID válido por sí solo |

## 6. Diseño

### 6.1 Correcciones aplicadas a los archivos

| Colección | Cambio frente a la base |
|---|---|
| `users` | Se agregan `failedLoginCount`, `lockoutEndsAt`, `unreadNotices`, `city` y `defaultGuests` |
| `sessions` | Se agregan `device` y `revokedAt` |
| `passwordResets` | `codeHash` y `attempts` en lugar de `tokenHash` y `used`; `userId` único |
| `bookings` | Índice por `clientId` y `date` en lugar de `createdAt` |
| `monthlyStats` | `_id` determinista (ADR-29) |
| `venues` | Zonas del modelo; `amenities` y `allies` opcionales |
| `providers` | Categorías del modelo; `pricing` y `responseTime` en lugar de `basePrice`; `gallery` opcional |
| `events`, `quotes` | `XV años` en `eventType` |
| Todas | `maxLength` en cada campo de texto libre |

Se conservan dos mejoras que traía la base: una sola tarjeta predeterminada por usuario y una conversación única por cliente y destino.

### 6.2 Aplicación

Como la base está vacía, `collMod` actualiza cada validador y los dos índices cambiados se recrean. Se ejecuta solo con las preguntas P-01 a P-05 confirmadas (T704).

## 7. Costo

$0: sin recursos nuevos.

## 8. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Estática | `check_schema.py` en cada PR (CA-01, CA-05) |
| Integración | `check_schema.py --live` contra `virgo_dev` (CA-02) |
| Seguridad | Insertar un documento con 1500 caracteres en un campo limitado: rechazado (CA-03) |
| Unitarias, mutación y humo | ➖ No hay código de la aplicación en este módulo |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| El esquema vuelve a cambiarse a mano en Atlas | `--live` en T704 y al cerrar cada módulo; usuarios sin `dbAdmin` para el trabajo diario |
| Las longitudes propuestas no sirven al negocio | P-05; cambiarlas es editar un archivo |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Usuarios y decisiones | T700–T701 |
| F1 | Esquema en el repositorio | T702–T703 |
| F2 | Aplicación y cierre | T704–T706 |
