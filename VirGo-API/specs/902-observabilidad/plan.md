# Plan — 902 Observabilidad: X-Ray y tablero de CloudWatch

| Campo | Valor |
|---|---|
| Módulo | `902-observabilidad` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

## 1. Resumen técnico

- `Tracing: Active` en las dos funciones desde `Globals` de `template.yaml`; SAM agrega el permiso de escritura de X-Ray.
- Paquete `AWSXRayRecorder.Handlers.AwsSdk`: registra el SDK de AWS solo cuando la variable `AWS_LAMBDA_FUNCTION_NAME` existe, con la estrategia `LOG_ERROR` si no hay contexto.
- El *middleware* de correlación agrega el `traceId` de `_X_AMZN_TRACE_ID` al alcance del log.
- `AWS::CloudWatch::Dashboard` `virgo-api` con el cuerpo en JSON dentro de la plantilla.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | .NET 10 con `AWSXRayRecorder.Handlers.AwsSdk` |
| Infraestructura | SAM: `Globals.Function.Tracing`, `AWS::CloudWatch::Dashboard` |
| Seguridad | Sin metadatos sensibles en las trazas |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| VII | Costo cero | ✅ | X-Ray y Dashboards en la tabla de servicios (v1.3.0), dentro de la capa gratuita |
| IX | Observabilidad | ✅ | Amplía IX con rastreo y tablero (IX.4 y IX.5 en v1.3.0) |
| V | Seguridad | ✅ | Sin datos personales en trazas |

## 4. Estructura

```text
src/VirGo.Api/Observability/Tracing.cs   # registro condicional del SDK
src/VirGo.Worker/Function.cs             # mismo registro en el worker
infra/template.yaml                      # Tracing y VirGoDashboard
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-40 | X-Ray con muestreo por defecto | OpenTelemetry con ADOT | ADOT agrega una capa y memoria; X-Ray es nativo y gratis en este volumen |
| ADR-41 | Tablero definido como código | Tablero hecho a mano en la consola | Art. VII.4: nada se crea a mano |

## 6. Diseño

### 6.1 Widgets del tablero

| Fila | Widgets |
|---|---|
| API Gateway | `Count`, `4xx`, `5xx`; `Latency` p50, p95 y p99 |
| Lambda API | `Invocations`, `Errors`, `Duration` p95, `Throttles`, `ConcurrentExecutions` |
| Worker | `Invocations`, `Errors`, `Duration` p95 |
| Colas | `ApproximateNumberOfMessagesVisible` de `virgo-jobs` y `virgo-jobs-dlq` |

### 6.2 Seguridad

El SDK de X-Ray solo registra el nombre de la operación, el servicio y el código de respuesta.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| IaC | cfn-lint, Checkov | PR |
| Pruebas | xUnit | PR |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `VirGo.UnitTests` | El rastreo no se registra fuera de Lambda |
| Integración | `VirGo.IntegrationTests` | Las pruebas existentes pasan sin daemon de X-Ray |
| Humo | Consola de X-Ray y CloudWatch | CA-01 a CA-03 |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Excepciones por falta de segmento en local | Registro condicional y `LOG_ERROR` |
| Superar 100 000 trazas | Muestreo por defecto; el presupuesto de $1 avisa |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Rastreo | T90201–T90202 |
| F1 | Tablero | T90203 |
| F2 | Despliegue y cierre | T90204–T90205 |
