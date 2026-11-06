# Spec — 023 Inicio y calendario

| Campo | Valor |
|---|---|
| Módulo | `023-propietario-inicio-calendario` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| G54 | Inicio |
| G58 | Calendario |
| P9 | Bloquear fecha |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[G54]** Saludo y panel del propietario |
| RF-02 | **[G54]** Métricas del mes: visitas, solicitudes nuevas, reservaciones e ingresos |
| RF-03 | **[G54]** Próximas reservaciones |
| RF-04 | **[G54]** Avisos recientes |
| RF-05 | **[G54]** Accesos a Mis salones, Pagos, Estadísticas y Reseñas |
| RF-06 | **[G54]** Estado vacío con el botón «Registrar salón» |
| RF-07 | **[G58]** Vista mensual con días reservados, bloqueados y libres |
| RF-08 | **[G58]** Tocar un día muestra sus reservaciones |
| RF-09 | **[G58]** Aviso de empalme cuando hay dos reservaciones el mismo día |
| RF-10 | **[P9]** Selección de un rango de fechas |
| RF-11 | **[P9]** Motivo: Mantenimiento, Evento propio u Otro |
| RF-12 | **[P9]** Los clientes ven esas fechas ocupadas |
| RF-13 | **[P9]** Desbloquear un día |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla G54, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla G58, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla P9, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/venues/:id/availability` | `031-get-venues-id-availability` |
| `POST /api/v1/venues/:id/blocks` | `032-post-venues-id-blocks` |
| `DELETE /api/v1/venues/:id/blocks/:blockId` | `033-delete-venues-id-blocks-blockid` |
| `GET /api/v1/bookings` | `066-get-bookings` |
| `GET /api/v1/stats/overview` | `080-get-stats-overview` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
