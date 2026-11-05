# Memoria — VirGo API (Backend)

Bitácora oficial del proyecto (Art. X de [`constitution.md`](constitution.md)). Cada sesión de trabajo agrega una entrada al final de la sección «Bitácora» con la skill [`registrar-memoria`](.opencode/skills/registrar-memoria/SKILL.md).

## Estado actual

| Campo | Valor |
|---|---|
| Constitución | v1.2.0 ratificada por el equipo |
| Fuente de datos | `API-HANDOFF.md` + `VirGo-Database`; el modelo raíz queda como contexto histórico |
| Módulo en curso | Implementación local de `000`–`004`; cierre externo pendiente |
| Tareas | Base local implementada; ver estados detallados en cada `task.md` |
| URL de producción | Pendiente (se obtiene en `002`: T209) |
| Costo acumulado en AWS | $0 USD (todavía no se crea ningún recurso) |
| Siguiente paso | Validar IaC en CI, configurar GitHub/AWS/Atlas con autorización y completar `check.md` |

## Módulos

| Módulo | Estado | Notas |
|---|---|---|
| `000-arquitectura-base` | 🔄 Implementación local | Base técnica, seguridad transversal y persistencia abstracta |
| `001-infraestructura-iac` | 🔄 Validación pendiente | SAM, CloudFormation, Atlas documentado, HTTPS y alertas |
| `002-pipeline-cicd` | 🔄 Validación pendiente | Ruleset, CI/CD, DevSecOps y humo |
| `003-auth-login` | 🔄 Implementación local | `POST /api/v1/auth/login`; pruebas de integración |
| `004-auth-logout` | 🔄 Implementación local | `POST /api/v1/auth/logout`; pruebas de integración |
| `005-auth-register` | 🔄 Implementación local | `POST /api/v1/auth/register`; alta de cliente |
| `006-users-me` | 🔄 Implementación local | `GET /api/v1/users/me`; perfil propio |
| `007-venues-catalog` | 🔄 Implementación local | `GET /api/v1/venues`; catálogo publicado |
| `008-quotes-create` | 🔄 Implementación local | `POST /api/v1/quotes`; cotización para venue publicado |
| `009-bookings-create` | 🔄 Implementación local | `POST /api/v1/bookings`; reservación pending_payment |
| `005-auth-registro`, `006-auth-renovar-sesion` | 💡 Propuestos | Salieron del alcance de `003` y `004` |

## Decisiones (ADR)

