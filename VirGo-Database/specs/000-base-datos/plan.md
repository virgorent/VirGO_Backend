# Plan — 000 Base de datos MongoDB

| Campo | Valor |
|---|---|
| Módulo | `000-base-datos` |
| Versión | 1.0.0 |
| Estado | Implementado; pendiente de revisión del equipo |
| Fecha | 2026-11-05 |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Resumen técnico

Proyecto Node.js con el driver oficial de MongoDB. `src/catalog` declara colecciones, validadores e índices; `src/init` calcula, aplica y verifica; `src/cli` expone `init`, `verify` e `inspect`. La arquitectura detallada está en [`docs/ARCHITECTURE.md`](../../docs/ARCHITECTURE.md) y las decisiones en [`docs/DECISIONS.md`](../../docs/DECISIONS.md).

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Plataforma | Node.js 20.19 o superior, ES modules |
| Base de datos | MongoDB Atlas, clúster `virgo-dev`; bases `virgo_dev` y `virgo` |
| Configuración | `.env.*` local con `dotenv`; solo los `*.example` se versionan |
| Pruebas | Unitarias y de integración contra MongoDB 8 local |

## 3. Alineación con la constitución global

| Sección | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| 2 | Principios | ✅ Cumple | Sin secretos, validación estricta y sin operaciones destructivas |
| 4 | Arquitectura | ⚠️ Justificado | Se construyó antes de tener spec; este spec lo regulariza |
| 6 | Seguridad | ✅ Cumple | Usuario DDL separado del usuario de la aplicación |
| 7 | Pruebas | ⚠️ Parcial | Las pruebas existen, pero no corren en el CI (T006) |
| 9 | Memoria | ⚠️ Parcial | Las decisiones están en `docs/DECISIONS.md`; se enlazan desde `Memoria.md` |

## 4. Estructura

Ver la sección «Estructura» de [`README.md`](../../README.md).

## 5. Decisiones técnicas

Se conservan las de [`docs/DECISIONS.md`](../../docs/DECISIONS.md). Las principales: `_id` como UUID en texto, dinero en `Decimal128`, fechas de calendario en `YYYY-MM-DD`, y `venueDates` solo al confirmar la reservación.

## 6. Diseño

La responsabilidad se reparte así:

| Proyecto | Puede | No puede |
|---|---|---|
| `VirGo-Database` | Crear y cambiar colecciones, validadores e índices | Implementar endpoints |
| `VirGo-API` | Leer y escribir documentos | Ejecutar DDL o cambiar la estructura |

## 7. Costo

$0.

## 8. Estrategia de pruebas

| Tipo | Cómo |
|---|---|
| Unitarias | `npm test` (catálogo, formato y plan) |
| Integración | `npm run test:integration` contra MongoDB local |
| Deriva | `npm run db:verify:dev` |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| La API y el catálogo se separan | Spec `001-contrato-con-api` y comprobación en el CI |
| Las pruebas no corren en el CI | T006 |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | Lo ya construido | T001–T004 |
| F1 | Regularización | T005–T007 |
