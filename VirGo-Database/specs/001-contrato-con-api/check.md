# Check — 001 Contrato de datos con la API

| Campo | Valor |
|---|---|
| Módulo | `001-contrato-con-api` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación

- [ ] P-01, P-02 y P-05 respondidas.
- [ ] Cada RF y cada CA están en la matriz.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Usuario de la API aceptado | Salida de T106 | ⏳ |
| CA-02 1500 caracteres rechazados | Salida de T106 | ⏳ |
| CA-03 Idempotencia | Salida de T106 | ⏳ |
| CA-04 Sin diferencias | `db:verify:dev` | ⏳ |
| CA-05 `db/schema/` igual al catálogo | `check_schema.py --live` con 0 diferencias | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `npm test` | ⏳ |
| Integración | `npm run test:integration` | ⏳ |
| Validación de longitudes | 0 textos sin límite | ⏳ |

## 4. PR e integración continua

- [ ] PR: enlace —
- [ ] Una revisión aprobada.

## 5. Aplicación

- [ ] Plan revisado antes de aplicar.
- [ ] `virgo_dev` actualizado; fecha —

## 6. Memoria y aprendizaje

- [ ] Decisiones en `Memoria.md`.
