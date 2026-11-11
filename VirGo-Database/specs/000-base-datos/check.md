# Check — 000 Base de datos MongoDB

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Estado del spec | Implementado; pendiente de revisión del equipo |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] P-01 y P-02 respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Catálogo igual a la base | Comparación de validadores en modo lectura, 2026-11-05: 22 de 22 | ✅ |
| CA-02 `plan` no escribe | Salida de `npm run db:plan:dev` | ⏳ |
| CA-03 `apply` exige `--yes` | Salida sin `--yes` | ⏳ |
| CA-04 Sin secretos | Gitleaks en el PR #7 en verde; solo valores de ejemplo | ✅ |
| CA-05 Pruebas | Salida de `npm test` | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `README.md` declara 16; falta la salida | ⏳ |
| Integración | `README.md` declara 20; falta la salida | ⏳ |
| Deriva | `npm run db:verify:dev` | ⏳ |

## 4. PR e integración continua

- [ ] PR #7 revisado por otra persona.
- [ ] `dependency review` en verde; en el PR #7 falló.
- [ ] `database-ci.yml` activo (T006).

## 5. Despliegue

➖ No aplica; `virgo_dev` ya tiene esta estructura.

## 6. Memoria y aprendizaje

- [ ] Decisiones enlazadas desde `Memoria.md`.
