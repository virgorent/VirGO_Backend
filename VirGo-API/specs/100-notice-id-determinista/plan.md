# Plan — 100 ID determinista de notificaciones

| Campo | Valor |
|---|---|
| Módulo | `100-notice-id-determinista` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-26 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.2.0 (ratificada) |

## 1. Resumen técnico

- Crear un helper local en `VirGo.Application/Jobs` que implemente UUIDv5 RFC 4122 sin paquetes nuevos.
- Usar el namespace DNS estándar `6ba7b810-9dad-11d1-80b4-00c04fd430c8` como constante congelada de la aplicación.
- Derivar cada ID desde `booking.created:{bookingId}` y devolverlo con `Guid.ToString("D")`.
- Sustituir solo la asignación de `Notice.Id` en `BookingCreatedHandler`.
- Conservar `INoticeRepository`, ambos repositorios, el modelo `Notice`, la publicación SQS y la lógica de `BookingHandler`.
- Enmendar únicamente la redacción del formato de `_id` en el spec y plan de `099-colas-sqs`.
- Registrar D-03 en `Memoria.md` solo después de que los gates locales resulten satisfactorios.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | .NET 10, C#, sin paquetes nuevos |
| Hash | SHA-1 de `System.Security.Cryptography`, exigido por UUIDv5 |
| Persistencia | Sin cambios; `MongoNoticeRepository` sigue usando `_id` como clave natural |
| Contrato HTTP | Sin cambios; no existen endpoints de notificaciones implementados |
| CI/CD | Gates existentes de PR; no se modifica ningún workflow |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| I | Spec-first | Cumple | Este plan pertenece a la spec atómica `100`; `099` solo se enmienda para coherencia de formato. |
| II | SDD y aprobación | Cumple | Cuatro archivos creados; tareas no se ejecutan sin autorización explícita del corte. |
| III | Capas | Cumple | El helper vive en `Application/Jobs`; la infraestructura no cambia. |
| IV | Contrato HTTP | Cumple | No hay cambios en controllers, DTOs, rutas ni OpenAPI. |
| V | Seguridad | Cumple | Sin secretos, sin datos personales en la clave y sin nuevos permisos. |
| VI | Pruebas | Cumple | Pruebas unitarias del vector, determinismo, patrón, versión, variante y handler. |
| VII | Costo cero | Cumple | Sin recursos AWS ni dependencias nuevas. |
| VIII | Observabilidad | Cumple | No se agregan logs ni métricas; se conserva el comportamiento previo. |
| IX | Calidad y CI | Cumple | Build local, tests unitarios y `git diff --check`; CI existente sin cambios. |
| XII | Idioma | Cumple | Documentación en español; identificadores en inglés. |

> Un resultado "Bloquea" impediría la aprobación del plan.

## 4. Estructura

```text
VirGo-API/
├── specs/100-notice-id-determinista/
│   ├── spec.md
│   ├── plan.md
│   ├── task.md
│   └── check.md
├── src/VirGo.Application/Jobs/
│   ├── BookingCreatedHandler.cs
│   └── DeterministicNoticeIds.cs
└── tests/VirGo.UnitTests/Jobs/
    ├── JobTests.cs
    └── DeterministicNoticeIdsTests.cs
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-D03 | UUIDv5 RFC 4122 local con namespace DNS fijo y clave `booking.created:{bookingId}` | Mantener `booking-created-{bookingId}` y ampliar el catálogo; usar `Guid.NewGuid()`; calcular un hash y formatearlo como GUID sin versión ni variante | La primera contradice D-01 y exigiría DDL; la segunda rompe idempotencia; la tercera no garantiza RFC 4122 |

## 6. Diseño

### 6.1 Namespace y vector

| Elemento | Valor |
|---|---|
| Namespace congelado | `6ba7b810-9dad-11d1-80b4-00c04fd430c8` |
| Clave canónica | `booking.created:{bookingId}` |
| Vector RFC 4122 verificado | `uuid5(NAMESPACE_DNS, "www.example.com")` → `2ed6657d-e927-568b-95e1-2665a8aea6a2` |
| Ejemplo de negocio | `bookingId = 4bf8f8c2-1c5a-4e3b-8b2a-7f9f3d1c2e44` → `booking.created:4bf8f8c2-1c5a-4e3b-8b2a-7f9f3d1c2e44` → `238b0bd9-52c0-5976-86f0-b2b4a220b44a` |

### 6.2 Algoritmo

1. Convertir el namespace de `Guid` a bytes en orden de red.
2. Concatenar esos bytes con la clave codificada en UTF-8.
3. Calcular SHA-1 sobre la concatenación.
4. Tomar los primeros 16 bytes.
5. Fijar el nibble de versión a `5` en el byte 6.
6. Fijar los dos bits de variante RFC 4122 en el byte 8.
7. Convertir los bytes a `Guid` respetando el orden interno de .NET.
8. Devolver `Guid.ToString("D")`.

### 6.3 Seguridad

- La clave no incluye correo, teléfono, folio ni secreto.
- El namespace y el prefijo quedan congelados en código y documentación.
- No se agregan logs con el contenido de la clave.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| Secretos | Gitleaks | PR y push a `main`, sin cambios en este corte |
| Dependencias | Dependabot + dependency review | PR, sin manifiestos modificados |
| SAST | CodeQL / linters | PR, sin cambios en workflows |
| Pruebas | xUnit y build local | PR y validación local de este corte |
| Release | Manual | Fuera de este corte |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | `DeterministicNoticeIdsTests.cs` | Vector RFC, determinismo, unicidad, prefijo, patrón, versión y variante |
| Unitarias | `JobTests.cs` | Handler usa el helper y la reentrega no duplica avisos |
| Integración | Sin cambios | No se ejecutan pruebas de persistencia real en este corte |
| Seguridad | Revisión del diff | Sin secretos, sin datos personales en la clave y sin dependencias nuevas |
| Humo | No aplica | No hay endpoints nuevos ni cambios HTTP |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| Cambiar el namespace rompe todos los IDs futuros | Constante congelada y vector de prueba. |
| Implementar solo un hash con forma GUID | Pruebas de versión, variante y vector RFC 4122. |
| Volver a `Guid.NewGuid()` | Prueba de determinismo y revisión del handler. |
| Validación real de MongoDB no cubierta | Se declara explícitamente que solo se valida el patrón del esquema; no se afirma persistencia real. |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Documentación SDD y enmienda de `099` | T1001–T1002 |
| F1 | Helper, integración y pruebas | T1003–T1005 |
| F2 | Gates locales, memoria y cierre documental | T1006–T1008 |
