# Check — 051 DELETE /events/:id/guests/:guestId

| Campo | Valor |
|---|---|
| Módulo | `051-delete-events-id-guests-guestid` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | Ruta devuelve `204 No Content` al eliminar | ✅ |
| CA-02 | `[Authorize]` global del controlador | ✅ |
| CA-03 | Filtro por evento, invitado y permiso `edit` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 previa | ✅ |
| Integración | Pendiente por bloqueo intermitente del host Windows | ⏳ |
| Seguridad | Pendiente por bloqueo intermitente del host Windows | ⏳ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
