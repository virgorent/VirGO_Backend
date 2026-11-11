# Plan — 000 Arquitectura base del Backend

| Campo | Valor |
|---|---|
| Módulo | `000-arquitectura-base` |
| Versión | 1.1.0 |
| Estado | En revisión |
| Fecha | 2026-10-29 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.1.0 |

## 1. Resumen técnico

Se crea una solución .NET 10 con cuatro capas: Api, Application, Domain e Infrastructure.

- **Ejecución:** la API corre en local con Kestrel y en **AWS Lambda** detrás de **API Gateway HTTP API**, que da HTTPS sin dominio propio.
- **Persistencia:** queda abstraída con dos implementaciones, en memoria y **MongoDB Atlas M0**.
- **Infraestructura:** se define con **AWS SAM**.
- **CI/CD:** **GitHub Actions** con conexión a AWS por **OIDC**. El despliegue es manual (*Run workflow*) y publica la URL HTTPS.

> **Cambio v1.1.0:** el detalle y las tareas de la infraestructura (secciones 7 y F4) viven ahora en [`900-infraestructura-iac`](../900-infraestructura-iac/plan.md), y los del CI/CD (F6) en [`901-pipeline-cicd`](../901-pipeline-cicd/plan.md). Aquí se conservan como contexto de la arquitectura global.

No se implementa ninguna funcionalidad del negocio.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje y runtime | C# 14 / .NET 10 (SDK 10.0.x) |
| Framework | ASP.NET Core Web API con controladores |
| Solución | `VirGo.slnx` |
| Alojamiento en la nube | AWS Lambda con el paquete `Amazon.Lambda.AspNetCoreServer.Hosting` (evento `HttpApi`) |
| Entrada pública | API Gateway HTTP API: HTTPS en `https://{id}.execute-api.us-east-1.amazonaws.com` |
| Base de datos | MongoDB Atlas M0 (AWS `us-east-1`), base `virgo` en producción y `virgo_dev` en desarrollo; driver `MongoDB.Driver` |
| Secretos | SSM Parameter Store (SecureString), inyectados como variables de entorno al desplegar |
| Documentación de la API | OpenAPI nativo de .NET (`Microsoft.AspNetCore.OpenApi`) en `/openapi/v1.json` |
| Pruebas | xUnit, `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory`) |
| Infraestructura como código | AWS SAM (`infra/template.yaml`) y CloudFormation (`infra/bootstrap.yaml`) |
| CI/CD | GitHub Actions: `api-ci.yml` y `api-deploy.yml` |
| Seguridad del código | CodeQL, Dependabot y analizadores de .NET (`AnalysisLevel=latest-recommended`) |
| Región | `us-east-1` |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| I | SDD | ✅ Cumple | Este plan deriva del spec aprobado; las tareas están en `task.md` y no se implementa nada antes de aprobarlo. |
| II | Módulo base primero | ✅ Cumple | Sin entidades ni endpoints del negocio; solo `/health` y una ruta de diagnóstico que existe únicamente en el ambiente de pruebas. |
| III | Arquitectura | ✅ Cumple | Cuatro proyectos por capa, inyección de dependencias nativa, persistencia por interfaces y ejecución dual Kestrel/Lambda. |
| IV | Contrato | ✅ Cumple | Prefijo `/api/v1`, Problem Details, `camelCase`, UTC y OpenAPI publicado. |
| V | Seguridad | ✅ Cumple | HTTPS por API Gateway, JWT con llave ≥ 256 bits validada al arrancar, CORS explícito, límites de solicitudes, encabezados de seguridad, OIDC, SSM, CodeQL y Dependabot. |
| VI | Pruebas | ✅ Cumple | Proyectos de pruebas unitarias y de integración desde el inicio; el CI bloquea la fusión si fallan. |
| VII | Costo cero | ✅ Cumple | Lambda, HTTP API, SSM estándar, CloudWatch Logs (7 días) y Budgets en AWS; MongoDB Atlas M0 sin método de pago. Ver la sección 7. |
| VIII | CI/CD | ✅ Cumple | Se delega a `901-pipeline-cicd`: ruleset, CI en cada PR y CD manual con humo y URL HTTPS. |
| IX | Observabilidad | ✅ Cumple | Logs JSON, `X-Correlation-Id` y `/health`. |
| X | Memoria | ✅ Cumple | Cada tarea cierra con una entrada en `memoria.md`; commits con el ID de la tarea. |
| XI | Agentes | ✅ Cumple | Cada tarea tiene un subagente responsable definido en `AGENTS.md`. |
| XII | Idioma | ✅ Cumple | Código en inglés y documentación en español; existe `.editorconfig`. |