| ADR | Fecha | Decisión | Motivo |
|---|---|---|---|
| ADR-00 | 2026-10-29 | Backend en **.NET 10** (ASP.NET Core Web API) | Mismo stack que el proyecto de referencia del curso |
| ADR-01 | 2026-10-29 | **Lambda + API Gateway HTTP API** en lugar de EC2 + ALB | HTTPS sin dominio y costo $0; el ALB cobra por hora |
| ADR-02 | 2026-10-29 | Runtime `dotnet10` administrado o `provided.al2023` *self-contained* | Se decide en T020 tras verificar la disponibilidad |
| ADR-03 | 2026-10-29 | ~~DynamoDB provisionado~~ **Reemplazada por ADR-23** | — |
| ADR-04 | 2026-10-29 | **Persistencia en memoria** para local y pruebas | Arranque en local con un solo comando |
| ADR-05 | 2026-10-29 | **OIDC** GitHub → AWS | Sin llaves de acceso de larga duración |
| ADR-06 | 2026-10-29 | **AWS SAM** como infraestructura como código | Nativo para Lambda y API Gateway; `sam delete` borra todo |
| ADR-07 | 2026-10-29 | Secretos en **SSM Parameter Store** | Gratis; Secrets Manager cuesta $0.40 por secreto al mes |
| ADR-08 | 2026-10-29 | Prefijo fijo `/api/v1` | Simplicidad mientras solo exista la v1 |
| ADR-09 | 2026-11-03 | **CD manual** (`workflow_dispatch`, *environment* `production`) | Indicación del curso (clase del 29/10) |
| ADR-10 | 2026-11-03 | **Ruleset** de GitHub en lugar de la protección de rama clásica | Mecanismo actual; exportable a JSON |
| ADR-11 | 2026-11-03 | **Índice TTL de MongoDB** para las sesiones | Limpieza gratis |
| ADR-12 | 2026-11-03 | **Checkov** para la IaC | Revisa seguridad, no solo sintaxis |
| ADR-13 | 2026-11-03 | **SNS + CloudWatch Alarms** | Alarmas de 5xx y *throttling* gratis |
| ADR-14 | 2026-11-03 | **Stryker.NET** para mutación, primero en modo reporte | Mide la calidad de las pruebas; umbral tras medir |
| ADR-15 | 2026-11-03 | **Gitleaks + dependency review** | Funcionan sin GitHub Advanced Security |
| ADR-16 | 2026-11-03 | Proyecto **`VirGo.SecurityTests`** | Check independiente para seguridad |
| ADR-17 | 2026-11-03 | `PasswordHasher<T>` de Identity (PBKDF2) | Incluido en el SDK, sin dependencias nativas |
| ADR-18 | 2026-11-03 | JWT de 15 min + token de sesión opaco de 30 días con huella y `sid` | Tokens cortos y sesiones revocables |
| ADR-19 | 2026-11-03 | Bloqueo por cuenta guardado en la tabla | En Lambda la memoria no se comparte |
| ADR-20 | 2026-11-03 | Validar `sid` contra la tabla en cada ruta protegida | Logout efectivo en todas las instancias |
| ADR-21 | 2026-11-03 | Logout sin cuerpo; la sesión sale del `sid` | Menos datos sensibles en tránsito |
| ADR-22 | 2026-11-03 | MCP permitidos: `aws-docs`, `mongodb` (solo lectura) y `github` | La base es MongoDB, como en la clase |
| ADR-23 | 2026-11-03 | **MongoDB Atlas M0** en AWS `us-east-1` como base de datos | Decisión del equipo: la base se hace en MongoDB; M0 es gratis permanente |
| ADR-24 | 2026-11-03 | Atlas con lista de acceso `0.0.0.0/0` y controles compensatorios | Lambda no tiene IP fija sin NAT (prohibido); M0 no admite *private endpoint* |
| ADR-25 | 2026-11-03 | **Testcontainers** para probar los repositorios de MongoDB en el CI | Gratis en GitHub Actions; local sigue sin Docker (en memoria) |
| ADR-26 | 2026-11-03 | **Repositorio único** `virgorent/virGO_App` con carpetas por proyecto; `.github/` en la raíz | Decisión del equipo |

## Cambios de contrato (compartidos con Móvil)

| Fecha | Cambio | Aviso a Móvil |
|---|---|---|
| 2026-10-29 | Contrato base definido: `/api/v1`, Problem Details, `camelCase`, UTC, JWT Bearer, `GET /health`, `GET /openapi/v1.json` | ✅ Registrado también en `VirGo-Movil/memoria.md` |
| 2026-11-03 | **Propuesto:** `POST /api/v1/auth/login` (JSON en `specs/003-auth-login/plan.md` §6.1) y `POST /api/v1/auth/logout` (`specs/004-auth-logout/plan.md` §6.1). Se confirma al desplegar | ✅ Registrado como propuesto en `VirGo-Movil/memoria.md` |

## Costos y recursos en la nube

| Fecha | Recurso | Acción | Costo estimado |
|---|---|---|---|
| — | — | Aún no se crean recursos | $0 |
| (planeado) | MongoDB Atlas M0 `virgo-cluster` | Se crea en `001`: T101, sin método de pago | $0 |

---

## Bitácora

### 2026-10-29 · Análisis del proyecto de referencia

- **Autor:** equipo + Claude Code
- **Módulo / tareas:** — (preparación)
- **Qué se hizo:**
  - Se revisó el repositorio de referencia del profesor (`edelomeza/WebAPIDevSecOpsScalling`): .NET 10 Web API, `AGENTS.md`, archivos `Memoria*.md`, GitHub Actions y AWS EC2 + ALB con CloudFormation.
  - Se identificó que el ALB genera costo (alrededor de $7–10 USD cada 2 semanas), lo que contradice el requisito de costo cero.
