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
| CA-01 0 campos sin límite | `check_schema.py`: 22 colecciones, 0 diferencias (2026-11-13) | ✅ |
| CA-02 Base igual al esquema | `check_schema.py --live --db virgo_dev`: 22 colecciones, 0 diferencias (2026-11-13, salida 0) | ✅ |
| CA-03 Documentos válidos aceptados | Pruebas con Testcontainers | ⏳ |
| CA-04 El CI detecta cambios a mano | Enlace al PR de prueba | ⏳ |
| CA-05 Texto de más → 400 | Prueba de seguridad | ⏳ |

### Evidencia live 2026-11-13

- `virgo_dev`: `check_schema.py --live --db virgo_dev` → 22 colecciones, 0 diferencias, salida 0 (`2026-11-13T00:15:41-06:00`).
- `virgo`: `check_schema.py --live --db virgo` → 22 colecciones esperadas, 2 presentes, 20 faltantes, 2 validadores diferentes, 4 índices faltantes y 26 diferencias, salida 1 (`2026-11-13T00:16:02-06:00`).
- TLS: PyMongo construyó el cliente con `tls=True`; sin `tlsInsecure` ni overrides de certificado/hostname.
- Auditoría completa: [audit-live-atlas-2026-11-13.md](audit-live-atlas-2026-11-13.md).
- Limitación: el script no verifica `validationLevel`, `validationAction` ni documentos; CA-02 no cubre esos elementos.

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
