# Check — NV-00 Constitución SDD

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Fase | `nv-00` |
| Versión | 1.0.0 |
| Estado | Cumplido |
| Spec / Plan / Task | [spec.md](spec.md) · [plan.md](plan.md) · [task.md](task.md) |
| Revisó | Equipo virGO_APP |
| Fecha de cierre | 2026-11-04 |

## 1. Criterios de aceptación

| CA | Criterio | Evidencia | Estado |
|---|---|---|---|
| CA-01 | `constitution.md` versionada (v1.0.0) y vigente | `constitution.md` encabezado | ✅ |
| CA-02 | `AGENTS.md` coherente con la constitución | `AGENTS.md` §1–§8 | ✅ |
| CA-03 | `Memoria.md` creado y actualizado | `Memoria.md` decisiones 2026-10-30 y 2026-11-04 | ✅ |
| CA-04 | Estructura `.opencode/` y `.specify/` definida | `.opencode/`, `.specify/` | ✅ |
| CA-05 | Sin funcionalidad específica en esta fase | No hay código de negocio | ✅ |
| CA-06 | Principios y gates DevSecOps documentados | `constitution.md` §2, §6; `AGENTS.md` §7 | ✅ |

## 2. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| Sin secretos en el repositorio | Revisión estática; Gitleaks queda como gate de CI | ✅ |
| `.gitignore` raíz con archivos sensibles | `.gitignore` | ✅ |
| Baseline de secretos y dependencias | `.github/workflows/security.yml` | ✅ |
| SAST y pruebas diferidos a workflows de proyecto | `constitution.md` §6, `AGENTS.md` §7 | ✅ |

## 3. Resultado

**Cumplido.** La gobernanza global está ratificada y lista para las fases de proyecto.
