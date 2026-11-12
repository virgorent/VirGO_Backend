# Tareas — 099 Colas con Amazon SQS

| Campo | Valor |
|---|---|
| Módulo | `099-colas-sqs` |
| Versión | 0.1.0 |
| Estado | Borrador. **Las tareas no se ejecutan hasta aprobar este archivo.** |
| Fecha | 2026-11-11 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**🧑 Requiere confirmación humana** = crea recursos en AWS.

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9901 | `IJobPublisher`, `JobMessage`, `JobTypes` en `Application` e `InMemoryJobPublisher` en `Infrastructure` | — | `dev-dotnet` | Compila; pruebas unitarias | ✅ local |
| T9902 | `SqsJobPublisher` y publicación de `booking.created` en `BookingService` con fallo abierto | T9901 | `dev-dotnet` | CA-01, CA-04 | ✅ local |
| T9903 | Proyecto `VirGo.Worker` con `JobDispatcher` y `BookingCreatedHandler` idempotente | T9901 | `dev-dotnet` | CA-02, CA-03 | ✅ local |
| T9904 | `template.yaml`: `virgo-jobs`, `virgo-jobs-dlq`, `VirGoWorkerFunction`, permisos y alarma de la DLQ | T9903 | `devops-aws` | `sam validate --lint`, cfn-lint, Checkov | ✅ local |
| T9905 | Pruebas unitarias e integración de la cola y del worker | T9902, T9903 | `qa` | `dotnet test` en verde | ✅ local |
| T9906 | Registrar el costo estimado en `memoria.md` (Art. VII.5) | T9904 | `devops-aws` | Entrada en `memoria.md` | ✅ local |
| T9907 | 🧑 Desplegar y probar: reservación → aviso en `notices`; mensaje inválido → DLQ y alarma | T9904, T9905 | `devops-aws` | CA-01 a CA-05 en la nube | ⏳ |
| T9908 | Completar `check.md` y registrar el cierre en `memoria.md` | T9907 | `qa` | `check.md` sin ⏳ | ⏳ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| HT-01, RF-01, RF-02 | T9902, T9903 |
| HT-02, RF-04 | T9904, T9907 |
| HT-03, RF-06 | T9901, T9903 |
| RF-03 | T9903 |
| RF-05 | T9902 |
| RF-07 | T9901 |
| RNF-01 … RNF-05 | T9904 · T9905 · T9903 · T9906 · T9902 |
| CA-01 … CA-05 | T9902 · T9903 · T9903 · T9902 · T9907 |
