# Check — 097 GET /health/ready

| Campo | Valor |
|---|---|
| Módulo | `097-get-health-ready` |
| Estado del spec | Implementación local; despliegue pendiente |
| Revisó | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] Preguntas abiertas respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | `BaseApiTests.ReadinessReturnsOk` | ✅ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | No aplica: endpoint delegado al sistema de health checks | ➖ |
| Integración | `BaseApiTests`: 7/7 | ✅ |
| Contrato | `MobileContractTests`: 15/15 | ✅ |
| Seguridad | `InputSecurityTests`: 23/23 | ✅ |
| Mutación | Puntaje: — % | ⏳ |
| Humo | `smoke.sh` | ⏳ |
| Validación de longitudes | ➖ Sin cuerpo | ➖ |

## 4. PR y despliegue

- [ ] PR: enlace —
- [ ] Una revisión aprobada.
- [ ] URL y JSON probado entregados a Móvil.

## 5. Memoria

- [ ] Entrada en `memoria.md`.
