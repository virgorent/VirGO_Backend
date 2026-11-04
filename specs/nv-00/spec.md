# Spec — NV-00 Constitución SDD

| Campo | Valor |
|---|---|
| Fase | `nv-00` |
| Versión | 1.0.0 |
| Estado | Cumplido |
| Fecha | 2026-11-04 |
| Constitución | v1.0.0 |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Base metodológica para virGO_APP (Backend y Móvil). Definir reglas, límites y gobernanza SDD antes de escribir cualquier funcionalidad.

## 2. Requerimientos

| ID | Requerimiento |
|---|---|
| RF-01 | `constitution.md` como fuente de verdad, versionada y ratificada. |
| RF-02 | `AGENTS.md` coherente con la constitución. |
| RF-03 | `Memoria.md` como bitácora de decisiones. |
| RF-04 | Estructura `.opencode/` (agentes y skills) y `.specify/` (plantillas y memoria). |
| RF-05 | Principios no negociables y gates DevSecOps explícitos. |
| RF-06 | Mantener la fase sin funcionalidad específica. |

## 3. Criterios de aceptación

| ID | Criterio | Estado |
|---|---|---|
| CA-01 | `constitution.md` existe, versionada (v1.0.0) y vigente. | ✅ |
| CA-02 | `AGENTS.md` coherente con `constitution.md`. | ✅ |
| CA-03 | `Memoria.md` creado y actualizado. | ✅ |
| CA-04 | Estructura `.opencode/` y `.specify/` definida. | ✅ |
| CA-05 | Sin funcionalidad específica en esta fase. | ✅ |
| CA-06 | Principios no negociables y gates DevSecOps documentados. | ✅ |

## 4. Seguridad y DevSecOps

| Amenaza | Control |
|---|---|
| Fuga de secretos | Sin secretos en código ni historial; Gitleaks como gate. |
| Cambios sin revisión | `main` protegida por ruleset y PR obligatorio. |
| Dependencias vulnerables | Dependabot y dependency review. |

## 5. Límites

Solo metodología. Sin código, sin CRUDs, sin entidades de negocio.

## 6. Aprobación

- **Estado**: Cumplido
- **Fecha**: 2026-11-04
- **Revisores**: Equipo virGO_APP
