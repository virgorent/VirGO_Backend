# Check — 099 Colas con Amazon SQS

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `099-colas-sqs` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | PR con la revisión del equipo | ⏳ |
| Constitution Check sin ❌ | [plan.md](plan.md#3-constitution-check) | ⏳ |
| Enmienda v1.3.0 (Art. VII) aprobada | PR | ⏳ |
| Sin secretos en el repositorio | Gitleaks | ⏳ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Reservación publica `booking.created` | Prueba de integración | ⏳ |
| CA-02 El worker crea el aviso | Prueba unitaria y humo en la nube | ⏳ |
| CA-03 Sin duplicados | Prueba unitaria | ⏳ |
| CA-04 Fallo abierto | Prueba unitaria | ⏳ |
| CA-05 DLQ y alarma | Prueba en la nube (T9907) | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `dotnet test tests/VirGo.UnitTests` | ⏳ |
| Integración | `dotnet test tests/VirGo.IntegrationTests` | ⏳ |
| IaC | `sam validate --lint`, cfn-lint, Checkov | ⏳ |
| Humo | T9907 | ⏳ |

## 4. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Cola cifrada y sin política pública | Checkov | ⏳ |
| Mínimo privilegio | Políticas de SAM en `template.yaml` | ⏳ |
| Costo registrado | `memoria.md` (T9906) | ⏳ |
