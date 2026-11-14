# Plan — 903 Correo con Amazon SES

| Campo | Valor |
|---|---|
| Módulo | `903-correo-ses` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

## 1. Resumen técnico

- `Application` define `IEmailSender` y las plantillas `EmailTemplates`.
- `Infrastructure` implementa `SesEmailSender` (AWSSDK.SimpleEmailV2) y `LoggingEmailSender`.
- El worker agrega `EmailSendHandler` para el tipo `email.send`.
- `template.yaml` recibe el parámetro `SenderEmail` y da al worker el permiso de enviar solo desde esa dirección. La identidad se crea una vez con `aws sesv2 create-email-identity` y se verifica abriendo el correo (ADR-52).

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | .NET 10, `AWSSDK.SimpleEmailV2` |
| Seguridad | Política `ses:SendEmail` sobre la identidad, con condición `ses:FromAddress` |
| Configuración | `Email__SenderAddress` en el worker; vacío = `LoggingEmailSender` |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| III | Capas | ✅ | La interfaz en `Application`; SES en `Infrastructure` |
| V | Seguridad | ✅ | Mínimo privilegio; códigos fuera de logs |
| VII | Costo cero | ✅ | SES en la tabla de servicios (v1.3.0); solo *sandbox* |

## 4. Estructura

```text
src/VirGo.Application/Email/      # IEmailSender, EmailTemplates, EmailMessage
src/VirGo.Infrastructure/Email/   # SesEmailSender, LoggingEmailSender
src/VirGo.Worker/Handlers/        # EmailSendHandler
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-42 | SES en *sandbox* con un remitente verificado por correo | Dominio propio con DKIM; proveedor externo | Sin costo de dominio; suficiente para la demo |
| ADR-43 | Enviar desde el worker vía la cola | Enviar desde la API | La API no espera a SES y los fallos se reintentan |
| ADR-52 | La identidad de SES vive fuera del stack | `AWS::SES::EmailIdentity` en la plantilla | La verificación necesita un clic humano; si el stack la crea y se revierte, la identidad queda huérfana y bloquea el siguiente despliegue (`NAME_CONFLICT_VALIDATION`, 2026-11-13) |

## 6. Diseño

### 6.1 Mensaje

```json
{ "type": "email.send", "correlationId": "…", "data": { "template": "password-reset", "to": "persona@correo.com", "code": "482913", "expiresInMinutes": 15 } }
```

### 6.2 Seguridad

La plantilla recibe el código como parámetro; el *handler* nunca registra `to` completo ni `code`.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| IaC | cfn-lint, Checkov | PR |
| Pruebas | xUnit | PR |
| SAST | CodeQL, Semgrep | PR |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `VirGo.UnitTests/Email` | Plantillas, enmascarado del correo, *handler* con un `IEmailSender` falso |
| Seguridad | `VirGo.SecurityTests` | El log no contiene el código ni el correo completo |
| Humo | T90304 | Correo real a una dirección verificada |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Correo en *spam* (Gmail sin DKIM) | Documentado en P-01; revisar la carpeta de *spam* en la demo |
| Superar el límite del *sandbox* | 200 al día; reintentos limitados |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Interfaz, plantillas y emisores | T90301 |
| F1 | *Handler* e infraestructura | T90302–T90303 |
| F2 | Despliegue y cierre | T90304–T90305 |
