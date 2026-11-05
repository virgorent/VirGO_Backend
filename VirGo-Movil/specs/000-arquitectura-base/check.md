# Check — 000 Arquitectura base del Móvil

Lista de verificación del módulo (Art. I.4). Cada casilla lleva su evidencia. El módulo se marca *Cerrado* solo con todas las casillas en ✅ o ➖ justificado.

| Campo | Valor |
|---|---|
| Módulo | `000-arquitectura-base` |
| Estado del spec | Aprobado; implementación pendiente |
| Revisó | Equipo (2026-11-05) |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

- [ ] Spec, plan y tareas aprobados por el profesor y el equipo. Equipo: 2026-11-05; profesor: pendiente.
- [ ] El Constitution Check del plan no tiene ❌ (constitución v1.1.0 aprobada).
- [ ] `README.md` en inglés y `.gitignore` DevSecOps en el primer commit (T001).
- [ ] `verificar-constitucion` sobre la gobernanza: «Alineado» (T004).
- [ ] MCP `figma` y `github` instalados y probados (T037).
- [ ] P-01 (ID de la app) y P-02 (custodia del keystore) respondidas.

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 | Compilación de módulos y CI (T007, T031) | ⏳ |
| CA-02 | Arranque e inicio provisional en el emulador (T015, T017, T030) | ⏳ |
| CA-03 | Sistema de diseño y modo oscuro (T014, T030) | ⏳ |
| CA-04 | Diagnóstico contra la API (T024, T030) | ⏳ |
| CA-05 | Diagnóstico en *debug* (T024) | ⏳ |
| CA-06 | *Release* sin tráfico en claro (T026, T030) | ⏳ |
| CA-07 | Token cifrado (T022) | ⏳ |
| CA-08 | CI bloquea la fusión (T031, T039) | ⏳ |
| CA-09 | Release manual con APK firmado (T035) | ⏳ |
| CA-10 | Accesibilidad y escala de fuente (T013, T029) | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `./gradlew testDebugUnitTest` · totales: — | ⏳ |
| Integración | MockWebServer y Robolectric (T028) · totales: — | ⏳ |
| UI | Compose Testing (T029) · totales: — | ⏳ |
| Seguridad | Token cifrado, sin tráfico en claro, Lint, CodeQL y Gitleaks sin hallazgos altos | ⏳ |
| Mutación | `./gradlew pitest` (T038) · puntaje base: — % | ⏳ |
| Humo | APK `v0.1.0` instalado y diagnóstico en verde contra producción (T035) | ⏳ |

## 4. PR e integración continua

- [ ] PRs del módulo: enlaces —
- [ ] Ruleset exportado en `.github/rulesets/main-protection.json` (T039).
- [ ] Todos con los checks requeridos en verde y una revisión.

## 5. Release

- [ ] Ejecución de `movil-release.yml`: enlace —
- [ ] Versión y commit: —
- [ ] `API_BASE_URL` de *release*: — (entregada por `VirGo-API` T210)
- [ ] Arranque en frío medido: — s (RNF-03)

## 6. Memoria y aprendizaje

- [ ] Entrada de cierre en `memoria.md` con lo aprendido.
- [ ] Mejoras propuestas a la constitución, `AGENTS.md`, skills o subagentes (o «ninguna»).
