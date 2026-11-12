# Plan — 000 base-datos

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Versión | 1.0.0 |
| Estado | Implementado, pendiente de revisión |
| Fecha | 2026-11-07 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.0.0 |

## 1. Resumen técnico

- Mantener el catálogo en `src/catalog` con 22 colecciones, validadores por dominio, enums compartidos e índices declarativos.
- Aplicar el esquema con el inicializador idempotente existente, sin operaciones destructivas.
- Exportar los esquemas de la API desde el catálogo con `src/export/schema.js`.
- Alinear campos, enums, límites de strings e índices con la API.
- Verificar con pruebas unitarias, integración local, `--check` del exportador y `check_schema.py`.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | Node.js 20.19+, ESM, MongoDB Driver 7. |
| Persistencia | MongoDB 8; validadores `$jsonSchema` e índices declarativos. |
| Seguridad | Sin secretos versionados; URI fuera de logs; guardas de entorno. |
| CI/CD | GitHub Actions con MongoDB 8 como servicio y Node.js 22. |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| 2 | Principios no negociables | ✅ | Spec, plan, tareas y verificación versionados. |
| 4 | Arquitectura | ✅ | Separación catálogo, inicialización, CLI y exportación. |
| 6 | Seguridad y DevSecOps | ✅ | Sin secretos, permisos mínimos y Gitleaks existente. |
| 7 | Testing y QA | ✅ | 45 pruebas unitarias y 28 de integración. |
| 8 | CI y GitHub | ✅ | Workflow `database-ci.yml` planificado para PR y `main`. |
| 10 | Gobernanza SDD | ✅ | Cambios trazados en este spec y en `docs/DECISIONS.md`. |

## 4. Estructura

```text
VirGo-Database/
  src/
    catalog/          validadores, enums, helpers e índices
    cli/              init, verify, inspect y export
    export/           generación de esquemas para la API
    init/             plan, apply, verify e inspect
  tests/
    unit/             catálogo, formato, plan y exportador
    integration/      MongoDB local e idempotencia
  specs/
    000-base-datos/
    001-contrato-con-api/
  docs/
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-01 | `src/catalog` es la fuente de verdad. | Mantener dos fuentes manuales. | Evita divergencia entre API y base. |
| ADR-02 | Exportar JSON determinista con LF. | Generar bajo demanda sin archivos versionados. | Permite revisar diffs y consumir en la API. |
| ADR-03 | `bookings.idempotencyKey` opcional con índice parcial único por cliente. | Colección separada de idempotencia. | Menor complejidad y garantía atómica. |
| ADR-04 | `maxLength` obligatorio en strings libres. | Confiar solo en validación de la API. | La base también protege el contrato. |
| ADR-05 | Índices adicionales solo generan advertencia. | Eliminarlos automáticamente. | Evita operaciones destructivas. |

## 6. Diseño

### 6.1 Catálogo

Cada colección expone `name`, `validator` e `indexes`. Los validadores se agrupan por dominio y los enums compartidos viven en `src/catalog/enums.js`.

### 6.2 Inicialización

`buildPlan` calcula colecciones faltantes, validadores desactualizados, índices faltantes, conflictos y extras. `applyPlan` solo ejecuta el plan aprobado. `verifyInitialization` confirma el estado.

### 6.3 Exportación

`buildApiSchemas` transforma el catálogo al contrato de la API. `writeApiSchemas` escribe 22 archivos; `checkApiSchemas` detecta faltantes, diferencias y archivos extra.

### 6.4 Seguridad

- No se imprimen URIs ni credenciales.
- `apply` exige `--yes`; producción exige `--confirm-production`.
- No se ejecutan `drop`, `delete` ni `dropIndexes`.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| Secretos | Gitleaks | PR y push a `main` |
| Dependencias | Dependabot + dependency review | Semanal y PR |
| Pruebas unitarias | `npm test` | PR y push |
| Integración | MongoDB 8 + `npm run test:integration` | PR y push |
| Contrato | `db:export:api:check` + `check_schema.py` | PR y push |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `tests/unit` | Catálogo, enums, índices, exportador, formato y plan. |
| Integración | `tests/integration` | 22 colecciones, idempotencia, validadores, índices y TTL. |
| Seguridad | CI | Gitleaks, permisos mínimos y ausencia de secretos. |
| Humo | CLI | `plan`, `apply`, `verify` e `inspect` sobre MongoDB local. |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Diffs inesperados en API | Exportador determinista y `--check` en CI. |
| Índice duplicado en datos existentes | Plan bloquea conflictos; limpieza manual revisada. |
| Deriva entre documentación y catálogo | Pruebas y actualización de `API-HANDOFF.md`. |
| Pérdida de cambios locales | No se ejecutan comandos Git destructivos. |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Catálogo, validadores, enums e índices | T001–T003 |
| F1 | Exportador y paridad con la API | T004–T005 |
| F2 | Pruebas, CI y documentación | T006–T010 |
