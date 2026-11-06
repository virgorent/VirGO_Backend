# Plan — 021 PATCH /payment-cards/:id

| Campo | Valor |
|---|---|
| Módulo | `021-patch-payment-cards-id` |
| Endpoint | `PATCH /api/v1/payment-cards/:id` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Tarjetas de pago |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.4 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `PATCH` | `/api/v1/payment-cards/:id` | Sí | 200 | 400, 401, 404, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.4. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Nunca devuelve `gatewayToken`.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `paymentCards`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ✅ | — |
| `brand` | string | ✅ | 20 |
| `last4` | string | ✅ | — |
| `expMonth` | int | ✅ | — |
| `expYear` | int | ✅ | — |
| `gatewayToken` | string | ✅ | 255 |
| `isDefault` | bool | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
