# Spec — 903 Correo con Amazon SES

| Campo | Valor |
|---|---|
| Módulo | `903-correo-ses` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Constitución | v1.3.0 (propuesta) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

`011-post-auth-password-forgot` y `014-post-auth-email-verify` necesitan enviar códigos por correo. Este spec deja lista la **capacidad de enviar correo** con Amazon SES, a través de la cola de `099`, para que esos endpoints solo publiquen un mensaje.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **persona que olvidó su contraseña**, quiero recibir un código por correo para recuperar mi cuenta. |
| HT-02 | Como **persona recién registrada**, quiero recibir un código para verificar mi correo. |
| HT-03 | Como **equipo**, quiero que enviar correo no haga esperar a la API ni la haga fallar si SES falla. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Existe una identidad de SES para el remitente (parámetro `SenderEmail`), verificada por correo. |
| RF-02 | El worker procesa mensajes `email.send` con `template` (`password-reset` o `email-verification`), `to` y `code`, y envía el correo con SES. |
| RF-03 | Las plantillas están en español, en texto plano y HTML, e indican la vigencia del código y qué hacer si no se pidió. |
| RF-04 | Un error de SES hace fallar solo ese mensaje, que se reintenta según `099` (3 intentos y cola de errores). |
| RF-05 | En local y en pruebas, el envío usa `LoggingEmailSender`, que registra el envío sin el destinatario completo ni el código. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** el worker solo puede usar `ses:SendEmail` con la identidad del remitente. Los logs enmascaran el correo (`a***@gmail.com`) y nunca muestran el código. |
| RNF-02 | **Privacidad:** el código viaja en el mensaje cifrado de la cola y en el correo, nunca en logs ni trazas. |
| RNF-03 | **Costo:** SES en *sandbox*: hasta 200 correos al día, solo a direcciones verificadas. Se usan los créditos del plan; el costo esperado es menor a $0.01. |
| RNF-04 | **Entrega:** el correo llega en menos de 1 minuto en condiciones normales. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el stack desplegado, **cuando** abro SES → Identidades, **entonces** el remitente aparece como verificado. |
| CA-02 | **Dado** un `email.send` con `password-reset` a un correo verificado, **cuando** el worker lo procesa, **entonces** el correo llega con el código. |
| CA-03 | **Dado** un error de SES, **cuando** el worker procesa el mensaje, **entonces** solo ese mensaje se reintenta. |
| CA-04 | **Dado** los logs del worker, **cuando** busco el código o el correo completo, **entonces** no aparecen. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Uso de SES para spam | Solo el worker envía; permiso limitado a la identidad; *sandbox* con límites. |
| Fuga del código | Sin código en logs ni trazas; cola cifrada. |
| Suplantación del remitente | Solo la identidad verificada puede enviar. |

## 7. Fuera de alcance

- Los endpoints `011` y `014`: generan el código, lo guardan con hash y publican `email.send`.
- Salir del *sandbox* y usar un dominio propio con DKIM (requiere un dominio de pago, Art. VII.2).
- Manejo de rebotes y quejas.

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- En *sandbox*, cada destinatario de prueba se verifica en SES.

**Dependencias**

- `099-colas-sqs`.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Qué correo se usa como remitente? Una dirección de Gmail puede caer en *spam* porque SES no la firma con DKIM de Gmail. | Angelica |
| P-02 | ¿Se solicita salir del *sandbox* para la demo con usuarios reales? | Equipo |
