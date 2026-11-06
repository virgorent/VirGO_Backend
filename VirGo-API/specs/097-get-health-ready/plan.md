# Plan — 097 GET /health/ready

| Campo | Valor |
|---|---|
| Módulo | `097-get-health-ready` |
| Endpoint | `GET /api/v1/health/ready` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Salud y operación |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.17 |
| Consumido por Móvil | uso operativo |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `GET` | `/api/v1/health/ready` | No | 200 | 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.17. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- No expone datos sensibles.
- Se usan en las pruebas de humo.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

No usa colecciones.

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
