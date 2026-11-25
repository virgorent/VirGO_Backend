# Check — 092 POST /notices/read-all

| Campo | Valor |
|---|---|
| Módulo | `092-post-notices-read-all` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `AuthTests.AuthenticatedUserCanMarkAllTheirUnreadNoticesAsRead` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 | ✅ |
| Integración | `AuthTests`: incluido en 69/69 | ✅ |
| Contrato | `MobileContractTests`: 23/23 | ✅ |
| Seguridad | `InputSecurityTests`: 33/33 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
