# Check — NNN <nombre-del-módulo>

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `NNN-<nombre>` |
| Estado del spec | Borrador / En revisión / Aprobado / Publicado / Cerrado |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | <enlace o acta> | ⏳ |
| Constitution Check sin ❌ | <enlace a plan.md> | ⏳ |
| Sin secretos en el repositorio | <ejecución de Gitleaks> | ⏳ |
| Preguntas abiertas respondidas | <...> | ⏳ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | <...> | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | <comando> · totales: — | ⏳ |
| Integración | <...> | ⏳ |
| Seguridad | <...> | ⏳ |
| Humo | <...> | ⏳ |

## 4. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Gitleaks sin hallazgos | <enlace al check> | ⏳ |
| Dependency review sin vulnerabilidades altas | <enlace al check> | ⏳ |
| SAST (CodeQL / linters) en verde | <enlace al check> | ⏳ |
| Workflows con permisos mínimos | <revisión de `permissions`> | ⏳ |
| Sin credenciales en logs ni artefactos | <revisión> | ⏳ |

## 5. PR e integración continua

- [ ] PRs del módulo: enlaces —
- [ ] Checks requeridos en verde y una revisión.

## 6. Release

- [ ] Versión y commit: —
- [ ] Artefacto firmado y verificado: —

## 7. Memoria y aprendizaje

- [ ] Entrada de cierre en `memoria.md`.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes (o «ninguna»).
