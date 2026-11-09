# Spec — 001 contrato-con-api

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Versión | 1.0.0 |
| Estado | Implementado, pendiente de revisión |
| Fecha | 2026-11-07 |
| Constitución | v1.0.0 |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

`VirGo-API/db/schema` era la fuente de verdad y se editaba por separado. Eso produjo divergencias en recuperación de contraseña, sesiones, usuarios, proveedores, zonas, enums e índices. El objetivo es invertir el flujo: `VirGo-Database/src/catalog` genera el contrato que consume la API.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **responsable de datos**, quiero **generar los esquemas de la API desde el catálogo** para **eliminar ediciones manuales divergentes**. |
| HT-02 | Como **desarrollador de la API**, quiero **un contrato estable y versionado** para **implementar sin ambigüedad**. |
| HT-03 | Como **QA**, quiero **un comando de verificación** para **detectar deriva antes de fusionar**. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | `db:export:api` genera 22 archivos en `VirGo-API/db/schema`. |
| RF-02 | Cada archivo contiene `collection`, `validationLevel`, `validationAction`, `validator` e `indexes`. |
| RF-03 | La salida es determinista: dos espacios, LF y salto final. |
| RF-04 | `db:export:api:check` no escribe y detecta archivos faltantes, distintos o extra. |
| RF-05 | `bookings` incluye `idempotencyKey` opcional de máximo 100 caracteres. |
| RF-06 | El índice `clientId_1_idempotencyKey_1_unique` es único y parcial. |
| RF-07 | `passwordResets` usa `codeHash` y `attempts`; `userId` es único. |
| RF-08 | `providers` usa `pricing` y `responseTime`; no existe `basePrice`. |
| RF-09 | `venues.allies[].category` usa `PROVIDER_CATEGORIES`; `zone` usa `ZONES`. |
| RF-10 | `monthlyStats._id` es determinista: `<targetType>:<targetId>:<AAAA-MM>`. |
| RF-11 | Los 22 esquemas pasan `check_schema.py` sin diferencias. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** el exportador no imprime secretos ni URIs. |
| RNF-02 | **Reproducibilidad:** mismo catálogo produce byte a byte el mismo JSON. |
| RNF-03 | **Trazabilidad:** los cambios quedan en Git como diff revisable. |
| RNF-04 | **Automatización:** CI ejecuta el chequeo de exportación y paridad. |
| RNF-05 | **Compatibilidad:** no requiere MongoDB para exportar. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el catálogo actual, **cuando** se ejecuta `db:export:api`, **entonces** se escriben 22 archivos. |
| CA-02 | **Dado** los archivos generados, **cuando** se ejecuta `db:export:api:check`, **entonces** no hay diferencias. |
| CA-03 | **Dado** un archivo alterado manualmente, **cuando** se ejecuta `--check`, **entonces** se reporta `different`. |
| CA-04 | **Dado** un archivo extra en el directorio, **entonces** se reporta `unexpected`. |
| CA-05 | **Dado** `check_schema.py`, **entonces** reporta `22 colecciones, 0 diferencias`. |
| CA-06 | **Dado** una reserva duplicada por cliente y clave, **entonces** MongoDB la rechaza. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Edición manual de la API | CI falla si el esquema generado difiere. |
| Reintentos de creación de reservas | Índice parcial único por cliente y clave. |
| Exposición de datos sensibles | El exportador solo copia el contrato; no lee documentos. |
| Deriva de enums | Pruebas comparan contra `src/catalog/enums.js`. |

## 7. Fuera de alcance

- Implementar endpoints o lógica de negocio en la API.
- Migrar datos existentes en Atlas.
- Decidir la pasarela de pagos.
- Definir el TTL de `contactMessages`.

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- La API leerá los JSON generados sin transformarlos.
- El orden de colecciones e índices del catálogo es estable.
- Los cambios de contrato requieren revisión humana.

**Dependencias**

- `VirGo-Database/src/catalog`.
- `VirGo-API/db/schema` como destino generado.
- `VirGo-API/db/tools/check_schema.py`.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿La API conservará algún archivo manual fuera de `db/schema`? | Mantenedores de API |
| P-02 | ¿Se aplicará el nuevo índice en `virgo_dev` antes del PR? | Responsable de datos |
