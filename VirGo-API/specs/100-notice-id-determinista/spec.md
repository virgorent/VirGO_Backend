# Spec — 100 ID determinista de notificaciones

| Campo | Valor |
|---|---|
| Módulo | `100-notice-id-determinista` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-26 |
| Constitución | v1.2.0 (ratificada) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

El worker de `booking.created` genera actualmente el `_id` del aviso como `booking-created-{bookingId}`. Ese valor es determinista y permite deduplicación, pero no cumple el patrón UUID exigido por el validador autoritativo de `notices` en `VirGo-Database/src/catalog/`. Este módulo reconcilia el formato sin cambiar el catálogo MongoDB ni el contrato HTTP.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **worker**, quiero generar un `_id` UUID válido y determinista para el aviso de `booking.created`, para que el documento pase el validador y los reintentos no dupliquen avisos. |
| HT-02 | Como **equipo**, quiero conservar catálogo, esquemas generados y contrato HTTP sin cambios, para no ampliar el alcance de D-03. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | El aviso generado por `booking.created` usa un UUIDv5 RFC 4122 derivado del namespace fijo de la aplicación y de la clave canónica `booking.created:{bookingId}`. |
| RF-02 | El identificador se entrega al repositorio como `Guid.ToString("D")`. |
| RF-03 | El mismo `bookingId` produce siempre el mismo identificador; dos `bookingId` distintos producen identificadores distintos. |
| RF-04 | La deduplicación sigue siendo responsabilidad de `INoticeRepository.AddIfAbsentAsync` y de la unicidad de `_id`. |
| RF-05 | No se modifica `VirGo-Database/src/catalog/`, `VirGo-API/db/schema/` ni `VirGo-Database/src/export/`. |
| RF-06 | No se modifica el contrato HTTP vigente, OpenAPI, controllers, DTOs ni los specs `090`, `091` o `092`. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** el mensaje de cola sigue llevando solo identificadores; el helper no registra secretos ni datos personales. |
| RNF-02 | **Rendimiento:** el cálculo SHA-1 sobre una clave corta no agrega operaciones de red ni dependencias nuevas. |
| RNF-03 | **Compatibilidad:** el UUID generado cumple el patrón de `VirGo-API/db/schema/notices.json` y los bits de versión y variante de UUIDv5. |
| RNF-04 | **Costo:** $0; no se crean recursos de AWS ni se agregan paquetes. |
| RNF-05 | **Trazabilidad:** no se introducen logs nuevos; el comportamiento observable del worker permanece igual salvo el formato del `_id`. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el vector RFC 4122 verificable `uuid5(NAMESPACE_DNS, "www.example.com")`, **cuando** se ejecuta el helper con ese namespace y nombre, **entonces** devuelve `2ed6657d-e927-568b-95e1-2665a8aea6a2`. |
| CA-02 | **Dado** un mismo `bookingId`, **cuando** se deriva dos veces el ID de `booking.created`, **entonces** ambos resultados son idénticos. |
| CA-03 | **Dado** dos `bookingId` distintos, **cuando** se deriva el ID de `booking.created`, **entonces** los resultados son distintos. |
| CA-04 | **Dado** un `bookingId`, **cuando** se deriva el ID, **entonces** la clave usada incluye el prefijo `booking.created:`. |
| CA-05 | **Dado** el UUID derivado, **cuando** se compara con el patrón de `_id` de `notices.json`, **entonces** cumple el formato UUID. |
| CA-06 | **Dado** el UUID derivado, **cuando** se inspeccionan sus bits, **entonces** la versión es `5` y la variante es RFC 4122. |
| CA-07 | **Dado** un mensaje `booking.created`, **cuando** lo procesa `BookingCreatedHandler`, **entonces** el aviso usa el ID determinista del helper. |
| CA-08 | **Dado** el mismo mensaje entregado dos veces, **cuando** lo procesa el handler con el repositorio de prueba, **entonces** queda un solo aviso. |
| CA-09 | **Dado** el cambio implementado, **cuando** se revisa el diff, **entonces** no aparecen cambios en catálogo, esquemas generados, exportador, OpenAPI, controllers, DTOs, CI/CD, infraestructura, configuración ni dependencias. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Secreto o dato personal en el ID | La clave usa solo el tipo de evento y el `bookingId`; no incluye correo, teléfono ni secreto. |
| Colisión deliberada entre eventos | La clave incluye `booking.created:` y un namespace congelado. |
| Deriva del algoritmo | Vector conocido y pruebas de versión, variante y patrón. |

## 7. Fuera de alcance

- Decisiones D-04 a D-12.
- Endpoints `090-get-notices`, `091-patch-notices-id-read` y `092-post-notices-read-all`.
- Notificaciones push, correo, pagos, cancelaciones y otros eventos de negocio.
- Cambios en `VirGo-Database/src/catalog/`, `VirGo-API/db/schema/` o `VirGo-Database/src/export/`.
- Cambios en SQS, reintentos, repositorios, modelo `Notice`, OpenAPI, controllers, DTOs, CI/CD, infraestructura, configuración o dependencias.

## 8. Supuestos, dependencias y preguntas abiertas

**Supuestos**

- D-01 y D-02 están ratificados desde el Corte 4A-GOV.
- La colección `notices` se administra desde `VirGo-Database`; la API no ejecuta DDL.
- El contrato HTTP actual no expone avisos.

**Dependencias**

- `099-colas-sqs` define el mensaje `booking.created`.
- `010-contrato-datos` define la autoridad del catálogo y del esquema generado.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| — | Ninguna. Namespace, clave y vector quedan congelados en este spec. | Equipo |
