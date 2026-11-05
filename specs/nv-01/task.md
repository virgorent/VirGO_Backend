# Tareas — NV-01 Arquitectura base SDD

| Campo | Valor |
|---|---|
| Fase | `nv-01` |
| Versión | 1.0.0 |
| Estado | Cumplido (2026-11-04) |
| Fecha | 2026-11-04 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.0.0 |

| ID | Tarea | Estado |
|---|---|---|
| T001 | Verificar la estructura completa | ✅ |
| T002 | Crear `specs/nv-00/` y `specs/nv-01/` con sus cuatro archivos | ✅ |
| T003 | Verificar sub-agentes en `.opencode/agents/` | ✅ |
| T004 | Verificar skills en `.opencode/skills/` | ✅ |
| T005 | Crear plantillas en `.specify/templates/` y `.specify/memory/` | ✅ |
| T006 | Agregar baseline DevSecOps en `.github/` | ✅ |
| T007 | Revisar alineación con `constitution.md` | ✅ |
| T008 | Actualizar `Memoria.md` con el estado | ✅ |
| T009 | Validación final | ✅ |
| T010 | Verificar gobernanza propia de `VirGo-API/` y `VirGo-Movil/` | ✅ |
| T011 | Verificar ausencia de código funcional | ✅ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 / CA-08 | T001, T010 |
| RF-02 / CA-07 | T011 |
| RF-03 / CA-01, CA-02, CA-03, CA-04, CA-05 | T002, T003, T004, T005 |
| RF-04 / CA-06 | T006, T007 |
| RF-05 / CA-09 | T001, T009 |
| Documentación | T008 (CA-06) |