- **Evidencia:** `MemoriaAWS.md` del repositorio de referencia, sección «Cálculo 14 días».
- **Decisiones:** ADR-00 y ADR-01 (mantener .NET y cambiar a serverless).
- **Costo:** $0, sin cambios.
- **Pendiente:** crear los archivos SDD.

### 2026-10-29 · Archivos de gobernanza y specs de la arquitectura base

- **Autor:** equipo + Claude Code (`arquitecto`)
- **Módulo / tareas:** `specs/000-arquitectura-base` · ninguna ejecutada
- **Qué se hizo:**
  - Se crearon `constitution.md` v1.0.0, `AGENTS.md` y esta `memoria.md`.
  - Se crearon 5 subagentes en `.opencode/agents/`: `arquitecto`, `dev-dotnet`, `qa`, `seguridad` y `devops-aws`.
  - Se crearon 4 skills internas en `.opencode/skills/`: `sdd-flujo`, `registrar-memoria`, `verificar-constitucion` y `desplegar-aws`, más la lista filtrada de skills externas en `AGENTS.md`.
  - Se redactaron `spec.md` (8 historias técnicas, 15 RF, 7 RNF y 10 CA), `plan.md` (Constitution Check, 8 ADR, diseño y costo de $0) y `task.md` (35 tareas en 8 fases, con matriz de trazabilidad).
- **Evidencia:** archivos en el repositorio. El Constitution Check del plan tiene los 12 artículos en ✅.
- **Decisiones:** ADR-02 a ADR-08.
- **Costo:** $0, sin recursos creados.
- **Pendiente:**
  - Revisión del profesor y del equipo.
  - Responder las preguntas abiertas P-01 (correo del presupuesto) y P-02 (quién aprueba los PR).
  - Al aprobarse, empezar por T001. **No se ejecutan tareas hasta la aprobación.**

### 2026-11-03 · Lineamientos de clase: specs atómicos, check.md, DevSecOps, CD manual y MCP

- **Autor:** equipo + Claude Code (`arquitecto`)
- **Módulo / tareas:** gobernanza · `specs/000`–`004` · ninguna tarea ejecutada
- **Qué se hizo:**
  - Se revisaron las grabaciones de clase del 28 y 29 de octubre y se aplicaron sus lineamientos.
  - Se propuso la enmienda **v1.1.0** de `constitution.md`: `check.md` y specs atómicos, redacción anticipada de specs funcionales, `.gitignore` y escaneos DevSecOps, cinco tipos de prueba con mutación, servicios auxiliares de AWS, ruleset, **CD manual**, aprendizaje tras el despliegue, MCP y README en inglés.
  - Se crearon `README.md` (inglés), `.gitignore` DevSecOps y `opencode.json` (MCP `aws-docs`, `dynamodb`, `github`).
  - La infraestructura y el CI/CD de `000` se separaron en `001-infraestructura-iac` y `002-pipeline-cicd`; `000` conserva el contexto global y la tabla de equivalencias de tareas.
  - Se redactaron `003-auth-login` y `004-auth-logout` con su JSON de contrato para Móvil.
  - Se agregó `check.md` a `000`–`004` y se actualizaron `AGENTS.md`, los 5 subagentes y las skills `sdd-flujo`, `verificar-constitucion`, `desplegar-aws` y `registrar-memoria`.
- **Evidencia:** archivos en el repositorio.
- **Decisiones:** ADR-09 a ADR-22.
- **Costo:** $0, sin recursos creados.
- **Aprendizaje:** el ejemplo de clase usa MongoDB; se mantuvo DynamoDB por costo cero y se eligió su MCP equivalente.
- **Pendiente:**
  - Aprobar la enmienda v1.1.0 (la aprueba el equipo, Art. XI.2).
  - Responder: `001` P-01 (correo de alertas) y P-02 (vigencia de créditos), `002` P-01 (quién aprueba PR) y P-02 (repositorio público o privado), `003` P-01 (bloqueo) y P-02 (agregar `intentos_fallidos` y `bloqueado_hasta` al modelo de datos).
  - Fijar las versiones de los MCP al instalarlos (`001`: T100, `002`: T200).

### 2026-11-03 · Cambio de base de datos: DynamoDB → MongoDB Atlas M0

