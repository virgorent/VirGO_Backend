# Check — 904 Medios con S3 y CloudFront

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `904-medios-s3-cloudfront` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | PR con la revisión del equipo | ⏳ |
| Constitution Check sin ❌ | [plan.md](plan.md#3-constitution-check) | ⏳ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Imagen por CloudFront | `curl -I` 200 | ⏳ |
| CA-02 S3 directo bloqueado | `curl -I` 403 | ⏳ |
| CA-03 HTTP redirige a HTTPS | `curl -I` 301 | ⏳ |
| CA-04 Checkov | `api-iac` en verde | ⏳ |

## 3. DevSecOps

| Control | Evidencia | Estado |
|---|---|---|
| *Block Public Access* activo | `aws s3api get-public-access-block` | ⏳ |
| Costo registrado | `memoria.md` | ⏳ |
