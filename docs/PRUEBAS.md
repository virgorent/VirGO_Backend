# Estado de las pruebas de VirGo

**Fecha de ejecución:** 2026-11-05. Los resultados de abajo vienen de ejecutar las pruebas, no de contarlas.

## Resumen

| Proyecto | Tipo | Pruebas | Resultado | Ubicación |
|---|---|---|---|---|
| `VirGo-API` | Unitarias | 2 | ✅ 2 de 2 | PR #5 |
| `VirGo-API` | Integración | 30 | ✅ 30 de 30 | PR #5 |
| `VirGo-API` | Seguridad | 3 | ✅ 3 de 3 | PR #5 |
| `VirGo-API` | Mutación (Stryker) | — | ✅ El job del CI pasó; falta registrar el puntaje | PR #5 |
| `VirGo-API` | Humo | Script listo | ⏳ Sin ejecutar: aún no hay despliegue | PR #5 |
| `VirGo-Database` | Unitarias | 16 | ✅ 16 de 16 | `main` |
| `VirGo-Database` | Integración | 20 declaradas | ⏳ No ejecutadas en esta revisión: requieren MongoDB local | `main` |
| `VirGo-Movil` | Todas | 0 | ❌ Aún no existen | — |

**Total ejecutado:** 51 pruebas, 51 aprobadas y 0 fallidas.

## Cómo reproducirlas

```bash
# VirGo-API (rama feat/vertical-slice-7-apis, PR #5)
cd VirGo-API
dotnet test VirGo.slnx -c Release

# VirGo-Database
cd VirGo-Database
npm ci
npm test
```

## Qué cubren

### VirGo-API

| Proyecto | Pruebas |
|---|---|
| `VirGo.UnitTests` | El dominio no depende de otras capas; el repositorio en memoria guarda y lee |
| `VirGo.SecurityTests` | Token sin sesión rechazado; encabezados de seguridad; CORS con origen no permitido |
| `VirGo.IntegrationTests` · `AuthTests` (5) | Login y logout |
| `VirGo.IntegrationTests` · `BaseApiTests` (6) | Salud, errores y OpenAPI |
| `VirGo.IntegrationTests` · `RateLimitTests` (1) | Límite de solicitudes |
| `VirGo.IntegrationTests` · `VerticalSliceTests` (18) | Registro, perfil, catálogo, cotizaciones, reservaciones, idempotencia y bloqueo de cuenta |

### VirGo-Database

| Archivo | Pruebas |
|---|---|
| `tests/unit/catalog.test.js` | 11: catálogo de colecciones e índices |
| `tests/unit/format.test.js` | 2: formato del plan y de la verificación |
| `tests/unit/plan.test.js` | 3: comparación de índices |
| `tests/integration/database.test.js` | 20 declaradas, contra MongoDB 8 local |

## Análisis automático en GitHub

| Check | Resultado |
|---|---|
| `gitleaks` | ✅ En verde en `main` |
| `dependency review` | ✅ En verde en los PRs #5 y #9 |
| `CodeQL` | ✅ Check en verde en el PR #5, con 3 avisos por revisar |
| `api-iac` (SAM, `cfn-lint`, Checkov) | ✅ En verde en el PR #5 |
| `api-schema` | ✅ En verde en el PR #5 |

## Limitaciones conocidas

- Las 35 pruebas de la API usan **persistencia en memoria**: todavía no prueban contra MongoDB ni sus validadores.
- Por eso no detectan errores que solo aparecen en Atlas. Una revisión de código encontró varios, como el conflicto al generar el folio de las reservaciones.
- `virgo_dev` tiene **25 diferencias** frente a `VirGo-API/db/schema/`, pendientes de `VirGo-Database` `001`.
- Solo hay 3 pruebas de seguridad y 2 unitarias en la API; las pruebas de longitud por campo aún no existen.
- Móvil no tiene pruebas, aunque su arquitectura base ya está en `main`.

## Lo que sigue

1. Pruebas de repositorios con Testcontainers y los validadores reales (`010-contrato-datos`, T1006).
2. Pruebas de longitud por campo: límite, límite + 1 y 1500 caracteres.
3. Ampliar las pruebas unitarias y de seguridad de la API.
4. Pruebas de Móvil: T028, T029 y T038.
5. Ejecutar las pruebas de integración de `VirGo-Database` en el CI (T006).
6. Ejecutar las pruebas de humo tras el primer despliegue.
