# Spec — 101 amenities opcionales en venues

| Campo | Valor |
|---|---|
| Módulo | `101-amenities-opcionales` |
| Versión | 0.1.0 |
| Estado | Aprobado para implementación por dictamen 4C |
| Fecha | 2026-11-27 |
| Constitución | v1.2.0 ratificada |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

El catálogo ejecutable de MongoDB declara `amenities` como campo opcional en `venues`, pero el mapper Mongo de la API accede a él sin protección. El objetivo es corregir esa lectura para que los endpoints de catálogo y detalle sigan publicando `amenities` como array.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **cliente de la API**, quiero que un venue válido sin `amenities` se lea sin errores para **recibir la respuesta HTTP acordada**. |
| HT-02 | Como **operador del catálogo**, quiero que la ausencia del campo opcional se traduzca en `[]` para **no tener que modificar MongoDB ni el contrato**. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **Dado** un documento `venues` válido sin `amenities`, **cuando** el mapper Mongo lo lee, **entonces** la entidad resultante expone `amenities` como colección vacía. |
| RF-02 | **Dado** un documento `venues` con `amenities` de tipo BSON no array, **cuando** el mapper Mongo lo lee, **entonces** la entidad resultante expone `amenities` como colección vacía sin lanzar. |
| RF-03 | **Dado** un documento `venues` con `amenities` como `BsonArray` de strings, **cuando** el mapper Mongo lo lee, **entonces** conserva los valores según la implementación actual. |
| RF-04 | La corrección no modifica ningún otro campo del mapper ni el comportamiento de otros repositorios. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** sin secretos, credenciales ni datos sensibles nuevos; sin conexiones a bases de datos externas durante la validación. |
| RNF-02 | **Rendimiento:** la lectura defensiva no agrega E/S ni operaciones asíncronas. |
| RNF-03 | **Accesibilidad / compatibilidad:** la forma JSON de `amenities` permanece como array, según el contrato móvil vigente. |
| RNF-04 | **Costo:** $0 USD; no se crean recursos de nube ni de base de datos. |
| RNF-05 | **Trazabilidad:** cada criterio de aceptación se verifica en `check.md` y el cierre se registra en `VirGo-API/memoria.md`. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** un documento Mongo válido sin `amenities`, **cuando** se invoca el mapper, **entonces** no lanza y `Amenities` está vacío. |
| CA-02 | **Dado** un documento Mongo válido con `amenities` como array de strings, **cuando** se invoca el mapper, **entonces** `Amenities` contiene los mismos valores. |
| CA-03 | **Dado** un documento Mongo con `amenities` de tipo BSON inesperado, **cuando** se invoca el mapper, **entonces** no lanza y `Amenities` está vacío. |
| CA-04 | **Dado** el cambio aplicado, **cuando** se revisa el diff, **entonces** no hay cambios en catálogo, esquema generado, OpenAPI, DTOs ni specs anteriores. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Fuga de secretos | Sin secretos en código ni logs; sin uso de credenciales. |
| Acceso no autorizado | Sin cambios de autenticación ni autorización. |
| Datos personales | Sin datos personales nuevos; el mapper solo normaliza un campo de catálogo. |
| Abuso o fuerza bruta | Sin cambios de límites de tasa; el comportamiento HTTP existente permanece. |

## 7. Fuera de alcance

- Modificar `VirGo-Database/src/catalog/`.
- Modificar `VirGo-API/db/schema/*.json`.
- Modificar OpenAPI, controladores, DTOs ni el contrato móvil.
- Cambiar otros repositorios, otros campos del mapper o el corte D-03.
- Ejecutar inicialización de MongoDB, escrituras en base de datos, conexiones a Atlas, AWS, SSM, producción o despliegues.

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- El catálogo ejecutable (`VirGo-Database/src/catalog/validators/venues.js:60,91-109`) es autoridad sobre la opcionalidad de `amenities`.
- El contrato HTTP exige que la respuesta exponga `amenities` como array (`MobileContractTests.cs:64`).

**Dependencias**

- D-01: catálogo MongoDB como autoridad ejecutable.
- D-02: contrato HTTP/OpenAPI ratificado.
- Specs existentes `007-venues-catalog` y `024-get-venues-id` como contexto documental.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| — | Ninguna; el dictamen 4C resolvió el comportamiento esperado. | — |
