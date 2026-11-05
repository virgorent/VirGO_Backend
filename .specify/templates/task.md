# Tareas — NNN <nombre-del-módulo>

| Campo | Valor |
|---|---|
| Módulo | `NNN-<nombre>` |
| Versión | 0.1.0 |
| Estado | Borrador. **Las tareas no se ejecutan hasta aprobar este archivo.** |
| Fecha | AAAA-MM-DD |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | vX.Y.Z |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**[P]** = puede ejecutarse en paralelo con otras [P] de la misma fase.

**🧑 Requiere confirmación humana** = cambia configuración de GitHub, secretos o publica un release.

## F0 · <nombre>

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T001 | <...> | — | <agente> | <comando o evidencia> | ⏳ |
| T002 | 🧑 <...> | T001 | <agente> | <...> | ⏳ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| HT-01 | T001 |
| RF-01 | T001 |
| RNF-01 | T001 |
| CA-01 | T001 |
