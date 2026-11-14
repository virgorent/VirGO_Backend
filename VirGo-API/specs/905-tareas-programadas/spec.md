# Spec — 905 Tareas programadas con EventBridge Scheduler

| Campo | Valor |
|---|---|
| Módulo | `905-tareas-programadas` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Constitución | v1.3.0 (propuesta) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Algunas reglas dependen del tiempo y nadie las dispara con una petición: una cotización cuya fecha de evento ya pasó no debería seguir "enviada". Este spec agrega **tareas programadas** con EventBridge Scheduler, que dejan un trabajo en la cola de `099`. La primera tarea vence cotizaciones.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **propietario o proveedor**, quiero que las cotizaciones de eventos ya pasados dejen de aparecer como pendientes. |
| HT-02 | Como **equipo**, quiero agregar tareas periódicas sin servidores encendidos. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | El horario `virgo-quotes-expire` se ejecuta todos los días a las 00:00 de la Ciudad de México y envía a `virgo-jobs` un mensaje `quotes.expire`. |
| RF-02 | El worker, al procesar `quotes.expire`, cambia a `expired` las cotizaciones en `sent` o `answered` cuya `eventDate` es anterior a la fecha actual de la Ciudad de México, y actualiza `updatedAt`. |
| RF-03 | La tarea es idempotente: correrla dos veces el mismo día no cambia más documentos. |
| RF-04 | El worker registra cuántas cotizaciones venció. |
| RF-05 | Si el envío a la cola falla, Scheduler reintenta hasta 3 veces en 1 hora. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** el rol del horario solo puede `sqs:SendMessage` en `virgo-jobs`. El mensaje no lleva datos. |
| RNF-02 | **Costo:** $0; Scheduler incluye 14 millones de invocaciones al mes. |
| RNF-03 | **Rendimiento:** la tarea termina en menos de 10 s con hasta 10 000 cotizaciones. |
| RNF-04 | **Trazabilidad:** el horario se define en `template.yaml`. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** una cotización `sent` con `eventDate` de ayer, **cuando** corre `quotes.expire`, **entonces** queda `expired`. |
| CA-02 | **Dado** una cotización `accepted` con fecha pasada, **cuando** corre la tarea, **entonces** no cambia. |
| CA-03 | **Dado** una cotización `sent` con fecha de hoy o futura, **cuando** corre la tarea, **entonces** no cambia. |
| CA-04 | **Dado** el stack desplegado, **cuando** abro EventBridge → Programaciones, **entonces** existe `virgo-quotes-expire` con la zona `America/Mexico_City`. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Uso del rol del horario para otra cosa | Solo `sqs:SendMessage` en una cola. |
| Cambios masivos por error | Filtro cerrado por estado y fecha; pruebas de los tres casos. |

## 7. Fuera de alcance

- Generar `monthlyStats` (pregunta abierta P-01).
- Recordatorios de eventos próximos.

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- `eventDate` se guarda como texto `AAAA-MM-DD`.

**Dependencias**

- `099-colas-sqs`.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Se agrega la tarea mensual de `monthlyStats` en este spec o en el de estadísticas? | Equipo |