- **Autor:** equipo + Claude Code (`arquitecto`)
- **Módulo / tareas:** gobernanza · `specs/000`–`004` · ninguna tarea ejecutada
- **Qué se hizo:**
  - Por decisión del equipo, la base de datos será **MongoDB** (ADR-23 reemplaza a ADR-03).
  - Se ajustó la propuesta de enmienda v1.1.0: Art. III.4 (MongoDB e índices creados por la app), V.11 (seguridad de Atlas) y VII (Atlas M0 como única excepción externa, sin método de pago).
  - `000`: proveedor `MongoDb`, `MongoClient` único e inicializador de índices (T019). `001`: nueva T101 para crear el clúster M0, los usuarios y el acceso; la antigua T020 pasa a ser T110; la cadena de conexión va a SSM. `003` y `004`: colecciones `users` y `sessions`, índices (único en `email`, TTL en `purgeAt`) y pruebas con Testcontainers.
  - MCP: `dynamodb` → `mongodb` (`mongodb-mcp-server`, solo lectura).
- **Decisiones:** ADR-23, ADR-24 y ADR-25; ADR-11 pasa a índice TTL de MongoDB.
- **Costo:** $0.
- **Aprendizaje:** el aislamiento de red de Atlas (*private endpoint*, IP fija) requiere servicios de pago; se documentaron controles compensatorios.
  - `Modelo de datos VirGo.md` pasó al borrador 3 para MongoDB: 22 colecciones con sus índices (únicos, TTL y `2dsphere`), `$jsonSchema` y transacciones en lugar de la tabla única de DynamoDB.
- **Pendiente:** `001` P-03 (dueño de la organización de Atlas); `003` P-02 (confirmar `failedLoginCount` y `lockoutEndsAt` en `users`).

### 2026-11-03 · Repositorio único `Karasu0001/virGO_App`

- **Autor:** equipo + Claude Code (`arquitecto`)
- **Módulo / tareas:** gobernanza · `specs/000`, `001`, `002` · ninguna tarea ejecutada
- **Qué se hizo:**
  - El equipo usará un solo repositorio con carpetas `VirGo-API/` y `VirGo-Movil/` en `main`. La base SDD de Cesar Augusto (raíz, `nv-00` y `nv-01`) queda como gobernanza global y esta constitución se subordina a ella en los principios generales.
  - `.github/` pasa a la raíz; workflows con prefijo `api-`, `security.yml` común, ruleset único y job `changes` en lugar de filtros `paths`.
  - El rol OIDC queda limitado a `Karasu0001/virGO_App`.
- **Evidencia:** PR desde la rama `docs/sdd-virgo`.
- **Decisiones:** ADR-26.
- **Costo:** $0.
- **Aprendizaje:** un check requerido omitido por `paths` deja el PR pendiente; en repositorio único hay que usar jobs condicionales.
- **Pendiente:** `002` P-02 (el repositorio parece privado y el ruleset no aplicaría) y P-03 (uso de `rama-apis`); `.specify/` lo exige la constitución global, pero no existe en el repositorio.

### 2026-11-04 · El repositorio pasa a la organización `virgorent`

- **Autor:** equipo + Claude Code (`arquitecto`)
- **Módulo / tareas:** gobernanza · `specs/000`, `001`, `002` · ninguna tarea ejecutada
- **Qué se hizo:** no se pudo transferir `Karasu0001/virGO_App`, así que se creó `virgorent/virGO_App` vacío y se subieron `main`, `rama-apis`, `rama-backend`, `rama-movil` y `docs/sdd-virgo` con su historial. Las referencias vigentes, incluido el `sub` de OIDC, apuntan al repositorio nuevo; las entradas anteriores de esta bitácora conservan el nombre de entonces.
- **Evidencia:** `git ls-remote` muestra los mismos commits en ambos repositorios.
- **Decisiones:** `virgorent/virGO_App` es el repositorio oficial.
- **Costo:** $0.
- **Aprendizaje:** ninguno.
- **Pendiente:** archivar `Karasu0001/virGO_App` cuando el equipo confirme; `002` P-02 (el repositorio es privado y el ruleset no aplicaría).

