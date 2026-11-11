# Plan — 098 Caché distribuida con Redis

| Campo | Valor |
|---|---|
| Módulo | `098-cache-redis` |
| Estado | Implementado en local; falta probar con Redis real |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Resumen técnico

`ICacheService` vive en Application. `DistributedCacheService` lo implementa sobre `IDistributedCache`, y `CachedVenueRepository` decora `IVenueRepository`, sin cambiar controladores ni casos de uso.

## 2. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-34 | Redis externo con plan gratuito (Upstash propuesto) | ElastiCache | ElastiCache tiene costo mínimo mensual y requiere VPC; una Lambda en VPC necesitaría NAT, prohibido por el Art. VII |
| ADR-35 | Fallo abierto: si Redis falla se usa la base | Fallar la solicitud | La caché nunca debe tumbar la API |
| ADR-36 | Sesiones y bloqueos siguen en MongoDB | Blacklist en Redis, como la referencia | Un logout debe ser definitivo aunque Redis se reinicie |

## 3. Configuración

| Clave | Origen |
|---|---|
| `Redis:ConnectionString` | SSM `/virgo/prod/redis-url` y variable `Redis__ConnectionString` (usar TLS) |

## 4. Costo

$0 con el plan gratuito propuesto: 256 MB y 500 000 comandos al mes, según Upstash. Hay que confirmarlo en su página de precios antes de crearlo.

## 5. Riesgos

| Riesgo | Mitigación |
|---|---|
| Agotar los comandos gratuitos | Solo catálogo público, TTL cortos y fallo abierto |
| Datos desactualizados hasta 60 s | Aceptable para el catálogo; invalidación con `026` y `027` |
