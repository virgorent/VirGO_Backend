# Tareas — 010 Asistente y contacto

| Campo | Valor |
|---|---|
| Módulo | `010-soporte` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión |
| Fecha | 2026-11-05 |
| Fuente | `src/catalog` y `docs/COLLECTIONS.md` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1001 | Comprobar `assistantMessages` contra el catálogo | — | Esau | Comparación de validadores: coincide (2026-11-05) | ✅ |
| T1002 | Comprobar `contactMessages` contra el catálogo | — | Esau | Comparación de validadores: coincide (2026-11-05) | ✅ |
| T1003 | Agregar `maxLength` a los textos de este dominio | `001`: T103 | Esau | 0 textos sin límite | ⏳ |
