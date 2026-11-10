# Spec — 024 Solicitudes y mensajes

| Campo | Valor |
|---|---|
| Módulo | `024-propietario-solicitudes` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| P7 | Solicitudes |
| P8 | Responder cotización |
| P16 | Mensajes |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[P7]** Pestañas Nuevas, Respondidas y Todas |
| RF-02 | **[P7]** Datos del cliente y del evento |
| RF-03 | **[P8]** Cotización precargada con los precios del salón |
| RF-04 | **[P8]** Editar conceptos, cantidades y precios |
| RF-05 | **[P8]** Agregar o quitar conceptos |
| RF-06 | **[P8]** Nota para el cliente |
| RF-07 | **[P8]** «Enviar al cliente»: le llega en sus cotizaciones, con notificación |
| RF-08 | **[P16]** Conversaciones con los clientes |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla P7, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla P8, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla P16, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/quotes` | `060-get-quotes` |
| `GET /api/v1/quotes/:id` | `061-get-quotes-id` |
| `POST /api/v1/quotes/:id/response` | `062-post-quotes-id-response` |
| `GET /api/v1/conversations` | `084-get-conversations` |
| `GET /api/v1/conversations/:id/messages` | `087-get-conversations-id-messages` |
| `POST /api/v1/conversations/:id/messages` | `088-post-conversations-id-messages` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
