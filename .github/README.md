# .github — DevSecOps compartido

GitHub solo lee esta carpeta desde la raíz del repositorio. Contiene los controles de `VirGo-API` y `VirGo-Database`.

| Archivo | Propósito |
|---|---|
| `workflows/security.yml` | Gitleaks (PR, push a `main` y semanal) y dependency review (solo PRs) |
| `dependabot.yml` | Actualizaciones semanales de Actions, NuGet y Gradle |
| `pull_request_template.md` | Checklist SDD + seguridad |
| `SECURITY.md` | Canal privado para reportar vulnerabilidades |
| `rulesets/main-protection.json` | Ruleset de protección de `main` (aplicación manual) |
| `.gitleaks.toml` (raíz) | Allowlist de tokens ficticios usados en documentación de ejemplo |

## Pinning y actualizaciones

- Las acciones se fijan por **SHA completo** y la imagen de Gitleaks por **versión** (`v8.30.1`).
- Dependabot propone las actualizaciones de Actions, NuGet y Gradle; una persona revisa y fusiona el PR.
- La imagen de Gitleaks referenciada en el workflow se actualiza manualmente por versión (Dependabot no cubre imágenes dentro de workflows).
- No usar `@latest` ni tags móviles en workflows.

## Alcance del baseline

Este baseline cubre **secretos** y **dependencias**. El análisis estático (SAST/CodeQL/linters) y las pruebas automatizadas los aportan los workflows de cada proyecto:

- `VirGo-API`: `api-*.yml` (spec `002-pipeline-cicd`).

Hasta que existan esos workflows, el ruleset solo exige `gitleaks` y `dependency review`. Antes del primer PR de código hay que añadir los checks de cada proyecto al ruleset.

`dependency review` se ejecuta solo cuando el PR cambia manifiestos o lockfiles de dependencias. Requiere que un administrador habilite **Dependency graph** en `Settings → Code security and analysis`.

## Pendientes operativos

- Aplicar el ruleset de `main`.
- Habilitar **Dependency graph** para que `dependency review` funcione cuando cambien dependencias.
- Añadir los checks SAST/pruebas de cada proyecto al ruleset antes del primer PR de código.

## Endurecimiento futuro

- `CODEOWNERS` sobre `.github/` y `require_code_owner_review` para que un PR no pueda relajar su propio workflow.
- `required_workflows` en el ruleset para exigir que los checks provengan del workflow de la rama por defecto.

## Aplicar el ruleset

El archivo por sí solo **no protege** `main`: hay que aplicarlo una vez con permisos de administración.

```bash
gh api repos/virgorent/VirGO_Backend/rulesets \
  --method POST \
  --input .github/rulesets/main-protection.json
```

Verificar:

```bash
gh api repos/virgorent/VirGO_Backend/rulesets
```
