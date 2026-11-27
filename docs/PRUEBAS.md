# Estado de las pruebas de VirGo

**Fecha de ejecución:** 15 de noviembre de 2026. Los resultados vienen de **ejecutar** las pruebas, no de contarlas. La API y la base de datos se ejecutaron sobre `main` (commit `b76fbd8`); Móvil, sobre su CI de `main`.

## Resumen

| Proyecto | Tipo | Pruebas | Resultado |
|---|---|---|---|
| `VirGo-API` | Unitarias | 34 | ✅ 34 de 34 |
| `VirGo-API` | Integración | 49 | ✅ 49 de 49 |
| `VirGo-API` | Seguridad | 25 | ✅ 25 de 25 |
| `VirGo-API` | Contrato | 12 | ✅ 12 de 12 |
| `VirGo-API` | Base de datos (validadores e índices) | 96 | ✅ 96 de 96 |
| `VirGo-API` | Rendimiento | 3 | ✅ 3 de 3 |
| `VirGo-API` | Mutación (Stryker) | Nocturna | ✅ Workflow `api-mutation-nightly` |
| `VirGo-API` | Humo, en producción después de cada despliegue | Script | ✅ En verde en el último despliegue |
| `VirGo-Database` | Unitarias | 82 | ✅ 82 de 82 |
| `VirGo-Movil` | Unitarias, de red (MockWebServer) y de UI (Robolectric) | 107 | ✅ 107 de 107 en el CI |
| `VirGo-Movil` | Mutación (Pitest) | Línea base | ✅ 73 % (35 de 48 mutantes) |

**Total ejecutado:** 408 pruebas, **408 aprobadas y 0 fallidas**.

## Pruebas manuales en producción

| Prueba | Resultado |
|---|---|
| `GET /health` | ✅ 200 |
| Registro e inicio de sesión de la cuenta de revisión de Play | ✅ 201 y 200 |
| `DELETE /api/v1/users/me` con una cuenta desechable | ✅ 204. Después, el token anterior da 401, el login da 401 y el perfil público queda como "Cuenta eliminada" sin correo ni teléfono |
| Cola SQS → *worker*: `quotes.expire` y envío de correo con SES | ✅ Procesados; el correo llegó |
| CloudFront: imagen pública 200, acceso directo al bucket 403 | ✅ |
| X-Ray y alarmas de CloudWatch | ✅ Trazas visibles; alarmas en OK |
| Páginas legales de Play (`/legal/privacidad`, `/terminos`, `/eliminar-cuenta`) | ✅ 200 |
| Firma del AAB de v0.2.0 | ✅ Firmado con la llave de subida oficial (verificado con `keytool -printcert`) |

## Cómo reproducirlas

```bash
# VirGo-API: todas las suites (unitarias, integración, seguridad, contrato, base de datos y rendimiento)
cd VirGo-API
dotnet test VirGo.slnx -c Release

# VirGo-Database
cd VirGo-Database
npm ci
npm test

# VirGo-Movil (repositorio virgorent/VirGO_Movil)
cd VirGo-Movil
./gradlew testDebugUnitTest pitest
```

En GitHub, el job `api-build-test` publica en el resumen de cada ejecución una tabla por tipo de prueba con la cobertura y la lista de todas las pruebas (`tools/test_report.py`).

## Análisis automático en GitHub

| Check | Proyecto | Requerido para fusionar |
|---|---|---|
| `gitleaks` y `dependency review` | Ambos | ✅ |
| `CodeQL` / `analyze` | Ambos | ✅ |
| `api-build-test` (pruebas y cobertura) | API | ✅ |
| `api-iac` (`sam validate`, `cfn-lint`, Checkov) | API | ✅ |
| `api-schema` y `database` | API y base de datos | ✅ |
| `movil-build-test` (ktlint, detekt, Lint, pruebas, `assembleDebug`) | Móvil | ✅ |
| Semgrep, SonarCloud y OWASP ZAP | API | Informativos en cada PR |

## Limitaciones conocidas

- **Integración:** las pruebas de la API usan persistencia en memoria. Las de base de datos validan los esquemas e índices de `db/schema/`, pero no corren contra Atlas.
- **Base de producción:** `virgo` tiene 26 diferencias frente a `db/schema/`; se remediarán con respaldo y Go/No-Go.
- **Móvil:** la mutación es una línea base; el umbral se fijará después de medir más módulos.
- **Cola SQS:** el flujo `booking.created` no se ha probado en la nube porque la base de producción aún no tiene salones.

## Lo que sigue

1. Pruebas de longitud por campo: límite, límite + 1 y 1500 caracteres.
2. Pruebas de integración contra MongoDB real (Testcontainers) en el CI.
3. Pruebas de Móvil para "Eliminar cuenta" y el registro conectado a la API.
4. Prueba de `booking.created` en la nube, cuando haya datos de ejemplo.
