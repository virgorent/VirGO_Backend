# Tareas — 904 Medios con S3 y CloudFront

| Campo | Valor |
|---|---|
| Módulo | `904-medios-s3-cloudfront` |
| Versión | 0.1.0 |
| Estado | Borrador. **Las tareas no se ejecutan hasta aprobar este archivo.** |
| Fecha | 2026-11-11 |
| Spec / Plan / Check | [spec.md](spec.md) · [plan.md](plan.md) · [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

**Estados:** ⏳ Pendiente · 🔄 En curso · ✅ Hecha (con evidencia) · ⛔ Bloqueada

| ID | Tarea | Depende de | Responsable | Verificación | Estado |
|---|---|---|---|---|---|
| T90401 | `template.yaml`: bucket, política, OAC, distribución, salida `MediaBaseUrl` y `Media__BaseUrl` | — | `devops-aws` | `sam validate --lint`, cfn-lint | ⏳ |
| T90402 | Justificar en `.checkov.yaml` las reglas de la sección 6.1 del plan | T90401 | `seguridad` | Checkov sin fallas | ⏳ |
| T90403 | 🧑 Desplegar y probar con una imagen: CloudFront 200, S3 directo 403, HTTP → HTTPS | T90402 | `devops-aws` | CA-01 a CA-03 | ⏳ |
| T90404 | Registrar el costo y completar `check.md` | T90403 | `qa` | `check.md` sin ⏳ | ⏳ |

## Matriz de trazabilidad

| Requerimiento / Criterio | Tareas |
|---|---|
| RF-01 … RF-05 | T90401 |
| RF-06 | Specs de salones y proveedores |
| HT-01 … HT-03 | T90401, T90403 |
| RNF-01 … RNF-04 | T90401 · T90401 · T90404 · T90401 |
| CA-01 … CA-04 | T90403 · T90403 · T90403 · T90402 |
