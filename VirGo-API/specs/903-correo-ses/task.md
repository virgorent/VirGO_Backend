# Tareas — 903 Correo con Amazon SES

| Campo | Valor |
|---|---|
| Módulo | `903-correo-ses` |
| Versión | 0.1.0 |
| Estado | Borrador. **Las tareas no se ejecutan hasta aprobar este archivo.** |
| Fecha | 2026-11-11 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**🧑 Requiere confirmación humana** = crea recursos en AWS o requiere abrir un correo.

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T90301 | `IEmailSender`, `EmailTemplates`, `SesEmailSender` y `LoggingEmailSender` | — | `dev-dotnet` | Pruebas unitarias | ✅ local |
| T90302 | `EmailSendHandler` en el worker | T90301, `099`: T9903 | `dev-dotnet` | CA-03, CA-04 | ✅ local |
| T90303 | `template.yaml`: parámetro `SenderEmail` y permiso del worker limitado a esa dirección | `099`: T9904 | `devops-aws` | cfn-lint, Checkov | ✅ local |
| T90304 | 🧑 Desplegar con `SenderEmail` y enviar un correo de prueba | T90302, T90303 | `devops-aws` | CA-02 | ✅ 2026-11-13 |
| T90306 | 🧑 Crear y verificar la identidad de SES `anyiarguelles34@gmail.com` | — | Angelica | `aws sesv2 get-email-identity`: `VerificationStatus = SUCCESS` (CA-01) | ✅ 2026-11-13 |
| T90305 | Completar `check.md` y registrar en `memoria.md` | T90304 | `qa` | `check.md` sin ⏳ | ⏳ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01, CA-01 | T90303, T90304 |
| RF-02, RF-03, HT-01, HT-02 | T90301, T90302 |
| RF-04, HT-03, CA-03 | T90302 |
| RF-05 | T90301 |
| RNF-01 … RNF-04 | T90303 · T90302 · T90304 · T90304 |
| CA-02, CA-04 | T90304 · T90302 |
