# Tareas — 100 ID determinista de notificaciones

| Campo | Valor |
|---|---|
| Módulo | `100-notice-id-determinista` |
| Versión | 0.1.0 |
| Estado | Ejecutado localmente; pendiente de revisión/PR |
| Fecha | 2026-11-26 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.2.0 (ratificada) |

**Estados:** Pendiente · En curso · Hecha (con evidencia) · Bloqueada

**[P]** = puede ejecutarse en paralelo con otras [P] de la misma fase.

## F0 · Documentación y coherencia

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1001 | Crear los cuatro documentos SDD de `100-notice-id-determinista` | — | arquitecto | `spec.md`, `plan.md`, `task.md`, `check.md` creados | Hecha |
| T1002 | Enmendar `099` spec y plan para eliminar el formato `_id` no UUID | T1001 | arquitecto | RF-03 y plan referencian UUIDv5 y `100` | Hecha |

## F1 · Implementación y pruebas

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1003 | Crear helper UUIDv5 local en `VirGo.Application/Jobs` | T1002 | dev-dotnet | Vector `2ed6657d-e927-568b-95e1-2665a8aea6a2` verificado; sin paquetes nuevos | Hecha |
| T1004 | Integrar el helper en `BookingCreatedHandler` | T1003 | dev-dotnet | `Notice.Id` usa `DeterministicNoticeIds.ForBookingCreated` | Hecha |
| T1005 | Actualizar `JobTests` y crear pruebas unitarias del helper | T1003 | qa | UnitTests 64/64; filtro `DeterministicNoticeIdsTests|JobTests` 27/27 | Hecha |

## F2 · Gates y cierre documental

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1006 | Ejecutar tests unitarios relevantes, build y `git diff --check` | T1004, T1005 | qa | Tests en verde; build 0 errores/0 advertencias; `git diff --check` sin errores | Hecha |
| T1007 | Registrar D-03 en `Memoria.md` solo si los gates son satisfactorios | T1006 | arquitecto | Entrada con fecha 2026-11-26 agregada | Hecha |
| T1008 | Completar `check.md` con evidencia real y revisar diff completo | T1006, T1007 | qa | Check actualizado; alcance revisado | Hecha |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| HT-01 | T1003, T1004, T1005 |
| HT-02 | T1002, T1006, T1008 |
| RF-01 | T1003, T1005 |
| RF-02 | T1003, T1004 |
| RF-03 | T1005 |
| RF-04 | T1004, T1005 |
| RF-05 | T1006, T1008 |
| RF-06 | T1002, T1006, T1008 |
| RNF-01 | T1003, T1008 |
| RNF-02 | T1003, T1006 |
| RNF-03 | T1005 |
| RNF-04 | T1006, T1008 |
| RNF-05 | T1005, T1008 |
| CA-01 | T1005 |
| CA-02 | T1005 |
| CA-03 | T1005 |
| CA-04 | T1005 |
| CA-05 | T1005 |
| CA-06 | T1005 |
| CA-07 | T1004, T1005 |
| CA-08 | T1005 |
| CA-09 | T1006, T1008 |
