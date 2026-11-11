# Memoria de pruebas

**Actualizada:** 2026-11-06

| Proyecto | Pruebas | Qué cubre |
|---|---|---|
| `VirGo.UnitTests` | 14 | Capas, repositorio en memoria y caché |
| `VirGo.IntegrationTests` | 30 | Endpoints con persistencia en memoria |
| `VirGo.SecurityTests` | 21 | Tokens, CORS, encabezados, longitudes e inyección |
| `VirGo.ContractTests` | 10 | OpenAPI y JSON que consume Móvil |
| `VirGo.DatabaseTests` | 96 | Esquema: validación, límites, TTL y unicidad |
| `VirGo.PerformanceTests` | 3 | Línea base de latencia, sin umbrales |

## Lecciones

- **Pruebas inestables:** `ApiFactory` usaba el límite de 20 solicitudes por segundo. En corridas rápidas fallaban 11 de 30 con 429. Las fábricas de prueba ahora usan 1000; `RateLimitFactory` conserva su límite propio.
- Las pruebas de contrato no usan Pact: fijan el JSON con xUnit. Falta decidir si se adopta Pact.
- `VirGo.DatabaseTests` revisa los archivos, no MongoDB.

## Pendiente

- Pruebas de repositorios contra MongoDB real (Testcontainers).
- Fijar umbrales de cobertura, mutación y rendimiento después de medir.
