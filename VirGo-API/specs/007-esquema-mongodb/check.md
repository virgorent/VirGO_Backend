# Check — 007 Esquema de MongoDB

Lista de verificación del módulo (Art. I.4). Cada casilla lleva su evidencia.

| Campo | Valor |
|---|---|
| Módulo | `007-esquema-mongodb` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación

- [ ] El spec es atómico: solo el esquema de datos.
- [ ] Cada RF y cada CA están cubiertos en la matriz.
- [ ] El Constitution Check no tiene ❌.
- [ ] P-01 a P-05 respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 0 campos sin límite | `check_schema.py`: 22 colecciones, 0 diferencias (2026-11-04) | ✅ |
| CA-02 Base igual al esquema | `check_schema.py --live`; hoy reporta 25 diferencias pendientes de T704 | ⏳ |
| CA-03 1500 caracteres rechazados | Salida de la prueba (T705) | ⏳ |
| CA-04 Bloqueo aceptado | Salida de la prueba (T705) | ⏳ |
| CA-05 El CI detecta un campo sin límite | Enlace al PR de prueba (T706) | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | ➖ Sin código de la aplicación | ➖ |
| Integración | `--live` contra `virgo_dev` | ⏳ |
| Seguridad | Rechazo de 1500 caracteres | ⏳ |
| Mutación | ➖ Sin lógica que mutar | ➖ |
| Humo | ➖ Sin despliegue | ➖ |
| Validación de longitudes | `check_schema.py` | ✅ |

## 4. PR e integración continua

- [ ] PR: enlace —
- [ ] Una revisión aprobada.

## 5. Despliegue

➖ No aplica; T704 actualiza `virgo_dev`.

## 6. Memoria y aprendizaje

- [ ] Decisiones P-01 a P-05 en `memoria.md`.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes (o «ninguna»).
