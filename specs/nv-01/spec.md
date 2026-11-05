# Spec — NV-01 Arquitectura base SDD

| Campo | Valor |
|---|---|
| Fase | `nv-01` |
| Versión | 1.0.0 |
| Estado | Cumplido |
| Fecha | 2026-11-04 |
| Constitución | v1.0.0 |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Arquitectura base SDD sin funcionalidad específica para virGO_APP (Backend y Móvil), lista para recibir specs de funcionalidad.

## 2. Requerimientos

| ID | Requerimiento |
|---|---|
| RF-01 | Estructura SDD completa (global y por proyecto). |
| RF-02 | Sin entidades, sin CRUDs, sin lógica de negocio. |
| RF-03 | Specs, agentes, skills y plantillas presentes. |
| RF-04 | Todo alineado a `constitution.md`. |
| RF-05 | Listo para recibir specs de funcionalidad futura. |

## 3. Criterios de aceptación

| ID | Criterio | Estado |
|---|---|---|
| CA-01 | `specs/nv-00/` completo (spec, plan, task, check). | ✅ |
| CA-02 | `specs/nv-01/` completo (spec, plan, task, check). | ✅ |
| CA-03 | `.opencode/agents/` con sub-agentes. | ✅ |
| CA-04 | `.opencode/skills/` con skills. | ✅ |
| CA-05 | `.specify/templates/` y `.specify/memory/` con plantillas. | ✅ |
| CA-06 | Todo coherente con `constitution.md`. | ✅ |
| CA-07 | Sin código funcional. | ✅ |
| CA-08 | `VirGo-API/` y `VirGo-Movil/` conservan su gobernanza propia. | ✅ |
| CA-09 | La estructura está lista para recibir specs de funcionalidad. | ✅ |

## 4. Seguridad y DevSecOps

| Amenaza | Control |
|---|---|
| Secretos en el repositorio | `.gitignore` raíz, Gitleaks y plantilla de PR. |
| Dependencias vulnerables | Dependabot y dependency review. |
| Cambios sin control | Ruleset de `main` y checks requeridos. |

## 5. Límites

Solo estructura. Sin implementación, sin CRUDs, sin entidades.

## 6. Aprobación

- **Estado**: Cumplido
- **Fecha**: 2026-11-04
- **Revisores**: Equipo virGO_APP
