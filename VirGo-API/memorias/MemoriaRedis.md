# Memoria de Redis

**Actualizada:** 2026-11-06

- Caché *cache-aside* del catálogo: lista 30 s y detalle 60 s.
- Si Redis falla, se usa MongoDB.
- Sin `Redis:ConnectionString`, se usa caché en memoria.
- Sesiones y bloqueos siguen en MongoDB (ADR-36).

## Pendiente

- Aprobar el proveedor (Art. VII).
- Crear el Redis, guardar su URL en SSM y conectarlo a la Lambda.
- Probar contra un Redis real.
