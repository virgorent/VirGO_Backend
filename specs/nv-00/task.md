# Tareas — NV-00 Constitución SDD

| Campo | Valor |
|---|---|
| Fase | `nv-00` |
| Versión | 1.0.0 |
| Estado | Cumplido (2026-11-04) |
| Fecha | 2026-11-04 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.0.0 |

| ID | Tarea | Estado |
|---|---|---|
| T001 | Crear `constitution.md` | ✅ |
| T002 | Crear `AGENTS.md` coherente con la constitución | ✅ |
| T003 | Crear `Memoria.md` | ✅ |
| T004 | Crear estructura `.opencode/agents` y `.opencode/skills` | ✅ |
| T005 | Crear estructura `.specify/templates` y `.specify/memory` | ✅ |
| T006 | Definir gates DevSecOps y baseline `.github/` | ✅ |
| T007 | Verificar alineación entre archivos | ✅ |
| T008 | Documentar en `Memoria.md` | ✅ |
| T009 | Verificar ausencia de funcionalidad específica | ✅ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 / CA-01 | T001 |
| RF-02 / CA-02 | T002 |
| RF-03 / CA-03 | T003, T008 |
| RF-04 / CA-04 | T004, T005 |
| RF-05 / CA-06 | T006 |
| RF-06 / CA-05 | T009 |
| Verificación transversal | T007 (CA-01…CA-06) |
