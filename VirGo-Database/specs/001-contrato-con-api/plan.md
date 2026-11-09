# Plan — 001 contrato-con-api

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Versión | 1.0.0 |
| Estado | Implementado, pendiente de revisión |
| Fecha | 2026-11-07 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.0.0 |

## 1. Resumen técnico

- Crear `src/export/schema.js` para transformar el catálogo al contrato de la API.
- Crear `src/cli/export.js` con escritura y verificación (`--check`).
- Agregar `db:export:api` y `db:export:api:check` a `package.json`.
- Generar `VirGo-API/db/schema/*.json` y validar con `check_schema.py`.
- Alinear campos e índices con la API y proteger la paridad con pruebas y CI.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | Node.js 20.19+, ESM. |
| Persistencia | No requiere conexión a MongoDB para exportar. |
| Seguridad | Sin secretos; solo lee el catálogo local. |
| CI/CD | `database-ci.yml` ejecutará `db:export:api:check` y `check_schema.py`. |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| 2 | Principios no negociables | ✅ | La fuente de verdad queda en un solo lugar. |
| 4 | Arquitectura | ✅ | Exportador separado del catálogo y del inicializador. |
| 6 | Seguridad y DevSecOps | ✅ | Sin secretos y con verificación en CI. |
| 7 | Testing y QA | ✅ | Pruebas del exportador y de paridad. |
| 8 | CI y GitHub | ✅ | Gate de contrato en PR y `main`. |
| 10 | Gobernanza SDD | ✅ | Spec y decisión documentados. |

## 4. Estructura

```text
VirGo-Database/
  src/
    export/schema.js
    cli/export.js
  tests/unit/export.test.js
  specs/001-contrato-con-api/
VirGo-API/db/
  schema/*.json          generado
  tools/check_schema.py  verificación
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-01 | Catálogo → JSON de API. | Editar ambos manualmente. | Elimina deriva. |
| ADR-02 | JSON con LF y dos espacios. | Formato dependiente de la plataforma. | Diffs estables en CI. |
| ADR-03 | `--check` compara texto normalizado. | Comparar solo JSON parseado. | Detecta también formato y orden. |
| ADR-04 | `idempotencyKey` en `bookings`. | Colección de idempotencia. | Menos piezas y atomicidad nativa. |
| ADR-05 | Script dedicado `db:export:api:check`. | Depender de `npm run ... -- --check`. | Evita diferencias de reenvío de flags en Windows. |

## 6. Diseño

### 6.1 Exportador

`toApiSchema` conserva el validador y los índices del catálogo. `buildApiSchemas` produce 22 objetos. `serializeApiSchema` garantiza formato estable.

### 6.2 Verificación

`checkApiSchemas` compara cada archivo esperado, detecta faltantes y reporta JSON extra. No escribe archivos.

### 6.3 Idempotencia de reservas

`bookings.idempotencyKey` es opcional. El índice único parcial por `clientId` e `idempotencyKey` solo indexa documentos con clave string.

### 6.4 Seguridad

El exportador solo lee `src/catalog` y escribe en `VirGo-API/db/schema`. No conecta a MongoDB ni usa credenciales.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| Secretos | Gitleaks | PR y push a `main` |
| Dependencias | Dependabot + dependency review | Semanal y PR |
| Pruebas | `npm test` | PR y push |
| Contrato | `npm run db:export:api:check` | PR y push |
| Paridad API | `check_schema.py` | PR y push |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `tests/unit/export.test.js` | Serialización, validación, escritura y `--check`. |
| Integración | `tests/integration` | Índice de idempotencia y contrato aplicado. |
| Contrato | `check_schema.py` | Strings sin `maxLength` y paridad de archivos. |
| Humo | CLI | Exportación y verificación sobre el monorepo. |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Sobrescribir cambios manuales en la API | Revisar `git diff`; la API deja de editarse a mano. |
| Flags npm no reenviados en Windows | Script `db:export:api:check`. |
| Diffs por formato | Serialización determinista y comparación normalizada. |
| Índice único con datos previos | Plan bloquea conflictos; limpieza manual. |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Exportador y CLI | T001–T002 |
| F1 | Contrato e idempotencia | T003–T004 |
| F2 | Pruebas, CI y handoff | T005–T007 |
