# Check — 034 GET /providers

| Campo | Valor |
|---|---|
| Módulo | `034-get-providers` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `AuthTests.PublicProviderCatalogSupportsFiltersAndHidesInternalFields` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 | ✅ |
| Integración | `AuthTests`: incluido en 74/74 | ✅ |
| Contrato | `MobileContractTests`: 26/26 | ✅ |
| Seguridad | `InputSecurityTests`: 35/35 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
