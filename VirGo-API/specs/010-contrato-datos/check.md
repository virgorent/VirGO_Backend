# Check — 010 Contrato de datos con VirGo-Database

| Campo | Valor |
|---|---|
| Módulo | `010-contrato-datos` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación

- [ ] Otra persona revisó el spec.
- [ ] `VirGo-Database` `001` cerrado.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 0 campos sin límite | `check_schema.py`: 22 colecciones, 0 diferencias (2026-11-05) | ✅ |
| CA-02 Base igual al esquema | `check_schema.py --live`; hoy reporta 25 diferencias | ⏳ |
| CA-03 Documentos válidos aceptados | Pruebas con Testcontainers | ⏳ |
| CA-04 El CI detecta cambios a mano | Enlace al PR de prueba | ⏳ |
| CA-05 Texto de más → 400 | Prueba de seguridad | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | ➖ Sin lógica propia | ➖ |
| Integración | Repositorios contra validadores reales | ⏳ |
| Seguridad | Longitudes por campo | ⏳ |
| Mutación | ➖ Sin lógica que mutar | ➖ |
| Humo | Se cubre en el despliegue | ⏳ |
| Validación de longitudes | `check_schema.py` | ✅ |

## 4. PR e integración continua

- [ ] PR: enlace —
- [ ] Una revisión aprobada.

## 5. Despliegue

➖ No aplica.

## 6. Memoria y aprendizaje

- [ ] Registro en `memoria.md`.
