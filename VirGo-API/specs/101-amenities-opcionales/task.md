# Tareas — 101 amenities opcionales en venues

| Campo | Valor |
|---|---|
| Módulo | `101-amenities-opcionales` |
| Versión | 0.1.0 |
| Estado | Aprobado por dictamen 4C; ejecución en BUILD MODE |
| Fecha | 2026-11-27 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.2.0 ratificada |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**[P]** = puede ejecutarse en paralelo con otras [P] de la misma fase.

**🧑 Requiere confirmación humana** = cambia configuración de GitHub, secretos o publica un release.

## F0 · Documentación SDD

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T101 | Crear `spec.md`, `plan.md`, `task.md` y `check.md` desde las plantillas oficiales | — | `dev-dotnet` | Cuatro archivos presentes en `specs/101-amenities-opcionales/` | ✅ |

## F1 · Implementación y pruebas

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T102 | Lectura defensiva de `amenities` en `MongoVenueRepository.Map` | T101 | `dev-dotnet` | Diff limitado a la expresión de `amenities` | ✅ |
| T103 | Crear `MongoVenueMappingTests.cs` con ausencia, tipo inesperado y array válido | T102 | `qa` | Prueba dirigida en verde | ✅ |

## F2 · Validación y cierre

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T104 | Ejecutar prueba dirigida, suite completa y `git diff --check` | T103 | `qa` | Evidencia en [check.md](check.md) | ✅ |
| T105 | Registrar el cierre del corte en `VirGo-API/memoria.md` | T104 | `arquitecto` | Entrada 2026-11-27 presente | ✅ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| HT-01 | T102, T103 |
| HT-02 | T102, T103 |
| RF-01 | T102, T103 |
| RF-02 | T102, T103 |
| RF-03 | T102, T103 |
| RF-04 | T102, T104 |
| RNF-01 | T104 |
| RNF-02 | T102 |
| RNF-03 | T103, T104 |
| RNF-04 | T104 |
| RNF-05 | T105 |
| CA-01 | T103, T104 |
| CA-02 | T103, T104 |
| CA-03 | T103, T104 |
| CA-04 | T104 |
