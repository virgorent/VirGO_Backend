# Plan — 096 POST /contact

| Campo | Valor |
|---|---|
| Módulo | `096-post-contact` |
| Endpoint | `POST /api/v1/contact` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Grupo | Contacto |
| Fuente | `VirGo-Database/docs/API-HANDOFF.md` §9.16 |
| Consumido por Móvil | `021-favoritos-perfil` |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contrato

| Método | Ruta | Auth | Éxito | Errores |
|---|---|---|---|---|
| `POST` | `/api/v1/contact` | Opcional | 201 | 400, 429, 500 |

El cuerpo exacto se toma de `API-HANDOFF.md` §9.16. Los errores siguen el Art. IV.3 (Problem Details), no el formato propuesto en ese documento.

## 2. Reglas

- Tiene límite de solicitudes.

## 3. Datos

Campos vigentes en `VirGo-Database/src/catalog`. La columna «Máx. propuesto» depende de `VirGo-Database` `001` P-05.

### `contactMessages`

| Campo | Tipo | Req. | Máx. propuesto |
|---|---|---|---|
| `_id` | string | ✅ | — |
| `userId` | string | ➖ | — |
| `name` | string | ✅ | 100 |
| `email` | string | ✅ | 150 |
| `message` | string | ✅ | 2000 |
| `createdAt` | date | ✅ | — |

## 4. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | Reglas de la sección 2 |
| Integración | Éxito y cada error |
| Repositorio | Testcontainers con los validadores reales (`010`) |
| Seguridad | Autorización, longitud (límite, límite + 1 y 1500 caracteres) e inyección de operadores |
| Mutación | Stryker sobre el caso de uso |
| Humo | Un caso por HTTPS tras el despliegue |
