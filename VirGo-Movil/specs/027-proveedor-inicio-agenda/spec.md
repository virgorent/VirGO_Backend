# Spec — 027 Inicio y agenda

| Campo | Valor |
|---|---|
| Módulo | `027-proveedor-inicio-agenda` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Proveedor |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| S3 | Inicio |
| H74 | Agenda |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[S3]** Panel del proveedor |
| RF-02 | **[S3]** Solicitudes nuevas y próximos eventos |
| RF-03 | **[S3]** Cotizaciones enviadas y aceptadas |
| RF-04 | **[S3]** Avisos recientes |
| RF-05 | **[H74]** Vista semanal de los eventos contratados |
| RF-06 | **[H74]** Flechas para cambiar de semana |
| RF-07 | **[H74]** Agregar un evento con fecha, horario, cliente y lugar |
| RF-08 | **[H74]** Aviso de empalme en rojo cuando dos eventos se cruzan |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla S3, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla H74, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/providers/me/gigs` | `039-get-providers-me-gigs` |
| `POST /api/v1/providers/me/gigs` | `040-post-providers-me-gigs` |
| `PATCH /api/v1/providers/me/gigs/:id` | `041-patch-providers-me-gigs-id` |
| `DELETE /api/v1/providers/me/gigs/:id` | `042-delete-providers-me-gigs-id` |
| `GET /api/v1/stats/overview` | `080-get-stats-overview` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
