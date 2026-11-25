# Check — 096 POST /contact

| Campo | Valor |
|---|---|
| Módulo | `096-post-contact` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `AuthTests.AnyoneCanSubmitAContactMessage` | ⏳ |
| CA-02 | `AuthTests.ContactRejectsOversizedInputAsProblemDetails` | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 | ✅ |
| Integración | Bloqueada por Control de aplicaciones de Windows | ⏳ |
| Contrato | Bloqueada por Control de aplicaciones de Windows | ⏳ |
| Seguridad | Endpoint público; validación pendiente por Control de aplicaciones | ⏳ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | Límite, límite + 1 y 1500 caracteres | ⏳ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
