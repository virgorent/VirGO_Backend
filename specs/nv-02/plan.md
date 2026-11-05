# Plan — NV-02 Gobernanza global de proyectos

| Campo | Valor |
|---|---|
| Fase | `nv-02` |
| Versión | 1.0.0 |
| Estado | Cumplido con pendiente operativo |
| Fecha | 2026-11-04 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.0.0 |

## 1. Enfoque

1. Ratificar la constitución y aprobar el monorepo.
2. Publicar plantillas y memoria reutilizable.
3. Configurar el baseline DevSecOps compartido.
4. Documentar carpetas de apoyo.
5. Verificar coherencia y trazabilidad.

## 2. Constitution Check

| Principio | Cumple | Nota |
|---|---|---|
| SDD antes que código | ✅ | Solo gobernanza y plantillas. |
| Seguridad por defecto | ✅ | Gitleaks, dependency review y Dependabot. |
| Mínimo privilegio | ✅ | `permissions` acotados en workflows. |
| Sin duplicidad | ✅ | `Memoria.md` global; `memoria.md` por proyecto. |
| Trazabilidad | ✅ | Spec, plan, tareas y check enlazados. |

## 3. Decisiones (ADR)

| ID | Decisión | Motivo |
|---|---|---|
| ADR-01 | Monorepo con gobernanza raíz y specs por proyecto. | Coherencia y reutilización. |
| ADR-02 | Acciones de GitHub fijadas por SHA. | Evitar tags móviles. |
| ADR-03 | SAST y pruebas en workflows de proyecto. | El baseline compartido no puede conocer stacks. |

## 4. DevSecOps

- `security.yml`: Gitleaks y dependency review.
- `dependabot.yml`: Actions, NuGet y Gradle semanal.
- `rulesets/main-protection.json`: PR, revisión y checks requeridos.
- `pull_request_template.md`: checklist SDD + seguridad.

## 5. Riesgos

| Riesgo | Mitigación |
|---|---|
| Ruleset no aplicado | Documentado en `.github/README.md`; aplicación manual verificable. |
| Workflows de proyecto inexistentes | SAST y pruebas diferidos explícitamente. |
| Deriva de versiones de acciones | Dependabot + revisión humana del SHA. |

## 6. Verificación

Ver [check.md](check.md).
