# Check — 002 Cuentas y acceso

| Campo | Valor |
|---|---|
| Módulo | `002-cuentas` |
| Revisó | — |

## 1. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 `users` | Comparación del 2026-11-05 | ✅ |
| CA-02 `passwordResets` | Comparación del 2026-11-05 | ✅ |
| CA-03 `sessions` | Comparación del 2026-11-05 | ✅ |
| CA-04 `paymentCards` | Comparación del 2026-11-05 | ✅ |

## 2. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `npm test`: 16 de 16 en todo el proyecto (2026-11-05) | ✅ |
| Integración | `npm run test:integration` | ⏳ |
| Validación de longitudes | 0 textos sin límite | ⏳ |

## 3. Revisión

- [ ] Otra persona revisó el spec.
