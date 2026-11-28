# Check — 101 amenities opcionales en venues

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `101-amenities-opcionales` |
| Estado del spec | Aprobado; verificación local completa; PR pendiente |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | Dictamen 4C; cuatro archivos en `VirGo-API/specs/101-amenities-opcionales/` | ✅ |
| Constitution Check sin ❌ | `plan.md` §3 | ✅ |
| Sin secretos en el repositorio | Revisión del diff: sin secretos ni credenciales introducidos | ✅ |
| Preguntas abiertas respondidas | `spec.md` §8: ninguna | ✅ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `MongoVenueMappingTests.MapWithoutAmenitiesReturnsEmptyArray` · 1/1 | ✅ |
| CA-02 | `MongoVenueMappingTests.MapPreservesAmenitiesArray` · 1/1 | ✅ |
| CA-03 | `MongoVenueMappingTests.MapWithUnexpectedAmenitiesTypeReturnsEmptyArray` · 1/1 | ✅ |
| CA-04 | `git diff --check` sin hallazgos; inventario final sin catálogo, `db/schema`, OpenAPI, DTOs ni specs anteriores | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `dotnet test VirGo-API/tests/VirGo.UnitTests/VirGo.UnitTests.csproj --configuration Release --filter "FullyQualifiedName~MongoVenueMappingTests"` · 3/3; suite Unit 45/45 | ✅ |
| Integración | `dotnet test VirGo-API/VirGo.slnx --configuration Release` · 238/238; Integration 57/57; Contract 12/12; Database 96/96; Performance 3/3; Security 25/25 | ✅ |
| Seguridad | `VirGo.SecurityTests` 25/25; sin cambios de autenticación ni autorización | ✅ |
| Humo | ➖ Sin despliegue en este corte | ➖ |

## 4. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Gitleaks sin hallazgos | Se ejecutará en PR y push a `main` | ⏳ |
| Dependency review sin vulnerabilidades altas | Se ejecutará en PR | ⏳ |
| SAST (CodeQL / linters) en verde | Se ejecutará en PR | ⏳ |
| Workflows con permisos mínimos | Sin cambios de workflows | ✅ |
| Sin credenciales en logs ni artefactos | Revisión del diff y salida de pruebas | ✅ |

## 5. PR e integración continua

- [ ] PRs del módulo: enlaces —
- [ ] Checks requeridos en verde y una revisión.

## 6. Release

- [ ] Versión y commit: —
- [ ] Artefacto firmado y verificado: —

## 7. Memoria y aprendizaje

- [x] Entrada de cierre en `VirGo-API/memoria.md`.
- [x] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes: ninguna.
