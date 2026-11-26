# Check — 027 PATCH /venues/:id

| Campo | Valor |
|---|---|
| Módulo | `027-patch-venues-id` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `VerticalSliceTests.OwnerCanPatchTheirVenueAndServerFieldsRemainProtected` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` | ✅ |
| CA-03 | `VerticalSliceTests.OwnerCannotPatchAnotherOwnersVenue` | ✅ |
| CA-04 | `VerticalSliceTests.VenuePatchRejectsOversizedInputAsProblemDetails` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `CacheTests`: 35/35 | ✅ |
| Integración | `VerticalSliceTests`: 64/64 | ✅ |
| Contrato | `MobileContractTests`: 20/20 | ✅ |
| Seguridad | `InputSecurityTests`: 31/31 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | Límite, límite + 1 y 1500 caracteres | ⏳ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
