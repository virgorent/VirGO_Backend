# Check — 017 GET /users/:id/public

| Campo | Valor |
|---|---|
| Módulo | `017-get-users-id-public` |
| Estado del spec | En implementación |
| Revisó | Rafael Hernández |

## 1. Especificación

- [x] Requerimientos de perfil público y seguridad revisados.
- [x] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 200 con o sin sesión | `AuthTests.PublicProfileReturnsOnlyPublicFieldsWithoutAuthentication` | ✅ |
| CA-02 404 inexistente | `AuthTests.PublicProfileReturnsNotFoundForUnknownUser` | ✅ |
| CA-03 sin datos privados | `AuthTests.PublicProfileReturnsOnlyPublicFieldsWithoutAuthentication` | ✅ |
| CA-04 sin inyección | `AuthTests.PublicProfileTreatsInjectionPayloadAsAnUnknownId`; `InputSecurityTests.PublicProfileRejectsOperatorInjectionAsAnUnknownId` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | No aplica: lógica de proyección en controller | ➖ |
| Integración | `AuthTests`: 8/8 | ✅ |
| Contrato | `MobileContractTests`: 11/11 | ✅ |
| Seguridad | `InputSecurityTests`: 20/20 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md` al cerrar el PR.