## 4. Estructura de proyectos y carpetas

```text
VirGo-API/
├── VirGo.slnx
├── .editorconfig
├── Directory.Build.props           # TargetFramework, Nullable, analizadores y TreatWarningsAsErrors para seguridad
├── src/
│   ├── VirGo.Api/                  # Presentación
│   │   ├── Program.cs              # Arranque: servicios, middleware, Lambda hosting
│   │   ├── Controllers/            # (vacío; lo llenan los módulos funcionales)
│   │   ├── Middleware/             # CorrelationId, SecurityHeaders
│   │   ├── Errors/                 # Manejo global de excepciones a Problem Details
│   │   └── appsettings.Example.json # Plantilla; appsettings.json no se sube
│   ├── VirGo.Application/          # Aplicación
│   │   ├── Abstractions/           # IRepository<T>, IClock, IUnitOfWork (si aplica)
│   │   └── Common/                 # Result<T>, errores de aplicación, paginación
│   ├── VirGo.Domain/               # Dominio
│   │   └── Common/                 # Entity base, value objects comunes
│   └── VirGo.Infrastructure/       # Infraestructura
│       ├── Persistence/InMemory/   # Implementación en memoria
│       ├── Persistence/MongoDb/    # MongoClient único, opciones, inicializador de índices
│       └── DependencyInjection.cs  # AddInfrastructure(config)
├── tests/
│   ├── VirGo.UnitTests/
│   └── VirGo.IntegrationTests/
├── infra/
│   ├── bootstrap.yaml              # Una sola vez: OIDC, rol de despliegue y presupuesto de $1
│   └── template.yaml               # SAM: HTTP API, Lambda, parámetros SSM y LogGroup
```

`.github/` no va aquí: vive en la raíz del repositorio `virgorent/VirGO_Backend` y se detalla en `901-pipeline-cicd`.

**Dependencias entre proyectos:** `Api → Application, Infrastructure` · `Infrastructure → Application` · `Application → Domain` · `Domain → (ninguno)`.

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-01 | Ejecutar ASP.NET Core en **Lambda + API Gateway HTTP API** | EC2 + ALB (como el proyecto de referencia), App Runner, Fargate | Da HTTPS gratis sin dominio y no cobra por hora. El ALB cuesta alrededor de $7–10 USD cada 2 semanas (Art. VII). |
| ADR-02 | Runtime de Lambda: **`dotnet10` administrado** si está disponible en `us-east-1`; si no, **`provided.al2023`** con publicación *self-contained* | Contenedor en ECR | Se verifica empíricamente en T020. El contenedor necesita ECR, que tiene costo fuera de la capa gratuita. |
| ADR-03 | ~~DynamoDB provisionado~~ **Reemplazada por ADR-23 (2026-11-03): MongoDB Atlas M0** | DynamoDB, RDS | El equipo modela y opera la base en MongoDB, como en el curso; M0 es gratis permanente. RDS está prohibido. |
| ADR-04 | **Persistencia en memoria** para local y pruebas; **Testcontainers** (`mongo`) solo para las pruebas de los repositorios de MongoDB en el CI | MongoDB local instalado | Arranque con un solo comando, sin Docker ni Atlas (RNF-07). |
| ADR-05 | **OIDC** de GitHub a AWS | Llaves de acceso en GitHub Secrets | Sin credenciales de larga duración (Art. V.4). |
| ADR-06 | **AWS SAM** para la infraestructura | Terraform, CDK | Integración nativa con Lambda y API Gateway; un solo archivo YAML; `sam delete` borra todo. |
| ADR-07 | **Secretos en SSM SecureString**, inyectados al desplegar | Secrets Manager | Secrets Manager cuesta $0.40 por secreto al mes; SSM estándar es gratis. |
| ADR-08 | Rutas con el prefijo fijo **`/api/v1`** | Paquete `Asp.Versioning` | Más simple mientras solo exista la v1; se puede migrar si aparece una v2. |

