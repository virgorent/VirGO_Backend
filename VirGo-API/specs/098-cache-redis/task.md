# Tareas — 098 Caché distribuida con Redis

| Campo | Valor |
|---|---|
| Módulo | `098-cache-redis` |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T9801 | `ICacheService` y `DistributedCacheService` con fallo abierto | — | `dev-dotnet` | Pruebas unitarias | ✅ |
| T9802 | `CachedVenueRepository` y registro en DI | T9801 | `dev-dotnet` | Pruebas unitarias | ✅ |
| T9803 | Crear Redis gratuito y guardar la URL en SSM | P-01 | `devops-aws` | Conexión por TLS | ⏳ |
| T9804 | Pasar `Redis__ConnectionString` a la Lambda | T9803, corrección de `ssm-secure` | `devops-aws` | Variable presente tras desplegar | ⏳ |
| T9805 | Prueba de integración con Redis real (Testcontainers) | T9802 | `qa` | CA-04 | ⏳ |
