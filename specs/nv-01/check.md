# Check — NV-01 Arquitectura base SDD

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Fase | `nv-01` |
| Versión | 1.0.0 |
| Estado | Cumplido |
| Spec / Plan / Task | [spec.md](spec.md) · [plan.md](plan.md) · [task.md](task.md) |
| Revisó | Equipo virGO_APP |
| Fecha de cierre | 2026-11-04 |

## 1. Criterios de aceptación

| CA | Criterio | Evidencia | Estado |
|---|---|---|---|
| CA-01 | `specs/nv-00/` completo | `spec.md`, `plan.md`, `task.md`, `check.md` | ✅ |
| CA-02 | `specs/nv-01/` completo | `spec.md`, `plan.md`, `task.md`, `check.md` | ✅ |
| CA-03 | `.opencode/agents/` con sub-agentes | 5 agentes | ✅ |
| CA-04 | `.opencode/skills/` con skills | `00-constitution`, `01-foundation` | ✅ |
| CA-05 | `.specify/templates/` y `.specify/memory/` con plantillas | `spec`, `plan`, `task`, `check`, `README`; `memoria-template` | ✅ |
| CA-06 | Todo coherente con `constitution.md` | Revisión cruzada de artefactos | ✅ |
| CA-07 | Sin código funcional | No hay código de negocio | ✅ |
| CA-08 | Proyectos con gobernanza propia | `VirGo-API/`, `VirGo-Movil/` | ✅ |
| CA-09 | Listo para recibir specs de funcionalidad | Estructura y plantillas disponibles | ✅ |

## 2. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Sin secretos en el repositorio | Revisión estática; Gitleaks como gate de CI | ✅ |
| Dependabot, Gitleaks y dependency review configurados | `.github/` | ✅ |
| Ruleset de `main` documentado | `.github/rulesets/main-protection.json` y `.github/README.md` | ✅ |
| Aplicación del ruleset | Requiere permisos de administración | ⏳ |
| SAST y pruebas diferidos a workflows de proyecto | `constitution.md` §6, `AGENTS.md` §7 | ✅ |

## 3. Resultado

**Cumplido.** La arquitectura base SDD está lista para recibir specs de funcionalidad.
