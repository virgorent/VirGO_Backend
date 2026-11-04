# VirGo Mobile (Android)

Android app for **VirGo**, a platform that connects people organizing an event with venues and service providers. This folder of the [`virGO_App`](https://github.com/virgorent/virGO_App) repository consumes the REST API from `../VirGo-API`.

> **Status:** specification phase. The base architecture spec is under review. There is no application code yet.

## Tech stack

| Area | Choice |
|---|---|
| Language and UI | Kotlin, Jetpack Compose, Material 3 |
| Android versions | `minSdk 26`, `targetSdk`/`compileSdk 36` |
| Architecture | MVVM with unidirectional data flow (`StateFlow`), multi-module Gradle (`:app`, `:core:*`, future `:feature:*`) |
| Dependency injection | Hilt |
| Networking | Retrofit, OkHttp, kotlinx.serialization |
| Secure storage | DataStore encrypted with Tink and an Android Keystore key |
| Tests | JUnit, MockK, Turbine, MockWebServer, Robolectric, Compose Testing, Pitest (mutation) |
| Quality and security | ktlint, detekt, Android Lint, CodeQL, Dependabot, Gitleaks, GitHub rulesets |
| CI/CD | GitHub Actions: CI on every PR, manual release to GitHub Releases |

Design reference: the Figma file "Pantallas VirGo" (tokens: indigo `#6366F1`, Manrope and DM Sans).

## Repository layout

```text
VirGo-Movil/
├── constitution.md        # Project rules (Spanish). Everything else must align with it
├── AGENTS.md              # Guide for AI agents and contributors
├── memoria.md             # Project log: decisions (ADR), contract changes, releases
├── opencode.json          # Approved MCP servers (Figma, GitHub)
├── .opencode/             # Sub-agents and internal skills
├── specs/                 # One folder per module: spec.md, plan.md, task.md, check.md
├── app/                   # (planned) Application, navigation, root theme
├── core/                  # (planned) designsystem, network, data, datastore, common, testing
```

GitHub workflows live in `.github/` at the repository root (`api-*`, `movil-*` and `security.yml`). Commands below run inside `VirGo-Movil/`.

## Specs

| Module | Scope | Status |
|---|---|---|
| [`000-arquitectura-base`](specs/000-arquitectura-base/spec.md) | Modules, design system, navigation, networking, secure storage, CI/CD | In review |

Screen specs (`001-…`) are written one screen or flow at a time, once the backend hands over the HTTPS URL and the tested JSON of the endpoints they consume.

## Getting started

These commands will work once module `000` is implemented:

```bash
./gradlew ktlintCheck detekt lint     # Static analysis
./gradlew testDebugUnitTest           # Unit, integration and UI tests (Robolectric)
./gradlew pitest                      # Mutation tests on pure JVM modules
./gradlew assembleDebug               # Debug APK (points to the local API)
```

The release `API_BASE_URL` is the HTTPS URL published by the `VirGo-API` deploy workflow and is injected from a GitHub variable. It is not a secret.

## Releases

Releases are manual: run the `movil-release.yml` workflow from GitHub (**Run workflow**) on `main`. It builds a signed APK and publishes it as a GitHub Release. Publishing to Google Play is planned for later.

## Contributing

1. Pick a task from an approved `task.md`.
2. Branch from `rama-movil` (`feat/NNN-module`), commit with Conventional Commits citing the task ID (`feat(T012): ...`).
3. Merge into `rama-movil`, then open a PR to `main`. CI must be green and one review is required.
4. Fill in the module's `check.md` and add an entry to `memoria.md`.

Project documentation (constitution, specs, log) is written in Spanish.
