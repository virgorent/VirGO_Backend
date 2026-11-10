# Spec — 016 Invitados e invitación

| Campo | Valor |
|---|---|
| Módulo | `016-invitados` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| D32 | Invitados |
| C31 | Invitación digital |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[D32]** Agregar invitados con nombre, acompañantes, mesa y estado |
| RF-02 | **[D32]** Estados: Confirmado, Pendiente y No asiste |
| RF-03 | **[D32]** Totales por estado |
| RF-04 | **[D32]** Ajustar el evento al número de confirmados |
| RF-05 | **[D32]** Editar y eliminar invitados |
| RF-06 | **[C31]** Datos del evento |
| RF-07 | **[C31]** 3 plantillas |
| RF-08 | **[C31]** Mensaje editable |
| RF-09 | **[C31]** Compartir por WhatsApp o por correo, o copiar el texto |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla D32, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C31, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/events/:id/guests` | `048-get-events-id-guests` |
| `POST /api/v1/events/:id/guests` | `049-post-events-id-guests` |
| `PATCH /api/v1/events/:id/guests/:guestId` | `050-patch-events-id-guests-guestid` |
| `DELETE /api/v1/events/:id/guests/:guestId` | `051-delete-events-id-guests-guestid` |
| `PATCH /api/v1/events/:id` | `046-patch-events-id` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
