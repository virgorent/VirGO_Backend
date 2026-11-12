# Backend: mapa de specs

Incluye `VirGo-API` y `VirGo-Database`, en el orden que pidió el profesor.

## 1. Arquitectura base

- `VirGo-API/specs/000-arquitectura-base`
- `VirGo-API/specs/010-contrato-datos`
- `VirGo-Database/specs/000-base-datos`
- `VirGo-Database/specs/001-contrato-con-api`

## 2. Endpoints: un spec por endpoint (94 specs)

La lista está en [`ENDPOINTS.md`](ENDPOINTS.md). `003` y `004` están en `main`; `005` a `009`, en el PR #5.

## 3. Base de datos por dominio

- `002-cuentas`
- `003-salones`
- `004-proveedores`
- `005-planeacion`
- `006-cotizaciones`
- `007-reservaciones`
- `008-reputacion`
- `009-interaccion`
- `010-soporte`

## 4. Infraestructura como código

- `VirGo-API/specs/900-infraestructura-iac` (incluye despliegue *canary* e IAM Identity Center)
- `VirGo-API/specs/902-observabilidad` (X-Ray y tablero de CloudWatch)
- `VirGo-API/specs/903-correo-ses`
- `VirGo-API/specs/904-medios-s3-cloudfront`
- `VirGo-API/specs/905-tareas-programadas`
- `VirGo-API/specs/906-ia-bedrock` (bloqueado hasta aprobar el gasto)
- `VirGo-API/specs/098-cache-redis` y `VirGo-API/specs/099-colas-sqs`

## 5. Pipeline CI/CD con DevSecOps

- `VirGo-API/specs/901-pipeline-cicd`
