# Spec — 006 Cotizaciones

| Campo | Valor |
|---|---|
| Módulo | `006-cotizaciones` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Documenta las colecciones de este dominio, quién las usa y qué falta. La definición ejecutable está en `src/catalog`; este spec no la sustituye.

**Colecciones:** `quotes`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | La colección `quotes` existe con validación estricta y sus índices. |
| RF-02 | Todo texto libre tiene longitud máxima (`001-contrato-con-api`). |

## 3. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** `virgo_dev`, **cuando** comparo `quotes` con el catálogo, **entonces** coinciden. |

## 4. Fuera de alcance

- Lógica de negocio y endpoints: están en `VirGo-API/specs`.
