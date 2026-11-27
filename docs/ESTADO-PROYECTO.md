# VirGo: estado del proyecto

**Fecha:** 15 de noviembre de 2026
**Repositorios:**
- [`virgorent/VirGO_Backend`](https://github.com/virgorent/VirGO_Backend): API y base de datos.
- [`virgorent/VirGO_Movil`](https://github.com/virgorent/VirGO_Movil): app Android.

## Resumen

| Entregable | Estado |
|---|---|
| API en AWS con URL HTTPS | ✅ `https://rcof55a3c9.execute-api.us-east-1.amazonaws.com` (`/health` → 200) |
| Despliegue continuo | ✅ GitHub Actions con OIDC, sin llaves de larga duración, y prueba de humo después de cada despliegue |
| App en Google Play | ✅ v0.2.0 activa en **prueba interna**; **prueba cerrada** en revisión con 12 *testers* |
| Pruebas | ✅ 408 de 408 aprobadas (API 219, base de datos 82, Móvil 107). Detalle en [`PRUEBAS.md`](PRUEBAS.md) |
| Costo | $0 en AWS (plan gratuito, presupuesto con alerta); la cuenta de Play tuvo un pago único |

## Specs (SDD)

- Cada cambio sigue `spec → plan → task → check` y se registra en `memoria.md`.
- 142 specs en total: 98 de API, 11 de base de datos, 30 de Móvil y 3 globales.
- El mapa está en [`backend/MAPA-SPECS.md`](backend/MAPA-SPECS.md) y en el [`MAPA-SPECS.md` de Móvil](https://github.com/virgorent/VirGO_Movil/blob/main/docs/frontend/MAPA-SPECS.md).
- **Enmiendas recientes:**
  - Constitución de la API v1.3.0 (propuesta): servicios de AWS.
  - Constitución de Móvil v1.2.0 (propuesta): Google Play como canal de distribución.

## API (.NET 10 en AWS Lambda)

**Endpoints en producción:**

| Endpoint | Spec |
|---|---|
| `POST /api/v1/auth/register` · `login` · `logout` | 003–005 |
| `GET /api/v1/users/me` | 006 |
| `DELETE /api/v1/users/me` (eliminar cuenta: anonimiza los datos y revoca las sesiones) | 018 |
| `GET /api/v1/users/{id}/public` | 017 |
| `GET /api/v1/venues` · `POST /api/v1/quotes` · `POST /api/v1/bookings` | 007–009 (catálogo, cotizaciones y reservaciones) |
| `GET /health` | 000 |

**Infraestructura en AWS** (SAM, todo en el plan gratuito):

| Servicio | Para qué |
|---|---|
| Lambda (`dotnet10`, arm64) + API Gateway HTTP | La API, con alias `live` y rollback manual |
| SSM Parameter Store (SecureString) | Secretos: cadena de conexión y llave JWT. Nunca en el código |
| SQS (`virgo-jobs` + cola de errores) | Trabajos en segundo plano: reservación creada, envío de correos y vencimiento de cotizaciones |
| Lambda *worker* | Procesa la cola y reporta fallas por mensaje |
| EventBridge Scheduler | Vence las cotizaciones cada día a las 00:00 (hora de la Ciudad de México) |
| SES | Correos del servicio (en *sandbox*) |
| S3 + CloudFront | Imágenes de salones y proveedores, con bucket privado y acceso solo por CloudFront |
| X-Ray + CloudWatch | Trazas, *dashboard* `virgo-api` y alarmas (errores 5xx, errores de Lambda, cola de errores) |
| IAM con OIDC | GitHub despliega con un rol acotado y CloudFormation usa otro rol solo para recursos `virgo-*` |

## Base de datos (MongoDB Atlas, M0 gratuito)

- `VirGo-Database` es la fuente de verdad: 22 colecciones con validadores `$jsonSchema` e índices.
- Producción usa la base `virgo`.
- **Pendiente:** remediar 26 diferencias con respaldo previo, congelamiento de escritura y decisión Go/No-Go del equipo.
- **Pendiente:** cargar datos de ejemplo.

## Móvil (Kotlin + Jetpack Compose)

- Arquitectura multimódulo (`:app` + `:core:*`), MVVM, Hilt, Retrofit y sesión cifrada con Tink y el Keystore.
- **Pantallas:** inicio de visitante con mapa de salones cercanos, inicio de sesión contra la API real, registro y recuperación de contraseña.
- El *release* se publica a mano desde `main`: genera el AAB y el APK firmados, verifica sus firmas y crea la GitHub Release.

**Google Play:**

| Paso | Estado |
|---|---|
| App `mx.virgo.app` creada; firma de apps de Play activa | ✅ |
| Prueba interna (v0.2.0, versionCode 2) | ✅ Activa |
| Declaraciones: seguridad de los datos, clasificación (Todos), público 18+, ID de publicidad, anuncios | ✅ Completas |
| Aviso de privacidad, términos y eliminación de cuenta | ✅ [Publicados en GitHub Pages](https://virgorent.github.io/VirGO_Movil/legal/privacidad/) |
| Cuenta de prueba para el revisor de Google | ✅ En la base de producción |
| Prueba cerrada (12 *testers*, 14 días) | ⏳ En revisión de Google |
| Producción | ⏳ Después de los 14 días de prueba cerrada |

## DevSecOps

| Control | Dónde |
|---|---|
| Rama `main` protegida (PR obligatorio, 1 revisión, checks requeridos, sin *force push*) | Los dos repositorios |
| Gitleaks y *dependency review* en cada PR; Dependabot semanal | Los dos repositorios |
| CodeQL | API (C#) y Móvil (Kotlin) |
| Semgrep, SonarCloud y OWASP ZAP (DAST contra la API levantada en el CI) | API |
| Checkov, cfn-lint y `sam validate` sobre la infraestructura | API |
| Mutación: Stryker cada noche en la API y Pitest en Móvil | Ambos |
| Secretos solo en GitHub Secrets y SSM; MFA en la cuenta root de AWS | AWS y GitHub |

## Lo que aprendimos

- **La primera subida a Play fija la llave de subida.** Un *release* salió firmado con una llave de desarrollo; se regeneró con la oficial **antes** de subirlo.
- **El plan gratuito de AWS no incluye CodeDeploy.** El despliegue *canary* se cambió por un alias `live` con *rollback* manual, documentado en un ADR.
- **Comparar documentos contra la realidad evita errores.** Así encontramos que la API y la base se habían separado, y por eso `VirGo-Database` es ahora la fuente de verdad.
- **Las declaraciones de Play deben describir lo que la app hace hoy.** Chat, pagos y ubicación se declararán antes de publicar la versión que los incluya.

## Lo que falta

1. Aprobación de la prueba cerrada y 14 días con 12 *testers*.
2. **Móvil:** botón "Eliminar cuenta" y registro conectado a la API (v0.3.0).
3. **Base de datos:** remediar las 26 diferencias, cargar datos de ejemplo y crear un usuario dedicado para la API.
4. **API:** los siguientes specs aprobados; 10 endpoints esperan decisiones del equipo.
5. Producción en Google Play.
