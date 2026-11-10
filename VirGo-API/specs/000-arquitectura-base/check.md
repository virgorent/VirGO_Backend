# Check — 000 Arquitectura base del Backend

Lista de verificación del módulo (Art. I.4). Cada casilla lleva su evidencia. El módulo se marca *Cerrado* solo con todas las casillas en ✅ o ➖ justificado. Los criterios marcados con ➡️ se verifican en `900` o `901`; aquí solo se enlaza su evidencia.

| Campo | Valor |
|---|---|
| Módulo | `000-arquitectura-base` |
| Estado del spec | En revisión |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

- [ ] Spec, plan y tareas aprobados por el profesor y el equipo.
- [ ] El Constitution Check del plan no tiene ❌ (constitución v1.1.0 aprobada).
- [ ] `README.md` en inglés y `.gitignore` DevSecOps en el primer commit (T001).
- [ ] `verificar-constitucion` sobre la gobernanza: «Alineado» (T004).
- [ ] Los MCP de `opencode.json` están instalados antes de codificar (`900`: T100, `901`: T200).

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Arranque local sin nube | Salida de `dotnet run` y `curl /health` (T027) | ⏳ |
| CA-02 404 en Problem Details | Prueba de integración (T026) | ⏳ |
| CA-03 500 sin trazas y con correlación | Prueba de integración y línea del log (T026) | ⏳ |
| CA-04 401 sin token | Prueba de integración (T016, T026) | ⏳ |
| CA-05 Llave corta detiene el arranque | Prueba unitaria (T025) | ⏳ |
| CA-06 ➡️ CI bloquea la fusión | `901`: check CA-02 | ⏳ |
| CA-07 ➡️ Despliegue con URL HTTPS | `901`: check CA-05 | ⏳ |
| CA-08 ➡️ Solo servicios permitidos y presupuesto | `900`: check CA-03 y CA-04 | ⏳ |
| CA-09 OpenAPI publicado | Prueba de integración y enlace entregado a Móvil (`901`: T210) | ⏳ |
| CA-10 ➡️ Borrado de la infraestructura | `900`: check CA-07 | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `dotnet test tests/VirGo.UnitTests` · totales: — | ⏳ |
| Integración | `dotnet test tests/VirGo.IntegrationTests` · totales: — | ⏳ |
| Seguridad | `VirGo.SecurityTests` (`901`: T202) · totales: — | ⏳ |
| Mutación | Stryker sobre `Domain` y `Application` (`901`: T203) · puntaje: — % | ⏳ |
| Humo | `smoke.sh` tras el primer despliegue (`901`: T209) | ⏳ |
| Arquitectura | Prueba de dependencias de `VirGo.Domain` (T010) | ⏳ |
| Validación de longitudes | Regla global en DTO y `db/schema/` (T036) | ⏳ |

## 4. PR e integración continua

- [ ] PRs del módulo: enlaces —
- [ ] Todos con los checks requeridos en verde y una revisión.

## 5. Despliegue

- [ ] URL HTTPS: —
- [ ] Commit desplegado: —
- [ ] Arranque en frío medido: — s (RNF-03)

## 6. Memoria y aprendizaje

- [ ] Entrada de cierre en `memoria.md` con lo aprendido.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes (o «ninguna»).
