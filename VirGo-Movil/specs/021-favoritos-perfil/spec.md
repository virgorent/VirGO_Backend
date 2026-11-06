# Spec — 021 Favoritos, perfil y ayuda

| Campo | Valor |
|---|---|
| Módulo | `021-favoritos-perfil` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C38 | Favoritos |
| C39 | Perfil |
| C40 | Ayuda |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C38]** Pestañas de Salones y Proveedores |
| RF-02 | **[C38]** Quitar un favorito con el corazón |
| RF-03 | **[C38]** Se conservan al cerrar sesión |
| RF-04 | **[C39]** Tarjeta con tus datos, editable |
| RF-05 | **[C39]** Mis reservaciones, Mis cotizaciones, Mensajes, Notificaciones y Asistente IA |
| RF-06 | **[C39]** Preferencias, como los invitados por defecto |
| RF-07 | **[C39]** Métodos de pago |
| RF-08 | **[C39]** Cerrar sesión, que conserva los datos |
| RF-09 | **[C39]** Borrar cuenta y datos, que elimina solo tu cuenta |
| RF-10 | **[C40]** Preguntas frecuentes con buscador |
| RF-11 | **[C40]** Formulario de contacto |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C38, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C39, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C40, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/users/me` | `006-users-me` |
| `PATCH /api/v1/users/me` | `015-patch-users-me` |
| `PATCH /api/v1/users/me/password` | `016-patch-users-me-password` |
| `DELETE /api/v1/users/me` | `018-delete-users-me` |
| `GET /api/v1/users/:id/public` | `017-get-users-id-public` |
| `GET /api/v1/favorites` | `081-get-favorites` |
| `GET /api/v1/payment-cards` | `019-get-payment-cards` |
| `PATCH /api/v1/payment-cards/:id` | `021-patch-payment-cards-id` |
| `DELETE /api/v1/payment-cards/:id` | `022-delete-payment-cards-id` |
| `POST /api/v1/payment-cards/:id/default` | `023-post-payment-cards-id-default` |
| `POST /api/v1/contact` | `096-post-contact` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
