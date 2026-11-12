# Memoria de despliegue

**Actualizada:** 2026-11-11

**URL de producción:** <https://rcof55a3c9.execute-api.us-east-1.amazonaws.com>

| Prueba | Resultado |
|---|---|
| `GET /health` | 200 `{"status":"Healthy"}` |
| `GET /api/v1/venues` | 200 `{"data":[]}`: conecta a Atlas; la base `virgo` aún no tiene datos |

## Cómo se desplegó (2026-11-11)

1. Cuenta AWS nueva en plan Free; presupuesto `virgo-1usd` ($1 al mes) con alertas por correo.
2. Sesión local con `aws login` (credenciales temporales, sin llaves permanentes).
3. Secretos en SSM como SecureString: `/virgo/prod/Jwt/SigningKey` (aleatoria de 64 bytes, nunca mostrada) y `/virgo/prod/Persistence/ConnectionString`.
4. `sam build -t infra/template.yaml` y `sam deploy --config-file infra/samconfig.toml --parameter-overrides AlertEmail=…`.
5. Stack `virgo-api-prod` en `us-east-1`.

## Pendiente

- Usuario de Atlas `virgo-api-prod` (`readWrite@virgo`). Hoy la API usa un usuario administrador; se cambia solo el parámetro de SSM, sin volver a desplegar.
- Aplicar el catálogo de `VirGo-Database` y datos de ejemplo a la base `virgo`.
- Stack `bootstrap` (OIDC) y environment `production` para desplegar desde `api-deploy.yml`.
- Confirmar la suscripción de SNS a las alarmas.
