# Check — 050 PATCH /events/:id/guests/:guestId

| Campo | Valor |
|---|---|
| Módulo | `050-patch-events-id-guests-guestid` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | Ruta implementada y compilada | ✅ |
| CA-02 | `[Authorize]` global del controlador | ✅ |
| CA-03 | Filtro por `eventId` y permiso `edit` | ✅ |
| CA-04 | Reutiliza validación de invitados | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 previa | ✅ |
| Integración | Pendiente por bloqueo intermitente del host Windows | ⏳ |
| Seguridad | Pendiente por bloqueo intermitente del host Windows | ⏳ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | Límite, límite + 1 y 1500 caracteres | ⏳ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
