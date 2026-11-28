# Plan — 101 amenities opcionales en venues

| Campo | Valor |
|---|---|
| Módulo | `101-amenities-opcionales` |
| Versión | 0.1.0 |
| Estado | Aprobado para implementación por dictamen 4C |
| Fecha | 2026-11-27 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.2.0 ratificada |

## 1. Resumen técnico

- Localizar `MongoVenueRepository.Map` en `VirGo-API/src/VirGo.Infrastructure/MarketplaceRepositories.cs`.
- Reemplazar el acceso directo a `document["amenities"]` por una lectura con `TryGetValue` y verificación de `IsBsonArray`.
- Devolver `[]` cuando el campo falte o tenga un tipo BSON inesperado; conservar el mapeo actual cuando sea un array válido.
- Probar el mapper privado por reflexión para evitar cambiar su visibilidad y no introducir `InternalsVisibleTo`.
- Ejecutar prueba dirigida, suite completa y `git diff --check`; cerrar solo `VirGo-API/memoria.md`.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | .NET 10, ASP.NET Core; proyecto `VirGo.Infrastructure` |
| Persistencia | MongoDB solo como origen de lectura en el mapper; sin DDL, init ni escrituras |
| Seguridad | Sin secretos, sin credenciales, sin conexiones externas |
| CI/CD | Sin cambios; PR posterior sujeto a los checks requeridos existentes |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| I | SDD | ✅ | Este módulo tiene los cuatro documentos antes de tocar código. |
| III.4 | Persistencia y fuente de verdad | ✅ | El catálogo manda; no se edita `db/schema/` ni se ejecuta DDL. |
| IV.2 | Versionado de contrato | ✅ | No se cambia la ruta `/api/v1`. |
| IV.5 | OpenAPI | ✅ | No se modifica el documento generado; solo se normaliza una lectura interna. |
| V | Seguridad DevSecOps | ✅ | Sin secretos, credenciales, infraestructura ni despliegue. |
| VII | Costo cero | ✅ | No se crean recursos; `$0 USD`. |
| X | Memoria | ✅ | El cierre se registra en `VirGo-API/memoria.md`, no en la bitácora global. |

> Un ❌ bloquea la aprobación del plan.

## 4. Estructura

```text
VirGo-API/specs/101-amenities-opcionales/
├── spec.md
├── plan.md
├── task.md
└── check.md

VirGo-API/src/VirGo.Infrastructure/
└── MarketplaceRepositories.cs

VirGo-API/tests/VirGo.UnitTests/
└── MongoVenueMappingTests.cs

VirGo-API/memoria.md
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-101-01 | Normalizar `amenities` dentro de `MongoVenueRepository.Map` | Modificar el catálogo o el esquema generado | El catálogo es autoridad y declara el campo opcional; `db/schema/` es derivado. |
| ADR-101-02 | Probar el mapper privado mediante reflexión | Cambiar `Map` a `internal` y agregar `InternalsVisibleTo` | La reflexión mantiene el cambio de producción en una sola línea y evita ampliar la superficie de visibilidad. |
| ADR-101-03 | No agregar integración contra MongoDB | Testcontainers o base efímera | El corte solo cubre la lógica de mapeo y la validación prohibió init/escrituras/base externa. |

## 6. Diseño

### 6.1 Mapper

La expresión actual:

```csharp
Amenities = document["amenities"].AsBsonArray.Select(value => value.AsString).ToArray(),
```

se sustituye por una lectura condicional equivalente a:

```csharp
Amenities = document.TryGetValue("amenities", out var amenities) && amenities.IsBsonArray
    ? amenities.AsBsonArray.Select(value => value.AsString).ToArray()
    : [],
```

Si el campo existe y es `BsonArray`, se conserva la conversión actual. En ausencia o tipo inesperado, se asigna `[]`.

### 6.2 Seguridad

No hay entradas nuevas del usuario, secretos ni credenciales. La normalización reduce una excepción interna sin relajar validaciones ni autorización.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| Secretos | Gitleaks | PR y push a `main` |
| Dependencias | Dependabot + dependency review | Semanal y PR |
| SAST | CodeQL / linters | PR |
| Pruebas | `dotnet test VirGo-API/VirGo.slnx --configuration Release` | PR |
| Release | Manual, fuera de este corte | — |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `MongoVenueMappingTests.cs` | CA-01, CA-02 y CA-03 sobre el mapper |
| Integración | Suite existente | Regresión de endpoints y contrato |
| Seguridad | Suite existente | Sin cambios de autenticación/autorización |
| Humo | Fuera de alcance | Sin despliegue en este corte |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| La reflexión depende del nombre del método privado | El test falla explícitamente si `Map` no existe; no se cambia visibilidad. |
| Un `BsonArray` con elementos no string aún puede lanzar | El catálogo valida los elementos como enum; este corte no amplía el requisito. |
| Staging accidental del worktree original | Ejecución en worktree aislado y verificación final de `git status`. |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Documentos SDD y aprobación | T101 |
| F1 | Implementación y pruebas unitarias | T102–T103 |
| F2 | Validación, bitácora y auditoría final | T104–T105 |
