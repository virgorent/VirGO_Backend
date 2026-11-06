# Spec — 010 Mis reservaciones

| Campo | Valor |
|---|---|
| Módulo | `010-reservaciones` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C32 | Reservaciones |
| C33 | Gestionar reservación |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C32]** Lista de reservaciones con su estado y comprobante |
| RF-02 | **[C33]** Muestra las condiciones antes de hacer cambios |
| RF-03 | **[C33]** Cancelar con 30 días o más de anticipación: se reembolsa el 50% del anticipo |
| RF-04 | **[C33]** Cancelar con menos de 30 días: no hay reembolso |
| RF-05 | **[C33]** Reprogramar sin costo si la nueva fecha está libre |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C32, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C33, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/bookings` | `066-get-bookings` |
| `GET /api/v1/bookings/:id` | `067-get-bookings-id` |
| `GET /api/v1/bookings/by-folio/:folio` | `068-get-bookings-by-folio-folio` |
| `POST /api/v1/bookings/:id/cancel` | `069-post-bookings-id-cancel` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
