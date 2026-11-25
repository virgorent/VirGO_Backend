# Check — 090 GET /notices

| Campo | Valor |
|---|---|
| Módulo | `090-get-notices` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `AuthTests.NoticesReturnsOnlyTheAuthenticatedUsersCollection` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` incluye `GET /api/v1/notices` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | No aplica: lectura cubierta por integración | ➖ |
| Integración | `AuthTests`: 22/22 | ✅ |
| Contrato | `MobileContractTests`: 16/16 | ✅ |
| Seguridad | `InputSecurityTests`: 24/24 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
