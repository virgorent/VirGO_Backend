# Check — 025 GET /venues/mine

| Campo | Valor |
|---|---|
| Módulo | `025-get-venues-mine` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `VerticalSliceTests.OwnerCanReadOnlyTheirVenueCollection` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `CacheTests`: 35/35 | ✅ |
| Integración | `VerticalSliceTests`: 59/59 | ✅ |
| Contrato | `MobileContractTests`: 18/18 | ✅ |
| Seguridad | `InputSecurityTests`: 29/29 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
