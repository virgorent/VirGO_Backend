# Tareas — 000 base-datos

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Versión | 1.0.0 |
| Estado | Aprobado e implementado; pendiente de revisión final. |
| Fecha | 2026-11-07 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.0.0 |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**[P]** = puede ejecutarse en paralelo con otras [P] de la misma fase.

## F0 · Catálogo y contrato

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T001 | Alinear enums, campos y límites con la API. | — | opencode | `npm test` | ✅ |
| T002 | Declarar índices nuevos y conteos aprobados. | T001 | opencode | Prueba de índices | ✅ |
| T003 | Agregar validadores para `passwordResets`, `sessions` y campos nuevos. | T001 | opencode | `tests/integration` | ✅ |

## F1 · Exportación y API

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T004 | Crear `src/export/schema.js` y `src/cli/export.js`. | T001 | opencode | `tests/unit/export.test.js` | ✅ |
| T005 | Regenerar `VirGo-API/db/schema` y verificar paridad. | T004 | opencode | `db:export:api:check`; `check_schema.py` | ✅ |

## F2 · Pruebas, CI y documentación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T006 | Actualizar fixtures y pruebas de integración. | T001–T003 | opencode | `npm run test:integration` | ✅ |
| T007 | Agregar pruebas unitarias del exportador y del catálogo. | T004 | opencode | `npm test` | ✅ |
| T008 | Crear workflow `.github/workflows/database-ci.yml`. | T004 | opencode | Revisión del YAML; ejecución pendiente en PR | ✅ |
| T009 | Actualizar README, docs y `API-HANDOFF.md`. | T005 | opencode | Revisión de enlaces y conteos | ✅ |
| T010 | Regenerar `API-HANDOFF.zip`. | T009 | opencode | Inspección del ZIP: solo `API-HANDOFF.md` | ✅ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| HT-01 | T001, T002, T003 |
| HT-02 | T004, T005 |
| HT-03 | T006, T007 |
| RF-01 | T001 |
| RF-02 | T001 |
| RF-03 | T002 |
| RF-04 | T003 |
| RF-05 | T004, T005 |
| RF-06 | T002, T006 |
| RF-07 | T001 |
| RF-08 | T005 |
| RNF-05 | T008 |
| CA-01 | T007 |
| CA-02 | T006 |
| CA-03 | T005 |
| CA-04 | T005 |
| CA-05 | T006 |
| CA-06 | T006 |
