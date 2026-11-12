# Tareas — 001 contrato-con-api

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Versión | 1.0.0 |
| Estado | Aprobado e implementado; pendiente de ejecución de CI y revisión humana. |
| Fecha | 2026-11-07 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.0.0 |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

## F0 · Exportador

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T001 | Crear `src/export/schema.js`. | — | opencode | `tests/unit/export.test.js` | ✅ |
| T002 | Crear CLI y scripts npm de exportación/verificación. | T001 | opencode | `npm run db:export:api:check` | ✅ |

## F1 · Contrato e idempotencia

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T003 | Alinear campos, enums e índices con la API. | — | opencode | `check_schema.py` | ✅ |
| T004 | Generar los 22 esquemas de la API. | T001–T003 | opencode | `db:export:api` | ✅ |
| T005 | Probar escritura, `--check`, faltantes y extras. | T001 | opencode | `npm test` | ✅ |

## F2 · CI y handoff

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T006 | Agregar gate de contrato a `database-ci.yml`. | T002, T004 | opencode | Revisión del YAML; ejecución pendiente en PR | ✅ |
| T007 | Actualizar `API-HANDOFF.md` y regenerar ZIP. | T004 | opencode | `check_schema.py`; ZIP inspeccionado | ✅ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| HT-01 | T001, T002, T004 |
| HT-02 | T003, T004 |
| HT-03 | T005, T006 |
| RF-01 | T001, T004 |
| RF-02 | T001 |
| RF-03 | T001 |
| RF-04 | T001, T005 |
| RF-05 | T003, T004 |
| RF-06 | T003 |
| RF-07 | T003 |
| RF-08 | T003 |
| RF-09 | T003 |
| RF-10 | T003 |
| RF-11 | T004 |
| CA-01 | T004 |
| CA-02 | T002 |
| CA-03 | T005 |
| CA-04 | T005 |
| CA-05 | T003 |
| CA-06 | T003 |
