# Plan — 905 Tareas programadas con EventBridge Scheduler

| Campo | Valor |
|---|---|
| Módulo | `905-tareas-programadas` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

## 1. Resumen técnico

- `AWS::Scheduler::Schedule` `virgo-quotes-expire`: `cron(0 0 * * ? *)` en `America/Mexico_City`, destino la cola `virgo-jobs` con el cuerpo `{"type":"quotes.expire"}`.
- `SchedulerRole`: confianza en `scheduler.amazonaws.com` con condición `aws:SourceAccount` y permiso `sqs:SendMessage` solo en `virgo-jobs`.
- `IQuoteExpiry` en `Application` y su implementación con `UpdateMany` en `Infrastructure`.
- `QuotesExpireHandler` en el worker.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | .NET 10, MongoDB.Driver |
| Infraestructura | `AWS::Scheduler::Schedule` y rol propio |
| Zona horaria | `America/Mexico_City` (UTC−6, sin horario de verano) |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| III | Capas | ✅ | La regla en `Application`; la consulta en `Infrastructure` |
| V | Seguridad | ✅ | Rol del horario con una sola acción sobre una cola |
| VII | Costo cero | ✅ | Scheduler en la tabla de servicios (v1.3.0) |

## 4. Estructura

```text
src/VirGo.Application/Jobs/IQuoteExpiry.cs
src/VirGo.Infrastructure/Jobs/MongoQuoteExpiry.cs
src/VirGo.Worker/Handlers/QuotesExpireHandler.cs
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-46 | El horario deja un mensaje en la cola en lugar de invocar la Lambda | Invocar el worker directo | Reutiliza los reintentos, la cola de errores y la alarma de `099` |
| ADR-47 | EventBridge Scheduler | Reglas programadas de EventBridge | Admite zona horaria y reintentos propios |

## 6. Diseño

### 6.1 Filtro

```text
status ∈ { sent, answered } AND eventDate < hoy (America/Mexico_City)
→ $set: { status: "expired", updatedAt: ahora }
```

### 6.2 Seguridad

El horario no recibe datos de entrada ni secretos.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| IaC | cfn-lint, Checkov | PR |
| Pruebas | xUnit | PR |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `VirGo.UnitTests/Jobs` | Cálculo de la fecha en la zona horaria y despacho del *handler* |
| Integración | `VirGo.IntegrationTests` | CA-01 a CA-03 con el repositorio en memoria |
| Humo | T90504 | Enviar `quotes.expire` a mano y revisar el log del worker |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Consulta sin índice por `status` y `eventDate` | Volumen bajo; si crece, se pide el índice a `VirGo-Database` |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Regla y *handler* | T90501–T90502 |
| F1 | Horario | T90503 |
| F2 | Despliegue y cierre | T90504–T90505 |
