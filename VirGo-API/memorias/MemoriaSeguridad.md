# Memoria de seguridad

**Actualizada:** 2026-11-06

## Controles en el pipeline

Gitleaks, *dependency review*, CodeQL, Semgrep, SonarCloud, OWASP ZAP y Checkov.

Reglas propias de Semgrep en `.semgrep/semgrep.yaml`: filtros de MongoDB armados con texto, datos sensibles en logs y cadenas de conexión con credenciales.

## Hallazgos abiertos (revisión del PR #5)

- Bookings y Quotes no exigen el rol `client`.
- `failedLoginCount` no se reinicia al vencer el bloqueo.
- Correo y UUID se validan distinto que en la base.
- 3 avisos de CodeQL.
