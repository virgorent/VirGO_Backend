# Spec — 010 Contrato de datos con VirGo-Database

| Campo | Valor |
|---|---|
| Módulo | `010-contrato-datos` |
| Versión | 2.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Constitución | v1.1.0 en `main`; v1.2.0 en el PR #5, que asigna la estructura a `VirGo-Database` |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Origen | Antes `007-esquema-mongodb`. Se renumeró porque el PR #5 usa `007-venues-catalog`, y cambió de objetivo al aparecer `VirGo-Database` |

## 1. Contexto y objetivo

`VirGo-Database` define la estructura real de MongoDB y coincide 22 de 22 con `virgo_dev`. `VirGo-API/db/schema/` nació como fuente de verdad, pero difiere en 8 colecciones, y la API del PR #5 escribe campos que la base rechazaría.

El objetivo es que la API use el mismo contrato que la base: `db/schema/` pasa a ser una **copia generada** del catálogo, y el CI detecta si se separan.

## 2. Historias técnicas

| ID | Historia |
|---|---|
| HT-01 | Como **desarrollador**, quiero saber qué define la estructura, para no adivinar cuál documento es correcto. |
| HT-02 | Como **equipo**, quiero detectar en el CI si la API y la base se separan, para no descubrirlo en producción. |
| HT-03 | Como **QA**, quiero probar los repositorios contra los validadores reales, porque la persistencia en memoria no los detecta. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | `VirGo-Database/src/catalog` es la fuente de verdad; `db/schema/` no se edita a mano. |
| RF-02 | La API no ejecuta DDL ni modifica la estructura de MongoDB. |
| RF-03 | Una comprobación sin credenciales falla si algún texto libre no tiene límite. |
| RF-04 | Una comprobación contra la base lista las diferencias. |
| RF-05 | Los repositorios de MongoDB se prueban contra una base con los validadores reales. |
| RF-06 | Los valores permitidos en los DTO coinciden con los del catálogo. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** ninguna credencial en el repositorio. |
| RNF-02 | **Costo:** $0. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el repositorio, **cuando** ejecuto la comprobación sin credenciales, **entonces** reporta 0 campos sin límite. |
| CA-02 | **Dado** `virgo_dev`, **cuando** ejecuto la comprobación contra la base, **entonces** reporta 0 diferencias. |
| CA-03 | **Dado** las 7 APIs, **cuando** sus repositorios escriben contra los validadores reales, **entonces** ningún documento válido es rechazado. |
| CA-04 | **Dado** un cambio a mano en `db/schema/`, **cuando** abro un PR, **entonces** el CI falla. |
| CA-05 | **Dado** un texto que supera su límite, **cuando** llega a la API, **entonces** responde 400, no 500. |

## 6. Fuera de alcance

- Cambiar el catálogo: lo hace [`VirGo-Database/specs/001-contrato-con-api`](../../../VirGo-Database/specs/001-contrato-con-api/spec.md).

## 7. Supuestos, dependencias y preguntas abiertas

**Dependencias:** `VirGo-Database` `001`, y el PR #5, que aporta la constitución v1.2.0 y las APIs.

Las preguntas sobre valores permitidos y longitudes están en el spec `001` de `VirGo-Database`.
