# Check — 016 PATCH /users/me/password

| Campo | Valor |
|---|---|
| Módulo | `016-patch-users-me-password` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `AuthTests.ChangePasswordReplacesThePreviousCredential` | ✅ |
| CA-02 | `InputSecurityTests.ProtectedRoutesRequireAToken` incluye `PATCH /api/v1/users/me/password` | ✅ |
| CA-03 | `AuthTests.ChangePasswordRejectsAnIncorrectCurrentCredential` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | No aplica: lógica cubierta por integración | ➖ |
| Integración | `AuthTests`: 22/22 | ✅ |
| Seguridad | `InputSecurityTests`: 28/28 | ✅ |
| Repositorio | Testcontainers con validador real | ⏳ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | 8–128 caracteres validado por el controlador | ✅ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