## 6. Diseño

### 6.1 Pipeline de middleware, en orden

1. `CorrelationIdMiddleware`: lee o genera `X-Correlation-Id`, lo agrega al *scope* de logging y a la respuesta.
2. Manejo global de excepciones (`UseExceptionHandler` + `IProblemDetailsService`): Problem Details con `traceId` y sin trazas.
3. `SecurityHeadersMiddleware`: `X-Content-Type-Options`, `X-Frame-Options: DENY`, `Referrer-Policy` y `Strict-Transport-Security`.
4. CORS con la política `MobileAndWeb`, que toma los orígenes de la configuración `Cors:AllowedOrigins`.
5. Rate limiting: una política global de ventana fija por IP, configurable.
6. Autenticación JWT Bearer (emisor, audiencia, expiración y llave de `Jwt:SigningKey`) y autorización.
7. `MapControllers()`, `MapHealthChecks("/health")` y `MapOpenApi()`.

### 6.2 Contrato base

| Elemento | Definición |
|---|---|
| `GET /health` | `200 {"status":"Healthy"}`; sin autenticación y sin detalles internos |
| `GET /openapi/v1.json` | Documento OpenAPI con el esquema de seguridad `Bearer` |
| Errores | `application/problem+json` con `type`, `title`, `status`, `detail`, `instance` y `traceId`; los errores de validación agregan `errors` |
| Encabezados | `X-Correlation-Id` en todas las respuestas |
| JSON | `camelCase`, enums como texto y fechas ISO-8601 en UTC |
| Ruta de diagnóstico | `GET /api/v1/_diagnostics/secure`: solo existe en el ambiente `Testing` y sirve para verificar CA-04 |

### 6.3 Configuración (`IOptions` validado al arrancar)

| Sección | Claves | Origen |
|---|---|---|
| `Persistence` | `Provider` (`InMemory` o `MongoDb`), `DatabaseName`, `ConnectionString` | `ConnectionString` desde SSM (`/virgo/prod/mongo-uri`) o variable de entorno local; el resto desde `appsettings` |
| `Jwt` | `Issuer`, `Audience`, `SigningKey` (≥ 32 bytes) | `SigningKey` desde SSM; el resto desde `appsettings` |
| `Cors` | `AllowedOrigins[]` | `appsettings.Production.json` y variables de entorno |
| `RateLimiting` | `PermitLimit`, `WindowSeconds` | `appsettings` |

### 6.4 Base de datos MongoDB (solo la estructura)

- Clúster Atlas M0 `virgo-dev`, base `virgo` (producción) y `virgo_dev` (desarrollo).
- Un `MongoClient` único por proceso (singleton), creado fuera del manejador para reutilizar conexiones entre invocaciones de Lambda, con `maxPoolSize` de 10 (M0 admite 500 conexiones).
- `MongoIndexInitializer` crea de forma idempotente validadores e índices desde `db/schema/` al arrancar. Así, al ejecutar el primer *task* de usuarios, la base y sus colecciones se crean solas.
- Documentos con `_id` y referencias como texto UUID (`BsonRepresentation(BsonType.String)`, ADR-28), nombres de campo en `camelCase` inglés y fechas en UTC.
- La fuente de verdad es `VirGo-Database/src/catalog`; `db/schema/` es una copia generada (`010-contrato-datos`).

La base no define entidades: los módulos funcionales documentan sus patrones de llave en su propio plan.

## 7. Infraestructura y costo estimado

```text
Móvil ──HTTPS──▶ API Gateway (HTTP API, throttling) ──▶ Lambda (VirGo.Api) ──TLS──▶ MongoDB Atlas M0 (virgo)
                                                            │
                                                            ├──▶ SSM Parameter Store (Jwt:SigningKey)
                                                            └──▶ CloudWatch Logs (7 días)
GitHub Actions ──OIDC──▶ Rol de despliegue (mínimo privilegio) ──▶ CloudFormation / SAM
AWS Budgets ($1 USD) ──▶ correo del equipo
```