### 2026-11-04 · El repositorio ya es público

- **Autor:** equipo + Claude Code (`arquitecto`)
- **Módulo / tareas:** gobernanza · ninguna tarea ejecutada
- **Qué se hizo:** se comprobó que `virgorent/virGO_App` es público, con lo que el ruleset y los revisores de los *environments* ya están disponibles en el plan Free. Se corrigió el conteo de tareas de `001` (11, por la T101 de Atlas).
- **Evidencia:** la API pública de GitHub responde `visibility: public`. `Karasu0001/virGO_App` responde «not found».
- **Decisiones:** se resuelve `VirGo-API` `002` P-02.
- **Costo:** $0.
- **Aprendizaje:** al ser público, todo lo que se suba es visible; Gitleaks y la protección de push son prioritarios.
- **Pendiente:** crear el ruleset (`002`: T207) y aclarar `002` P-03 (`rama-apis`).

### 2026-11-04 · Inicio de `000-arquitectura-base`

- **Autor:** equipo + OpenCode (`dev-dotnet`)
- **Módulo / tareas:** `000-arquitectura-base` · T003, T006–T016 y T026 iniciadas; T017–T019 siguen en curso o pendientes.
- **Qué se hizo:** se alinearon el modelo de datos y los specs con `API-HANDOFF.md`: `VirGo-Database` administra colecciones, validadores e índices; la API no ejecuta DDL; logout elimina la sesión; login usa rate limiting por IP y no persiste campos de bloqueo inexistentes. Se creó `VirGo.slnx` con las cuatro capas, proyectos de pruebas, configuración de opciones, JWT, CORS, rate limiting, Problem Details, correlación, encabezados de seguridad, `/health`, OpenAPI y cliente MongoDB sin inicializador de índices.
- **Evidencia:** `dotnet build VirGo.slnx -c Release` sin errores; `dotnet test VirGo.slnx -c Release` con 2 pruebas unitarias, 7 de integración y 2 de seguridad en verde; arranque manual con `curl /health` = 200 y `/openapi/v1.json` = 200; llave JWT corta rechazada por `OptionsValidationException`.
- **Decisiones:** el contrato de `VirGo-Database` prevalece para la persistencia. La enmienda constitucional 1.2.0 fue aprobada por el equipo y establece que la API no ejecuta DDL ni modifica la estructura de MongoDB.
- **Costo:** $0; no se crearon recursos externos.
- **Aprendizaje:** `MapHealthChecks` devuelve texto plano por defecto y los errores de estado requieren escribir explícitamente `application/problem+json`; ambos casos quedaron cubiertos por pruebas de integración.
- **Pendiente:** validar la conexión real del repository MongoDB contra un ambiente autorizado, ampliar `VirGo.SecurityTests` y continuar con `001`/`002` después de cerrar la base.

### 2026-11-05 · IaC, CI/CD y autenticación inicial

- **Autor:** equipo + OpenCode (`dev-dotnet`, `devops-aws`, `qa`)
- **Módulo / tareas:** `000`–`004` · arquitectura, IaC, pipeline y vertical slice de autenticación
- **Qué se hizo:** se crearon las plantillas SAM/CloudFormation, documentación de Atlas, configuración de Checkov, workflows de CI, CodeQL, seguridad y despliegue manual, ruleset exportado, Gitleaks, Stryker y smoke tests. Se implementaron `POST /api/v1/auth/login` y `POST /api/v1/auth/logout` con JWT de 15 minutos, token de sesión de 30 días, hash de token, validación de sesión por `sid`, eliminación de sesión al cerrar y adaptadores en memoria/MongoDB sin DDL.
- **Evidencia:** `dotnet test VirGo.slnx -c Release` con 2 unitarias, 12 integración y 3 seguridad en verde. Stryker terminó con línea base de 36.36% sobre Application; el umbral permanece en 0 hasta ampliar cobertura.
- **Decisiones:** no se desplegaron recursos; Atlas y AWS requieren credenciales, MCP y confirmación humana.
- **Costo:** $0; solo se agregaron archivos y paquetes locales.
- **Aprendizaje:** Stryker 5 requiere un objeto raíz `stryker-config` y la ruta de mutación debe ser relativa al proyecto mutado.
- **Pendiente:** validar SAM/CloudFormation/Checkov cuando estén disponibles, ejecutar workflows en GitHub, completar cobertura de login/logout y definir los módulos funcionales restantes.

