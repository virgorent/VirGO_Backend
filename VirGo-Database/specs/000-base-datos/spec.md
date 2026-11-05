# Spec — 000 Base de datos MongoDB

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión del equipo |
| Fecha | 2026-11-05 |
| Gobierno | Constitución global (`../../../constitution.md`) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Origen | Spec retrospectivo: `VirGo-Database` se construyó antes de tener spec; este documento registra lo que existe |

## 1. Contexto y objetivo

VirGo necesita una definición versionada de su base de datos, independiente de la API, que se pueda aplicar y verificar sin pasos manuales. `VirGo-Database` cumple ese papel: define las colecciones, los validadores y los índices, y es el único proyecto que modifica la estructura de MongoDB.

## 2. Historias técnicas

| ID | Historia |
|---|---|
| HT-01 | Como **equipo**, quiero un catálogo versionado de colecciones, validadores e índices, para que la estructura no dependa de cambios manuales en Atlas. |
| HT-02 | Como **operador**, quiero revisar el plan antes de aplicarlo, para saber exactamente qué va a cambiar. |
| HT-03 | Como **operador**, quiero detectar diferencias entre el catálogo y la base, para saber si alguien cambió la estructura. |
| HT-04 | Como **equipo Backend**, quiero un documento de transferencia, para construir la API sobre un contrato claro. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | El catálogo declara 22 colecciones con validación estricta. |
| RF-02 | El catálogo declara 37 índices secundarios, incluidos únicos, TTL, geoespacial y único parcial. |
| RF-03 | El inicializador es idempotente y tiene los modos `plan`, `validate` y `apply`. |
| RF-04 | `apply` imprime el plan y exige `--yes`; en producción exige además `--confirm-production`. |
| RF-05 | La configuración valida el nombre de la base contra el entorno: `virgo_dev`, `virgo` o `virgo_test_*`. |
| RF-06 | Nunca se eliminan colecciones, índices ni documentos. |
| RF-07 | Un verificador de solo lectura reporta diferencias con la base. |
| RF-08 | `docs/API-HANDOFF.md` documenta el contrato para el backend. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** ninguna credencial en el repositorio y ninguna URI en los logs. |
| RNF-02 | **Costo:** $0, con MongoDB Atlas M0. |
| RNF-03 | **Pruebas:** unitarias sin MongoDB e integración contra MongoDB 8 local. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** `virgo_dev`, **cuando** comparo sus validadores con el catálogo, **entonces** las 22 colecciones coinciden. |
| CA-02 | **Dado** el modo `plan`, **cuando** lo ejecuto, **entonces** no se escribe nada en la base. |
| CA-03 | **Dado** `apply` sin `--yes`, **cuando** lo ejecuto, **entonces** no se escribe nada. |
| CA-04 | **Dado** el repositorio, **cuando** busco secretos, **entonces** solo aparecen valores de ejemplo. |
| CA-05 | **Dado** `npm test`, **cuando** termina, **entonces** las pruebas pasan. |

## 6. Fuera de alcance

- La API, los pagos y la lógica de negocio.
- Sembrar datos.
- Crear usuarios y redes en Atlas.
- Los cambios que pide la API: van en [`001-contrato-con-api`](../001-contrato-con-api/spec.md).

## 7. Supuestos, dependencias y preguntas abiertas

**Dependencias:** clúster `virgo-dev` en Atlas.

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | `docs/API-HANDOFF.zip` duplica `API-HANDOFF.md`. ¿Se quita del repositorio? | Esau |
| P-02 | `API-HANDOFF.md` dice que no existe la API; ya existe en el PR #5. ¿Quién lo actualiza? | Esau |
