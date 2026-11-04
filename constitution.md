# Constitución — virGO_APP

> Rector SDD. Ante conflicto con otro artefacto, **manda esta constitución**.
> Filosofía madre: base SDD para proyecto Backend y Móvil, arquitectura ejecutable sin funcionalidad específica.
> Fuera de alcance en esta etapa: funcionalidades de negocio, entidades CRUD específicas y la ejecución de hosting/CD (los proyectos los especifican, pero no se ejecutan hasta su aprobación).

## 1. Propósito y alcance

- Proyecto virGO_APP (Backend y Móvil). 
- Metodología SDD. Toda funcionalidad futura deberá nacer de spec en `/specs` (`NV-00…NV-0X`).
- Objetivo: levantar base SDD estructurada, coherente y verificable. Solo arquitectura base, sin lógica de negocio.

## 2. Principios no negociables

1. **Stateless**: Sin sesión oculta. Estado compartido únicamente por DI.
2. **Seguridad primero**: Validar en frontera, denegar por defecto, mínimo privilegio, sin secretos en código/logs/errores.
3. **Sin estado oculto**: Todo compartido en DI; lo estático expone `Reset()` + `lock`.
4. **Fail-fast**: Errores uniformes, sin tragar excepciones.
5. **Empírico antes que teórico**: Medir antes de fijar timeouts/nombres/umbrales.
6. **Documentar límites** en `Memoria.md`, prohibido workaround sin justificación.
7. **Spec-first**: Nada de funcionalidad sin `spec → plan → task`. Esta fase solo crea la estructura SDD.

## 3. Stack base

| Capa | Tecnología |
|---|---|
| Metodología | Spec-Driven Development (SDD) |
| Documentación | Markdown (constitution.md, AGENTS.md, Memoria.md, specs/) |
| Estructura | .opencode (agents + skills), .specify (templates + memory) |

## 4. Arquitectura

- Estructura SDD obligatoria: `specs/`, `.opencode/`, `.specify/`.
- Specs globales numeradas `nv-XX` con `spec.md`, `plan.md`, `task.md`. Los proyectos usan sus propios specs (sección 11).
- Aprobación y trazabilidad obligatorias.
- Sin duplicidad de información entre archivos.

## 5. Código y API

- Inglés en código (cuando aplique).
- Sin `TODO` sin contexto.
- Sin código muerto.
- Sin secretos en Git.

## 6. Seguridad

- Sin secretos en código/logs/errores.
- Archivos sensibles en `.gitignore`.
- Principio de mínimo privilegio.

## 7. Testing y QA

- Estructura base verificable.
- Gates definidos en AGENTS.md.

## 8. CI y GitHub

- Seguir buenas prácticas de GitHub.
- Commits descriptivos.
- Sin commits de secretos.

## 9. Memoria

- `Memoria.md` por fase: estado, decisiones con porqué, errores a evitar.
- Sin datos sensibles.

## 10. Gobernanza SDD

- `constitution.md` es fuente de verdad.
- `AGENTS.md` dice cómo, `constitution.md` dice qué no se negocia.
- Specs en `/specs/nv-0X` con plantilla completa.
- Sin funcionalidad específica hasta siguiente fase.

## 11. Proyectos

| Proyecto | Carpeta | Gobernanza propia |
|---|---|---|
| Backend | `VirGo-API/` | `constitution.md`, `AGENTS.md`, `memoria.md`, `specs/NNN-…` |
| Móvil | `VirGo-Movil/` | `constitution.md`, `AGENTS.md`, `memoria.md`, `specs/NNN-…` |

- Esta constitución manda en los **principios generales**. La de cada proyecto los desarrolla y manda en lo **técnico** de ese proyecto.
- `.github/` vive en la raíz y se comparte: GitHub no lee esa carpeta dentro de subcarpetas.
- `Modelo de datos VirGo.md` vive en la raíz porque lo usan ambos proyectos.
