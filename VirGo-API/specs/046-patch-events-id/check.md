# Check — 046 PATCH /events/:id

| Campo | Valor |
|---|---|
| Módulo | `046-patch-events-id` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `VerticalSliceTests.EventOwnerCanUpdateTheirEvent` | ✅ |
| CA-02 | `InputSecurityTests.EventUpdateRequiresAuthentication` | ✅ |
| CA-03 | Ownership en `UpdateForUserAsync` | ✅ |
| CA-04 | `VerticalSliceTests.EventUpdateRejectsInvalidData` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 | ✅ |
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
