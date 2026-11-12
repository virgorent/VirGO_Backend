# Memoria de AWS

**Actualizada:** 2026-11-11

- Infraestructura en `infra/`: `bootstrap.yaml` y `template.yaml`.
- Objetivo: $0 con Lambda, API Gateway, SSM, CloudWatch y Atlas M0.
- Creado: stack `virgo-api-prod`, bucket administrado por SAM, 2 parámetros SSM y presupuesto `virgo-1usd`. Ver `MemoriaDespliegue.md`.

## Bloqueos resueltos

- `{{resolve:ssm-secure}}` no funciona en variables de Lambda → la API lee `/virgo/prod/` de SSM al arrancar.
- La alarma de 5xx usaba `5XXError` → la métrica de HTTP API es `5xx`.
- SNS con `alias/aws/sns` no entrega alarmas → se usa el cifrado por defecto de SNS.
- `ReservedConcurrentExecutions: 10` choca con la cuota de 10 de las cuentas nuevas → se quitó.

## Pendiente

- Al rol OIDC le faltan permisos para `sam deploy` (stack `bootstrap` sin crear).
