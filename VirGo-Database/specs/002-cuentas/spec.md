# Spec — 002 Cuentas y acceso

| Campo | Valor |
|---|---|
| Módulo | `002-cuentas` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Documenta las colecciones de este dominio, quién las usa y qué falta. La definición ejecutable está en `src/catalog`; este spec no la sustituye.

**Colecciones:** `users`, `passwordResets`, `sessions`, `paymentCards`.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | La colección `users` existe con validación estricta y sus índices. |
| RF-02 | La colección `passwordResets` existe con validación estricta y sus índices. |
| RF-03 | La colección `sessions` existe con validación estricta y sus índices. |
| RF-04 | La colección `paymentCards` existe con validación estricta y sus índices. |
| RF-05 | Todo texto libre tiene longitud máxima (`001-contrato-con-api`). |

## 3. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** `virgo_dev`, **cuando** comparo `users` con el catálogo, **entonces** coinciden. |
| CA-02 | **Dado** `virgo_dev`, **cuando** comparo `passwordResets` con el catálogo, **entonces** coinciden. |
| CA-03 | **Dado** `virgo_dev`, **cuando** comparo `sessions` con el catálogo, **entonces** coinciden. |
| CA-04 | **Dado** `virgo_dev`, **cuando** comparo `paymentCards` con el catálogo, **entonces** coinciden. |

## 4. Fuera de alcance

- Lógica de negocio y endpoints: están en `VirGo-API/specs`.
