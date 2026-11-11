# Tareas — 000 Base de datos MongoDB

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión del equipo |
| Fecha | 2026-11-05 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

---

## F0 · Lo ya construido

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T001 | Catálogo de 22 colecciones, validadores e índices | — | Esau | Los validadores de `virgo_dev` coinciden 22 de 22 con el catálogo (CA-01) | ✅ |
| T002 | Inicializador con `plan`, `validate` y `apply` | T001 | Esau | `README.md`; falta evidencia de ejecución en `check.md` | 🔄 |
| T003 | Verificador e `inspect` de solo lectura | T001 | Esau | Falta evidencia de ejecución en `check.md` | 🔄 |
| T004 | Documentación y `API-HANDOFF.md` | T001 | Esau | Archivos en `docs/` | ✅ |

## F1 · Regularización

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T005 | Registrar en `check.md` la salida de `npm test` y `db:verify:dev` | T002, T003 | Esau | Totales en `check.md` (CA-02, CA-03, CA-05) | ⏳ |
| T006 | Agregar un workflow `database-ci.yml` en `.github/` de la raíz con `npm test` | T005 | Esau | Check visible en un PR | ⏳ |
| T007 | Responder P-01 y P-02 | — | Esau | Respuestas en `Memoria.md` | ⏳ |

---

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01, RF-02 | T001 |
| RF-03, RF-04, RF-05, RF-06 | T002 |
| RF-07 | T003 |
| RF-08 | T004 |
| RNF-01 … RNF-03 | T001 · T001 · T005/T006 |
| CA-01 … CA-05 | T001 · T005 · T005 · T001 · T005 |
