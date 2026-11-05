# Tareas — 003 Iniciar sesión (login)

| Campo | Valor |
|---|---|
| Módulo | `003-auth-login` |
| Versión | 1.0.0 |
| Estado | Implementación local; despliegue pendiente |
| Fecha | 2026-11-03 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.2.0 |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**[P]** = puede ejecutarse en paralelo. **🧑** = requiere confirmación humana (Art. XI.5).

**Rama:** `feat/003-auth-login`. Si el CI falla, se corrige en esta misma rama y, si hace falta, se actualiza primero este spec o plan (Art. I.3).

---

## F0 · Contexto y MCP

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T300 | Confirmar que los MCP `mongodb` y `github` responden y que P-01 del spec está resuelta | `002`: T211 | `arquitecto` | Respuestas en `memoria.md` | ⏳ |

## F1 · Dominio y aplicación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T301 | Entidades `User`, `Email` (objeto de valor), `UserRole` y `Session` en `VirGo.Domain` | T300 | `dev-dotnet` | Compila; la prueba de arquitectura de `000` sigue en verde | ✅ Hecha |
| T302 | Abstracciones `IUserRepository`, `ISessionRepository`, `IPasswordHasher`, `ITokenService` y `IRefreshTokenGenerator` | T301 | `dev-dotnet` | Compila; sin referencias a AWS ni a Identity en Application | ✅ Hecha |
| T303 | `LoginCommand`, `LoginHandler` y `LoginResult` con el algoritmo de la sección 6.3 del plan | T302 | `dev-dotnet` | Pruebas unitarias de T308 en verde | ✅ Hecha |

## F2 · Infraestructura

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T304 | [P] `IdentityPasswordHasher`, `JwtTokenService` (claims de la sección 6.2) y `RefreshTokenGenerator` | T302 | `dev-dotnet` | Pruebas unitarias de los claims y de la longitud del token | ✅ Hecha |
| T305 | [P] Repositorios en memoria y `DevUserSeeder` (solo `Development`/`Testing`) | T302 | `dev-dotnet` | Arranque local: la persona sembrada existe; en `Production` no | ✅ Hecha |
| T306 | [P] Repositorios de MongoDB para `users` y `sessions` conforme al contrato existente, sin crear colecciones ni índices | T302 | `dev-dotnet` | Pruebas en verde; los documentos respetan el esquema existente, `tokenHash` no contiene el token y no se ejecuta DDL desde la API | ✅ Hecha |

## F3 · API

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T307 | `AuthController.Login` (`[AllowAnonymous]`), contratos `LoginRequest`/`LoginResponse`, política de *rate limiting* `login`, mapeo de resultados a 200/400/401/429 y ejemplos en OpenAPI | T303–T305 | `dev-dotnet` | `dotnet run` → login con la persona sembrada = 200 | ✅ Hecha |

## F4 · Pruebas

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T308 | Pruebas unitarias y de integración de la sección 8 del plan | T307 | `qa` | `dotnet test` en verde (CA-01 a CA-07) | 🔄 En curso |
| T309 | Pruebas de seguridad, mutación (Stryker sobre el login) y caso nuevo en `tests/smoke/smoke.sh`; revisión de `seguridad` | T308 | `qa` + `seguridad` | `VirGo.SecurityTests` en verde; puntaje de mutación ≥ umbral de `002`; reporte sin hallazgos altos (CA-08) | 🔄 En curso |

## F5 · PR, despliegue y entrega

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T310 | Abrir el PR, dejar el CI en verde, obtener la revisión y fusionar; registrar el cambio de contrato en `memoria.md` de ambos proyectos | T309 | `dev-dotnet` + `arquitecto` | PR fusionado con todos los checks en verde | ⏳ |
| T311 | 🧑 Desplegar con *Run workflow* y verificar el humo del login por HTTPS | T310 | `devops-aws` | Ejecución en verde; 401 por HTTPS (CA-09) | ⏳ |
| T312 | Entregar a Móvil la URL y el JSON probado (petición y respuestas 200/400/401/429); completar `check.md`; marcar el spec como *Desplegado*; registrar lo aprendido y las mejoras a skills o subagentes | T311 | `arquitecto` + `qa` | `check.md` completo; entrada en ambas memorias | ⏳ |

---

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 Correo normalizado | T301, T308 |
| RF-02 Tokens y datos básicos | T303, T304, T307 |
| RF-03 Sesión por persona | T303, T305, T306 |
| RF-04 Respuesta genérica | T303, T309 |
| RF-05 Límite por IP | T307, T308 |
| RF-06 Errores por campo | T307, T308 |
| RF-07 Huella del token | T304, T306, T309 |
| RF-08 OpenAPI | T307, T308 |
| RNF-01 … RNF-05 | T304/T309 · T309 · T303/T309 · T311 · T307/T309 |
| CA-01 … CA-09 | T308 · T308 · T308 · T308/T309 · T308 · T308 · T306/T308 · T309 · T311 |
