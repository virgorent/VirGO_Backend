# Plan — 906 Inteligencia artificial con Amazon Bedrock

| Campo | Valor |
|---|---|
| Módulo | `906-ia-bedrock` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

## 1. Resumen técnico

- `Application` define `IAssistantModel` y el servicio con límites por persona y globales.
- `Infrastructure` implementa `BedrockAssistantModel` con la API `Converse` de `AWSSDK.BedrockRuntime`.
- Parámetro `EnableAssistant` (por defecto `false`) en `template.yaml`: solo con `true` se agrega el permiso `bedrock:InvokeModel` sobre el ID del modelo.
- Presupuesto de AWS `virgo-bedrock` con filtro por servicio.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | .NET 10, `AWSSDK.BedrockRuntime` (API `Converse`) |
| Persistencia | `assistantMessages` (TTL de 30 días) y un contador diario |
| Seguridad | Permiso condicionado; bandera de apagado |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| VII | Costo | ⚠️ | Bedrock no es gratis: se agrega a la tabla como servicio **con límite de gasto aprobado** (v1.3.0, VII.6). Bloquea la implementación hasta responder P-01 |
| V | Seguridad | ✅ | Mínimo privilegio y filtros de privacidad |

## 4. Estructura

```text
src/VirGo.Application/Assistant/      # IAssistantModel, AssistantService, límites
src/VirGo.Infrastructure/Assistant/   # BedrockAssistantModel
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-48 | API `Converse` de Bedrock | `InvokeModel` con el formato de cada proveedor | Una sola interfaz para cambiar de modelo por configuración |
| ADR-49 | Apagado por defecto | Activo al desplegar | Ningún gasto hasta aprobarlo |

## 6. Diseño

### 6.1 Límites

| Límite | Valor inicial |
|---|---|
| Mensajes por persona al día | 20 |
| Mensajes totales al día | Se calcula con el gasto aprobado y el precio del modelo |
| *Tokens* de salida por respuesta | 500 |
| Historial enviado | Últimos 10 mensajes |

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| IaC | cfn-lint, Checkov | PR |
| Pruebas | xUnit con un modelo falso | PR |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `VirGo.UnitTests/Assistant` | Límites, bandera de apagado y filtro de datos personales |
| Seguridad | `VirGo.SecurityTests` | 401, 429 y textos de 1500 caracteres |
| Humo | T90604 | Una pregunta real con el asistente activo |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Consumir los créditos | Presupuesto, topes y apagado por defecto |
| El plan Free no permite Bedrock | P-03 antes de implementar |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Decisiones de costo y modelo | T90601 |
| F1 | Implementación | T90602–T90603 |
| F2 | Activación y cierre | T90604–T90605 |
