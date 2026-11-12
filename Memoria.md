# Memoria.md — virGO_APP

> Bitácora de decisiones, lecciones aprendidas y estado del proyecto.
> Sin datos sensibles (claves, tokens, datos personales).

## Estado actual

- **Fecha inicio**: 2026-10-30
- **Fase**: Gobernanza global cerrada (`nv-00`, `nv-01` y `nv-02` cumplidos; pendientes operativos: aplicar el ruleset de `main` y habilitar Dependency graph). `VirGo-Database` implementado y en endurecimiento; `VirGo-Movil` sigue en especificación.
- **Objetivo**: Monorepo SDD con base MongoDB verificada y lista para integrar. La API C# existe en `origin/feat/vertical-slice-7-apis`, no en `rama-backend` ni `origin/main`; su integración requiere autorización.
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
| 2026-11-04 | Cerrar `nv-02` como Cumplido con pendientes operativos | La gobernanza, plantillas y baseline están verificados; faltan aplicar el ruleset de `main` y habilitar Dependency graph |
| 2026-11-04 | `VirGo-API/db/schema/` es la fuente de verdad del esquema de MongoDB | La base y el modelo se contradecían; ver `VirGo-API/memoria.md` |
| 2026-11-07 | `VirGo-Database/src/catalog/` es la fuente de verdad; `db/schema/` se genera desde el catálogo | Elimina la edición manual duplicada y la deriva entre API y base |
| 2026-11-11 | Exigir TLS en toda conexión; el override inseguro solo se permite en development/test local | Evita transporte sin cifrar y bloquea configuraciones peligrosas en producción |
| 2026-11-11 | El inicializador solo aplica acciones declaradas del catálogo y detecta fallos parciales | Impide planes manipulados, acciones destructivas y deja una reanudación segura |
| 2026-11-11 | Seed DEMO determinista y opcional, bloqueado en production y sobre `virgo` | Permite pruebas locales sin datos reales ni riesgo de contaminar producción |
| 2026-11-11 | No inspeccionar, planificar ni aplicar sobre `virgo` sin credenciales y autorización explícitas | `virgo` es producción y no hay `.env.production` ni aprobación operativa |
| 2026-11-11 | No integrar ni modificar la API desde `rama-backend` sin confirmar la rama de trabajo | La implementación C# solo está en `origin/feat/vertical-slice-7-apis` |

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
