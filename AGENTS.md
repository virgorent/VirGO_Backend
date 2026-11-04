# AGENTS.md — virGO_APP

> Rector: `constitution.md` manda ante conflicto. 
> Este archivo dice **cómo** trabajar. La constitución dice **qué** no se negocia.

## 1. Forma de trabajo

- **Metodología**: Spec-Driven Development (SDD)
- **Fase actual**: Solo arquitectura base SDD. **Sin funcionalidad específica, sin CRUDs, sin entidades de negocio.**
- **Principio**: `spec → plan → task`. No ejecutar tareas aún.
- **Todo debe alinearse** a `constitution.md`

## 2. Estructura obligatoria

| Componente | Ruta | Propósito |
|---|---|---|
| Constitución | `constitution.md` | Fuente de verdad |
| Manual operativo | `AGENTS.md` | Cómo trabajar |
| Memoria | `Memoria.md` | Bitácora de decisiones |
| Specs | `specs/nv-00/`, `specs/nv-01/` | Especificaciones por fase |
| Agentes | `.opencode/agents/` | Sub-agentes SDD |
| Skills | `.opencode/skills/` | Guías por fase/habilidad |
| Templates | `.specify/templates/` | Plantillas reutilizables |
| Backend | `VirGo-API/` | Proyecto con su propia gobernanza y specs `NNN-…` |
| Móvil | `VirGo-Movil/` | Proyecto con su propia gobernanza y specs `NNN-…` |
| Datos | `Modelo de datos VirGo.md` | Modelo de MongoDB compartido |

## 3. Fases

| Fase | Objetivo | Estado |
|---|---|---|
| **nv-00** | Constitución SDD - Definir metodología, reglas, límites | Pendiente (crear estructura) |
| **nv-01** | Arquitectura base - Estructura sin funcionalidad | Pendiente (crear estructura) |

## 4. Convenciones

- **Idioma**: Español/inglés coherente con proyecto
- **Archivos**: Markdown bien formateado
- **Specs**: Siempre `spec.md`, `plan.md`, `task.md` por fase
- **Alineación**: Todos los archivos deben ser coherentes con `constitution.md`
- **Sin funcionalidad**: No crear código de negocio en esta fase

**Ramas:** `rama-backend` y `rama-movil` entran a `main` por PR. Dentro de una carpeta de proyecto, sigue el `AGENTS.md` de esa carpeta.

## 5. Sub-agentes

Usar sub-agentes ubicados en `.opencode/agents/` para mantener consistencia con metodología SDD.

## 6. Skills

Skills en `.opencode/skills/` guiarán el trabajo por fase.

## 7. Verificación

- [ ] `constitution.md` existe y es fuente de verdad
- [ ] `AGENTS.md` coherente con `constitution.md`
- [ ] `Memoria.md` creado
- [ ] `.opencode/agents/` con sub-agentes
- [ ] `.opencode/skills/` con skills
- [ ] `specs/nv-00/` con spec.md, plan.md, task.md
- [ ] `specs/nv-01/` con spec.md, plan.md, task.md
- [ ] Todo alineado y coherente
