# VirGo API Status

Fecha: 2026-11-16

## Resumen

- 73 APIs implementadas localmente en este ciclo.
- Producción reportada por César: 12 operaciones publicadas. Las 73 locales no se consideran disponibles para Móvil hasta pasar specs, PR, CI, merge y despliegue.
- Persistencia local: InMemory.
- Persistencia nominal: MongoDB mediante repositorios equivalentes.
- Último build Release: 0 errores, 0 advertencias.
- No hay commit, PR, CI, despliegue ni smoke HTTPS ejecutados desde este worktree.
- Rama local de preparación: `feat/api-mobile-priority`.
- Dependabot ignora majors de `Microsoft.Extensions.*` hasta la migración a .NET 11.
- `coverlet.collector` sigue en `6.0.4` en este worktree; la validación del collector actual sí generó reportes Cobertura.

## Decisión De Entrega

- Todos los specs continúan en estado `Borrador` en `main`; requieren aprobación o marcado explícito por el equipo antes de abrir PR.
- Los PR deben ser pequeños y separados por bloque para que CI ejecute las suites que Windows puede bloquear localmente.
- Pagos/tarjetas (`019–023`, `066–071`), chat (`084–089`) y asistente (`094`) quedan implementados localmente, pero no deben desplegarse a producción hasta nueva decisión.
- No se elimina código de esos módulos; se mantienen fuera del alcance de los PR de producción inmediatos.

## Estado De Consumo Móvil

La diferencia crítica es:

- `73`: rutas implementadas en el worktree local.
- `12`: operaciones actualmente publicadas en producción según la revisión de Móvil.
- `0`: nuevas rutas de este worktree desplegadas por este ciclo.

Bloqueadores reportados para Móvil:

- Detalle de salón.
- Eventos.
- Reservas GET.
- Favoritos.
- Avisos.
- Proveedores.
- Recuperación de contraseña.

La prioridad de entrega debe cubrir primero esos contratos, no pagos, chat ni asistente.

## Implementado

- Usuarios, venues, disponibilidad, proveedores y gigs: `015–018`, `024–027`, `031`, `034–041`.
- Eventos, invitados, tareas y colaboradores: `043–059`.
- Cotizaciones: `060–065`.
- Reservas y pagos: `066–071`.
- Reseñas: `072–075`.
- Estadísticas y favoritos: `078–083`.
- Conversaciones y mensajes: `084–089`.
- Mensajes del asistente: `094`.
- Avisos, asistente, contacto y readiness: `090–093`, `096–097`.
- Tarjetas de pago: `019–023`.
- Bloqueos manuales de venues: `032–033`.

## Candidatos A PR

- Usuarios: `015–018`.
- Venues y disponibilidad: `024–027`, `031–033`.
- Eventos, invitados, tareas y colaboradores: `043–059`.
- Cotizaciones: `060–065`.
- Reseñas: `072–075`.

Cada bloque requiere primero aprobación de sus specs y después un PR independiente con CI.

## Pendiente

- `011–014`: recuperación, reset, refresh y verificación de correo. Requieren decisión sobre proveedor de correo, token/hash y obligatoriedad de verificación.
- `028–030`: submit, publish y delete de venues.
- `042`: delete de gigs, pendiente de decisión de negocio.
- `047`: delete de eventos, pendiente de decisión de negocio.
- `076–077`: editar y eliminar reseñas, bloqueados por decisión previa.
- `095`: eliminar mensajes del asistente; permanece bloqueado.
- `098–099`: cache Redis y colas SQS, tratados como infraestructura.

## Diferidos Por Decisión

- `019–023`, `066–071`: pagos y tarjetas.
- `084–089`: chat y conversaciones.
- `094`: escritura del asistente.

## Riesgos Y Verificación

- Las suites de integración/contrato/seguridad pueden bloquearse en Windows por Control de aplicaciones (`0x800711C7`).
- La última verificación repetible en este worktree es el build Release.
- Deben agregarse o completar pruebas de repositorio, integración y seguridad para los bloques nuevos.
- La corrida con cobertura produjo 6 archivos `coverage.cobertura.xml`; la primera corrida detectó 2 contratos de colaboradores y 3 integraciones.
- Corrección aplicada: las rutas de colaboradores ahora publican `{userId}` y el conjunto específico de OpenAPI pasó `82/82`.
- Correcciones aplicadas: el test de gig crea primero su perfil proveedor y el test de eventos selecciona su fixture por nombre para no depender de estado compartido.
- Última suite completa: Database `96/96`, Unit `35/35`, Contract `85/85`, Performance `3/3`, Security `48/48`, Integration `95/95`.
- Revisar contratos de MongoDB y ejecutar smoke HTTPS antes de considerar el trabajo entregable.

## Próximo Orden

1. Resolver las decisiones de producto de autenticación y correo.
2. Implementar `028–030` si se aprueba el flujo de publicación.
3. Implementar `094` y dejar `095` bloqueado hasta decisión.
4. Completar `098–099` como trabajo de infraestructura separado.
