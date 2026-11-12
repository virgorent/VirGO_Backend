# Check — 905 Tareas programadas con EventBridge Scheduler

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `905-tareas-programadas` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | PR con la revisión del equipo | ⏳ |
| Constitution Check sin ❌ | [plan.md](plan.md#3-constitution-check) | ⏳ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Vence `sent` con fecha pasada | Prueba | ⏳ |
| CA-02 No toca `accepted` | Prueba | ⏳ |
| CA-03 No toca fechas de hoy o futuras | Prueba | ⏳ |
| CA-04 Horario creado | Captura de EventBridge | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `dotnet test tests/VirGo.UnitTests` | ⏳ |
| IaC | cfn-lint, Checkov | ⏳ |
| Humo | T90504 | ⏳ |
