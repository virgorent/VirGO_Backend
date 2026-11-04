# Check — 004 Cerrar sesión (logout)

Lista de verificación del módulo (Art. I.4). Cada casilla lleva su evidencia. El spec se marca *Cerrado* solo con todas las casillas en ✅ o ➖ justificado.

| Campo | Valor |
|---|---|
| Módulo | `004-auth-logout` · `POST /api/v1/auth/logout` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación

- [ ] El spec es atómico: solo el logout de la sesión actual.
- [ ] Cada RF y cada CA están cubiertos en la matriz de trazabilidad.
- [ ] El Constitution Check del plan no tiene ❌.
- [ ] P-01 resuelta con el equipo Móvil.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 204 y sesión revocada | Prueba de integración y consulta con el MCP `mongodb` | ⏳ |
| CA-02 Token de sesión cerrada → 401 en toda la API | Prueba de integración | ⏳ |
| CA-03 La otra sesión sigue funcionando | Prueba de integración | ⏳ |
| CA-04 Sin token o inválido → 401 | Prueba de seguridad | ⏳ |
| CA-05 Token sin `sid` → 401 | Prueba de seguridad | ⏳ |
| CA-06 Humo 401 por HTTPS | Resumen de `api-deploy.yml` | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Totales: — | ⏳ |
| Integración | Totales: — | ⏳ |
| Seguridad | Totales: — · Revisión de `seguridad` sin hallazgos altos | ⏳ |
| Mutación | Puntaje: — % (umbral — %) | ⏳ |
| Humo | Caso de logout en `smoke.sh` en verde | ⏳ |

## 4. PR e integración continua

- [ ] PR: enlace —
- [ ] Todos los checks requeridos en verde y una revisión aprobada.

## 5. Despliegue y entrega a Móvil

- [ ] Ejecución de `api-deploy.yml`: enlace — · commit —
- [ ] Latencia agregada medida: — ms · operaciones por segundo en Atlas: —
- [ ] Contrato entregado a Móvil y registrado en ambas memorias.

## 6. Memoria y aprendizaje

- [ ] Spec marcado como *Desplegado* y luego *Cerrado*.
- [ ] Entrada en `memoria.md` con lo aprendido.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes (o «ninguna»).
