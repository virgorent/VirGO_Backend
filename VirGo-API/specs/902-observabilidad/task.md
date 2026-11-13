# Tareas — 902 Observabilidad: X-Ray y tablero de CloudWatch

| Campo | Valor |
|---|---|
| Módulo | `902-observabilidad` |
| Versión | 0.1.0 |
| Estado | Borrador. **Las tareas no se ejecutan hasta aprobar este archivo.** |
| Fecha | 2026-11-11 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T90201 | `Tracing: Active` en `Globals` para la API y el worker | `099`: T9904 | `devops-aws` | `sam validate --lint`; pruebas en verde sin AWS (CA-04) | ✅ local |
| T90202 | Agregar el `traceId` al alcance del log junto al `correlationId` | T90201 | `dev-dotnet` | Prueba unitaria | ✅ local |
| T90203 | `VirGoDashboard` con los widgets de la sección 6.1 del plan | `099`: T9904 | `devops-aws` | `sam validate --lint`, cfn-lint | ✅ local |
| T90204 | 🧑 Desplegar y verificar la traza, el mapa de servicios y el tablero | T90201–T90203 | `devops-aws` | CA-01 a CA-03 | ✅ 2026-11-13 |
| T90205 | Completar `check.md` y registrar en `memoria.md` | T90204 | `qa` | `check.md` sin ⏳ | ⏳ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| HT-01, RF-01, RF-03 | T90201 |
| HT-02, RF-04 | T90203 |
| RF-05 | T90202 |
| RF-06, CA-04 | T90201 |
| RNF-01 … RNF-04 | T90201 · T90204 · T90203 · T90203 |
| CA-01 … CA-03 | T90204 |
