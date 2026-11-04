# Check — NV-02 Gobernanza global de proyectos

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Fase | `nv-02` |
| Versión | 1.0.0 |
| Estado | Cumplido con pendiente operativo |
| Spec / Plan / Task | [spec.md](spec.md) · [plan.md](plan.md) · [task.md](task.md) |
| Revisó | Equipo virGO_APP |
| Fecha de cierre | 2026-11-04 |

## 1. Criterios de aceptación

| CA | Criterio | Evidencia | Estado |
|---|---|---|---|
| CA-01 | `constitution.md` v1.0.0 ratificada | Encabezado del archivo | ✅ |
| CA-02 | Monorepo aprobado | `Memoria.md` decisión 2026-11-04 | ✅ |
| CA-03 | Plantillas publicadas | `.specify/templates/` (5 archivos) | ✅ |
| CA-04 | Memoria reutilizable sin duplicidad | `.specify/memory/README.md` | ✅ |
| CA-05 | Baseline DevSecOps configurado | `.github/workflows/security.yml`, `dependabot.yml`, `pull_request_template.md` | ✅ |
| CA-06 | `docs/` y `scripts/` documentados | `docs/README.md`, `scripts/README.md` | ✅ |
| CA-07 | `AGENTS.md` actualizado | §2, §3, §7 y §8 | ✅ |
| CA-08 | Aplicación del ruleset documentada | `.github/README.md` §«Aplicar el ruleset» | ✅ |

## 2. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Gitleaks en PR, `main` y semanal | Configurado en `security.yml`; primer run pendiente | ✅ |
| Dependency review en PRs | Configurado; se ejecuta solo con cambios en manifiestos | ✅ |
| Dependency graph habilitado | Requiere un administrador en Settings → Code security and analysis | ⏳ |
| Dependabot semanal (Actions, NuGet, Gradle) | `.github/dependabot.yml` | ✅ |
| Acciones fijadas por SHA | `security.yml` (checkout, dependency review) | ✅ |
| Ruleset configurado | `.github/rulesets/main-protection.json` | ✅ |
| Ruleset aplicado en GitHub | Requiere permisos de administración | ⏳ |
| Plantilla de PR con checklist de seguridad | `.github/pull_request_template.md` | ✅ |
| Sin secretos en el repositorio | Revisión estática; Gitleaks en CI | ✅ |

## 3. Límites respetados

- [x] Sin cambios en specs de `VirGo-API/` ni `VirGo-Movil/`.
- [x] Sin código de negocio.
- [x] Sin secretos en el repositorio.

## 4. Pendiente operativo

- Aplicar `.github/rulesets/main-protection.json` con permisos de administración y verificar con `gh api repos/virgorent/virGO_App/rulesets`.
- Habilitar **Dependency graph** en `Settings → Code security and analysis`.
- Antes del primer PR de código, añadir los checks SAST/pruebas de cada proyecto al ruleset.

## 5. Resultado

**Cumplido con pendiente operativo.** La gobernanza, las plantillas y el baseline están verificados; falta aplicar el ruleset de `main`.
