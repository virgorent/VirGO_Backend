# Check — 000 base-datos

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Estado del spec | En revisión |
| Revisó | Pendiente de revisión humana |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | Aprobación explícita del usuario en la sesión de trabajo | ✅ |
| Constitution Check sin ❌ | [plan.md](plan.md) | ✅ |
| Sin secretos en el repositorio | Revisión local; Gitleaks se ejecutará en el PR | ⏳ |
| Preguntas abiertas respondidas | P-01 y P-02 no bloquean la implementación local | ➖ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `npm test`: 45 pruebas aprobadas. | ✅ |
| CA-02 | `npm run test:integration`: 28 pruebas aprobadas con MongoDB 8 local. | ✅ |
| CA-03 | `npm run db:export:api:check`: 22 esquemas actualizados, sin diferencias. | ✅ |
| CA-04 | `python ../VirGo-API/db/tools/check_schema.py`: `22 colecciones, 0 diferencias`. | ✅ |
| CA-05 | Prueba `bookings idempotencyKey es unico por cliente y opcional`. | ✅ |
| CA-06 | Prueba `la segunda ejecucion es idempotente`. | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `npm test` · 45 aprobadas, 0 fallidas. | ✅ |
| Integración | `npm run test:integration` · 28 aprobadas, 0 fallidas. | ✅ |
| Seguridad | Gitleaks en CI al abrir PR. | ⏳ |
| Humo | `plan`, `apply`, `verify` e `inspect` cubiertos por integración. | ✅ |

## 4. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Gitleaks sin hallazgos | Se ejecutará en el PR. | ⏳ |
| Dependency review sin vulnerabilidades altas | Se ejecutará en el PR. | ⏳ |
| SAST (CodeQL / linters) en verde | No hay linter configurado en este proyecto. | ➖ |
| Workflows con permisos mínimos | `database-ci.yml` creado con `contents: read`; ejecución pendiente en el PR. | ✅ |
| Sin credenciales en logs ni artefactos | Revisión local de scripts y docs. | ✅ |

## 5. PR e integración continua

- [ ] PRs del módulo: pendiente de crear.
- [ ] Checks requeridos en verde y una revisión.

## 6. Release

- [ ] Versión y commit: pendiente.
- [ ] Artefacto firmado y verificado: no aplica a esta entrega local.

## 7. Memoria y aprendizaje

- [ ] Entrada de cierre en `memoria.md`.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes.
