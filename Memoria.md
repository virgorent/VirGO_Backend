# Memoria.md — virGO_APP

> Bitácora de decisiones, lecciones aprendidas y estado del proyecto.
> Sin datos sensibles (claves, tokens, datos personales).

## Estado actual

- **Fecha inicio**: 2026-10-30
- **Fase**: Gobernanza global cerrada (`nv-00`, `nv-01` y `nv-02` cumplidos; pendiente operativo: aplicar el ruleset de `main`).
- **Objetivo**: Monorepo SDD con `VirGo-API` y `VirGo-Movil` en especificación, sin código todavía.
- **Constitución global**: v1.0.0, ratificada el 2026-11-04.

## Decisiones

| Fecha | Decisión | Razón |
|---|---|---|
| 2026-10-30 | Seguir metodología SDD como en api_sdd | Requerido en clase |
| 2026-10-30 | Crear solo base SDD (nv-00, nv-01) sin funcionalidad | Instrucciones: "Aun no ejecuten las tareas", "Para la funcionalidad especifica... aun no trabajen" |
| 2026-11-03 | Un solo repositorio con carpetas `VirGo-API/` y `VirGo-Movil/`; esta base queda como gobernanza global | Propuesta en el PR de `docs/sdd-virgo` |
| 2026-11-04 | Aprobar el monorepo con `VirGo-API/` y `VirGo-Movil/` | El equipo confirmó la propuesta del PR #1 |
| 2026-11-04 | Ratificar `constitution.md` v1.0.0 | Da versión, fecha y trazabilidad al documento rector |
| 2026-11-04 | Cerrar `nv-00` y `nv-01` como Cumplido | La estructura existe y se verificó con `check.md` |
| 2026-11-04 | Plantillas SDD en `.specify/templates/` y `.specify/memory/` | Evitar specs ad hoc y asegurar controles de seguridad desde el diseño |
| 2026-11-04 | Baseline DevSecOps en `.github/` | Gitleaks, dependency review, Dependabot y ruleset compartidos por ambos proyectos |
| 2026-11-04 | Cerrar `nv-02` como Cumplido con pendiente operativo | La gobernanza, plantillas y baseline están verificados; falta aplicar el ruleset de `main` con permisos de administración |

## Errores a evitar

- Crear código funcional antes de tener specs completas.
- Ejecutar tareas de los specs antes de tiempo.
- Desalinear archivos con `constitution.md`.
- Duplicar información entre documentos.
- Dejar `AGENTS.md` o los checkboxes desactualizados respecto a la realidad.
- Versionar secretos, tokens o datos personales.
- Relajar los gates de seguridad por prisa.

## Lecciones aprendidas

- Las plantillas genéricas evitan que un proyecto sesgue al otro.
- La bitácora canónica debe ser una sola por ámbito para no duplicar.

## Notas

Este archivo se irá actualizando conforme avance el proyecto.
