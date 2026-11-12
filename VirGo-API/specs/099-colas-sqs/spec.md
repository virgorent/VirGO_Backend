# Spec — 099 Colas con Amazon SQS

| Campo | Valor |
|---|---|
| Módulo | `099-colas-sqs` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Constitución | v1.3.0 (propuesta) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Hay trabajo que no debe hacer esperar a quien usa la app: avisos, correos y tareas de mantenimiento. Este spec agrega una **cola de trabajos** en Amazon SQS y un **worker** que la procesa en segundo plano. El primer uso es avisar al cliente cuando se crea su reservación. Los specs `903` (correo) y `905` (tareas programadas) reutilizan la misma cola.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **cliente**, quiero recibir un aviso en la app cuando se crea mi reservación, sin que la respuesta tarde más. |
| HT-02 | Como **operador**, quiero que un trabajo que falla se reintente y, si sigue fallando, quede guardado para revisarlo, sin perderse. |
| HT-03 | Como **equipo**, quiero una sola cola con tipos de mensaje, para agregar trabajos nuevos sin crear infraestructura cada vez. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Al crear una reservación (`009`), después de guardarla, la API publica en la cola `virgo-jobs` un mensaje de tipo `booking.created` con el `bookingId`, el `clientId`, el folio y la fecha. |
| RF-02 | Un worker (`VirGoWorkerFunction`) consume la cola en lotes de hasta 10 mensajes y, para `booking.created`, crea un aviso en `notices` (`kind = booking`, `read = false`, `purgeAt` a 90 días). |
| RF-03 | El procesamiento es idempotente: el aviso usa el `_id` `booking-created-{bookingId}`; si el mensaje llega dos veces, no se duplica. |
| RF-04 | Un mensaje que falla 3 veces pasa a la cola de errores `virgo-jobs-dlq`, que lo guarda 14 días. Una alarma avisa por correo si la cola de errores tiene mensajes. |
| RF-05 | Si publicar en la cola falla, la reservación igual se crea (fallo abierto) y queda una advertencia en el log con el `correlationId`. |
| RF-06 | Un tipo de mensaje desconocido no bloquea el lote: se registra y se descarta. |
| RF-07 | En local y en pruebas, la cola es en memoria; no hace falta AWS. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** la cola cifra en reposo (SSE-SQS); solo el rol de la API puede enviar y solo el rol del worker puede recibir. Los mensajes llevan identificadores, nunca datos personales ni secretos. |
| RNF-02 | **Rendimiento:** publicar agrega menos de 100 ms al p95 de `POST /api/v1/bookings`. |
| RNF-03 | **Confiabilidad:** el worker informa fallos por mensaje (`ReportBatchItemFailures`), así que solo se reintenta lo que falló. |
| RNF-04 | **Costo:** $0; la capa gratuita de SQS incluye 1 millón de solicitudes al mes. |
| RNF-05 | **Trazabilidad:** el `correlationId` de la solicitud viaja como atributo del mensaje y aparece en los logs del worker. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** un cliente autenticado, **cuando** crea una reservación, **entonces** la respuesta es 201 y la cola recibe un `booking.created` con su `bookingId`. |
| CA-02 | **Dado** un `booking.created` en la cola, **cuando** el worker lo procesa, **entonces** existe un aviso `booking` sin leer para ese cliente. |
| CA-03 | **Dado** el mismo mensaje entregado dos veces, **cuando** el worker lo procesa, **entonces** existe un solo aviso. |
| CA-04 | **Dado** que la cola no responde, **cuando** el cliente crea una reservación, **entonces** recibe 201 y el log registra la advertencia. |
| CA-05 | **Dado** un mensaje que siempre falla, **cuando** se agotan 3 intentos, **entonces** queda en `virgo-jobs-dlq` y la alarma pasa a `ALARM`. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Lectura de mensajes por terceros | Cola sin política pública; acceso solo por los roles de la API y del worker; cifrado SSE-SQS. |
| Datos personales en mensajes | Solo identificadores; el worker lee el resto de MongoDB. |
| Mensajes envenenados | Máximo 3 intentos, cola de errores y alarma. |
| Inyección de tipos de trabajo | Lista cerrada de tipos; lo desconocido se descarta. |

## 7. Fuera de alcance

- Notificaciones *push* al teléfono (FCM).
- Otros eventos (`quote.answered`, `booking.cancelled`); se agregan en el spec de cada endpoint.
- Envío de correo (`903`) y tareas programadas (`905`), que usan esta misma cola.

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- La colección `notices` ya existe con su validador (`VirGo-Database`).

**Dependencias**

- `009-bookings-create`, `900-infraestructura-iac`, `902-observabilidad`.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Qué otros eventos deben generar aviso (cotización respondida, cancelación)? | Equipo |
