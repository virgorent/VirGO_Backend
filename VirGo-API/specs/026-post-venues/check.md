# Check — 026 POST /venues

| Campo | Valor |
|---|---|
| Módulo | `026-post-venues` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `VerticalSliceTests.OwnerCanCreateADraftVenueWithoutControllingServerFields` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` | ✅ |
| CA-03 | `VerticalSliceTests.VenueCreationRejectsOversizedInputAsProblemDetails` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `CacheTests`: 35/35 | ✅ |
| Integración | `VerticalSliceTests`: 61/61 | ✅ |
| Contrato | `MobileContractTests`: 19/19 | ✅ |
| Seguridad | `InputSecurityTests`: 30/30 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | Límite, límite + 1 y 1500 caracteres | ⏳ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
