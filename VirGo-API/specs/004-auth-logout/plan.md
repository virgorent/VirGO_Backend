# Plan — 004 Cerrar sesión (logout)

| Campo | Valor |
|---|---|
| Módulo | `004-auth-logout` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-03 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.1.0 |

## 1. Resumen técnico

Dos piezas:

1. **Endpoint** `AuthController.Logout` (`[Authorize]`) que llama a `LogoutHandler`: lee `sub` y `sid` del JWT y marca `revokedAt` en la sesión.
2. **Validación de sesión en cada ruta protegida**: en `JwtBearerEvents.OnTokenValidated` se busca en `sessions` el documento `{ _id: sid, userId: sub }`; si no existe, está revocado o expiró, la autenticación falla con 401. Esto vuelve efectivo el logout en todas las instancias de Lambda (RF-03).

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Capas | `Api/Controllers/AuthController` · `Application/Auth/Logout` · `Application/Auth/SessionValidator` · `Infrastructure/Persistence` |
| Lectura de la sesión | `Find` por `_id` y `userId` en el primario (lectura consistente) por solicitud protegida |
| Respuesta | 204 `NoContent` |
| MCP | `mongodb` (ver `revokedAt` en `virgo_dev`) y `github` |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| I | SDD | ✅ Cumple | Spec atómico; tareas `T4NN`. |
| II | Modularidad | ⚠️ Justificado | Redactado por adelantado (Art. II.2). |
| III | Arquitectura | ✅ Cumple | La validación vive en Application y se conecta en Api; repositorio por interfaz. |
| IV | Contrato | ✅ Cumple | `/api/v1/auth/logout`, 204, 401 en Problem Details, OpenAPI con Bearer. |
| V | Seguridad | ✅ Cumple | Revocación del lado del servidor; `sid` obligatorio en los JWT. |
| VI | Pruebas | ✅ Cumple | Cinco tipos (sección 8). |
| VII | Costo cero | ✅ Cumple | 1 lectura indexada por solicitud protegida, dentro de M0; ver sección 7. |
| VIII | CI/CD | ✅ Cumple | PR, despliegue manual y humo. |
| IX | Observabilidad | ✅ Cumple | Log del logout con `correlationId` y el id de sesión, sin tokens. |
| X | Memoria | ✅ Cumple | Cambio de contrato en ambas memorias. |
| XI | Agentes y MCP | ✅ Cumple | Responsables definidos. |
| XII | Idioma | ✅ Cumple | |

## 4. Archivos

```text
src/VirGo.Application/Auth/Logout/LogoutCommand.cs, LogoutHandler.cs
src/VirGo.Application/Auth/SessionValidator.cs
src/VirGo.Domain/Sessions/Session.cs           # método Revoke(now)
src/VirGo.Api/Controllers/AuthController.cs     # acción Logout
src/VirGo.Api/Security/JwtSessionEvents.cs      # OnTokenValidated
tests/VirGo.UnitTests/Auth/Logout…  tests/VirGo.IntegrationTests/Auth/LogoutTests.cs
tests/VirGo.SecurityTests/Auth/LogoutSecurityTests.cs  tests/smoke/smoke.sh (caso nuevo)
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-20 | Validar `sid` contra la colección `sessions` en cada solicitud protegida | Lista negra en memoria; esperar a que expire el JWT (15 min) | En Lambda la memoria no se comparte; esperar 15 min incumple RF-03 |
| ADR-21 | Logout sin cuerpo: la sesión sale del `sid` del JWT | Enviar el *refresh token* en el cuerpo | Menos datos sensibles en tránsito; una persona solo puede cerrar su propia sesión |

## 6. Diseño

### 6.1 Contrato (JSON para el equipo Móvil)

**Petición**

```http
POST /api/v1/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

Sin cuerpo.

**Respuestas**

| Estado | Cuerpo | Cuándo |
|---|---|---|
| 204 | (vacío) | CA-01 |
| 401 | Problem Details, `title`: «La sesión no es válida o expiró.» | CA-02, CA-04, CA-05 |

> **Para Móvil:** ante 204 **o** 401, la app borra sus tokens y vuelve a la pantalla de inicio de sesión. Sin red, también los borra (P-01 del spec).

### 6.2 `LogoutHandler`

1. `GetAsync(userId = sub, sessionId = sid)`.
2. Si no existe o ya está revocada → `Unauthorized` (en la práctica ya lo detecta el paso de autenticación).
3. `session.Revoke(clock.UtcNow)` y `UpdateOneAsync` con el filtro `{ _id: sid, userId: sub, revokedAt: null }` y `$set: { revokedAt }`.
4. Devolver éxito.

### 6.3 `SessionValidator` en `OnTokenValidated`

1. Leer `sub` y `sid`. Si falta `sid` → `context.Fail("missing sid")`.
2. Buscar la sesión. Si no existe, `revokedAt != null` o `expiresAt <= now` → `context.Fail(...)`.
3. El 401 resultante sale en Problem Details por el manejador de `000`.

Las rutas anónimas (`/health`, `/openapi/v1.json`, `POST /auth/login`) no hacen esta lectura.

## 7. Costo

Cada solicitud protegida: 1 lectura por `_id` (índice por defecto), un documento de menos de 1 KB, en la misma región que la Lambda. M0 limita las operaciones por segundo del clúster; para el tráfico del curso alcanza. Si se queda corto, se puede guardar en memoria por 30 s la lista de sesiones válidas, aceptando ese retraso en RNF-01 con una enmienda al spec. **$0.**

## 8. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | `Session.Revoke` fija `revokedAt` y no la cambia si ya estaba; `LogoutHandler` revoca solo la sesión indicada; `SessionValidator` rechaza sesión inexistente, revocada, expirada y sin `sid` |
| Integración | `UpdateOneAsync` condicional contra Testcontainers; login → logout = 204 sin cuerpo; el mismo token en logout y en la ruta de diagnóstico = 401; dos sesiones, se cierra una y la otra sigue en 200 |
| Seguridad | Sin token, token con firma inválida, token sin `sid` y token con `sid` de otra persona → 401; logs sin tokens; el JWT revocado no sirve aunque falten 14 min para expirar |
| Longitudes | ➖ El endpoint no recibe cuerpo; un encabezado `Authorization` de 1500 caracteres da 401 |
| Mutación | Stryker sobre `LogoutHandler`, `SessionValidator` y `Session.Revoke`; sin sobrevivientes en las condiciones de revocación y expiración |
| Humo | `POST {ApiUrl}/api/v1/auth/logout` sin token → 401 `application/problem+json` |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| La lectura extra consume las operaciones por segundo de M0 | Medir en T407; plan B de la sección 7 |
| Olvidar la validación en un endpoint nuevo | Está en el esquema de autenticación por defecto, no en cada controlador |
| La renovación de sesión (`006`) acepta sesiones revocadas | Queda como criterio obligatorio en el spec `006` |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Contexto | T400 |
| F1 | Dominio, aplicación y API | T401–T403 |
| F2 | Pruebas | T404–T405 |
| F3 | PR, despliegue y entrega | T406–T408 |
