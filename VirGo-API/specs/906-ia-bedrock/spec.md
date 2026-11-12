# Spec — 906 Inteligencia artificial con Amazon Bedrock

| Campo | Valor |
|---|---|
| Módulo | `906-ia-bedrock` |
| Versión | 0.1.0 |
| Estado | Borrador. **No se implementa hasta aprobar el límite de gasto (P-01).** |
| Fecha | 2026-11-11 |
| Constitución | v1.3.0 (propuesta) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

El asistente de planeación (`093`–`095` en la API; `012-ia-studio` y `013-asistente-ia` en Móvil) necesita un modelo de lenguaje. Este spec define cómo usar **Amazon Bedrock** con un **gasto máximo fijo**, porque Bedrock cobra por uso y no tiene capa gratuita permanente.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **cliente**, quiero preguntarle al asistente ideas para mi evento y recibir una respuesta en segundos. |
| HT-02 | Como **responsable del proyecto**, quiero que el asistente nunca gaste más del límite aprobado. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | `POST /api/v1/assistant/messages` (`094`) envía al modelo el mensaje y, como contexto, los últimos 10 mensajes de la conversación. |
| RF-02 | El modelo y el máximo de *tokens* de respuesta se configuran (`Assistant__ModelId`, `Assistant__MaxOutputTokens`), sin cambiar código. |
| RF-03 | Cada persona tiene un máximo de mensajes al día (`Assistant__DailyLimitPerUser`, inicial 20); al superarlo recibe 429 en Problem Details. |
| RF-04 | Hay un tope global de mensajes al día para toda la app; al superarlo, el asistente responde 503 con un mensaje claro. |
| RF-05 | Con `Assistant__Enabled = false` (valor por defecto) el endpoint responde 503 y no llama a Bedrock. |
| RF-06 | Las instrucciones del sistema limitan al asistente a temas de planeación de eventos y le prohíben pedir o repetir datos personales. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Costo:** presupuesto de AWS exclusivo para Bedrock con alerta al 50 % y al 100 % del límite aprobado; el tope de RF-04 se calcula para no superarlo. |
| RNF-02 | **Seguridad:** la Lambda solo puede `bedrock:InvokeModel` sobre el modelo configurado; sin otros permisos de Bedrock. |
| RNF-03 | **Privacidad:** no se envían al modelo correos, teléfonos ni datos de pago; los mensajes se borran a los 30 días (`095`). |
| RNF-04 | **Rendimiento:** respuesta en menos de 10 s en el p95. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** `Assistant__Enabled = false`, **cuando** envío un mensaje, **entonces** recibo 503 y Bedrock no registra invocaciones. |
| CA-02 | **Dado** una persona que ya envió 20 mensajes hoy, **cuando** envía otro, **entonces** recibe 429. |
| CA-03 | **Dado** el asistente activo, **cuando** pregunto por ideas para una boda, **entonces** recibo una respuesta en español en menos de 10 s. |
| CA-04 | **Dado** la cuenta de AWS, **cuando** reviso Budgets, **entonces** existe el presupuesto de Bedrock con sus dos alertas. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Gasto descontrolado | Límite por persona, tope global, presupuesto y bandera de apagado. |
| *Prompt injection* | Instrucciones del sistema fijas; el modelo no tiene herramientas ni acceso a datos. |
| Fuga de datos personales | Filtro antes de enviar y retención de 30 días. |

## 7. Fuera de alcance

- IA Studio con generación de imágenes.
- Uso de herramientas o búsqueda en el catálogo por el modelo.

## 8. Supuestos, dependencias y preguntas abiertas

**Dependencias**

- `093`–`095`, `902-observabilidad`.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Cuál es el gasto máximo mensual aprobado para Bedrock (por ejemplo, $5 de los créditos)? | Angelica |
| P-02 | ¿Qué modelo se usa? Se elige el más económico que responda bien en español, confirmando su ID y su precio vigente en la consola de Bedrock de `us-east-1`. | Equipo |
| P-03 | ¿La cuenta en plan Free tiene acceso a Bedrock o hay que pasar al plan de pago? | Angelica |
