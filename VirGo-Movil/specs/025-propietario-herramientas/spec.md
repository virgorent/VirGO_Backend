# Spec — 025 Herramientas del propietario

| Campo | Valor |
|---|---|
| Módulo | `025-propietario-herramientas` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| P6 | Mis salones |
| P10 | Paquetes y precios |
| P11 | Planos y montajes |
| P12 | Pagos recibidos |
| P13 | Estadísticas |
| P14 | Reseñas |
| P15 | Proveedores aliados |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[P6]** Lista con estado Publicado, Borrador o En revisión |
| RF-02 | **[P6]** Editar un salón |
| RF-03 | **[P10]** Precio base |
| RF-04 | **[P10]** Aumento para viernes y sábado |
| RF-05 | **[P10]** Aumento por temporada alta, con los meses a elegir |
| RF-06 | **[P10]** Vista previa del precio que verá el cliente en fechas reales |
| RF-07 | **[P11]** Subir el plano (JPG o PNG, máximo 5 MB) |
| RF-08 | **[P11]** Capacidad por montaje: banquete, auditorio y coctel |
| RF-09 | **[P11]** La IA de mesas usa la capacidad de banquete |
| RF-10 | **[P12]** Anticipos y pagos con fecha, cliente, folio, método y estado |
| RF-11 | **[P12]** Total recibido y lo que falta por cobrar |
| RF-12 | **[P13]** Visitas por mes, con vista de 3 o 6 meses |
| RF-13 | **[P13]** Solicitudes, reservaciones y porcentaje de conversión |
| RF-14 | **[P14]** Opiniones de los clientes |
| RF-15 | **[P14]** Responder y editar la respuesta |
| RF-16 | **[P15]** Activar o desactivar proveedores recomendados |
| RF-17 | **[P15]** Los activos aparecen en la ficha del salón |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla P6, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla P10, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla P11, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** la pantalla P12, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-05 | **Dado** la pantalla P13, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-06 | **Dado** la pantalla P14, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-07 | **Dado** la pantalla P15, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-08 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-09 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/venues/mine` | `025-get-venues-mine` |
| `PATCH /api/v1/venues/:id` | `027-patch-venues-id` |
| `DELETE /api/v1/venues/:id` | `030-delete-venues-id` |
| `GET /api/v1/bookings` | `066-get-bookings` |
| `GET /api/v1/venues/:id/stats` | `078-get-venues-id-stats` |
| `GET /api/v1/venues/:id/reviews` | `072-get-venues-id-reviews` |
| `POST /api/v1/reviews/:id/reply` | `075-post-reviews-id-reply` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
