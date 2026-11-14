# Plan — 904 Medios con S3 y CloudFront

| Campo | Valor |
|---|---|
| Módulo | `904-medios-s3-cloudfront` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | v1.3.0 (propuesta) |

## 1. Resumen técnico

- `MediaBucket` (`AWS::S3::Bucket`): privado, SSE-S3, `BucketOwnerEnforced`, regla de ciclo de vida para cargas incompletas.
- `MediaOriginAccessControl` y `MediaDistribution` (`AWS::CloudFront::Distribution`): HTTPS, OAC, `CachingOptimized` y `SecurityHeadersPolicy` administradas.
- `MediaBucketPolicy`: `s3:GetObject` solo para `cloudfront.amazonaws.com` con `AWS:SourceArn` de la distribución, y denegación sin TLS.
- Salida `MediaBaseUrl` y variable `Media__BaseUrl` en la API.

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Infraestructura | SAM/CloudFormation, sin código nuevo en este spec |
| Seguridad | OAC (no OAI), *Block Public Access*, TLS obligatorio |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| V | Seguridad | ✅ | Bucket privado, OAC y TLS |
| VII | Costo cero | ✅ | S3 de medios y CloudFront en la tabla de servicios (v1.3.0); sin dominio de pago |
| VII.4 | IaC | ✅ | Todo en `template.yaml` |

## 4. Estructura

```text
infra/template.yaml   # MediaBucket, MediaBucketPolicy, MediaOriginAccessControl, MediaDistribution
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-44 | Bucket privado detrás de CloudFront con OAC | Bucket público; URLs prefirmadas de lectura | Seguro, en caché y gratis hasta 1 TB |
| ADR-45 | Subida directa a S3 con URL prefirmada | Subir a través de la API | Lambda tiene límite de 6 MB por petición y cobraría por tiempo |

## 6. Diseño

### 6.1 Reglas de Checkov omitidas (justificadas en `.checkov.yaml`)

| Regla | Motivo |
|---|---|
| Registro de acceso del bucket | Requiere otro bucket y costo; CloudFront ya registra métricas |
| Versionado | Duplica almacenamiento; las fotos se pueden volver a subir |
| Replicación entre regiones | Costo; no es dato crítico |
| WAF en CloudFront | WAF cuesta (Art. VII.2) |
| Registro de acceso de CloudFront | Requiere un bucket de logs con costo |

### 6.2 Seguridad

La política del bucket niega toda petición sin `aws:SecureTransport`.

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| IaC | cfn-lint, Checkov | PR |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| IaC | `api-iac` | Plantilla válida y reglas justificadas |
| Humo | T90403 | CA-01 a CA-03 con `curl` |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| La distribución tarda en crearse (5 a 15 min) | Esperado en el primer despliegue |
| El bucket no se borra con `sam delete` si tiene objetos | Vaciarlo antes (documentado en T90403) |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Infraestructura | T90401–T90402 |
| F1 | Despliegue y cierre | T90403–T90404 |
