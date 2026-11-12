# Tareas — 905 Tareas programadas con EventBridge Scheduler

| Campo | Valor |
|---|---|
| Módulo | `905-tareas-programadas` |
| Versión | 0.1.0 |
| Estado | Borrador. **Las tareas no se ejecutan hasta aprobar este archivo.** |
| Fecha | 2026-11-11 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T90501 | `IQuoteExpiry`, implementación en memoria y en MongoDB | — | `dev-dotnet` | CA-01 a CA-03 | ✅ local |
| T90502 | `QuotesExpireHandler` en el worker | T90501, `099`: T9903 | `dev-dotnet` | Prueba unitaria | ✅ local |
| T90503 | `template.yaml`: `QuotesExpireSchedule` y `SchedulerRole` | `099`: T9904 | `devops-aws` | cfn-lint, Checkov | ✅ local |
| T90504 | 🧑 Desplegar, revisar el horario y enviar `quotes.expire` a mano | T90502, T90503 | `devops-aws` | CA-04 y log del worker | ⏳ |
| T90505 | Completar `check.md` y registrar en `memoria.md` | T90504 | `qa` | `check.md` sin ⏳ | ⏳ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01, RF-05, HT-02 | T90503 |
| RF-02 … RF-04, HT-01 | T90501, T90502 |
| RNF-01 … RNF-04 | T90503 · T90503 · T90501 · T90503 |
| CA-01 … CA-04 | T90501 · T90501 · T90501 · T90504 |
