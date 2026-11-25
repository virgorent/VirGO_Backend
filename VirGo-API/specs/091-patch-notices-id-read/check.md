# Check — 091 PATCH /notices/:id/read

| Campo | Valor |
|---|---|
| Módulo | `091-patch-notices-id-read` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `AuthTests.AuthenticatedUserCanMarkTheirNoticeAsRead` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` | ✅ |
| CA-03 | `AuthTests.UserCannotMarkAnotherUsersNoticeAsRead` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Suite unitaria: 35/35 | ✅ |
| Integración | `AuthTests`: incluido en 68/68 | ✅ |
| Contrato | `MobileContractTests`: 22/22 | ✅ |
| Seguridad | `InputSecurityTests`: 32/32 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
