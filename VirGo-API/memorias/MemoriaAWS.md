# Memoria de AWS

**Actualizada:** 2026-11-06

- Infraestructura en `infra/`: `bootstrap.yaml` y `template.yaml`.
- Objetivo: $0 con Lambda, API Gateway, SSM, CloudWatch y Atlas M0.
- No se ha creado ningún recurso.

## Bloqueos

- `{{resolve:ssm-secure}}` no funciona en variables de Lambda.
- Al rol OIDC le faltan permisos para `sam deploy`.
- La alarma de 5xx usa una métrica incorrecta.
- SNS con `alias/aws/sns` no entrega alarmas.
- Faltan los accesos de AWS.
