# Tareas — 002 Pipeline CI/CD con DevSecOps

| Campo | Valor |
|---|---|
| Módulo | `002-pipeline-cicd` |
| Versión | 1.0.0 |
| Estado | Borrador. **Las tareas aún no se ejecutan.** |
| Fecha | 2026-11-03 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.1.0 |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**[P]** = puede ejecutarse en paralelo. **🧑** = requiere confirmación humana (Art. XI.5).

**Equivalencias con `000`:** T204 = antes T028 · T205 = T029 · T207 = T002 · T208 = T031 · T209 = T032 · T210 = T033.

---

## F0 · MCP y plantillas del repositorio

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T200 | Instalar y probar el MCP `github` de `opencode.json` con un token de grano fino (solo este repositorio; lectura de código, PRs y Actions) | — | `devops-aws` | `opencode mcp list` lo muestra conectado | ⏳ |
| T201 | [P] Actualizar `.github/pull_request_template.md` con el ID de tarea, el spec, los artículos revisados y el estado del `check.md` | `000`: T005 | `devops-aws` | La plantilla aparece al abrir un PR | ⏳ |

## F1 · Pruebas de seguridad y mutación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T202 | Crear `tests/VirGo.SecurityTests` con las pruebas de la sección 6.4 del plan | `000`: T016, T026 | `qa` + `seguridad` | `dotnet test tests/VirGo.SecurityTests` en verde | ⏳ |
| T203 | [P] Agregar Stryker.NET como herramienta local y `stryker-config.json` (proyectos `Domain` y `Application`, reporte HTML y JSON); medir la línea base | `000`: T025 | `qa` | `dotnet stryker` termina; puntaje base registrado en `memoria.md` | ⏳ |

## F2 · CI y análisis de seguridad

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T204 | En la raíz: `.github/workflows/api-ci.yml` con los jobs `changes`, `api-build-test`, `api-mutation` y `api-iac`, y `security.yml` con `secrets` y `dependency-review` (sección 6.1 del plan) | T202, T203, `001`: T106 | `devops-aws` | PR de prueba con los 5 checks en verde (CA-04); un PR que solo toca `VirGo-Movil/` no queda pendiente | ⏳ |
| T205 | [P] `.github/workflows/api-codeql.yml` para C# | T204 | `seguridad` | Check de CodeQL en el PR sin alertas altas | ⏳ |
| T206 | [P] `.gitleaks.toml` y prueba con una llave falsa en un PR desechable | T204 | `seguridad` | El job `secrets` falla con la llave falsa (CA-03); PR cerrado sin fusionar | ⏳ |

## F3 · Ruleset

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T207 | 🧑 Crear el ruleset único `main-protection` del repositorio (sección 6.5 del plan), activar la protección de push contra secretos y exportar el JSON a `.github/rulesets/` | T204, T205 | `devops-aws` | *Push* directo rechazado (CA-01); PR con prueba en rojo bloqueado (CA-02) | ⏳ |

## F4 · CD manual y humo

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T208 | `.github/workflows/api-deploy.yml` (solo `workflow_dispatch`, `environment: production`, OIDC) y `VirGo-API/tests/smoke/smoke.sh` (sección 6.3 del plan) | T204, `001`: T107 | `devops-aws` | YAML válido; usa `id-token: write` y ninguna llave de acceso; desde otra rama se niega (CA-06) | ⏳ |
| T209 | 🧑 Primer despliegue con *Run workflow*: humo en verde, URL en el resumen, recursos permitidos, alarmas activas y arranque en frío medido | T208 | `devops-aws` + `qa` | URL `https://…execute-api.us-east-1.amazonaws.com` (CA-05; `001`: CA-02, CA-03, CA-08); un humo fallido deja el workflow en rojo (CA-07) | ⏳ |
| T210 | Entregar la URL HTTPS y el enlace a `/openapi/v1.json` al proyecto Móvil y registrarlos en la `memoria.md` de ambos proyectos | T209 | `arquitecto` | La URL aparece en las dos memorias y como `VIRGO_API_BASE_URL` en Móvil (CA-08) | ⏳ |

## F5 · Cierre

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T211 | Completar `check.md`, fijar el umbral de mutación con la línea base, ejecutar `verificar-constitucion` y registrar el cierre y lo aprendido en `memoria.md` | T200–T210 | `qa` + `arquitecto` | `check.md` completo y resultado «Alineado» | ⏳ |

---

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 Ruleset | T207 |
| RF-02 Pruebas en el CI | T202, T204 |
| RF-03 Mutación | T203, T204 |
| RF-04 Análisis de seguridad | T204, T205, T206 |
| RF-05 Validación de IaC | T204 |
| RF-06 CD manual con OIDC | T208 |
| RF-07 Humo | T208, T209 |
| RF-08 Resumen con URL | T208, T209 |
| RF-09 Dependabot | `000`: T005 |
| RF-10 Plantilla de PR | T201 |
| RNF-01 … RNF-04 | T204 · T204 · T204/T208 · T209 |
| CA-01 … CA-08 | T207 · T207 · T206 · T204 · T209 · T208 · T209 · T210 |
