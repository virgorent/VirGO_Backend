# Tareas — 906 Inteligencia artificial con Amazon Bedrock

| Campo | Valor |
|---|---|
| Módulo | `906-ia-bedrock` |
| Versión | 0.1.0 |
| Estado | Borrador. **Bloqueado hasta responder P-01, P-02 y P-03 del spec.** |
| Fecha | 2026-11-11 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T90601 | 🧑 Aprobar el gasto máximo, elegir el modelo con su precio vigente y confirmar el acceso desde el plan Free | — | Angelica | Respuestas en `memoria.md` | ⛔ |
| T90602 | `IAssistantModel`, `AssistantService` con límites y `BedrockAssistantModel` | T90601, `093`–`095` | `dev-dotnet` | CA-01, CA-02 | ⛔ |
| T90603 | `template.yaml`: parámetro `EnableAssistant`, permiso condicionado y presupuesto `virgo-bedrock` | T90601 | `devops-aws` | cfn-lint, Checkov | ⛔ |
| T90604 | 🧑 Activar el asistente y hacer una prueba real | T90602, T90603 | `devops-aws` | CA-03, CA-04 | ⛔ |
| T90605 | Completar `check.md` y registrar en `memoria.md` | T90604 | `qa` | `check.md` sin ⏳ | ⛔ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01, RF-02, RF-06, HT-01 | T90602 |
| RF-03 … RF-05, HT-02 | T90602, T90603 |
| RNF-01 … RNF-04 | T90603 · T90603 · T90602 · T90604 |
| CA-01 … CA-04 | T90602 · T90602 · T90604 · T90604 |
