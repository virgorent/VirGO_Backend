# Spec — 014 Mi Evento

| Campo | Valor |
|---|---|
| Módulo | `014-mi-evento` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| D27 | Mi Evento (centro de planeación) |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[D27]** Encabezado con el tipo de evento, los días que faltan, la fecha, el salón y los invitados |
| RF-02 | **[D27]** Tocar las iniciales permite editar los datos del evento |
| RF-03 | **[D27]** Barra de pasos (Reserva, Catering, Música, Deco) que avanza con el estado de pago |
| RF-04 | **[D27]** Gráfica de lo gastado contra el presupuesto |
| RF-05 | **[D27]** Lista de proveedores con su precio y estado de pago |
| RF-06 | **[D27]** El estado de pago se cambia tocándolo: Pendiente → Anticipo → Pagado |
| RF-07 | **[D27]** Editar o calificar a cada proveedor |
| RF-08 | **[D27]** Chat con cada proveedor |
| RF-09 | **[D27]** 12 accesos rápidos |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla D27, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-03 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/events` | `043-get-events` |
| `GET /api/v1/events/:id` | `045-get-events-id` |
| `PATCH /api/v1/events/:id` | `046-patch-events-id` |
| `DELETE /api/v1/events/:id` | `047-delete-events-id` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
