# Plan — NV-01 Arquitectura base SDD

| Campo | Valor |
|---|---|
| Fase | `nv-01` |
| Versión | 1.0.0 |
| Estado | Cumplido |
| Fecha | 2026-11-04 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.0.0 |

## Objetivo

Levantar la arquitectura base SDD sin funcionalidad.

## Constitution Check

| Principio | Cumple | Nota |
|---|---|---|
| SDD antes que código | ✅ | Solo estructura y plantillas. |
| Seguridad por defecto | ✅ | Baseline de secretos y dependencias. |
| Sin duplicidad | ✅ | Bitácora canónica por ámbito. |
| Trazabilidad | ✅ | RF/CA con tareas y evidencia en `check.md`. |

## Pasos

1. ✅ Completar la estructura `.opencode/`.
2. ✅ Completar la estructura `.specify/`.
3. ✅ Crear los specs `nv-00` y `nv-01` con sus cuatro archivos.
4. ✅ Agregar el baseline DevSecOps en `.github/`.
5. ✅ Verificar la coherencia total.
6. ✅ Actualizar `Memoria.md`.

## Recursos

- `nv-00` como base.
- `constitution.md`.
- `.specify/templates/`.

## Riesgos y mitigación

| Riesgo | Mitigación |
|---|---|
| Agregar funcionalidad por error | Revisión con `security-reviewer` y `traceability-clerk`. |
| Duplicar información | Bitácora canónica única por ámbito. |

## Resultado

Arquitectura base cerrada el 2026-11-04.
