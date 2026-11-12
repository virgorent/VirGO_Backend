# Check — 902 Observabilidad: X-Ray y tablero de CloudWatch

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `902-observabilidad` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | PR con la revisión del equipo | ⏳ |
| Constitution Check sin ❌ | [plan.md](plan.md#3-constitution-check) | ⏳ |
| Sin secretos en el repositorio | Gitleaks | ⏳ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Traza de la Lambda | Captura de X-Ray | ⏳ |
| CA-02 Traza del worker | Captura de X-Ray | ⏳ |
| CA-03 Tablero `virgo-api` | Captura de CloudWatch | ⏳ |
| CA-04 Sin errores en local | `dotnet test` | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `dotnet test tests/VirGo.UnitTests` | ⏳ |
| IaC | `sam validate --lint`, cfn-lint, Checkov | ⏳ |
| Humo | T90204 | ⏳ |
