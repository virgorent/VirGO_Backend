# Check — 031 GET /venues/:id/availability

| Campo | Valor |
|---|---|
| Módulo | `031-get-venues-id-availability` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `VerticalSliceTests.PublicVenueAvailabilityHidesInternalBookingDetails` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `CacheTests`: 35/35 | ✅ |
| Integración | `VerticalSliceTests`: 66/66 | ✅ |
| Contrato | `MobileContractTests`: 21/21 | ✅ |
| Seguridad | No requiere autenticación; endpoint público validado por integración | ➖ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
