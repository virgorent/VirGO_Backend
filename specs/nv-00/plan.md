# Plan — NV-00 Constitución SDD

| Campo | Valor |
|---|---|
| Fase | `nv-00` |
| Versión | 1.0.0 |
| Estado | Cumplido |
| Fecha | 2026-11-04 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.0.0 |

## Objetivo

Establecer la base constitucional SDD del monorepo.

## Constitution Check

| Principio | Cumple | Nota |
|---|---|---|
| SDD antes que código | ✅ | Solo metodología y gobernanza. |
| Seguridad por defecto | ✅ | Gates DevSecOps definidos. |
| Sin duplicidad | ✅ | Una bitácora canónica. |
| Trazabilidad | ✅ | RF/CA con tareas y evidencia en `check.md`. |

## Pasos

1. ✅ Redactar `constitution.md` con principios, alcance y gobernanza.
2. ✅ Crear `AGENTS.md` alineado.
3. ✅ Crear `Memoria.md`.
4. ✅ Definir la estructura `.opencode/` y `.specify/`.
5. ✅ Verificar coherencia y ratificar la versión 1.0.0.

## Recursos

- `.specify/templates/`
- `constitution.md` (referencia)

## Riesgos y mitigación

| Riesgo | Mitigación |
|---|---|
| Desalineación entre archivos | Revisión cruzada y `check.md` con evidencia. |
| Secretos en el repositorio | `.gitignore` raíz y Gitleaks en CI. |

## Resultado

Gobernanza global ratificada el 2026-11-04.
