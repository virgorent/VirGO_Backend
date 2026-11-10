# Spec — 017 Pendientes y recordatorios

| Campo | Valor |
|---|---|
| Módulo | `017-pendientes` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C27 | Pendientes |
| C29 | Recordatorios |
| C28 | Línea de tiempo |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C27]** Checklist con fechas límite |
| RF-02 | **[C27]** Grupos: Vencidas, Próximas y Hechas |
| RF-03 | **[C27]** Lista sugerida según la fecha del evento |
| RF-04 | **[C27]** Marcar como hecho |
| RF-05 | **[C29]** Se crean solos con los pendientes, los pagos por hacer y las confirmaciones |
| RF-06 | **[C29]** Agregar recordatorios propios |
| RF-07 | **[C28]** Etapas con fechas calculadas desde el día del evento |
| RF-08 | **[C28]** Resalta la etapa actual y las próximas tareas |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C27, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C29, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C28, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/events/:id/tasks` | `052-get-events-id-tasks` |
| `POST /api/v1/events/:id/tasks` | `053-post-events-id-tasks` |
| `PATCH /api/v1/events/:id/tasks/:taskId` | `054-patch-events-id-tasks-taskid` |
| `DELETE /api/v1/events/:id/tasks/:taskId` | `055-delete-events-id-tasks-taskid` |
| `PATCH /api/v1/events/:id` | `046-patch-events-id` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
