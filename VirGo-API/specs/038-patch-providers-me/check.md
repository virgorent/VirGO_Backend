# Check — 038 PATCH /providers/me

| Campo | Valor |
|---|---|
| Módulo | `038-patch-providers-me` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `VerticalSliceTests.ProviderCanUpdateOwnProfileWithoutChangingStatusOrRating` | ✅ |
| CA-02 | `InputSecurityTests.ProviderUpdateRequiresAuthentication` | ✅ |
| CA-03 | `VerticalSliceTests.ProviderCreationRejectsOversizedDescription` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 | ✅ |
| Integración | `VerticalSliceTests`: pendiente de ejecución por bloqueo del host | ⏳ |
| Seguridad | `InputSecurityTests`: pendiente de ejecución por bloqueo intermitente | ⏳ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | Límite, límite + 1 y 1500 caracteres | ⏳ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