| Recurso | Configuración | Capa gratuita | Costo estimado al mes |
|---|---|---|---|
| Lambda | 512 MB, 15 s de timeout | 1 M de solicitudes y 400 000 GB-s al mes (permanente) | $0 |
| API Gateway HTTP API | Throttling de 20 rps y ráfaga de 40 | 1 M de llamadas al mes (12 meses o créditos del plan gratuito) | $0 |
| MongoDB Atlas M0 | Clúster compartido en AWS `us-east-1` | 512 MB, gratis permanente | $0 |
| SSM Parameter Store | 1 parámetro estándar | Gratis | $0 |
| CloudWatch Logs | 7 días de retención | 5 GB al mes | $0 |
| AWS Budgets | 1 presupuesto con alerta | Gratis (las 2 primeras) | $0 |
| Bucket de artefactos de SAM | Menos de 50 MB | 5 GB (12 meses o créditos) | ≈ $0 |
| **Total** | | | **$0 USD** |

> **Arranque inicial (manual, una sola vez):** crear el stack `infra/bootstrap.yaml` desde la consola de AWS (subiendo el archivo) o con el CLI. Crea el proveedor OIDC, el rol `virgo-github-deploy` y el presupuesto. El resto lo despliega el pipeline. Es la única excepción a «nada a mano» del Art. VII.4.

## 8. Estrategia de pruebas

| Nivel | Proyecto | Qué cubre en este módulo |
|---|---|---|
| Unitarias | `VirGo.UnitTests` | Validación de opciones (llave JWT corta = error) y repositorio en memoria |
| Integración | `VirGo.IntegrationTests` | `/health` = 200; ruta inexistente = 404 Problem Details; excepción = 500 Problem Details sin trazas; ruta segura sin token = 401; encabezados de seguridad y `X-Correlation-Id`; `/openapi/v1.json` = 200 |
| Humo (CD) | `api-deploy.yml` | `curl -fsS https://…/health` después de desplegar |
| Infraestructura | `api-ci.yml` | `sam validate --lint` y `cfn-lint` |
| Seguridad y mutación | `VirGo.SecurityTests`, Stryker.NET | Se agregan en `901-pipeline-cicd` (T202, T203) y cubren también el código de este módulo |

## 9. Riesgos y mitigaciones

| Riesgo | Impacto | Mitigación |
|---|---|---|
| El runtime `dotnet10` no está disponible en Lambda | No se puede desplegar | ADR-02: `provided.al2023` *self-contained*; se verifica en T020 |
| Arranque en frío lento (más de 3 s) | Falla RNF-03 | 512 MB de memoria, ReadyToRun y medición en T032; documentar el valor real |
| Cuenta nueva con el «plan gratuito» por créditos | Algún servicio deja de ser gratis a los 6 meses | Presupuesto de $1, revisión mensual y `sam delete` disponible |
| Límites de M0 (512 MB, 500 conexiones, operaciones por segundo limitadas) | Errores o lentitud | Pool de 10 conexiones, índices por consulta, alertas de Atlas y revisión del tamaño en cada cierre de módulo |
| Atlas abierto a `0.0.0.0/0` (Lambda sin IP fija) | Exposición de la base | TLS, usuarios de mínimo privilegio, contraseñas largas aleatorias y cadena de conexión solo en SSM (Art. V.11) |
| Secretos filtrados en un commit | Compromiso de la API | `.gitignore`, revisión de `seguridad` y escaneo de secretos de GitHub (push protection) |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Preparación del repositorio y la gobernanza | T001–T005 |
| F1 | Solución y capas | T006–T010 |
| F2 | Núcleo transversal: errores, configuración, logs y seguridad | T011–T016 |
| F3 | Persistencia base | T017–T019 |
| F4 | ➡️ Movida a `900-infraestructura-iac` | T102–T105, T110 |
| F5 | Pruebas | T025–T027 |
| F6 | ➡️ Movida a `901-pipeline-cicd` | T204–T210 |
| F7 | Cierre del módulo (el borrado pasó a `900`: T108) | T035 |
