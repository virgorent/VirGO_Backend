# Spec — 904 Medios con S3 y CloudFront

| Campo | Valor |
|---|---|
| Módulo | `904-medios-s3-cloudfront` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | 2026-11-11 |
| Constitución | v1.3.0 (propuesta) |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

Los salones y proveedores necesitan fotos. Este spec crea el **almacenamiento privado** en S3 y la **entrega por HTTPS** con CloudFront, y define cómo se suben las fotos, para que los specs de salones y proveedores solo las usen.

## 2. Historias

| ID | Historia |
|---|---|
| HT-01 | Como **cliente**, quiero ver las fotos de un salón rápido y por HTTPS. |
| HT-02 | Como **propietario o proveedor**, quiero subir fotos sin que pasen por la API. |
| HT-03 | Como **responsable de seguridad**, quiero que nadie pueda leer el bucket directamente ni subir archivos sin permiso. |

## 3. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Existe un bucket privado `virgo-media-{cuenta}` con todo el acceso público bloqueado y cifrado SSE-S3. |
| RF-02 | Existe una distribución de CloudFront que solo sirve por HTTPS, lee el bucket con *Origin Access Control* y agrega encabezados de seguridad. |
| RF-03 | La URL base de CloudFront es una salida del stack (`MediaBaseUrl`) y llega a la API como `Media__BaseUrl`. |
| RF-04 | Las rutas siguen el patrón `venues/{venueId}/{uuid}.jpg` y `providers/{providerId}/{uuid}.jpg`. |
| RF-05 | Las cargas incompletas se borran a 1 día. |
| RF-06 | Contrato de subida (lo implementan los specs de salones y proveedores): la API entrega una URL prefirmada de `PUT` válida 5 minutos, solo para `image/jpeg`, `image/png` o `image/webp` y hasta 5 MB. |

## 4. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Seguridad:** el bucket solo admite lectura desde la distribución (condición `AWS:SourceArn`) y solo TLS; nadie lo lista públicamente. |
| RNF-02 | **Rendimiento:** las imágenes se guardan en caché en CloudFront con la política `CachingOptimized`. |
| RNF-03 | **Costo:** CloudFront incluye 1 TB y 10 millones de peticiones al mes gratis siempre. S3 usa los créditos del plan; el estimado con 1 GB es menor a $0.03 al mes. Clase de precio `PriceClass_100`. |
| RNF-04 | **Trazabilidad:** todo se define en `template.yaml`. |

## 5. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** el stack desplegado, **cuando** subo una imagen de prueba al bucket, **entonces** se ve en `{MediaBaseUrl}/ruta`. |
| CA-02 | **Dado** la URL directa de S3 de esa imagen, **cuando** la abro, **entonces** responde 403. |
| CA-03 | **Dado** la URL de CloudFront por HTTP, **cuando** la abro, **entonces** redirige a HTTPS. |
| CA-04 | **Dado** Checkov, **cuando** valida la plantilla, **entonces** no hay hallazgos sin justificar. |

## 6. Seguridad y privacidad

| Amenaza | Control |
|---|---|
| Bucket público por error | *Block Public Access* en las 4 opciones y política solo para CloudFront. |
| Subida de archivos maliciosos | URL prefirmada con tipo y tamaño fijos; solo usuarios autenticados con el rol correcto. |
| Metadatos con ubicación (EXIF) | Pregunta abierta P-01. |

## 7. Fuera de alcance

- Los endpoints que generan la URL prefirmada y guardan la ruta en `venues` y `providers`.
- Redimensionar imágenes y quitar metadatos.
- Dominio propio para CloudFront (Art. VII.2).

## 8. Supuestos, dependencias y preguntas abiertas

**Dependencias**

- `900-infraestructura-iac`.

**Preguntas abiertas**

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Se quitan los metadatos EXIF en la app antes de subir? | Equipo Móvil |
| P-02 | ¿Cuántas fotos por salón y por proveedor? | Equipo |
