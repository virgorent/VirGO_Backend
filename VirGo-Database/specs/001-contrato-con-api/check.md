# Check — 001 contrato-con-api

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Estado del spec | En revisión |
| Revisó | Pendiente de revisión humana |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | Aprobación explícita del usuario en la sesión de trabajo | ✅ |
| Constitution Check sin ❌ | [plan.md](plan.md) | ✅ |
| Sin secretos en el repositorio | Revisión local; Gitleaks se ejecutará en el PR | ⏳ |
| Preguntas abiertas respondidas | P-01 y P-02 no bloquean la generación local | ➖ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `npm run db:export:api`: 22 archivos generados. | ✅ |
| CA-02 | `npm run db:export:api:check`: sin diferencias. | ✅ |
| CA-03 | `tests/unit/export.test.js`: modificación detectada como `different`. | ✅ |
| CA-04 | `tests/unit/export.test.js`: archivo extra detectado como `unexpected`. | ✅ |
| CA-05 | `check_schema.py`: `22 colecciones, 0 diferencias`. | ✅ |
| CA-06 | Integración: duplicado por `clientId` + `idempotencyKey` rechazado. | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `npm test` · 45 aprobadas. | ✅ |
| Integración | `npm run test:integration` · 28 aprobadas. | ✅ |
| Contrato | `db:export:api:check` + `check_schema.py` · OK. | ✅ |
| Seguridad | Gitleaks en CI al abrir PR. | ⏳ |

## 4. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Gitleaks sin hallazgos | Se ejecutará en el PR. | ⏳ |
| Dependency review sin vulnerabilidades altas | Se ejecutará en el PR. | ⏳ |
| SAST (CodeQL / linters) en verde | No hay linter configurado en este proyecto. | ➖ |
| Workflows con permisos mínimos | `database-ci.yml` creado con `contents: read`; ejecución pendiente en el PR. | ✅ |
| Sin credenciales en logs ni artefactos | El exportador no conecta a MongoDB. | ✅ |

## 5. PR e integración continua

- [ ] PRs del módulo: pendiente de crear.
- [ ] Checks requeridos en verde y una revisión.

## 6. Release

- [ ] Versión y commit: pendiente.
- [ ] Artefacto firmado y verificado: no aplica a esta entrega local.

## 7. Memoria y aprendizaje

- [ ] Entrada de cierre en `memoria.md`.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes.

## 8. Aplicación en MongoDB Atlas DEV

**Fecha y hora:** 2026-11-09 01:08:51 -06:00.

| Control | Evidencia | Estado |
|---|---|---|
| Destino autorizado | Clúster `virgo-dev`; base `virgo_dev`; entorno `development`. | ✅ |
| Producción intacta | La base `virgo` no se consultó ni modificó. | ✅ |
| Catálogo local como fuente de verdad | Plan generado desde `src/catalog`; el plan previo detectó 21 validadores y 3 índices pendientes. | ✅ |
| Plan previo | 0 colecciones por crear, 21 validadores por actualizar, 3 índices por crear; 2 índices obsoletos advertidos. | ✅ |
| Operaciones destructivas | El plan no incluía eliminaciones. Los 2 índices obsoletos se eliminaron con autorización explícita; ninguna colección o documento fue eliminado. | ✅ |
| Inicialización | `db:init:dev -- --yes`: 24 operaciones aplicadas. | ✅ |
| Índices obsoletos eliminados | `passwordResets.userId_1` y `bookings.clientId_1_createdAt_-1`, con 0 documentos y verificación exacta de nombre y clave. | ✅ |
| Verificación posterior | `db:verify:dev`: OK. | ✅ |
| Plan posterior | `db:plan:dev`: 0 colecciones, 0 validadores y 0 índices pendientes. | ✅ |
| Colecciones | 22 verificadas. | ✅ |
| Índices | 38 secundarios y 60 totales; 12 únicos, 2 parciales, 4 TTL y 1 geoespacial. | ✅ |
| Validadores | 22 colecciones con `validationLevel: strict` y `validationAction: error`; `db:verify:dev` sin deriva. | ✅ |
| Documentos | 0 antes y después; no hubo eliminación ni pérdida de datos. | ✅ |
| Operaciones pendientes | Ninguna. | ✅ |
