# Check — 018 DELETE /users/me

| Campo | Valor |
|---|---|
| Módulo | `018-delete-users-me` |
| Estado del spec | En implementación |
| Revisó | Rafael Hernández |

## 1. Especificación

- [x] Decisión aprobada: anonimizar y desactivar.
- [x] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 204 autenticado | `AuthTests.DeleteAccountAnonymizesUserAndRevokesAllSessions` | ✅ |
| CA-02 401 sin token | `InputSecurityTests.ProtectedRoutesRequireAToken` | ✅ |
| CA-03 credenciales anteriores rechazadas | `AuthTests.DeleteAccountAnonymizesUserAndRevokesAllSessions` | ✅ |
| CA-04 todas las sesiones revocadas | `AuthTests.DeleteAccountAnonymizesUserAndRevokesAllSessions` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | No aplica: orquestación cubierta por integración | ➖ |
| Integración | `AuthTests`: 6/6 | ✅ |
| Contrato | `MobileContractTests`: 11/11 | ✅ |
| Seguridad | `InputSecurityTests`: 19/19 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md` al cerrar el PR.
