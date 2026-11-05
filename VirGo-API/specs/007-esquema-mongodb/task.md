# Tareas — 007 Esquema de MongoDB

| Campo | Valor |
|---|---|
| Módulo | `007-esquema-mongodb` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-04 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.1.0 |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**🧑** = requiere confirmación humana (Art. XI.5).

---

## F0 · Usuarios y decisiones

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T700 | 🧑 Crear en Atlas `virgo-api-dev` (`readWrite@virgo_dev`) y `virgo-mcp` (`read`) con contraseñas de 32 caracteres o más | — | `devops-aws` | `virgo-mcp` lee y no puede escribir | ⏳ |
| T701 | 🧑 Responder las preguntas P-01 a P-05 del spec | — | Equipo | Respuestas en `memoria.md` | ⏳ |

## F1 · Esquema en el repositorio

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T702 | Exportar `virgo_dev` a `db/schema/`, corregirlo según la sección 6.1 del plan y agregar `maxLength` | — | `arquitecto` | `check_schema.py` reporta 0 diferencias (CA-01) | ✅ |
| T703 | Crear `db/tools/check_schema.py` y alinear el modelo y los specs `000`, `003` y `004` | T702 | `arquitecto` | `--live` lista las diferencias pendientes | ✅ |

## F2 · Aplicación y cierre

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T704 | 🧑 Aplicar `db/schema/` a `virgo_dev` (sección 6.2 del plan) | T701 | `devops-aws` | `check_schema.py --live` reporta 0 diferencias (CA-02) | ⏳ |
| T705 | Comprobar el rechazo de 1500 caracteres y la aceptación del bloqueo, y borrar los documentos de prueba | T704 | `qa` | Salida en `check.md` (CA-03, CA-04) | ⏳ |
| T706 | Agregar `check_schema.py` al CI, completar `check.md` y registrar lo aprendido | T705, `002`: T204 | `qa` + `arquitecto` | PR de prueba con un campo sin límite falla (CA-05) | ⏳ |

---

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 Archivos versionados | T702 |
| RF-02 Longitudes | T702 |
| RF-03 Comprobación sin credenciales | T703, T706 |
| RF-04 Comprobación contra la base | T703, T704 |
| RF-05 Documentos alineados | T703 |
| RF-06 Base reproducible | T704, `000`: T019 |
| RNF-01, RNF-02 | T703 · T702 |
| CA-01 … CA-05 | T702 · T704 · T705 · T705 · T706 |
