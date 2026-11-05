# Check — 002 Pipeline CI/CD con DevSecOps

Lista de verificación del módulo (Art. I.4). Cada casilla lleva su evidencia. El módulo se marca *Cerrado* solo con todas las casillas en ✅ o ➖ justificado.

| Campo | Valor |
|---|---|
| Módulo | `002-pipeline-cicd` |
| Estado del spec | Implementación local; ejecución en GitHub pendiente |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación

- [ ] El spec es atómico: solo el pipeline y el ruleset.
- [ ] Cada RF y cada CA están cubiertos en la matriz de trazabilidad.
- [ ] El Constitution Check del plan no tiene ❌.
- [ ] P-01 (quién aprueba) y P-02 (público o privado) respondidas.
- [ ] El MCP `github` está instalado y probado (T200).

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 *Push* directo rechazado | Salida de `git push` rechazado | ⏳ |
| CA-02 PR con prueba en rojo bloqueado | Enlace al PR de prueba | ⏳ |
| CA-03 Secreto detectado | Enlace al job `secrets` fallido | ⏳ |
| CA-04 Todos los checks y reporte de mutación | Workflows creados; ejecución de GitHub pendiente; Stryker local 36.36 % | 🔄 |
| CA-05 Despliegue manual con URL `https://` | Enlace a la ejecución de `api-deploy.yml` | ⏳ |
| CA-06 Despliegue desde otra rama se niega | Enlace a la ejecución omitida | ⏳ |
| CA-07 Humo fallido deja el workflow en rojo | Ejecución de prueba o salida local de `smoke.sh` contra una URL inválida | ⏳ |
| CA-08 URL en ambas memorias | Enlaces a las entradas de `memoria.md` | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | Job `build-test` en verde | ⏳ |
| Integración | Job `build-test` en verde | ⏳ |
| Seguridad | `VirGo.SecurityTests` 3 pruebas en verde; CodeQL, Gitleaks, *dependency review* y Checkov pendientes de GitHub | 🔄 |
| Mutación | Puntaje base: 36.36 % · Umbral provisional: 0 % | 🔄 |
| Humo | `smoke.sh` en verde contra producción | ⏳ |

## 4. PR e integración continua

- [ ] PR: enlace —
- [ ] Ruleset exportado en `.github/rulesets/main-protection.json`.
- [ ] Una revisión aprobada.

## 5. Despliegue

- [ ] Ejecución de `api-deploy.yml`: enlace —
- [ ] Commit desplegado: —
- [ ] URL HTTPS: —
- [ ] Arranque en frío medido: — s

## 6. Memoria y aprendizaje

- [ ] Entrada de cierre en `memoria.md` con lo aprendido.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes (o «ninguna»).
