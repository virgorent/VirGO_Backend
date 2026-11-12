# AGENTS.md — virGO_APP

> Rector: `constitution.md` manda ante conflicto.
> Este archivo dice **cómo** trabajar. La constitución dice **qué** no se negocia.

## 1. Forma de trabajo

- **Metodología**: Spec-Driven Development (SDD).
- **Fase actual**: gobernanza global cerrada (`nv-00`, `nv-01` y `nv-02` cumplidos; pendiente operativo: aplicar el ruleset de `main`). `VirGo-Database` tiene implementación y endurecimiento; `VirGo-Movil` tiene la base Android y el login en progreso; `VirGo-API` continúa en especificación.
- **Principio**: `spec → plan → task → check`. No ejecutar tareas sin aprobación explícita.
- **DevSecOps**: ningún cambio se fusiona sin escaneo de secretos, revisión de dependencias y, cuando el proyecto ya tenga su workflow, análisis estático en verde.
- **Todo debe alinearse** a `constitution.md`.

## 2. Estructura obligatoria

| Componente | Ruta | Propósito |
|---|---|---|
| Constitución | `constitution.md` | Fuente de verdad global |
| Manual operativo | `AGENTS.md` | Cómo trabajar |
| Memoria global | `Memoria.md` | Bitácora de decisiones |
| Specs globales | `specs/nv-00/`, `specs/nv-01/`, `specs/nv-02/` | Fases de gobernanza (spec, plan, task, check) |
| Plantillas | `.specify/templates/` | Plantillas reutilizables por proyecto |
| Memoria reutilizable | `.specify/memory/` | Plantilla de bitácora; no duplica `Memoria.md` |
| Agentes globales | `.opencode/agents/` | Sub-agentes SDD |
| Skills globales | `.opencode/skills/` | Guías por fase/habilidad |
| CI/CD compartido | `.github/` | Workflows, Dependabot, ruleset y plantilla de PR |
| Documentación de apoyo | `docs/` | Notas y diagramas (README) |
| Scripts de apoyo | `scripts/` | Automatizaciones locales (README) |
| Backend | `VirGo-API/` | Gobernanza y specs `NNN-…` propios |
| Móvil | `VirGo-Movil/` | Gobernanza y specs `NNN-…` propios |
| Base de datos | `VirGo-Database/` | Catálogo de MongoDB; specs `NNN-…` propios |
| Datos | `Modelo de datos VirGo.md` | Modelo de MongoDB compartido |

## 3. Fases

| Fase | Objetivo | Estado |
|---|---|---|
| **nv-00** | Constitución SDD y reglas globales | Cumplido (2026-11-04) |
| **nv-01** | Arquitectura base SDD | Cumplido (2026-11-04) |
| **nv-02** | Gobernanza global de proyectos | Cumplido con pendiente operativo (aplicar ruleset) |
| **VirGo-API** | Backend .NET 10 (specs `000`–`004`) | Especificación |
| **VirGo-Movil** | App Android (spec `000`) | Base Android implementada; login en progreso |
| **VirGo-Database** | Catálogo MongoDB (specs `000`–`001`) | Implementación y endurecimiento |

## 4. Convenciones

- **Idioma**: documentación en español; código e identificadores en inglés; README raíz y de proyecto en inglés (los README internos de carpetas van en español).
- **Archivos**: Markdown bien formateado.
- **Specs globales**: `nv-XX` con `spec.md`, `plan.md`, `task.md` y `check.md`.
- **Specs de proyecto**: `NNN-…` con los mismos cuatro archivos; un spec es atómico.
- **Plantillas**: todo spec nuevo parte de `.specify/templates/`.
- **Alineación**: todos los archivos deben ser coherentes con `constitution.md`.
- **Sin duplicidad**: la bitácora canónica es `Memoria.md` (global) o `memoria.md` (proyecto).
- **Sin funcionalidad**: no crear código de negocio sin spec aprobado.

**Ramas:** `main` se protege con el ruleset de `.github/rulesets/main-protection.json` (aplicación manual pendiente). `rama-backend`, `rama-apis` y `rama-movil` integran por PR. Las ramas de trabajo usan `feat/NNN-modulo` o `fix/…`.

## 5. Sub-agentes

Usar los sub-agentes de `.opencode/agents/` para mantener la metodología SDD. Cada proyecto define además los suyos.

## 6. Skills

Skills en `.opencode/skills/` guían el trabajo por fase. Cada proyecto define además las suyas.

## 7. DevSecOps (gates obligatorios)

- **Secretos**: Gitleaks en cada PR y push a `main`; sin secretos en código, logs, errores ni historial.
- **Dependencias**: Dependabot semanal y `dependency-review` en PRs que cambien manifiestos; requiere Dependency graph habilitado. No fusionar con vulnerabilidades altas.
- **SAST y pruebas**: los workflows de cada proyecto (`api-*.yml`, `movil-*.yml`) ejecutan CodeQL/linters y pruebas; hasta que existan, el ruleset solo exige `gitleaks` y `dependency review`. Antes del primer PR de código se añaden esos checks al ruleset.
- **Mínimo privilegio**: workflows con `permissions` acotados; nube por OIDC, sin llaves de larga duración.
- **Rama protegida**: PR obligatorio, una revisión, checks requeridos, sin force push ni borrado.
- **Release**: artefactos firmados y verificables; credenciales solo en GitHub Secrets.
- **Plantilla de PR**: `.github/pull_request_template.md` incluye la checklist de seguridad.

## 8. Verificación

- [x] `constitution.md` versionada y vigente
- [x] `AGENTS.md` coherente con `constitution.md`
- [x] `Memoria.md` actualizado
- [x] `.opencode/agents/` con sub-agentes
- [x] `.opencode/skills/` con skills
- [x] `specs/nv-00/` con spec, plan, task y check
- [x] `specs/nv-01/` con spec, plan, task y check
- [x] `specs/nv-02/` con spec, plan, task y check (pendiente operativo: aplicar ruleset)
- [x] `.specify/templates/` con plantillas
- [x] `.specify/memory/` con plantilla de bitácora
- [x] `.github/` con baseline DevSecOps
- [ ] Constituciones de proyecto v1.1.0 aprobadas
- [ ] Specs `000` de cada proyecto aprobados
