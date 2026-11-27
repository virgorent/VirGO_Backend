# Check — 100 ID determinista de notificaciones

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en "Cumple" o "No aplica" justificado.

**Leyenda:** Cumple · No aplica (justificar) · Pendiente

| Campo | Valor |
|---|---|
| Módulo | `100-notice-id-determinista` |
| Estado del spec | En revisión |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas creados | `spec.md`, `plan.md`, `task.md`, `check.md` | Cumple |
| Constitution Check sin bloqueos | `plan.md`, sección 3; sin resultados "Bloquea" | Cumple |
| Sin secretos en el repositorio | Revisión del diff; no se agregaron credenciales | Cumple |
| Preguntas abiertas respondidas | `spec.md`, sección 8; namespace, clave y vector congelados | Cumple |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `DeterministicNoticeIdsTests.ImplementsTheRfc4122UuidV5Vector` → `2ed6657d-e927-568b-95e1-2665a8aea6a2` | Cumple |
| CA-02 | `DeterministicNoticeIdsTests.SameBookingIdProducesTheSameNoticeId` | Cumple |
| CA-03 | `DeterministicNoticeIdsTests.DifferentBookingIdsProduceDifferentNoticeIds` | Cumple |
| CA-04 | `DeterministicNoticeIdsTests.CanonicalKeyContainsTheBookingCreatedPrefix` y `BookingCreatedUsesTheCanonicalKeyAndFrozenNamespace` | Cumple |
| CA-05 | `DeterministicNoticeIdsTests.GeneratedIdMatchesTheNoticesSchemaPattern` leyendo `db/schema/notices.json` | Cumple |
| CA-06 | `DeterministicNoticeIdsTests.GeneratedIdHasUuidV5VersionAndRfcVariant` | Cumple |
| CA-07 | `JobTests.BookingCreatedCreatesAnUnreadNoticeFor90Days` con ID `238b0bd9-52c0-5976-86f0-b2b4a220b44a` | Cumple |
| CA-08 | `JobTests.BookingCreatedTwiceKeepsASingleNotice` y `JobTests.RepeatedBookingCreatedEventKeepsOneDeterministicNotice` | Cumple |
| CA-09 | Revisión del diff: sin cambios en catálogo, esquemas, exportador, OpenAPI, controllers, DTOs, CI/CD, infraestructura, configuración ni dependencias | Cumple |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `dotnet test tests/VirGo.UnitTests/VirGo.UnitTests.csproj -c Release --no-restore` → 64/64; filtro `DeterministicNoticeIdsTests|JobTests` → 27/27 | Cumple |
| Integración | No aplica: no se ejecutaron pruebas de persistencia real ni escrituras MongoDB | No aplica |
| Seguridad | Revisión del diff; sin secretos, sin datos personales en la clave y sin dependencias nuevas | Cumple |
| Humo | No aplica: no hay endpoints nuevos ni cambios HTTP | No aplica |

## 4. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Gitleaks sin hallazgos | No ejecutado en este corte; queda para el PR | Pendiente |
| Dependency review sin vulnerabilidades altas | No aplica: no hay cambios en manifiestos | No aplica |
| SAST en verde | No ejecutado localmente; CI remoto fuera de alcance | Pendiente |
| Workflows con permisos mínimos | No aplica: no hay cambios en workflows | No aplica |
| Sin credenciales en logs ni artefactos | Revisión del diff; el helper no registra la clave ni valores sensibles | Cumple |

## 5. PR e integración continua

- [ ] PRs del módulo: no creados en este corte.
- [ ] Checks requeridos en verde y una revisión: no aplica localmente; queda para el PR.

## 6. Release

- [ ] Versión y commit: no aplica; sin release en este corte.
- [ ] Artefacto firmado y verificado: no aplica.

## 7. Memoria y aprendizaje

- [x] Entrada de D-03 en `Memoria.md` con fecha 2026-11-26.
- [ ] Mejoras propuestas: ninguna derivada de este corte; pendiente de revisión humana.
