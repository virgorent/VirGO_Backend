# Tareas — 001 Contrato de datos con la API

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

**🧑** = requiere confirmación humana.

---

## F0 · Decisiones

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T101 | 🧑 Responder P-01, P-02 y P-05 del spec | — | Equipo | Respuestas en `Memoria.md` | ⏳ |

## F1 · Catálogo y pruebas

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T102 | Campos de `users`, `sessions` y `bookings`, e índices nuevos | T101 | Esau | `npm test` en verde (RF-01 a RF-03) | ⏳ |
| T103 | `string(max)` y longitud en todo texto libre; enumerados de P-01 y P-02 | T101 | Esau | Prueba unitaria: 0 textos sin límite (RF-04, RF-05) | ⏳ |
| T104 | `src/cli/export.js` y regeneración de `VirGo-API/db/schema/` | T102, T103 | Esau | `db/schema/` igual al catálogo (RF-06, CA-05) | ⏳ |

## F2 · Aplicación

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T105 | 🧑 `db:plan:dev`, revisar y `db:init:dev -- --yes` | T104 | Esau | `db:verify:dev` sin diferencias (CA-04) | ⏳ |
| T106 | Pruebas de aceptación en `virgo_dev`, borrando después los documentos de prueba | T105 | Esau | Salida en `check.md` (CA-01, CA-02, CA-03) | ⏳ |

---

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01, RF-02, RF-03 | T102 |
| RF-04, RF-05 | T103 |
| RF-06 | T104 |
| RF-07 | T105 |
| RNF-01, RNF-02 | T105 |
| CA-01 … CA-05 | T106 · T106 · T106 · T105 · T104 |
