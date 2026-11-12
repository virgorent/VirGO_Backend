# Spec — 000 base-datos

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Versión | 1.0.0 |
| Estado | Implementado, pendiente de revisión |
| Fecha | 2026-11-07 |
| Constitución | v1.0.0 |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

`VirGo-Database` declara el contrato MongoDB de VirGo, pero el catálogo y los esquemas consumidos por la API habían divergido. El objetivo es que el catálogo sea la única fuente de verdad, que la API consuma esquemas generados y que la paridad quede protegida por pruebas y CI.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **responsable de datos**, quiero **declarar colecciones, validadores e índices en un solo catálogo** para **evitar contratos duplicados**. |
| HT-02 | Como **desarrollador de la API**, quiero **esquemas generados desde el catálogo** para **implementar sin interpretar fuentes contradictorias**. |
| HT-03 | Como **QA**, quiero **pruebas unitarias e integración reproducibles** para **verificar el contrato antes de desplegar**. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Declarar exactamente 22 colecciones con validadores `$jsonSchema`. |
| RF-02 | Aplicar `validationLevel: strict` y `validationAction: error` en todas las colecciones. |
| RF-03 | Declarar 38 índices secundarios: 12 únicos, 2 parciales, 4 TTL y 1 geoespacial. |
| RF-04 | Mantener la inicialización idempotente: `plan`, `validate`, `apply`, `verify` e `inspect` sin operaciones destructivas. |
| RF-05 | Exportar los 22 esquemas de la API desde el catálogo de forma determinista. |
| RF-06 | Hacer cumplir `bookings.idempotencyKey` por cliente mediante índice parcial. |
| RF-07 | Aplicar `maxLength` a todos los strings libres del contrato. |
| RF-08 | Mantener alineados enums, campos obligatorios e índices con `VirGo-API/db/schema`. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** sin secretos en código, logs o artefactos; URI no se imprime. |
| RNF-02 | **Confiabilidad:** la segunda aplicación del plan no produce cambios. |
| RNF-03 | **Compatibilidad:** Node.js 20.19 o superior y MongoDB 8. |
| RNF-04 | **Trazabilidad:** cada cambio de contrato tiene spec, tarea y evidencia de prueba. |
| RNF-05 | **Automatización:** CI ejecuta pruebas, verificación de exportación y paridad con la API. |
| RNF-06 | **Portabilidad:** JSON generado con LF, dos espacios y salto final. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el catálogo actual, **cuando** se ejecuta `npm test`, **entonces** las 45 pruebas unitarias pasan. |
| CA-02 | **Dado** MongoDB local, **cuando** se ejecuta `npm run test:integration`, **entonces** las 28 pruebas pasan. |
| CA-03 | **Dado** el catálogo, **cuando** se ejecuta `npm run db:export:api:check`, **entonces** no hay diferencias. |
| CA-04 | **Dado** los esquemas de la API, **cuando** se ejecuta `check_schema.py`, **entonces** reporta `22 colecciones, 0 diferencias`. |
| CA-05 | **Dado** un `booking` repetido con el mismo `clientId` e `idempotencyKey`, **entonces** MongoDB rechaza el duplicado. |
| CA-06 | **Dado** un segundo `apply`, **entonces** no se ejecutan operaciones. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Fuga de secretos | Sin secretos en el repositorio; Gitleaks en CI; URI fuera de logs. |
| Escritura accidental en producción | Guardas por entorno y `--confirm-production`; `apply` exige `--yes`. |
| Deriva de esquema | `plan`/`validate` de solo lectura y comparación de índices. |
| Abuso de idempotencia | Índice único parcial por `clientId` e `idempotencyKey`. |
| Datos sensibles | Hash de contraseña y tokens; no se almacenan PAN ni CVV. |

## 7. Fuera de alcance

- Implementar endpoints, pagos, correo o lógica de negocio.
- Modificar Atlas o producción.
- Crear datos de negocio.
- Definir políticas de contraseña, retención o pasarela.

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- `VirGo-API` y `VirGo-Database` viven en el mismo monorepo.
- La API acepta que su carpeta `db/schema` sea generada por este proyecto.
- Los cambios de contrato se revisan antes de un PR.

**Dependencias**

- `src/catalog` como fuente de verdad.
- MongoDB 8 local o servicio CI para integración.
- `VirGo-API/db/tools/check_schema.py` para validar límites de strings.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿La generación de esquemas se ejecutará en CI o solo se verificará? | Mantenedores |
| P-02 | ¿Se requiere aplicar el nuevo índice en `virgo_dev` antes de fusionar? | Responsable de datos |
