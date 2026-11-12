# Spec — 902 Observabilidad: X-Ray y tablero de CloudWatch

| Campo | Valor |
|---|---|
| Módulo | `902-observabilidad` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Constitución | v1.3.0 (propuesta) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Hoy solo hay logs y dos alarmas. Para saber **dónde** se tarda o falla una petición, y para ver la salud de la API de un vistazo, se agregan el **rastreo distribuido con AWS X-Ray** y un **tablero de CloudWatch**.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **desarrollador**, quiero ver el recorrido de una petición (Lambda → MongoDB, SQS, SSM) con el tiempo de cada parte, para encontrar la causa de una falla o una demora. |
| HT-02 | Como **operador**, quiero un tablero con peticiones, errores, latencia, cola y worker en una sola pantalla. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Las funciones de la API y del worker tienen el rastreo de X-Ray activo. |
| RF-02 | Las llamadas al SDK de AWS (SQS, SES, SSM) aparecen como subsegmentos de la traza. |
| RF-03 | El mapa de servicios de X-Ray muestra la API, el worker y los servicios de AWS que usan. |
| RF-04 | Existe el tablero `virgo-api` con: peticiones, 4xx y 5xx, latencia p50/p95/p99 de API Gateway; invocaciones, errores, duración y *throttles* de las dos Lambdas; mensajes visibles en `virgo-jobs` y en su cola de errores. |
| RF-05 | Cada log de la API incluye el identificador de traza de X-Ray junto al `correlationId`. |
| RF-06 | Fuera de AWS (local y pruebas), el rastreo está apagado y no produce errores. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** las trazas no incluyen cuerpos de petición, tokens ni datos personales; solo rutas, códigos y tiempos. |
| RNF-02 | **Rendimiento:** el rastreo agrega menos de 5 ms al p95. |
| RNF-03 | **Costo:** $0; X-Ray incluye 100 000 trazas al mes y CloudWatch 3 tableros. Se usa el muestreo por defecto (1 por segundo y 5 % del resto). |
| RNF-04 | **Trazabilidad:** el tablero y el rastreo se definen en `template.yaml`. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el stack desplegado, **cuando** llamo a `/api/v1/venues`, **entonces** en X-Ray aparece una traza con el segmento de la Lambda. |
| CA-02 | **Dado** una reservación creada, **cuando** abro su traza, **entonces** aparece el subsegmento de SQS. |
| CA-03 | **Dado** el stack desplegado, **cuando** abro CloudWatch → Paneles, **entonces** existe `virgo-api` con todos los widgets de RF-04 con datos. |
| CA-04 | **Dado** las pruebas locales, **cuando** corren sin AWS, **entonces** pasan sin errores de X-Ray. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Datos sensibles en trazas | Solo metadatos; sin cuerpos ni encabezados de autorización. |
| Acceso a las trazas | Solo usuarios de la cuenta con permisos de lectura de X-Ray. |

## 7. Fuera de alcance

- Rastreo de API Gateway: el HTTP API no admite X-Ray; la traza empieza en la Lambda.
- Rastreo de las consultas a MongoDB como subsegmentos propios.
- CloudTrail (descartado por el equipo); la actividad de la cuenta se consulta en el historial de eventos gratuito de 90 días.

## 8. Supuestos, dependencias y preguntas abiertas

**Dependencias**

- `900-infraestructura-iac` y `099-colas-sqs` (métricas de la cola).

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Se agrega una alarma de latencia p95 > 1 s? | Equipo |
