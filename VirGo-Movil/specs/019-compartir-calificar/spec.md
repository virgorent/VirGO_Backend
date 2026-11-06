# Spec — 019 Compartir y calificar

| Campo | Valor |
|---|---|
| Módulo | `019-compartir-calificar` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C30 | Compartir evento |
| C34 | Calificar |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C30]** Invitar a colaboradores por correo |
| RF-02 | **[C30]** Permisos de Ver o Editar |
| RF-03 | **[C30]** Quitar el acceso |
| RF-04 | **[C34]** Estrellas y comentario para un salón o proveedor contratado |
| RF-05 | **[C34]** La reseña se publica en el perfil correspondiente y actualiza su promedio |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C30, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C34, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/events/:id/collaborators` | `056-get-events-id-collaborators` |
| `POST /api/v1/events/:id/collaborators` | `057-post-events-id-collaborators` |
| `PATCH /api/v1/events/:id/collaborators/:userId` | `058-patch-events-id-collaborators-userid` |
| `DELETE /api/v1/events/:id/collaborators/:userId` | `059-delete-events-id-collaborators-userid` |
| `POST /api/v1/reviews` | `074-post-reviews` |
| `PATCH /api/v1/reviews/:id` | `076-patch-reviews-id` |
| `DELETE /api/v1/reviews/:id` | `077-delete-reviews-id` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