### 2026-11-05 · Validación local final de la sesión

- **Evidencia:** `dotnet build VirGo.slnx -c Release` terminó sin advertencias ni errores; `dotnet test VirGo.slnx -c Release` terminó con 2 unitarias, 18 integración y 3 seguridad aprobadas; `dotnet list VirGo.slnx package --vulnerable --include-transitive` no reportó vulnerabilidades.
- **Limitación:** no están disponibles AWS SAM CLI, `cfn-lint`, Checkov ni Ruby; Docker está instalado pero su daemon no está iniciado. El ejecutable `python` disponible es un alias de Windows sin runtime verificable. Por eso los YAML/IaC no pudieron validarse localmente con esas herramientas. No se ejecutó ningún despliegue.

### 2026-11-05 · Correcciones de CI, alertas y gobernanza

- **Qué se hizo:** el job `changes` ahora compara todo el rango del PR o push, no solo el último commit; el despliegue exige `vars.ALERT_EMAIL`; la suscripción SNS ya no es opcional; se documentaron las alertas externas de Atlas; `AGENTS.md` y esta tabla reflejan la implementación existente.
- **Pendiente:** configurar `ALERT_EMAIL` en el environment `production`, ejecutar el workflow de CI y validar SAM/CloudFormation/Checkov en GitHub.

### 2026-11-05 · API-HANDOFF establecido como fuente de verdad

- **Qué se hizo:** se inspeccionó `C:\Users\Kansj\Downloads\API-HANDOFF.md` completo. El handoff define el contrato de datos y propuestas de endpoints, pero no aprueba una lista fija de siete APIs. `VirGo-Database` manda sobre colecciones, campos, validadores e índices; el modelo raíz queda como contexto histórico.
- **Decisión:** se conserva la Constitución API para gobernanza HTTP y seguridad; los campos y reglas de persistencia se toman del handoff. No se inventan endpoints, campos ni estados no autorizados.
- **Resultado:** la selección se resolvió en el vertical slice registrado en la entrada siguiente y sus specs atómicos `005`–`009`.

### 2026-11-05 · Primer vertical slice de siete endpoints

- **Qué se hizo:** se fijó el primer corte funcional con `POST /auth/register`, `POST /auth/login`, `POST /auth/logout`, `GET /users/me`, `GET /venues`, `POST /quotes` y `POST /bookings`. Se agregaron modelos, repositorios en memoria/MongoDB, registro, perfil, catálogo, cotización y reservación pendiente de pago.
- **Evidencia:** `dotnet test VirGo.slnx -c Release` con 2 unitarias, 18 integración y 3 seguridad aprobadas.
- **Decisiones:** el catálogo filtra solo `published`; las cotizaciones usan el objeto `target` del handoff; las reservaciones generan folios mediante contador atómico en MongoDB y no bloquean fecha mientras están en `pending_payment`.
- **Pendiente:** idempotencia obligatoria de reservaciones, pruebas de concurrencia, validación real contra `VirGo-Database`, usuario de humo y despliegue HTTPS.

### 2026-11-04 · Aprobación de ownership de MongoDB

- **Autor:** equipo + OpenCode (`arquitecto`)
- **Módulo / tareas:** gobernanza · enmienda constitucional 1.2.0
- **Qué se hizo:** el equipo aprobó que `VirGo-Database` sea la autoridad de colecciones, validadores e índices. Se actualizó el Artículo III.4 y se eliminó el estado pendiente de la versión 1.1.0.
- **Evidencia:** aprobación explícita del equipo en la sesión de trabajo; `constitution.md` versión 1.2.0.
- **Decisiones:** la API usa el esquema existente y no crea ni modifica DDL durante el arranque.
- **Costo:** $0; no se crearon recursos externos.
- **Aprendizaje:** el handoff de datos debe mantenerse como contrato de integración entre ambos repositorios.
- **Pendiente:** completar T019 y cerrar las evidencias restantes del módulo `000`.
