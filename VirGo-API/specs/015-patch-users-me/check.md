# Check — 015 PATCH /users/me

| Campo | Valor |
|---|---|
| Módulo | `015-patch-users-me` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `AuthTests.PatchProfileUpdatesAllowedFieldsAndPreservesProtectedFields` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` incluye `PATCH /api/v1/users/me` | ✅ |
| CA-03 | `AuthTests.PatchProfileRejectsInvalidFields` y `PatchProfileRejects1500CharacterName` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | No aplica: lógica cubierta por integración | ➖ |
| Integración | `AuthTests`: 12/12 | ✅ |
| Seguridad | `InputSecurityTests`: 22/22 | ✅ |
| Repositorio | Testcontainers con validador real | ⏳ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | 101 y 1500 caracteres rechazados; límite 100 cubierto por registro | ✅ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
