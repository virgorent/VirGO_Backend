---
name: desplegar-aws
description: Valida y despliega la infraestructura serverless del backend de VirGo (SAM con API Gateway y Lambda, más MongoDB Atlas M0) a costo cero y obtiene la URL HTTPS. Úsala para preparar o revisar un despliegue; el despliegue real requiere confirmación humana.
---

# Desplegar en AWS a costo cero

> ⚠️ Crear, actualizar o borrar recursos en AWS requiere **confirmación explícita** de la persona responsable (Art. XI).

## 1. Revisión previa (sin costo)

- [ ] `infra/template.yaml` solo usa los servicios permitidos del Art. VII.
- [ ] El clúster de Atlas es **M0**, su organización no tiene método de pago y la cadena de conexión está en SSM (`/virgo/prod/mongo-uri`), no en el template.
- [ ] API Gateway tiene throttling (`ThrottlingRateLimit` y `ThrottlingBurstLimit`).
- [ ] Los logs tienen una retención de 7 días.
- [ ] No hay secretos en el template; se reciben como parámetros `NoEcho` o desde SSM.
- [ ] El presupuesto de $1 USD de `infra/bootstrap.yaml` está activo.
- [ ] `sam validate --lint` y `cfn-lint infra/*.yaml` pasan sin errores.
- [ ] El costo estimado está registrado en `memoria.md`.

## 2. Despliegue

El camino normal es **manual desde GitHub** (Art. VIII.3): en *Actions → Deploy → Run workflow*, con la rama `main` y el CI en verde. `.github/workflows/api-deploy.yml` entra a AWS por OIDC con el *environment* `production`, empaqueta, ejecuta `sam deploy` y después `tests/smoke/smoke.sh`.

> Una fusión a `main` **no** despliega. Lanzar el workflow cuenta como la confirmación humana del Art. XI.5.

Solo si GitHub Actions no está disponible, y con confirmación, desde una terminal:

```bash
sam build
sam deploy --stack-name virgo-api-prod --region us-east-1 --resolve-s3 --no-fail-on-empty-changeset
```

## 3. Verificación

1. Obtén la URL de la salida `ApiUrl` del stack.
2. Revisa que las pruebas de humo del workflow estén en verde (o ejecuta `bash tests/smoke/smoke.sh https://<url>`).
3. Revisa que la URL empiece con `https://`.
4. Registra en `memoria.md` la URL, el commit desplegado, el resultado de la prueba de humo y el costo.
5. Comparte la URL con el proyecto Móvil como `API_BASE_URL`, junto con el JSON probado de los endpoints nuevos.
6. Marca el spec como *Desplegado*, llena la sección «Despliegue» de su `check.md` y registra lo aprendido (skill `registrar-memoria`).

## 4. Reversión

Vuelve a desplegar el commit anterior con `workflow_dispatch` del workflow de despliegue. Para borrar todo, usa `sam delete --stack-name virgo-api-prod`, también con confirmación.
