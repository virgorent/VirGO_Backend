# Plan — 099 Colas con Amazon SQS

| Campo | Valor |
|---|---|
| Módulo | `099-colas-sqs` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

## 1. Resumen técnico

- `Application` define `IJobPublisher` y el contrato `JobMessage` (tipo, carga útil y `correlationId`).
- `Infrastructure` implementa `SqsJobPublisher` (AWSSDK.SQS) y `InMemoryJobPublisher` para local y pruebas.
- `BookingService` publica `booking.created` después de guardar, con fallo abierto.
- Nuevo proyecto `src/VirGo.Worker`: una Lambda que recibe eventos de SQS y despacha por tipo a un `IJobHandler`.
- `infra/template.yaml` agrega la cola, la cola de errores, la función del worker, sus permisos y la alarma.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | .NET 10; `Amazon.Lambda.SQSEvents` y `Amazon.Lambda.Core` en el worker |
| Persistencia | MongoDB Atlas (`notices`), con el mismo `IMongoDatabase` de `Infrastructure` |
| Seguridad | `SQSSendMessagePolicy` para la API y `SQSPollerPolicy` para el worker; SSE-SQS |
| CI/CD | Mismas pruebas de `api-build-test`; Checkov y cfn-lint validan la cola |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| III | Capas | ✅ | La interfaz vive en `Application`; SQS solo en `Infrastructure` |
| V | Seguridad | ✅ | Mínimo privilegio por rol, cifrado y mensajes sin datos personales |
| VI | Pruebas | ✅ | Unitarias del worker y de integración con la cola en memoria |
| VII | Costo cero | ✅ | SQS y la Lambda del worker se agregan a la tabla de servicios (v1.3.0) |
| IX | Observabilidad | ✅ | `correlationId` en los atributos y trazas de X-Ray (`902`) |

## 4. Estructura

```text
src/
├── VirGo.Application/Jobs/        # IJobPublisher, JobMessage, JobTypes
├── VirGo.Infrastructure/Jobs/     # SqsJobPublisher, InMemoryJobPublisher
└── VirGo.Worker/                  # Function (SQS), JobDispatcher, BookingCreatedHandler
tests/VirGo.UnitTests/Jobs/        # dispatcher, idempotencia, fallo abierto
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-37 | Una cola estándar `virgo-jobs` con tipos de mensaje | Una cola por trabajo; cola FIFO | Menos recursos; el orden no importa y el consumidor es idempotente |
| ADR-38 | Worker en una Lambda aparte | Procesar dentro de la Lambda de la API | Escala y falla por separado; la API no espera |
| ADR-39 | Publicar con fallo abierto | Transacción con *outbox* | El aviso no es crítico; el *outbox* se evalúa si aparecen eventos críticos |

## 6. Diseño

### 6.1 Mensaje

```json
{ "type": "booking.created", "correlationId": "…", "data": { "bookingId": "…", "clientId": "…", "folio": "VG-000123", "date": "2026-12-05" } }
```

El tipo también va como atributo `type` y el `correlationId` como atributo `correlationId`.

### 6.2 Cola

| Propiedad | Valor |
|---|---|
| `virgo-jobs` | Estándar, `VisibilityTimeout` 60 s (6 veces el *timeout* del worker), retención 4 días, SSE-SQS |
| `virgo-jobs-dlq` | Retención 14 días, SSE-SQS; `maxReceiveCount` 3 |
| Worker | 256 MB, 10 s, lote 10, `ReportBatchItemFailures` |
| Alarma | `ApproximateNumberOfMessagesVisible` de la DLQ > 0 durante 5 min → SNS `virgo-alerts` |

### 6.3 Seguridad

La API recibe `Jobs__QueueUrl` como variable; si está vacía usa la cola en memoria. Ningún secreto viaja en el mensaje.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| Secretos | Gitleaks | PR y push a `main` |
| SAST | CodeQL, Semgrep | PR |
| IaC | cfn-lint, Checkov | PR |
| Pruebas | xUnit (`api-build-test`) | PR |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `VirGo.UnitTests/Jobs` | Despacho por tipo, tipo desconocido, idempotencia del aviso, fallo abierto al publicar |
| Integración | `VirGo.IntegrationTests` | `POST /bookings` deja un `booking.created` en la cola en memoria |
| Humo | Tras desplegar | Crear una reservación y ver el aviso en `notices` |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Mensajes duplicados (entrega al menos una vez) | `_id` determinista del aviso |
| Bucle de reintentos | `maxReceiveCount` 3 y DLQ con alarma |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Contrato y cola en memoria | T9901 |
| F1 | Publicación y worker | T9902–T9903 |
| F2 | Infraestructura y pruebas | T9904–T9906 |
| F3 | Despliegue y cierre | T9907–T9908 |
