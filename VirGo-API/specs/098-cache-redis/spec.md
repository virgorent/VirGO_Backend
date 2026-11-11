# Spec — 098 Caché distribuida con Redis

| Campo | Valor |
|---|---|
| Módulo | `098-cache-redis` |
| Versión | 1.0.0 |
| Estado | Implementado en local; falta probar con Redis real |
| Fecha | 2026-11-06 |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Contexto y objetivo

El proyecto de referencia del curso usa Redis para escalar sin estado. VirGo corre en Lambda, donde cada instancia tiene su propia memoria, así que necesita una caché compartida para reducir las lecturas a Atlas M0.

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | Las lecturas públicas del catálogo de salones usan caché con patrón *cache-aside*. |
| RF-02 | La lista dura 30 s en caché y el detalle 60 s. |
| RF-03 | Si Redis falla, la API sigue funcionando con la base de datos. |
| RF-04 | Sin `Redis:ConnectionString`, se usa caché en memoria. |
| RF-05 | Las llaves no contienen texto enviado por el cliente. |
| RF-06 | Sesiones, bloqueos y datos privados no se guardan en caché. |

## 3. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** un filtro ya consultado, **cuando** se repite, **entonces** no se consulta la base. |
| CA-02 | **Dado** Redis caído, **cuando** se consulta el catálogo, **entonces** responde con datos de la base. |
| CA-03 | **Dado** un filtro con saltos de línea, **cuando** se genera la llave, **entonces** no aparece en ella. |
| CA-04 | **Dado** un Redis real por TLS, **cuando** se consulta dos veces, **entonces** la segunda sale de la caché. |
| CA-05 | **Dado** el despliegue, **cuando** se revisa el costo, **entonces** sigue en $0. |

## 4. Fuera de alcance

- Invalidación por escritura: se agrega con `026` y `027`, que modifican salones.
- Rate limiting distribuido.

## 5. Preguntas abiertas

| # | Pregunta | Responsable |
|---|---|---|
| P-01 | ¿Se aprueba Upstash como segunda excepción externa del Art. VII? | Equipo |
| P-02 | ¿Basta con el plan gratuito para el tráfico del curso? | Equipo |
