# Memoria.md — virGO_APP

> Bitácora de decisiones, lecciones aprendidas y estado del proyecto.
> Sin datos sensibles (claves, tokens, datos personales).

## Estado actual

- **Fecha inicio**: 2026-10-30
- **Fase**: Levantamiento base SDD
- **Objetivo**: Crear estructura base SDD para Backend y Móvil (solo arquitectura, sin funcionalidad)

## Decisiones

| Fecha | Decisión | Razón |
|---|---|---|
| 2026-10-30 | Seguir metodología SDD como en api_sdd | Requerido en clase |
| 2026-10-30 | Crear solo base SDD (nv-00, nv-01) sin funcionalidad | Instrucciones: "Aun no ejecuten las tareas", "Para la funcionalidad especifica... aun no trabajen" |
| 2026-11-04 | `VirGo-API/db/schema/` es la fuente de verdad del esquema de MongoDB | La base y el modelo se contradecían; ver `VirGo-API/memoria.md` |
| 2026-11-03 | Un solo repositorio con carpetas `VirGo-API/` y `VirGo-Movil/`; esta base queda como gobernanza global | Propuesta en el PR de `docs/sdd-virgo`, para revisión del equipo |

## Errores a evitar

- Crear código funcional antes de tener specs completas
- Ejecutar tareas de los specs antes de tiempo
- Desalinear archivos con constitution.md
- Duplicar información entre documentos

## Lecciones aprendidas

_(Pendiente)_

## Notas

Este archivo se irá actualizando conforme avance el proyecto.
