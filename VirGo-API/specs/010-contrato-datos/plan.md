# Plan — 010 Contrato de datos con VirGo-Database

| Campo | Valor |
|---|---|
| Módulo | `010-contrato-datos` |
| Versión | 2.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Resumen técnico

`VirGo-Database` exporta `db/schema/` con `src/cli/export.js`. `db/tools/check_schema.py`, que ya existe, comprueba longitudes sin credenciales y, con `--live`, compara contra la base. Un job del CI regenera el esquema y falla si difiere. Los repositorios se prueban con Testcontainers, aplicando los validadores de `db/schema/`.

## 2. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| I | SDD | ✅ Cumple | Spec atómico; tareas `T10NN`, porque el módulo tiene dos dígitos significativos. |
| III | Arquitectura | ⚠️ Justificado | `main` todavía dice que la API crea la estructura; la v1.2.0 del PR #5 lo corrige. |
| V | Seguridad | ✅ Cumple | Longitudes en DTO y validadores; conexión por variable de entorno. |
| VI | Pruebas | ✅ Cumple | Pruebas contra validadores reales. |
| VII | Costo cero | ✅ Cumple | Sin recursos nuevos. |
| VIII | CI/CD | ✅ Cumple | Comprobación en cada PR. |
| X | Memoria | ✅ Cumple | Registrado en `memoria.md`. |

## 3. Decisiones técnicas (ADR)

| ADR | Decisión | Motivo |
|---|---|---|
| ADR-27 | **Modificada:** `db/schema/` es copia generada, no fuente de verdad | `VirGo-Database` coincide con la base y lo acordó el equipo |
| ADR-28 | `_id` y referencias como texto UUID | Lo confirma `VirGo-Database` |
| ADR-32 | **`VirGo-Database` define la estructura**; la API no ejecuta DDL | Constitución v1.2.0 y separación de responsabilidades |
| ADR-33 | Testcontainers con los validadores reales | La persistencia en memoria no detecta rechazos |

La regla de `monthlyStats._id` (ADR-29) queda sin efecto hasta que un spec use esa colección.

## 4. Cambios en la API

| Área | Cambio |
|---|---|
| `db/schema/` | Se regenera; deja de editarse a mano |
| `000` plan | Quitar `MongoIndexInitializer`: la API no crea la estructura (lo cubre el PR #5) |
| `003`, `005`–`009` | Validar con las mismas reglas del catálogo (correo, UUID, enumerados) |
| `002` | Job `api-schema` con regeneración y comparación |

## 5. Costo

$0.

## 6. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Estática | `check_schema.py` (CA-01) y regeneración (CA-04) |
| Integración | Repositorios con Testcontainers (CA-03) |
| Seguridad | Texto de más → 400 (CA-05) |
| Contra la base | `check_schema.py --live` (CA-02) |

## 7. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| El PR #5 se fusiona antes de aplicar el catálogo | El registro fallaría en Atlas: T1004 va antes del despliegue |
| Conflictos entre el PR #5 y este renombrado | Es un cambio de carpeta y referencias; se resuelve al actualizar la rama |

## 8. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Comprobación existente | T1002, T1003 |
| F1 | Conexión con `VirGo-Database` | T1004–T1005 |
| F2 | Pruebas reales y cierre | T1006–T1007 |
