# Tareas — 010 Contrato de datos con VirGo-Database

| Campo | Valor |
|---|---|
| Módulo | `010-contrato-datos` |
| Versión | 2.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**🧑** = requiere confirmación humana.

**Equivalencias:** T1000–T1006 eran T700–T706 de `007-esquema-mongodb`. T1000 y T1001 pasaron a `VirGo-Database` `001` (T101 y usuarios en Atlas).

---

## F0 · Comprobación existente

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1002 | `db/schema/` con correcciones y longitudes | — | `arquitecto` | `check_schema.py`: 0 diferencias (CA-01) | ✅ |
| T1003 | `db/tools/check_schema.py` y comparación con la base | T1002 | `arquitecto` | `--live` lista las diferencias | ✅ |

## F1 · Conexión con VirGo-Database

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1004 | 🧑 Aplicar `VirGo-Database` `001` a `virgo_dev` | `VirGo-Database` `001`: T105 | Esau | `check_schema.py --live --db virgo_dev`: 22 colecciones, 0 diferencias (2026-11-13); verificado, no reaplicado | ✅ |
| T1005 | Job `api-schema`: regenerar desde el catálogo y fallar si `db/schema/` difiere | `VirGo-Database` `001`: T104 | `devops-aws` | PR con un cambio a mano falla (CA-04) | ⏳ |

## F2 · Pruebas reales y cierre

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T1006 | Pruebas de repositorios con Testcontainers y validadores reales; DTO con las reglas del catálogo | T1005, PR #5 | `dev-dotnet` + `qa` | Ningún documento válido rechazado; texto de más → 400 (CA-03, CA-05) | ⏳ |
| T1007 | Completar `check.md` y registrar lo aprendido | T1004–T1006 | `qa` + `arquitecto` | `check.md` completo | ⏳ |

---

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 Fuente de verdad | T1005 |
| RF-02 Sin DDL en la API | PR #5 |
| RF-03 Comprobación sin credenciales | T1003 |
| RF-04 Comprobación contra la base | T1003, T1004 |
| RF-05 Validadores reales | T1006 |
| RF-06 Valores permitidos | T1006 |
| RNF-01, RNF-02 | T1003 · T1002 |
| CA-01 … CA-05 | T1002 · T1004 · T1006 · T1005 · T1006 |
