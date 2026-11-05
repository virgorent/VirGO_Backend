# Spec — NV-02 Gobernanza global de proyectos

| Campo | Valor |
|---|---|
| Fase | `nv-02` |
| Versión | 1.0.0 |
| Estado | Cumplido con pendiente operativo |
| Fecha | 2026-11-04 |
| Constitución | v1.0.0 |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Cerrar la gobernanza global del monorepo: ratificar la constitución, aprobar la organización del repositorio, publicar plantillas reutilizables y dejar un baseline DevSecOps compartido antes de iniciar las fases de proyecto.

## 2. Requerimientos

| ID | Requerimiento |
|---|---|
| RF-01 | Ratificar `constitution.md` v1.0.0 como fuente de verdad. |
| RF-02 | Aprobar el monorepo y registrar la decisión en `Memoria.md`. |
| RF-03 | Publicar plantillas SDD reutilizables en `.specify/templates/`. |
| RF-04 | Publicar la plantilla de bitácora en `.specify/memory/` sin duplicar `Memoria.md`. |
| RF-05 | Configurar el baseline DevSecOps compartido en `.github/`. |
| RF-06 | Documentar `docs/` y `scripts/` con README. |
| RF-07 | Actualizar `AGENTS.md` con el estado real y la estructura vigente. |
| RF-08 | Documentar la aplicación manual del ruleset de `main`. |

## 3. Criterios de aceptación

| ID | Criterio | Estado |
|---|---|---|
| CA-01 | `constitution.md` v1.0.0 ratificada y vigente. | ✅ |
| CA-02 | Monorepo aprobado y registrado en `Memoria.md`. | ✅ |
| CA-03 | Plantillas `spec`, `plan`, `task`, `check` y `README` disponibles. | ✅ |
| CA-04 | `.specify/memory/` referencia a `Memoria.md`/`memoria.md` sin duplicar. | ✅ |
| CA-05 | Gitleaks, dependency review, Dependabot, ruleset y plantilla de PR en `.github/`. | ✅ |
| CA-06 | `docs/README.md` y `scripts/README.md` presentes. | ✅ |
| CA-07 | `AGENTS.md` refleja la estructura y el estado de las fases. | ✅ |
| CA-08 | Aplicación del ruleset documentada; ejecución manual pendiente. | ✅ |

## 4. Seguridad y DevSecOps

| Amenaza | Control |
|---|---|
| Fuga de secretos | Gitleaks en PR, push a `main` y semanal; `.gitignore` raíz. |
| Dependencias vulnerables | Dependabot semanal y dependency review en PRs. |
| Cambios sin revisión | Ruleset de `main` (aplicación manual) y plantilla de PR. |
| Acciones comprometidas | Acciones fijadas por SHA; imagen de Gitleaks por versión. |

## 5. Límites

No modifica specs de `VirGo-API/` ni `VirGo-Movil/`. No crea código de negocio.

## 6. Aprobación

- **Estado**: Cumplido
- **Fecha**: 2026-11-04
- **Revisores**: Equipo virGO_APP
