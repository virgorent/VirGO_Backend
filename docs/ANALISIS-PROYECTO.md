# Análisis completo de VirGo

**Fecha:** 6 de noviembre de 2026
**Generado desde:** `docs/specs-completos`, `main` en `392ea41` y el PR #5 en `3e4a8e4`.

Este documento se genera leyendo los archivos. Las descripciones de archivos salen de su contenido (títulos y nombres definidos), no de una revisión manual del código.

**La columna «Dónde»:** `main` ya fusionado; `rama`, en `docs/specs-completos`; `PR #5`, en la rama de Gafas.

## 1. Resumen

| Dato | Valor |
|---|---|
| Specs | 142: 98 de API, 11 de base de datos, 30 de Móvil y 3 globales |
| Endpoints | 94 de negocio con spec, más `/health` y `/openapi/v1.json` |
| Endpoints programados | 7 de negocio, en el PR #5 |
| Colecciones | 22 |
| Pantallas | 73 |
| Archivos | 836 |
| Tareas de specs de proyecto | 617, con 50 hechas |
| URL en AWS | No existe |

**Cómo leer las tareas:** los 7 endpoints del PR #5 están programados aunque varios de sus specs tengan formato corto. `000` conserva filas de equivalencia con tareas movidas, por lo que su conteo es mayor al de tareas vigentes.

## 2. Orden de los specs

1. Gobernanza: `constitution.md`, `AGENTS.md`, memorias, subagentes y skills.
2. Arquitectura base: `000`, `010`.
3. Endpoints: `003` a `097`.
4. Infraestructura: `900-infraestructura-iac`.
5. Pipeline: `901-pipeline-cicd`.

## 3. Spec por spec

### 3.1 Globales

| Spec | Título | Estado | RF | CA | Tareas | Hechas | Bloqueadas | Preguntas |
|---|---|---|---|---|---|---|---|---|
| `nv-00` | NV-00 Constitución SDD | Cumplido | 6 | 6 | 9 | 9 | 0 | 0 |
| `nv-01` | NV-01 Arquitectura base SDD | Cumplido | 5 | 9 | 11 | 11 | 0 | 0 |
| `nv-02` | NV-02 Gobernanza global de proyectos | Cumplido con pendiente operativo | 8 | 8 | 9 | 9 | 0 | 0 |

### 3.2 API

| Spec | Título | Estado | RF | CA | Tareas | Hechas | Bloqueadas | Preguntas |
|---|---|---|---|---|---|---|---|---|
| `000-arquitectura-base` | 000 Arquitectura base del Backend | En revisión | 16 | 10 | 36 | 0 | 0 | 2 |
| `003-auth-login` | 003 Iniciar sesión (login) | Borrador. **Se redacta para dar contexto; no se ejecuta hasta cerrar `000`, `900` y `901` (Art. II.2).** | 8 | 9 | 13 | 0 | 0 | 2 |
| `004-auth-logout` | 004 Cerrar sesión (logout) | Borrador. **Se redacta para dar contexto; no se ejecuta hasta cerrar `003` (Art. II.2).** | 7 | 6 | 9 | 0 | 0 | 1 |
| `005-auth-register` | 005 Registro de cuenta | En el PR #5: Implementación local; despliegue pendiente | 0 | 5 | 4 | 2 | 0 | 0 |
| `006-users-me` | 006 Perfil propio | En el PR #5: Implementación local; despliegue pendiente | 0 | 3 | 4 | 2 | 0 | 0 |
| `007-venues-catalog` | 007 Catalogo publico de venues | En el PR #5: Implementación local; despliegue pendiente | 0 | 4 | 4 | 2 | 0 | 0 |
| `008-quotes-create` | 008 Crear cotizacion | En el PR #5: Implementación local; despliegue pendiente | 0 | 4 | 4 | 2 | 0 | 0 |
| `009-bookings-create` | 009 Crear reservacion | En el PR #5: Implementación local; despliegue pendiente | 0 | 5 | 5 | 4 | 0 | 0 |
| `010-contrato-datos` | 010 Contrato de datos con VirGo-Database | Borrador | 6 | 5 | 6 | 2 | 0 | 0 |
| `011-post-auth-password-forgot` | 011 POST /auth/password/forgot | Borrador | 4 | 2 | 3 | 0 | 0 | 2 |
| `012-post-auth-password-reset` | 012 POST /auth/password/reset | Borrador | 4 | 2 | 3 | 0 | 0 | 1 |
| `013-post-auth-refresh` | 013 POST /auth/refresh | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `014-post-auth-email-verify` | 014 POST /auth/email/verify | Borrador; **requiere decisión** | 2 | 1 | 3 | 0 | 3 | 1 |
| `015-patch-users-me` | 015 PATCH /users/me | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `016-patch-users-me-password` | 016 PATCH /users/me/password | Borrador | 1 | 3 | 3 | 0 | 0 | 0 |
| `017-get-users-id-public` | 017 GET /users/:id/public | Borrador | 2 | 1 | 3 | 0 | 0 | 0 |
| `018-delete-users-me` | 018 DELETE /users/me | Borrador; **requiere decisión** | 1 | 2 | 3 | 0 | 3 | 1 |
| `019-get-payment-cards` | 019 GET /payment-cards | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `020-post-payment-cards` | 020 POST /payment-cards | Borrador | 3 | 3 | 3 | 0 | 0 | 1 |
| `021-patch-payment-cards-id` | 021 PATCH /payment-cards/:id | Borrador | 2 | 4 | 3 | 0 | 0 | 0 |
| `022-delete-payment-cards-id` | 022 DELETE /payment-cards/:id | Borrador | 1 | 3 | 3 | 0 | 0 | 0 |
| `023-post-payment-cards-id-default` | 023 POST /payment-cards/:id/default | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `024-get-venues-id` | 024 GET /venues/:id | Borrador | 3 | 1 | 3 | 0 | 0 | 1 |
| `025-get-venues-mine` | 025 GET /venues/mine | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `026-post-venues` | 026 POST /venues | Borrador | 3 | 3 | 3 | 0 | 0 | 1 |
| `027-patch-venues-id` | 027 PATCH /venues/:id | Borrador | 3 | 4 | 3 | 0 | 0 | 1 |
| `028-post-venues-id-submit` | 028 POST /venues/:id/submit | Borrador; **requiere decisión** | 2 | 3 | 3 | 0 | 3 | 1 |
| `029-post-venues-id-publish` | 029 POST /venues/:id/publish | Borrador; **requiere decisión** | 2 | 3 | 3 | 0 | 3 | 1 |
| `030-delete-venues-id` | 030 DELETE /venues/:id | Borrador; **requiere decisión** | 2 | 3 | 3 | 0 | 3 | 1 |
| `031-get-venues-id-availability` | 031 GET /venues/:id/availability | Borrador | 2 | 1 | 3 | 0 | 0 | 0 |
| `032-post-venues-id-blocks` | 032 POST /venues/:id/blocks | Borrador | 3 | 4 | 3 | 0 | 0 | 0 |
| `033-delete-venues-id-blocks-blockid` | 033 DELETE /venues/:id/blocks/:blockId | Borrador | 1 | 3 | 3 | 0 | 0 | 0 |
| `034-get-providers` | 034 GET /providers | Borrador | 3 | 1 | 3 | 0 | 0 | 1 |
| `035-get-providers-id` | 035 GET /providers/:id | Borrador | 1 | 1 | 3 | 0 | 0 | 0 |
| `036-get-providers-me` | 036 GET /providers/me | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `037-post-providers` | 037 POST /providers | Borrador | 4 | 3 | 3 | 0 | 0 | 1 |
| `038-patch-providers-me` | 038 PATCH /providers/me | Borrador | 2 | 3 | 3 | 0 | 0 | 1 |
| `039-get-providers-me-gigs` | 039 GET /providers/me/gigs | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `040-post-providers-me-gigs` | 040 POST /providers/me/gigs | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `041-patch-providers-me-gigs-id` | 041 PATCH /providers/me/gigs/:id | Borrador | 3 | 4 | 3 | 0 | 0 | 0 |
| `042-delete-providers-me-gigs-id` | 042 DELETE /providers/me/gigs/:id | Borrador; **requiere decisión** | 2 | 3 | 3 | 0 | 3 | 1 |
| `043-get-events` | 043 GET /events | Borrador | 1 | 2 | 3 | 0 | 0 | 0 |
| `044-post-events` | 044 POST /events | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `045-get-events-id` | 045 GET /events/:id | Borrador | 1 | 3 | 3 | 0 | 0 | 0 |
| `046-patch-events-id` | 046 PATCH /events/:id | Borrador | 3 | 4 | 3 | 0 | 0 | 1 |
| `047-delete-events-id` | 047 DELETE /events/:id | Borrador; **requiere decisión** | 1 | 3 | 3 | 0 | 3 | 1 |
| `048-get-events-id-guests` | 048 GET /events/:id/guests | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `049-post-events-id-guests` | 049 POST /events/:id/guests | Borrador | 2 | 4 | 3 | 0 | 0 | 0 |
| `050-patch-events-id-guests-guestid` | 050 PATCH /events/:id/guests/:guestId | Borrador | 2 | 4 | 3 | 0 | 0 | 0 |
| `051-delete-events-id-guests-guestid` | 051 DELETE /events/:id/guests/:guestId | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `052-get-events-id-tasks` | 052 GET /events/:id/tasks | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `053-post-events-id-tasks` | 053 POST /events/:id/tasks | Borrador | 2 | 4 | 3 | 0 | 0 | 0 |
| `054-patch-events-id-tasks-taskid` | 054 PATCH /events/:id/tasks/:taskId | Borrador | 2 | 4 | 3 | 0 | 0 | 0 |
| `055-delete-events-id-tasks-taskid` | 055 DELETE /events/:id/tasks/:taskId | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `056-get-events-id-collaborators` | 056 GET /events/:id/collaborators | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `057-post-events-id-collaborators` | 057 POST /events/:id/collaborators | Borrador | 3 | 4 | 3 | 0 | 0 | 0 |
| `058-patch-events-id-collaborators-userid` | 058 PATCH /events/:id/collaborators/:userId | Borrador | 3 | 4 | 3 | 0 | 0 | 0 |
| `059-delete-events-id-collaborators-userid` | 059 DELETE /events/:id/collaborators/:userId | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `060-get-quotes` | 060 GET /quotes | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `061-get-quotes-id` | 061 GET /quotes/:id | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `062-post-quotes-id-response` | 062 POST /quotes/:id/response | Borrador | 3 | 4 | 3 | 0 | 0 | 0 |
| `063-post-quotes-id-accept` | 063 POST /quotes/:id/accept | Borrador | 5 | 3 | 3 | 0 | 0 | 0 |
| `064-post-quotes-id-reject` | 064 POST /quotes/:id/reject | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `065-post-quotes-id-expire` | 065 POST /quotes/:id/expire | Borrador | 2 | 1 | 3 | 0 | 0 | 1 |
| `066-get-bookings` | 066 GET /bookings | Borrador | 1 | 2 | 3 | 0 | 0 | 0 |
| `067-get-bookings-id` | 067 GET /bookings/:id | Borrador | 1 | 3 | 3 | 0 | 0 | 0 |
| `068-get-bookings-by-folio-folio` | 068 GET /bookings/by-folio/:folio | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `069-post-bookings-id-cancel` | 069 POST /bookings/:id/cancel | Borrador | 4 | 3 | 3 | 0 | 0 | 1 |
| `070-post-bookings-id-payments` | 070 POST /bookings/:id/payments | Borrador | 4 | 4 | 3 | 0 | 0 | 1 |
| `071-get-bookings-id-payments` | 071 GET /bookings/:id/payments | Borrador | 1 | 3 | 3 | 0 | 0 | 0 |
| `072-get-venues-id-reviews` | 072 GET /venues/:id/reviews | Borrador | 1 | 1 | 3 | 0 | 0 | 0 |
| `073-get-providers-id-reviews` | 073 GET /providers/:id/reviews | Borrador | 1 | 1 | 3 | 0 | 0 | 0 |
| `074-post-reviews` | 074 POST /reviews | Borrador | 4 | 3 | 3 | 0 | 0 | 0 |
| `075-post-reviews-id-reply` | 075 POST /reviews/:id/reply | Borrador | 1 | 4 | 3 | 0 | 0 | 0 |
| `076-patch-reviews-id` | 076 PATCH /reviews/:id | Borrador; **requiere decisión** | 1 | 4 | 3 | 0 | 3 | 1 |
| `077-delete-reviews-id` | 077 DELETE /reviews/:id | Borrador; **requiere decisión** | 1 | 3 | 3 | 0 | 3 | 1 |
| `078-get-venues-id-stats` | 078 GET /venues/:id/stats | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `079-get-providers-id-stats` | 079 GET /providers/:id/stats | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `080-get-stats-overview` | 080 GET /stats/overview | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `081-get-favorites` | 081 GET /favorites | Borrador | 1 | 2 | 3 | 0 | 0 | 0 |
| `082-post-favorites` | 082 POST /favorites | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `083-delete-favorites-targettype-targetid` | 083 DELETE /favorites/:targetType/:targetId | Borrador | 1 | 3 | 3 | 0 | 0 | 1 |
| `084-get-conversations` | 084 GET /conversations | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `085-post-conversations` | 085 POST /conversations | Borrador | 3 | 3 | 3 | 0 | 0 | 0 |
| `086-get-conversations-id` | 086 GET /conversations/:id | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `087-get-conversations-id-messages` | 087 GET /conversations/:id/messages | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `088-post-conversations-id-messages` | 088 POST /conversations/:id/messages | Borrador | 2 | 4 | 3 | 0 | 0 | 1 |
| `089-patch-conversations-id-read` | 089 PATCH /conversations/:id/read | Borrador | 2 | 3 | 3 | 0 | 0 | 0 |
| `090-get-notices` | 090 GET /notices | Borrador | 2 | 2 | 3 | 0 | 0 | 1 |
| `091-patch-notices-id-read` | 091 PATCH /notices/:id/read | Borrador | 1 | 3 | 3 | 0 | 0 | 0 |
| `092-post-notices-read-all` | 092 POST /notices/read-all | Borrador | 1 | 2 | 3 | 0 | 0 | 0 |
| `093-get-assistant-messages` | 093 GET /assistant/messages | Borrador | 2 | 2 | 3 | 0 | 0 | 0 |
| `094-post-assistant-messages` | 094 POST /assistant/messages | Borrador | 2 | 3 | 3 | 0 | 0 | 2 |
| `095-delete-assistant-messages` | 095 DELETE /assistant/messages | Borrador; **requiere decisión** | 1 | 2 | 3 | 0 | 3 | 0 |
| `096-post-contact` | 096 POST /contact | Borrador | 2 | 2 | 3 | 0 | 0 | 1 |
| `097-get-health-ready` | 097 GET /health/ready | Borrador | 3 | 1 | 3 | 0 | 0 | 1 |
| `900-infraestructura-iac` | 900 Infraestructura como código (IaC) | Borrador | 12 | 9 | 11 | 0 | 0 | 3 |
| `901-pipeline-cicd` | 901 Pipeline CI/CD con DevSecOps | Borrador | 12 | 10 | 17 | 0 | 0 | 3 |

### 3.3 Base de datos

| Spec | Título | Estado | RF | CA | Tareas | Hechas | Bloqueadas | Preguntas |
|---|---|---|---|---|---|---|---|---|
| `000-base-datos` | 000 Base de datos MongoDB | Implementado; pendiente de revisión del equipo | 8 | 5 | 7 | 2 | 0 | 2 |
| `001-contrato-con-api` | 001 Contrato de datos con la API | Borrador | 7 | 5 | 6 | 0 | 0 | 5 |
| `002-cuentas` | 002 Cuentas y acceso | Implementado; pendiente de revisión | 5 | 4 | 5 | 4 | 0 | 0 |
| `003-salones` | 003 Salones y disponibilidad | Implementado; pendiente de revisión | 3 | 2 | 3 | 2 | 0 | 0 |
| `004-proveedores` | 004 Proveedores y agenda | Implementado; pendiente de revisión | 3 | 2 | 3 | 2 | 0 | 0 |
| `005-planeacion` | 005 Planeación de eventos | Implementado; pendiente de revisión | 5 | 4 | 5 | 4 | 0 | 0 |
| `006-cotizaciones` | 006 Cotizaciones | Implementado; pendiente de revisión | 2 | 1 | 2 | 1 | 0 | 0 |
| `007-reservaciones` | 007 Reservaciones y folios | Implementado; pendiente de revisión | 3 | 2 | 3 | 2 | 0 | 0 |
| `008-reputacion` | 008 Reseñas y estadísticas | Implementado; pendiente de revisión | 3 | 2 | 3 | 2 | 0 | 0 |
| `009-interaccion` | 009 Conversaciones y avisos | Implementado; pendiente de revisión | 4 | 3 | 4 | 3 | 0 | 0 |
| `010-soporte` | 010 Asistente y contacto | Implementado; pendiente de revisión | 3 | 2 | 3 | 2 | 0 | 0 |

### 3.4 Móvil

| Spec | Título | Estado | RF | CA | Tareas | Hechas | Bloqueadas | Preguntas |
|---|---|---|---|---|---|---|---|---|
| `000-arquitectura-base` | 000 Arquitectura base del Móvil | Aprobado | 11 | 10 | 39 | 12 | 1 | 2 |
| `001-inicio-visitante` | 001 Inicio para visitantes | Borrador | 11 | 4 | 5 | 0 | 0 | 0 |
| `002-registro` | 002 Registro | Borrador | 9 | 4 | 5 | 0 | 0 | 0 |
| `003-iniciar-sesion` | 003 Iniciar y cerrar sesión | Borrador | 4 | 3 | 4 | 0 | 0 | 0 |
| `004-recuperar-contrasena` | 004 Recuperar contraseña | Borrador | 5 | 4 | 5 | 0 | 0 | 0 |
| `005-explorar-salones` | 005 Explorar salones | Borrador | 23 | 6 | 7 | 0 | 0 | 0 |
| `006-detalle-salon` | 006 Detalle del salón | Borrador | 18 | 5 | 6 | 0 | 0 | 0 |
| `007-crear-evento` | 007 Crear evento | Borrador | 5 | 3 | 4 | 0 | 0 | 0 |
| `008-cotizaciones-cliente` | 008 Cotizaciones del cliente | Borrador | 11 | 5 | 6 | 0 | 0 | 0 |
| `009-reservar-pagar` | 009 Reservar y pagar | Borrador | 10 | 4 | 5 | 0 | 0 | 0 |
| `010-reservaciones` | 010 Mis reservaciones | Borrador | 5 | 4 | 5 | 0 | 0 | 0 |
| `011-proveedores-cliente` | 011 Proveedores | Borrador | 12 | 5 | 6 | 0 | 0 | 0 |
| `012-ia-studio` | 012 IA Studio | Borrador | 15 | 6 | 7 | 0 | 0 | 0 |
| `013-asistente-ia` | 013 Asistente IA | Borrador | 3 | 3 | 4 | 0 | 0 | 0 |
| `014-mi-evento` | 014 Mi Evento | Borrador | 9 | 3 | 4 | 0 | 0 | 0 |
| `015-presupuesto` | 015 Presupuesto | Borrador | 5 | 4 | 5 | 0 | 0 | 0 |
| `016-invitados` | 016 Invitados e invitación | Borrador | 9 | 4 | 5 | 0 | 0 | 0 |
| `017-pendientes` | 017 Pendientes y recordatorios | Borrador | 8 | 5 | 6 | 0 | 0 | 0 |
| `018-pagos-evento` | 018 Pagos del evento | Borrador | 3 | 3 | 4 | 0 | 0 | 0 |
| `019-compartir-calificar` | 019 Compartir y calificar | Borrador | 5 | 4 | 5 | 0 | 0 | 0 |
| `020-comunicacion` | 020 Mensajes y notificaciones | Borrador | 6 | 5 | 6 | 0 | 0 | 0 |
| `021-favoritos-perfil` | 021 Favoritos, perfil y ayuda | Borrador | 11 | 5 | 6 | 0 | 0 | 0 |
| `022-propietario-registrar-salon` | 022 Registrar un salón | Borrador | 13 | 5 | 6 | 0 | 0 | 0 |
| `023-propietario-inicio-calendario` | 023 Inicio y calendario | Borrador | 13 | 5 | 6 | 0 | 0 | 0 |
| `024-propietario-solicitudes` | 024 Solicitudes y mensajes | Borrador | 8 | 5 | 6 | 0 | 0 | 0 |
| `025-propietario-herramientas` | 025 Herramientas del propietario | Borrador | 17 | 9 | 10 | 0 | 0 | 0 |
| `026-proveedor-perfil` | 026 Perfil del proveedor | Borrador | 12 | 5 | 6 | 0 | 0 | 0 |
| `027-proveedor-inicio-agenda` | 027 Inicio y agenda | Borrador | 8 | 4 | 5 | 0 | 0 | 0 |
| `028-proveedor-solicitudes` | 028 Solicitudes y mensajes | Borrador | 5 | 5 | 6 | 0 | 0 | 0 |
| `029-proveedor-resenas-estadisticas` | 029 Reseñas y estadísticas | Borrador | 3 | 4 | 5 | 0 | 0 | 0 |

## 4. Endpoint por endpoint

### 4.1 Tabla general

| Método | Ruta | Auth | Spec | Colecciones | Éxito | Flujos de Móvil | Estado |
|---|---|---|---|---|---|---|---|
| `GET` | `/health` | No | `000-arquitectura-base` | — | 200 | — | ✅ Programado (PR #5) |
| `GET` | `/openapi/v1.json` | No | `000-arquitectura-base` | — | 200 | — | ✅ Programado (PR #5) |
| `POST` | `/api/v1/auth/login` | No | `003-auth-login` | `users`, `sessions` | 200 | `003` | ✅ Programado (PR #5) |
| `POST` | `/api/v1/auth/logout` | Sí | `004-auth-logout` | `sessions` | 204 | `003` | ✅ Programado (PR #5) |
| `POST` | `/api/v1/auth/register` | No | `005-auth-register` | `users` | 201 | `002` | ✅ Programado (PR #5) |
| `GET` | `/api/v1/users/me` | Sí | `006-users-me` | `users` | 200 | `021` | ✅ Programado (PR #5) |
| `GET` | `/api/v1/venues` | No | `007-venues-catalog` | `venues` | 200 | `001`, `005` | ✅ Programado (PR #5) |
| `POST` | `/api/v1/quotes` | Sí | `008-quotes-create` | `quotes` | 201 | `008` | ✅ Programado (PR #5) |
| `POST` | `/api/v1/bookings` | Sí | `009-bookings-create` | `bookings`, `counters` | 201 | `009` | ✅ Programado (PR #5) |
| `POST` | `/api/v1/auth/password/forgot` | No | `011-post-auth-password-forgot` | `passwordResets`, `users` | 202 | `004` | 📝 Solo spec |
| `POST` | `/api/v1/auth/password/reset` | No | `012-post-auth-password-reset` | `passwordResets`, `users` | 200 | `004` | 📝 Solo spec |
| `POST` | `/api/v1/auth/refresh` | Sesión | `013-post-auth-refresh` | `sessions` | 200 | `003` | 📝 Solo spec |
| `POST` | `/api/v1/auth/email/verify` | No | `014-post-auth-email-verify` | `users` | 200 | `002` | ⛔ Requiere decisión |
| `PATCH` | `/api/v1/users/me` | Sí | `015-patch-users-me` | `users` | 200 | `021` | 📝 Solo spec |
| `PATCH` | `/api/v1/users/me/password` | Sí | `016-patch-users-me-password` | `users` | 204 | `021` | 📝 Solo spec |
| `GET` | `/api/v1/users/:id/public` | Opcional | `017-get-users-id-public` | `users` | 200 | `021` | 📝 Solo spec |
| `DELETE` | `/api/v1/users/me` | Sí | `018-delete-users-me` | `users` | 204 | `021` | ⛔ Requiere decisión |
| `GET` | `/api/v1/payment-cards` | Sí | `019-get-payment-cards` | `paymentCards` | 200 | `009`, `021` | 📝 Solo spec |
| `POST` | `/api/v1/payment-cards` | Sí | `020-post-payment-cards` | `paymentCards` | 201 | `009` | 📝 Solo spec |
| `PATCH` | `/api/v1/payment-cards/:id` | Sí | `021-patch-payment-cards-id` | `paymentCards` | 200 | `021` | 📝 Solo spec |
| `DELETE` | `/api/v1/payment-cards/:id` | Sí | `022-delete-payment-cards-id` | `paymentCards` | 204 | `021` | 📝 Solo spec |
| `POST` | `/api/v1/payment-cards/:id/default` | Sí | `023-post-payment-cards-id-default` | `paymentCards` | 200 | `021` | 📝 Solo spec |
| `GET` | `/api/v1/venues/:id` | No | `024-get-venues-id` | `venues` | 200 | `001`, `006` | 📝 Solo spec |
| `GET` | `/api/v1/venues/mine` | Sí | `025-get-venues-mine` | `venues` | 200 | `025` | 📝 Solo spec |
| `POST` | `/api/v1/venues` | Sí | `026-post-venues` | `venues` | 201 | `022` | 📝 Solo spec |
| `PATCH` | `/api/v1/venues/:id` | Sí | `027-patch-venues-id` | `venues` | 200 | `022`, `025` | 📝 Solo spec |
| `POST` | `/api/v1/venues/:id/submit` | Sí | `028-post-venues-id-submit` | `venues` | 200 | `022` | ⛔ Requiere decisión |
| `POST` | `/api/v1/venues/:id/publish` | Sí | `029-post-venues-id-publish` | `venues` | 200 | `022` | ⛔ Requiere decisión |
| `DELETE` | `/api/v1/venues/:id` | Sí | `030-delete-venues-id` | `venues` | 204 | `025` | ⛔ Requiere decisión |
| `GET` | `/api/v1/venues/:id/availability` | No | `031-get-venues-id-availability` | `venueDates` | 200 | `006`, `023` | 📝 Solo spec |
| `POST` | `/api/v1/venues/:id/blocks` | Sí | `032-post-venues-id-blocks` | `venueDates` | 201 | `023` | 📝 Solo spec |
| `DELETE` | `/api/v1/venues/:id/blocks/:blockId` | Sí | `033-delete-venues-id-blocks-blockid` | `venueDates` | 204 | `023` | 📝 Solo spec |
| `GET` | `/api/v1/providers` | No | `034-get-providers` | `providers` | 200 | `011` | 📝 Solo spec |
| `GET` | `/api/v1/providers/:id` | No | `035-get-providers-id` | `providers` | 200 | `011` | 📝 Solo spec |
| `GET` | `/api/v1/providers/me` | Sí | `036-get-providers-me` | `providers`, `users` | 200 | `026` | 📝 Solo spec |
| `POST` | `/api/v1/providers` | Sí | `037-post-providers` | `providers`, `users` | 201 | `026` | 📝 Solo spec |
| `PATCH` | `/api/v1/providers/me` | Sí | `038-patch-providers-me` | `providers`, `users` | 200 | `026` | 📝 Solo spec |
| `GET` | `/api/v1/providers/me/gigs` | Sí | `039-get-providers-me-gigs` | `gigs` | 200 | `027` | 📝 Solo spec |
| `POST` | `/api/v1/providers/me/gigs` | Sí | `040-post-providers-me-gigs` | `gigs` | 201 | `027` | 📝 Solo spec |
| `PATCH` | `/api/v1/providers/me/gigs/:id` | Sí | `041-patch-providers-me-gigs-id` | `gigs` | 200 | `027` | 📝 Solo spec |
| `DELETE` | `/api/v1/providers/me/gigs/:id` | Sí | `042-delete-providers-me-gigs-id` | `gigs` | 204 | `027` | ⛔ Requiere decisión |
| `GET` | `/api/v1/events` | Sí | `043-get-events` | `events` | 200 | `014` | 📝 Solo spec |
| `POST` | `/api/v1/events` | Sí | `044-post-events` | `events` | 201 | `007` | 📝 Solo spec |
| `GET` | `/api/v1/events/:id` | Sí | `045-get-events-id` | `events` | 200 | `014`, `015` | 📝 Solo spec |
| `PATCH` | `/api/v1/events/:id` | Sí | `046-patch-events-id` | `events` | 200 | `012`, `014`, `015`, `016`, `017`, `018` | 📝 Solo spec |
| `DELETE` | `/api/v1/events/:id` | Sí | `047-delete-events-id` | `events` | 204 | `014` | ⛔ Requiere decisión |
| `GET` | `/api/v1/events/:id/guests` | Sí | `048-get-events-id-guests` | `eventGuests` | 200 | `016` | 📝 Solo spec |
| `POST` | `/api/v1/events/:id/guests` | Sí | `049-post-events-id-guests` | `eventGuests` | 201 | `016` | 📝 Solo spec |
| `PATCH` | `/api/v1/events/:id/guests/:guestId` | Sí | `050-patch-events-id-guests-guestid` | `eventGuests` | 200 | `016` | 📝 Solo spec |
| `DELETE` | `/api/v1/events/:id/guests/:guestId` | Sí | `051-delete-events-id-guests-guestid` | `eventGuests` | 204 | `016` | 📝 Solo spec |
| `GET` | `/api/v1/events/:id/tasks` | Sí | `052-get-events-id-tasks` | `eventTasks` | 200 | `017` | 📝 Solo spec |
| `POST` | `/api/v1/events/:id/tasks` | Sí | `053-post-events-id-tasks` | `eventTasks` | 201 | `017` | 📝 Solo spec |
| `PATCH` | `/api/v1/events/:id/tasks/:taskId` | Sí | `054-patch-events-id-tasks-taskid` | `eventTasks` | 200 | `017` | 📝 Solo spec |
| `DELETE` | `/api/v1/events/:id/tasks/:taskId` | Sí | `055-delete-events-id-tasks-taskid` | `eventTasks` | 204 | `017` | 📝 Solo spec |
| `GET` | `/api/v1/events/:id/collaborators` | Sí | `056-get-events-id-collaborators` | `events` | 200 | `019` | 📝 Solo spec |
| `POST` | `/api/v1/events/:id/collaborators` | Sí | `057-post-events-id-collaborators` | `events` | 201 | `019` | 📝 Solo spec |
| `PATCH` | `/api/v1/events/:id/collaborators/:userId` | Sí | `058-patch-events-id-collaborators-userid` | `events` | 200 | `019` | 📝 Solo spec |
| `DELETE` | `/api/v1/events/:id/collaborators/:userId` | Sí | `059-delete-events-id-collaborators-userid` | `events` | 204 | `019` | 📝 Solo spec |
| `GET` | `/api/v1/quotes` | Sí | `060-get-quotes` | `quotes` | 200 | `008`, `024`, `028` | 📝 Solo spec |
| `GET` | `/api/v1/quotes/:id` | Sí | `061-get-quotes-id` | `quotes` | 200 | `008`, `024`, `028` | 📝 Solo spec |
| `POST` | `/api/v1/quotes/:id/response` | Sí | `062-post-quotes-id-response` | `quotes` | 200 | `024`, `028` | 📝 Solo spec |
| `POST` | `/api/v1/quotes/:id/accept` | Sí | `063-post-quotes-id-accept` | `quotes` | 200 | `008` | 📝 Solo spec |
| `POST` | `/api/v1/quotes/:id/reject` | Sí | `064-post-quotes-id-reject` | `quotes` | 200 | `008` | 📝 Solo spec |
| `POST` | `/api/v1/quotes/:id/expire` | Interno | `065-post-quotes-id-expire` | `quotes` | 200 | — | 📝 Solo spec |
| `GET` | `/api/v1/bookings` | Sí | `066-get-bookings` | `bookings` | 200 | `010`, `023`, `025` | 📝 Solo spec |
| `GET` | `/api/v1/bookings/:id` | Sí | `067-get-bookings-id` | `bookings` | 200 | `009`, `010` | 📝 Solo spec |
| `GET` | `/api/v1/bookings/by-folio/:folio` | Sí | `068-get-bookings-by-folio-folio` | `bookings` | 200 | `010` | 📝 Solo spec |
| `POST` | `/api/v1/bookings/:id/cancel` | Sí | `069-post-bookings-id-cancel` | `bookings`, `venueDates` | 200 | `010` | 📝 Solo spec |
| `POST` | `/api/v1/bookings/:id/payments` | Sí | `070-post-bookings-id-payments` | `bookings`, `venueDates`, `counters` | 201 | `009` | 📝 Solo spec |
| `GET` | `/api/v1/bookings/:id/payments` | Sí | `071-get-bookings-id-payments` | `bookings`, `venueDates`, `counters` | 200 | `018` | 📝 Solo spec |
| `GET` | `/api/v1/venues/:id/reviews` | No | `072-get-venues-id-reviews` | `reviews`, `venues`, `providers` | 200 | `006`, `025` | 📝 Solo spec |
| `GET` | `/api/v1/providers/:id/reviews` | No | `073-get-providers-id-reviews` | `reviews`, `venues`, `providers` | 200 | `011`, `029` | 📝 Solo spec |
| `POST` | `/api/v1/reviews` | Sí | `074-post-reviews` | `reviews`, `venues`, `providers` | 201 | `019` | 📝 Solo spec |
| `POST` | `/api/v1/reviews/:id/reply` | Sí | `075-post-reviews-id-reply` | `reviews`, `venues`, `providers` | 200 | `025`, `029` | 📝 Solo spec |
| `PATCH` | `/api/v1/reviews/:id` | Sí | `076-patch-reviews-id` | `reviews`, `venues`, `providers` | 200 | `019` | ⛔ Requiere decisión |
| `DELETE` | `/api/v1/reviews/:id` | Sí | `077-delete-reviews-id` | `reviews`, `venues`, `providers` | 204 | `019` | ⛔ Requiere decisión |
| `GET` | `/api/v1/venues/:id/stats` | Sí | `078-get-venues-id-stats` | `monthlyStats` | 200 | `025` | 📝 Solo spec |
| `GET` | `/api/v1/providers/:id/stats` | Sí | `079-get-providers-id-stats` | `monthlyStats` | 200 | `029` | 📝 Solo spec |
| `GET` | `/api/v1/stats/overview` | Sí | `080-get-stats-overview` | `monthlyStats` | 200 | `023`, `027` | 📝 Solo spec |
| `GET` | `/api/v1/favorites` | Sí | `081-get-favorites` | `favorites` | 200 | `021` | 📝 Solo spec |
| `POST` | `/api/v1/favorites` | Sí | `082-post-favorites` | `favorites` | 201 | `005`, `006` | 📝 Solo spec |
| `DELETE` | `/api/v1/favorites/:targetType/:targetId` | Sí | `083-delete-favorites-targettype-targetid` | `favorites` | 204 | `005`, `006` | 📝 Solo spec |
| `GET` | `/api/v1/conversations` | Sí | `084-get-conversations` | `conversations`, `messages` | 200 | `020`, `024`, `028` | 📝 Solo spec |
| `POST` | `/api/v1/conversations` | Sí | `085-post-conversations` | `conversations`, `messages` | 201 | `006`, `020` | 📝 Solo spec |
| `GET` | `/api/v1/conversations/:id` | Sí | `086-get-conversations-id` | `conversations`, `messages` | 200 | `020` | 📝 Solo spec |
| `GET` | `/api/v1/conversations/:id/messages` | Sí | `087-get-conversations-id-messages` | `conversations`, `messages` | 200 | `020`, `024`, `028` | 📝 Solo spec |
| `POST` | `/api/v1/conversations/:id/messages` | Sí | `088-post-conversations-id-messages` | `conversations`, `messages` | 201 | `020`, `024`, `028` | 📝 Solo spec |
| `PATCH` | `/api/v1/conversations/:id/read` | Sí | `089-patch-conversations-id-read` | `conversations`, `messages` | 200 | `020` | 📝 Solo spec |
| `GET` | `/api/v1/notices` | Sí | `090-get-notices` | `notices` | 200 | `005`, `020` | 📝 Solo spec |
| `PATCH` | `/api/v1/notices/:id/read` | Sí | `091-patch-notices-id-read` | `notices` | 200 | `020` | 📝 Solo spec |
| `POST` | `/api/v1/notices/read-all` | Sí | `092-post-notices-read-all` | `notices` | 200 | `020` | 📝 Solo spec |
| `GET` | `/api/v1/assistant/messages` | Sí | `093-get-assistant-messages` | `assistantMessages` | 200 | `013` | 📝 Solo spec |
| `POST` | `/api/v1/assistant/messages` | Sí | `094-post-assistant-messages` | `assistantMessages` | 201 | `013` | 📝 Solo spec |
| `DELETE` | `/api/v1/assistant/messages` | Sí | `095-delete-assistant-messages` | `assistantMessages` | 204 | `013` | ⛔ Requiere decisión |
| `POST` | `/api/v1/contact` | Opcional | `096-post-contact` | `contactMessages` | 201 | `021` | 📝 Solo spec |
| `GET` | `/api/v1/health/ready` | No | `097-get-health-ready` | — | 200 | — | 📝 Solo spec |

### 4.2 Detalle

#### `POST /api/v1/auth/password/forgot`

- **Spec:** `011-post-auth-password-forgot` · **Auth:** No · **Éxito:** 202 · **Errores:** 400, 429, 500
- **Colecciones:** `passwordResets`, `users`
- **Móvil:** `004`
- **Estado:** 📝 Solo spec
- **Reglas:** La respuesta no revela si el correo existe. El código vale 15 minutos y es de un solo uso. Solo se guarda su hash.
- **Pendiente:** No hay mecanismo de envío de correo aprobado; `API-HANDOFF.md` prohíbe inventarlo. `passwordResets`: `tokenHash` y `used` o `codeHash` y `attempts` (`VirGo-Database` `001` P-03).

#### `POST /api/v1/auth/password/reset`

- **Spec:** `012-post-auth-password-reset` · **Auth:** No · **Éxito:** 200 · **Errores:** 400, 429, 500
- **Colecciones:** `passwordResets`, `users`
- **Móvil:** `004`
- **Estado:** 📝 Solo spec
- **Reglas:** El código vale 15 minutos y es de un solo uso. Solo se guarda su hash. Código incorrecto o vencido: 400 con mensaje genérico.
- **Pendiente:** `passwordResets`: `tokenHash` y `used` o `codeHash` y `attempts` (`VirGo-Database` `001` P-03).

#### `POST /api/v1/auth/refresh`

- **Spec:** `013-post-auth-refresh` · **Auth:** Sesión · **Éxito:** 200 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `sessions`
- **Móvil:** `003`
- **Estado:** 📝 Solo spec
- **Reglas:** Rechaza sesiones revocadas o expiradas. Rota el token de sesión en cada uso.

#### `POST /api/v1/auth/email/verify`

- **Spec:** `014-post-auth-email-verify` · **Auth:** No · **Éxito:** 200 · **Errores:** 429, 500
- **Colecciones:** `users`
- **Móvil:** `002`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Actualiza `users.isVerified`.
- **Pendiente:** `API-HANDOFF.md` lo marca «si se aprueba»: falta decidir si la verificación es obligatoria y cómo se envía el correo.

#### `PATCH /api/v1/users/me`

- **Spec:** `015-patch-users-me` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `users`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** No permite cambiar `role`, `providerId`, `isVerified` ni `createdAt`.

#### `PATCH /api/v1/users/me/password`

- **Spec:** `016-patch-users-me-password` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `users`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/users/:id/public`

- **Spec:** `017-get-users-id-public` · **Auth:** Opcional · **Éxito:** 200 · **Errores:** 404, 429, 500
- **Colecciones:** `users`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** El perfil público no expone datos privados.

#### `DELETE /api/v1/users/me`

- **Spec:** `018-delete-users-me` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 429, 500
- **Colecciones:** `users`
- **Móvil:** `021`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Sin reglas adicionales a las del Art. IV.
- **Pendiente:** «Borrar cuenta y datos» (C39): baja lógica o anonimización.

#### `GET /api/v1/payment-cards`

- **Spec:** `019-get-payment-cards` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `paymentCards`
- **Móvil:** `009`, `021`
- **Estado:** 📝 Solo spec
- **Reglas:** Nunca devuelve `gatewayToken`.

#### `POST /api/v1/payment-cards`

- **Spec:** `020-post-payment-cards` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `paymentCards`
- **Móvil:** `009`
- **Estado:** 📝 Solo spec
- **Reglas:** La API nunca recibe ni guarda el número completo ni el CVV. Nunca devuelve `gatewayToken`.
- **Pendiente:** La pasarela de pago no está definida.

#### `PATCH /api/v1/payment-cards/:id`

- **Spec:** `021-patch-payment-cards-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `paymentCards`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** Nunca devuelve `gatewayToken`.

#### `DELETE /api/v1/payment-cards/:id`

- **Spec:** `022-delete-payment-cards-id` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `paymentCards`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `POST /api/v1/payment-cards/:id/default`

- **Spec:** `023-post-payment-cards-id-default` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `paymentCards`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** Nunca devuelve `gatewayToken`. Solo una tarjeta predeterminada por persona.

#### `GET /api/v1/venues/:id`

- **Spec:** `024-get-venues-id` · **Auth:** No · **Éxito:** 200 · **Errores:** 404, 429, 500
- **Colecciones:** `venues`
- **Móvil:** `001`, `006`
- **Estado:** 📝 Solo spec
- **Reglas:** Un salón sin publicar responde 404 a quien no es su dueño. Sin cuenta se ocultan precios y fechas (A3).
- **Pendiente:** ¿Dónde se calcula el precio de fin de semana y temporada alta: API o app?

#### `GET /api/v1/venues/mine`

- **Spec:** `025-get-venues-mine` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `venues`
- **Móvil:** `025`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el rol `owner` y solo sobre sus salones.

#### `POST /api/v1/venues`

- **Spec:** `026-post-venues` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `venues`
- **Móvil:** `022`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el rol `owner` y solo sobre sus salones. Ignora `rating`, `verified`, `createdAt` y `updatedAt` enviados por el cliente.
- **Pendiente:** No hay endpoint de carga de archivos para fotos y planos.

#### `PATCH /api/v1/venues/:id`

- **Spec:** `027-patch-venues-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `venues`
- **Móvil:** `022`, `025`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el rol `owner` y solo sobre sus salones. Ignora `rating`, `verified`, `createdAt` y `updatedAt` enviados por el cliente.
- **Pendiente:** No hay endpoint de carga de archivos para fotos y planos.

#### `POST /api/v1/venues/:id/submit`

- **Spec:** `028-post-venues-id-submit` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `venues`
- **Móvil:** `022`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Solo el rol `owner` y solo sobre sus salones.
- **Pendiente:** ¿Quién aprueba la publicación? No existe rol de administrador.

#### `POST /api/v1/venues/:id/publish`

- **Spec:** `029-post-venues-id-publish` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `venues`
- **Móvil:** `022`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Solo el rol `owner` y solo sobre sus salones.
- **Pendiente:** ¿Quién aprueba la publicación? No existe rol de administrador.

#### `DELETE /api/v1/venues/:id`

- **Spec:** `030-delete-venues-id` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `venues`
- **Móvil:** `025`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Solo el rol `owner` y solo sobre sus salones.
- **Pendiente:** No hay campo para baja lógica.

#### `GET /api/v1/venues/:id/availability`

- **Spec:** `031-get-venues-id-availability` · **Auth:** No · **Éxito:** 200 · **Errores:** 404, 429, 500
- **Colecciones:** `venueDates`
- **Móvil:** `006`, `023`
- **Estado:** 📝 Solo spec
- **Reglas:** No expone notas ni `bookingId` a personas no autorizadas.

#### `POST /api/v1/venues/:id/blocks`

- **Spec:** `032-post-venues-id-blocks` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `venueDates`
- **Móvil:** `023`
- **Estado:** 📝 Solo spec
- **Reglas:** El bloqueo crea `kind: blocked` y no admite `bookingId`. Un día ya ocupado responde 409.

#### `DELETE /api/v1/venues/:id/blocks/:blockId`

- **Spec:** `033-delete-venues-id-blocks-blockid` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `venueDates`
- **Móvil:** `023`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/providers`

- **Spec:** `034-get-providers` · **Auth:** No · **Éxito:** 200 · **Errores:** 429, 500
- **Colecciones:** `providers`
- **Móvil:** `011`
- **Estado:** 📝 Solo spec
- **Reglas:** Filtros: `category`, `zone`, `minPrice`, `maxPrice`, `rating`, `q`, `cursor`, `limit`. La comparación (B18) se resuelve en la app.
- **Pendiente:** Categorías: la maqueta usa Catering, Música, Fotografía, Decoración y Mobiliario; la base tiene 9 distintas.

#### `GET /api/v1/providers/:id`

- **Spec:** `035-get-providers-id` · **Auth:** No · **Éxito:** 200 · **Errores:** 404, 429, 500
- **Colecciones:** `providers`
- **Móvil:** `011`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/providers/me`

- **Spec:** `036-get-providers-me` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `providers`, `users`
- **Móvil:** `026`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el rol `provider`.

#### `POST /api/v1/providers`

- **Spec:** `037-post-providers` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `providers`, `users`
- **Móvil:** `026`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el rol `provider`. Un usuario tiene un solo perfil: el segundo responde 409. Crear el perfil actualiza `users.providerId` de forma consistente.
- **Pendiente:** `basePrice` o `pricing` y `responseTime` (`VirGo-Database` `001` P-04).

#### `PATCH /api/v1/providers/me`

- **Spec:** `038-patch-providers-me` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `providers`, `users`
- **Móvil:** `026`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el rol `provider`.
- **Pendiente:** `basePrice` o `pricing` y `responseTime` (`VirGo-Database` `001` P-04).

#### `GET /api/v1/providers/me/gigs`

- **Spec:** `039-get-providers-me-gigs` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `gigs`
- **Móvil:** `027`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo sobre la agenda propia.

#### `POST /api/v1/providers/me/gigs`

- **Spec:** `040-post-providers-me-gigs` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `gigs`
- **Móvil:** `027`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo sobre la agenda propia. Avisa de empalmes en la semana (H74).

#### `PATCH /api/v1/providers/me/gigs/:id`

- **Spec:** `041-patch-providers-me-gigs-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `gigs`
- **Móvil:** `027`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo sobre la agenda propia. Avisa de empalmes en la semana (H74).

#### `DELETE /api/v1/providers/me/gigs/:id`

- **Spec:** `042-delete-providers-me-gigs-id` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `gigs`
- **Móvil:** `027`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Solo sobre la agenda propia.
- **Pendiente:** ¿Se puede eliminar un servicio ligado a una reservación?

#### `GET /api/v1/events`

- **Spec:** `043-get-events` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `events`
- **Móvil:** `014`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `POST /api/v1/events`

- **Spec:** `044-post-events` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `events`
- **Móvil:** `007`
- **Estado:** 📝 Solo spec
- **Reglas:** `venue` se guarda como copia validada contra un salón publicado. `budgetPlan`, `reminders`, `vendors`, `externalPayments`, `design` e `invitation` viajan dentro del evento.

#### `GET /api/v1/events/:id`

- **Spec:** `045-get-events-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `events`
- **Móvil:** `014`, `015`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `PATCH /api/v1/events/:id`

- **Spec:** `046-patch-events-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `events`
- **Móvil:** `012`, `014`, `015`, `016`, `017`, `018`
- **Estado:** 📝 Solo spec
- **Reglas:** `venue` se guarda como copia validada contra un salón publicado. `budgetPlan`, `reminders`, `vendors`, `externalPayments`, `design` e `invitation` viajan dentro del evento.
- **Pendiente:** ¿Sub-recursos propios o `PATCH` del evento para esas listas?

#### `DELETE /api/v1/events/:id`

- **Spec:** `047-delete-events-id` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `events`
- **Móvil:** `014`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Sin reglas adicionales a las del Art. IV.
- **Pendiente:** No hay campo para baja lógica.

#### `GET /api/v1/events/:id/guests`

- **Spec:** `048-get-events-id-guests` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `eventGuests`
- **Móvil:** `016`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento: `view` para leer y `edit` para escribir.

#### `POST /api/v1/events/:id/guests`

- **Spec:** `049-post-events-id-guests` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `eventGuests`
- **Móvil:** `016`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento: `view` para leer y `edit` para escribir.

#### `PATCH /api/v1/events/:id/guests/:guestId`

- **Spec:** `050-patch-events-id-guests-guestid` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `eventGuests`
- **Móvil:** `016`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento: `view` para leer y `edit` para escribir.

#### `DELETE /api/v1/events/:id/guests/:guestId`

- **Spec:** `051-delete-events-id-guests-guestid` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `eventGuests`
- **Móvil:** `016`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento: `view` para leer y `edit` para escribir.

#### `GET /api/v1/events/:id/tasks`

- **Spec:** `052-get-events-id-tasks` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `eventTasks`
- **Móvil:** `017`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento.

#### `POST /api/v1/events/:id/tasks`

- **Spec:** `053-post-events-id-tasks` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `eventTasks`
- **Móvil:** `017`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento.

#### `PATCH /api/v1/events/:id/tasks/:taskId`

- **Spec:** `054-patch-events-id-tasks-taskid` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `eventTasks`
- **Móvil:** `017`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento.

#### `DELETE /api/v1/events/:id/tasks/:taskId`

- **Spec:** `055-delete-events-id-tasks-taskid` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `eventTasks`
- **Móvil:** `017`
- **Estado:** 📝 Solo spec
- **Reglas:** Requiere permiso sobre el evento.

#### `GET /api/v1/events/:id/collaborators`

- **Spec:** `056-get-events-id-collaborators` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `events`
- **Móvil:** `019`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el dueño administra colaboradores.

#### `POST /api/v1/events/:id/collaborators`

- **Spec:** `057-post-events-id-collaborators` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `events`
- **Móvil:** `019`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el dueño administra colaboradores. Permisos: `view` o `edit`.

#### `PATCH /api/v1/events/:id/collaborators/:userId`

- **Spec:** `058-patch-events-id-collaborators-userid` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `events`
- **Móvil:** `019`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el dueño administra colaboradores. Permisos: `view` o `edit`.

#### `DELETE /api/v1/events/:id/collaborators/:userId`

- **Spec:** `059-delete-events-id-collaborators-userid` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `events`
- **Móvil:** `019`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el dueño administra colaboradores.

#### `GET /api/v1/quotes`

- **Spec:** `060-get-quotes` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `quotes`
- **Móvil:** `008`, `024`, `028`
- **Estado:** 📝 Solo spec
- **Reglas:** Cada persona solo ve las suyas o las dirigidas a su salón o perfil.

#### `GET /api/v1/quotes/:id`

- **Spec:** `061-get-quotes-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `quotes`
- **Móvil:** `008`, `024`, `028`
- **Estado:** 📝 Solo spec
- **Reglas:** Cada persona solo ve las suyas o las dirigidas a su salón o perfil.

#### `POST /api/v1/quotes/:id/response`

- **Spec:** `062-post-quotes-id-response` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `quotes`
- **Móvil:** `024`, `028`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el destino responde y solo el cliente acepta o rechaza. Un estado incompatible responde 409.

#### `POST /api/v1/quotes/:id/accept`

- **Spec:** `063-post-quotes-id-accept` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `quotes`
- **Móvil:** `008`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el destino responde y solo el cliente acepta o rechaza. Un estado incompatible responde 409. La aceptación usa `Idempotency-Key`. Vigencia de 15 días (C7).

#### `POST /api/v1/quotes/:id/reject`

- **Spec:** `064-post-quotes-id-reject` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `quotes`
- **Móvil:** `008`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el destino responde y solo el cliente acepta o rechaza. Un estado incompatible responde 409.

#### `POST /api/v1/quotes/:id/expire`

- **Spec:** `065-post-quotes-id-expire` · **Auth:** Interno · **Éxito:** 200 · **Errores:** 404, 429, 500
- **Colecciones:** `quotes`
- **Móvil:** —
- **Estado:** 📝 Solo spec
- **Reglas:** Vigencia de 15 días (C7).
- **Pendiente:** ¿Quién ejecuta el vencimiento? No hay un job definido.

#### `GET /api/v1/bookings`

- **Spec:** `066-get-bookings` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `bookings`
- **Móvil:** `010`, `023`, `025`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/bookings/:id`

- **Spec:** `067-get-bookings-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `bookings`
- **Móvil:** `009`, `010`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/bookings/by-folio/:folio`

- **Spec:** `068-get-bookings-by-folio-folio` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `bookings`
- **Móvil:** `010`
- **Estado:** 📝 Solo spec
- **Reglas:** La búsqueda por folio tiene límite de solicitudes.

#### `POST /api/v1/bookings/:id/cancel`

- **Spec:** `069-post-bookings-id-cancel` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `bookings`, `venueDates`
- **Móvil:** `010`
- **Estado:** 📝 Solo spec
- **Reglas:** Con 30 días o más se reembolsa el 50% del anticipo; con menos, nada. Elimina `venueDates` por `venueId`, `date` y `bookingId`; si no coincide, aborta. Es una transacción.
- **Pendiente:** Reprogramar (C33): `VirGo-Database` lo marca pendiente y no tiene endpoint.

#### `POST /api/v1/bookings/:id/payments`

- **Spec:** `070-post-bookings-id-payments` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `bookings`, `venueDates`, `counters`
- **Móvil:** `009`
- **Estado:** 📝 Solo spec
- **Reglas:** Usa `Idempotency-Key`. La fecha se aparta solo cuando el pago queda validado. Confirmar es una transacción: inserta `venueDates`, y si el día está ocupado responde 409.
- **Pendiente:** La pasarela y sus webhooks no están definidos; hoy el pago es simulado.

#### `GET /api/v1/bookings/:id/payments`

- **Spec:** `071-get-bookings-id-payments` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `bookings`, `venueDates`, `counters`
- **Móvil:** `018`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/venues/:id/reviews`

- **Spec:** `072-get-venues-id-reviews` · **Auth:** No · **Éxito:** 200 · **Errores:** 404, 429, 500
- **Colecciones:** `reviews`, `venues`, `providers`
- **Móvil:** `006`, `025`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/providers/:id/reviews`

- **Spec:** `073-get-providers-id-reviews` · **Auth:** No · **Éxito:** 200 · **Errores:** 404, 429, 500
- **Colecciones:** `reviews`, `venues`, `providers`
- **Móvil:** `011`, `029`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `POST /api/v1/reviews`

- **Spec:** `074-post-reviews` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `reviews`, `venues`, `providers`
- **Móvil:** `019`
- **Estado:** 📝 Solo spec
- **Reglas:** Una reseña por persona y destino. La reseña de un salón exige `bookingId`. Guardar la reseña actualiza `rating` en una transacción.

#### `POST /api/v1/reviews/:id/reply`

- **Spec:** `075-post-reviews-id-reply` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `reviews`, `venues`, `providers`
- **Móvil:** `025`, `029`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `PATCH /api/v1/reviews/:id`

- **Spec:** `076-patch-reviews-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `reviews`, `venues`, `providers`
- **Móvil:** `019`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Sin reglas adicionales a las del Art. IV.
- **Pendiente:** Editar y eliminar reseñas siguen sin decisión.

#### `DELETE /api/v1/reviews/:id`

- **Spec:** `077-delete-reviews-id` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `reviews`, `venues`, `providers`
- **Móvil:** `019`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Sin reglas adicionales a las del Art. IV.
- **Pendiente:** Editar y eliminar reseñas siguen sin decisión.

#### `GET /api/v1/venues/:id/stats`

- **Spec:** `078-get-venues-id-stats` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `monthlyStats`
- **Móvil:** `025`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el dueño las consulta. `visits` queda fuera del MVP.

#### `GET /api/v1/providers/:id/stats`

- **Spec:** `079-get-providers-id-stats` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `monthlyStats`
- **Móvil:** `029`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo el dueño las consulta. `visits` queda fuera del MVP.

#### `GET /api/v1/stats/overview`

- **Spec:** `080-get-stats-overview` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `monthlyStats`
- **Móvil:** `023`, `027`
- **Estado:** 📝 Solo spec
- **Reglas:** `visits` queda fuera del MVP.

#### `GET /api/v1/favorites`

- **Spec:** `081-get-favorites` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `favorites`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `POST /api/v1/favorites`

- **Spec:** `082-post-favorites` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `favorites`
- **Móvil:** `005`, `006`
- **Estado:** 📝 Solo spec
- **Reglas:** Un destino se guarda una sola vez. `name`, `photoUrl` y `rating` son copias.

#### `DELETE /api/v1/favorites/:targetType/:targetId`

- **Spec:** `083-delete-favorites-targettype-targetid` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `favorites`
- **Móvil:** `005`, `006`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.
- **Pendiente:** `API-HANDOFF.md` propone dos formas de borrar; aquí se elige por destino.

#### `GET /api/v1/conversations`

- **Spec:** `084-get-conversations` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `conversations`, `messages`
- **Móvil:** `020`, `024`, `028`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo participan el cliente y el destino.

#### `POST /api/v1/conversations`

- **Spec:** `085-post-conversations` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `conversations`, `messages`
- **Móvil:** `006`, `020`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo participan el cliente y el destino. Una conversación por cliente y destino.

#### `GET /api/v1/conversations/:id`

- **Spec:** `086-get-conversations-id` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `conversations`, `messages`
- **Móvil:** `020`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo participan el cliente y el destino.

#### `GET /api/v1/conversations/:id/messages`

- **Spec:** `087-get-conversations-id-messages` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `conversations`, `messages`
- **Móvil:** `020`, `024`, `028`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo participan el cliente y el destino.

#### `POST /api/v1/conversations/:id/messages`

- **Spec:** `088-post-conversations-id-messages` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 404, 429, 500
- **Colecciones:** `conversations`, `messages`
- **Móvil:** `020`, `024`, `028`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo participan el cliente y el destino.
- **Pendiente:** Mensajes en tiempo real: no definidos; por ahora, consulta periódica.

#### `PATCH /api/v1/conversations/:id/read`

- **Spec:** `089-patch-conversations-id-read` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `conversations`, `messages`
- **Móvil:** `020`
- **Estado:** 📝 Solo spec
- **Reglas:** Solo participan el cliente y el destino.

#### `GET /api/v1/notices`

- **Spec:** `090-get-notices` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `notices`
- **Móvil:** `005`, `020`
- **Estado:** 📝 Solo spec
- **Reglas:** Se borran solos a los 90 días.
- **Pendiente:** Notificaciones push: fuera de alcance.

#### `PATCH /api/v1/notices/:id/read`

- **Spec:** `091-patch-notices-id-read` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 404, 429, 500
- **Colecciones:** `notices`
- **Móvil:** `020`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `POST /api/v1/notices/read-all`

- **Spec:** `092-post-notices-read-all` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `notices`
- **Móvil:** `020`
- **Estado:** 📝 Solo spec
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `GET /api/v1/assistant/messages`

- **Spec:** `093-get-assistant-messages` · **Auth:** Sí · **Éxito:** 200 · **Errores:** 401, 429, 500
- **Colecciones:** `assistantMessages`
- **Móvil:** `013`
- **Estado:** 📝 Solo spec
- **Reglas:** El historial se borra solo a los 30 días.

#### `POST /api/v1/assistant/messages`

- **Spec:** `094-post-assistant-messages` · **Auth:** Sí · **Éxito:** 201 · **Errores:** 400, 401, 429, 500
- **Colecciones:** `assistantMessages`
- **Móvil:** `013`
- **Estado:** 📝 Solo spec
- **Reglas:** El historial se borra solo a los 30 días.
- **Pendiente:** El proveedor de IA no está definido; `API-HANDOFF.md` prohíbe inventarlo. Cómo mantener el costo en $0.

#### `DELETE /api/v1/assistant/messages`

- **Spec:** `095-delete-assistant-messages` · **Auth:** Sí · **Éxito:** 204 · **Errores:** 401, 429, 500
- **Colecciones:** `assistantMessages`
- **Móvil:** `013`
- **Estado:** ⛔ Requiere decisión
- **Reglas:** Sin reglas adicionales a las del Art. IV.

#### `POST /api/v1/contact`

- **Spec:** `096-post-contact` · **Auth:** Opcional · **Éxito:** 201 · **Errores:** 400, 429, 500
- **Colecciones:** `contactMessages`
- **Móvil:** `021`
- **Estado:** 📝 Solo spec
- **Reglas:** Tiene límite de solicitudes.
- **Pendiente:** Purga a los 12 meses, pendiente en `VirGo-Database`.

#### `GET /api/v1/health/ready`

- **Spec:** `097-get-health-ready` · **Auth:** No · **Éxito:** 200 · **Errores:** 429, 500
- **Colecciones:** ninguna
- **Móvil:** —
- **Estado:** 📝 Solo spec
- **Reglas:** No expone datos sensibles. Se usan en las pruebas de humo.
- **Pendiente:** `000` define `/health`; hay que decidir si se conserva o se reemplaza.


## 5. Base de datos

La fuente de verdad es `VirGo-Database/src/catalog`.

| Colección | Campos | Obligatorios | Índices | Spec de dominio |
|---|---|---|---|---|
| `users` | 10 | 9 | 1 | `002-cuentas` |
| `passwordResets` | 5 | 5 | 2 | `002-cuentas` |
| `sessions` | 6 | 6 | 3 | `002-cuentas` |
| `paymentCards` | 8 | 8 | 2 | `002-cuentas` |
| `venues` | 23 | 20 | 3 | `003-salones` |
| `venueDates` | 7 | 4 | 1 | `003-salones` |
| `providers` | 14 | 13 | 2 | `004-proveedores` |
| `gigs` | 10 | 8 | 1 | `004-proveedores` |
| `reviews` | 10 | 8 | 2 | `008-reputacion` |
| `monthlyStats` | 7 | 7 | 1 | `008-reputacion` |
| `events` | 18 | 10 | 2 | `005-planeacion` |
| `eventGuests` | 7 | 6 | 1 | `005-planeacion` |
| `eventTasks` | 6 | 6 | 1 | `005-planeacion` |
| `quotes` | 14 | 10 | 2 | `006-cotizaciones` |
| `bookings` | 18 | 13 | 3 | `007-reservaciones` |
| `counters` | 2 | 2 | 0 | `007-reservaciones` |
| `favorites` | 8 | 8 | 1 | `005-planeacion` |
| `conversations` | 7 | 6 | 3 | `009-interaccion` |
| `messages` | 6 | 5 | 1 | `009-interaccion` |
| `notices` | 9 | 9 | 2 | `009-interaccion` |
| `assistantMessages` | 6 | 6 | 2 | `010-soporte` |
| `contactMessages` | 6 | 5 | 1 | `010-soporte` |

## 6. Móvil por flujo

| Spec | Título | Rol | Pantallas | Endpoints | Requisitos |
|---|---|---|---|---|---|
| `001-inicio-visitante` | 001 Inicio para visitantes | Visitante | A1, A3 | 2 | 11 |
| `002-registro` | 002 Registro | Visitante | A2, A4 | 2 | 9 |
| `003-iniciar-sesion` | 003 Iniciar y cerrar sesión | Visitante | A5 | 3 | 4 |
| `004-recuperar-contrasena` | 004 Recuperar contraseña | Visitante | A6, A7 | 2 | 5 |
| `005-explorar-salones` | 005 Explorar salones | Cliente | B8, C9, C10, C11 | 4 | 23 |
| `006-detalle-salon` | 006 Detalle del salón | Cliente | B13, C13, C14 | 6 | 18 |
| `007-crear-evento` | 007 Crear evento | Cliente | C12 | 1 | 5 |
| `008-cotizaciones-cliente` | 008 Cotizaciones del cliente | Cliente | C21, C22, C7 | 5 | 11 |
| `009-reservar-pagar` | 009 Reservar y pagar | Cliente | C8, C23 | 5 | 10 |
| `010-reservaciones` | 010 Mis reservaciones | Cliente | C32, C33 | 4 | 5 |
| `011-proveedores-cliente` | 011 Proveedores | Cliente | C19, B18, C20 | 3 | 12 |
| `012-ia-studio` | 012 IA Studio | Cliente | C15, E43, C16, C17 | 1 | 15 |
| `013-asistente-ia` | 013 Asistente IA | Cliente | C18 | 3 | 3 |
| `014-mi-evento` | 014 Mi Evento | Cliente | D27 | 4 | 9 |
| `015-presupuesto` | 015 Presupuesto | Cliente | C25, C24 | 2 | 5 |
| `016-invitados` | 016 Invitados e invitación | Cliente | D32, C31 | 5 | 9 |
| `017-pendientes` | 017 Pendientes y recordatorios | Cliente | C27, C29, C28 | 5 | 8 |
| `018-pagos-evento` | 018 Pagos del evento | Cliente | C26 | 2 | 3 |
| `019-compartir-calificar` | 019 Compartir y calificar | Cliente | C30, C34 | 7 | 5 |
| `020-comunicacion` | 020 Mensajes y notificaciones | Cliente | C35, C36, C37 | 9 | 6 |
| `021-favoritos-perfil` | 021 Favoritos, perfil y ayuda | Cliente | C38, C39, C40 | 11 | 11 |
| `022-propietario-registrar-salon` | 022 Registrar un salón | Propietario | P3, P4, P5 | 4 | 13 |
| `023-propietario-inicio-calendario` | 023 Inicio y calendario | Propietario | G54, G58, P9 | 5 | 13 |
| `024-propietario-solicitudes` | 024 Solicitudes y mensajes | Propietario | P7, P8, P16 | 6 | 8 |
| `025-propietario-herramientas` | 025 Herramientas del propietario | Propietario | P6, P10, P11, P12, P13, P14, P15 | 7 | 17 |
| `026-proveedor-perfil` | 026 Perfil del proveedor | Proveedor | S2, S6, S7 | 3 | 12 |
| `027-proveedor-inicio-agenda` | 027 Inicio y agenda | Proveedor | S3, H74 | 5 | 8 |
| `028-proveedor-solicitudes` | 028 Solicitudes y mensajes | Proveedor | S4, S5, S10 | 6 | 5 |
| `029-proveedor-resenas-estadisticas` | 029 Reseñas y estadísticas | Proveedor | S8, S9 | 3 | 3 |

## 7. Carpeta por carpeta

| Carpeta | Archivos | Para qué sirve |
|---|---|---|
| `(raíz)` | 8 | Gobernanza global y entrada al repositorio |
| `.github` | 9 | Configuración compartida de GitHub |
| `.github/rulesets` | 1 | Protección de ramas |
| `.github/workflows` | 4 | Pipelines de GitHub Actions |
| `.opencode` | 8 | Agentes y skills globales |
| `.opencode/agents` | 5 | — |
| `.opencode/skills` | 2 | — |
| `.specify` | 7 | Plantillas SDD |
| `.specify/memory` | 2 | — |
| `.specify/templates` | 5 | — |
| `VirGo-API` | 515 | Backend .NET 10 |
| `VirGo-API/.opencode` | 9 | Subagentes y skills del backend |
| `VirGo-API/db` | 23 | Copia del esquema y su comprobación |
| `VirGo-API/infra` | 5 | Infraestructura AWS (PR #5) |
| `VirGo-API/specs` | 404 | Specs del backend |
| `VirGo-API/src` | 47 | Código de la API (PR #5) |
| `VirGo-API/tests` | 15 | Pruebas de la API (PR #5) |
| `VirGo-Database` | 91 | Catálogo y herramientas de MongoDB |
| `VirGo-Database/docs` | 8 | Documentación de la base |
| `VirGo-Database/scripts` | 2 | — |
| `VirGo-Database/specs` | 44 | Specs de la base |
| `VirGo-Database/src` | 24 | Catálogo, inicializador y CLI |
| `VirGo-Database/tests` | 6 | Pruebas de la base |
| `VirGo-Movil` | 179 | App Android |
| `VirGo-Movil/.opencode` | 9 | Subagentes y skills de Móvil |
| `VirGo-Movil/app` | 5 | Módulo principal de la app |
| `VirGo-Movil/build-logic` | 7 | Convention plugins de Gradle |
| `VirGo-Movil/config` | 1 | — |
| `VirGo-Movil/core` | 22 | Módulos compartidos |
| `VirGo-Movil/gradle` | 3 | — |
| `VirGo-Movil/specs` | 120 | Specs de Móvil |
| `docs` | 6 | Documentación transversal |
| `docs/backend` | 2 | Mapa de backend e inventario de endpoints |
| `docs/frontend` | 1 | Mapa de Móvil |
| `scripts` | 1 | — |
| `specs` | 12 | Specs globales |
| `specs/nv-00` | 4 | — |
| `specs/nv-01` | 4 | — |
| `specs/nv-02` | 4 | — |

## 8. Archivo por archivo

### `(raíz)`

| Archivo | Qué es | Dónde |
|---|---|---|
| `.gitignore` | Archivos que git no sube | main |
| `.gitleaks.toml` | Reglas de búsqueda de secretos | main |
| `AGENTS.md` | AGENTS.md — virGO_APP | main |
| `CHECKLIST_PR.md` | Checklist de calidad para Pull Requests | main |
| `Memoria.md` | Memoria.md — virGO_APP | main |
| `Modelo de datos VirGo.md` | Modelo de datos NoSQL de VirGo (MongoDB) | main |
| `README.md` | VIRGO APP | main |
| `constitution.md` | Constitución — virGO_APP | main |

### `.github`

| Archivo | Qué es | Dónde |
|---|---|---|
| `README.md` | .github — DevSecOps compartido | main |
| `SECURITY.md` | Política de seguridad | main |
| `dependabot.yml` | Actualización de dependencias | main |
| `pull_request_template.md` | Qué cambia | main |

### `.github/rulesets`

| Archivo | Qué es | Dónde |
|---|---|---|
| `main-protection.json` | Ruleset de `main` | main |

### `.github/workflows`

| Archivo | Qué es | Dónde |
|---|---|---|
| `api-ci.yml` | Workflow: API CI | PR #5 |
| `api-codeql.yml` | Workflow: API CodeQL | PR #5 |
| `api-deploy.yml` | Workflow: API Deploy | PR #5 |
| `security.yml` | Workflow: security | main |

### `.opencode`

| Archivo | Qué es | Dónde |
|---|---|---|
| `.gitignore` | Archivos que git no sube | main |

### `.opencode/agents`

| Archivo | Qué es | Dónde |
|---|---|---|
| `foundation-builder.md` | Foundation Builder | main |
| `readme-keeper.md` | Readme Keeper | main |
| `security-reviewer.md` | Security Reviewer | main |
| `slice-scaffolder.md` | Slice Scaffolder | main |
| `traceability-clerk.md` | Traceability Clerk | main |

### `.opencode/skills/00-constitution`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | 00-constitution | main |

### `.opencode/skills/01-foundation`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | 01-foundation | main |

### `.specify/memory`

| Archivo | Qué es | Dónde |
|---|---|---|
| `README.md` | Memoria | main |
| `memoria-template.md` | memoria.md — <proyecto> | main |

### `.specify/templates`

| Archivo | Qué es | Dónde |
|---|---|---|
| `README.md` | Plantillas SDD | main |
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-API`

| Archivo | Qué es | Dónde |
|---|---|---|
| `.editorconfig` | Formato de código | main |
| `.gitignore` | Archivos que git no sube | main |
| `AGENTS.md` | AGENTS.md — VirGo API (Backend) | main |
| `API-HANDOFF.md` | API Handoff | PR #5 |
| `Directory.Build.props` | Configuración común de .NET | main |
| `README.md` | VirGo API | main |
| `VirGo.slnx` | Archivo de configuración | PR #5 |
| `constitution.md` | Constitution — VirGo API (Backend) | main |
| `dotnet-tools.json` | Archivo de configuración | PR #5 |
| `memoria.md` | Memoria — VirGo API (Backend) | main |
| `opencode.json` | Servidores MCP permitidos | main |
| `stryker-config.json` | Archivo de configuración | PR #5 |

### `VirGo-API/.opencode/agents`

| Archivo | Qué es | Dónde |
|---|---|---|
| `arquitecto.md` | Antes de actuar | main |
| `dev-dotnet.md` | Antes de actuar | main |
| `devops-aws.md` | Antes de actuar | main |
| `qa.md` | Antes de actuar | main |
| `seguridad.md` | Antes de actuar | main |

### `VirGo-API/.opencode/skills/desplegar-aws`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Desplegar en AWS a costo cero | main |

### `VirGo-API/.opencode/skills/registrar-memoria`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Registrar en memoria.md | main |

### `VirGo-API/.opencode/skills/sdd-flujo`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Flujo SDD para un módulo | main |

### `VirGo-API/.opencode/skills/verificar-constitucion`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Verificar la alineación con la constitución | main |

### `VirGo-API/db/schema`

| Archivo | Qué es | Dónde |
|---|---|---|
| `assistantMessages.json` | Copia del esquema de `assistantMessages`: validador e índices | main |
| `bookings.json` | Copia del esquema de `bookings`: validador e índices | main |
| `contactMessages.json` | Copia del esquema de `contactMessages`: validador e índices | main |
| `conversations.json` | Copia del esquema de `conversations`: validador e índices | main |
| `counters.json` | Copia del esquema de `counters`: validador e índices | main |
| `eventGuests.json` | Copia del esquema de `eventGuests`: validador e índices | main |
| `eventTasks.json` | Copia del esquema de `eventTasks`: validador e índices | main |
| `events.json` | Copia del esquema de `events`: validador e índices | main |
| `favorites.json` | Copia del esquema de `favorites`: validador e índices | main |
| `gigs.json` | Copia del esquema de `gigs`: validador e índices | main |
| `messages.json` | Copia del esquema de `messages`: validador e índices | main |
| `monthlyStats.json` | Copia del esquema de `monthlyStats`: validador e índices | main |
| `notices.json` | Copia del esquema de `notices`: validador e índices | main |
| `passwordResets.json` | Copia del esquema de `passwordResets`: validador e índices | main |
| `paymentCards.json` | Copia del esquema de `paymentCards`: validador e índices | main |
| `providers.json` | Copia del esquema de `providers`: validador e índices | main |
| `quotes.json` | Copia del esquema de `quotes`: validador e índices | main |
| `reviews.json` | Copia del esquema de `reviews`: validador e índices | main |
| `sessions.json` | Copia del esquema de `sessions`: validador e índices | main |
| `users.json` | Copia del esquema de `users`: validador e índices | main |
| `venueDates.json` | Copia del esquema de `venueDates`: validador e índices | main |
| `venues.json` | Copia del esquema de `venues`: validador e índices | main |

### `VirGo-API/db/tools`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check_schema.py` | Script de Python | main |

### `VirGo-API/infra`

| Archivo | Qué es | Dónde |
|---|---|---|
| `.checkov.yaml` | Configuración YAML | PR #5 |
| `atlas.md` | MongoDB Atlas Bootstrap | PR #5 |
| `bootstrap.yaml` | Configuración YAML | PR #5 |
| `samconfig.toml` | Archivo de configuración | PR #5 |
| `template.yaml` | Configuración YAML | PR #5 |

### `VirGo-API/specs/000-arquitectura-base`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-API/specs/001-infraestructura-iac`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-API/specs/002-pipeline-cicd`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-API/specs/003-auth-login`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-API/specs/004-auth-logout`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-API/specs/005-auth-register`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | PR #5 |
| `plan.md` | Cómo: contrato, datos, pruebas | PR #5 |
| `spec.md` | Qué y por qué: requisitos y criterios | PR #5 |
| `task.md` | Tareas y su estado | PR #5 |

### `VirGo-API/specs/006-users-me`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | PR #5 |
| `plan.md` | Cómo: contrato, datos, pruebas | PR #5 |
| `spec.md` | Qué y por qué: requisitos y criterios | PR #5 |
| `task.md` | Tareas y su estado | PR #5 |

### `VirGo-API/specs/007-esquema-mongodb`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | PR #5 |
| `plan.md` | Cómo: contrato, datos, pruebas | PR #5 |
| `spec.md` | Qué y por qué: requisitos y criterios | PR #5 |
| `task.md` | Tareas y su estado | PR #5 |

### `VirGo-API/specs/007-venues-catalog`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | PR #5 |
| `plan.md` | Cómo: contrato, datos, pruebas | PR #5 |
| `spec.md` | Qué y por qué: requisitos y criterios | PR #5 |
| `task.md` | Tareas y su estado | PR #5 |

### `VirGo-API/specs/008-quotes-create`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | PR #5 |
| `plan.md` | Cómo: contrato, datos, pruebas | PR #5 |
| `spec.md` | Qué y por qué: requisitos y criterios | PR #5 |
| `task.md` | Tareas y su estado | PR #5 |

### `VirGo-API/specs/009-bookings-create`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | PR #5 |
| `plan.md` | Cómo: contrato, datos, pruebas | PR #5 |
| `spec.md` | Qué y por qué: requisitos y criterios | PR #5 |
| `task.md` | Tareas y su estado | PR #5 |

### `VirGo-API/specs/010-contrato-datos`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-API/specs/011-post-auth-password-forgot`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/012-post-auth-password-reset`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/013-post-auth-refresh`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/014-post-auth-email-verify`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/015-patch-users-me`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/016-patch-users-me-password`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/017-get-users-id-public`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/018-delete-users-me`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/019-get-payment-cards`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/020-post-payment-cards`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/021-patch-payment-cards-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/022-delete-payment-cards-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/023-post-payment-cards-id-default`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/024-get-venues-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/025-get-venues-mine`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/026-post-venues`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/027-patch-venues-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/028-post-venues-id-submit`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/029-post-venues-id-publish`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/030-delete-venues-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/031-get-venues-id-availability`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/032-post-venues-id-blocks`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/033-delete-venues-id-blocks-blockid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/034-get-providers`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/035-get-providers-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/036-get-providers-me`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/037-post-providers`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/038-patch-providers-me`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/039-get-providers-me-gigs`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/040-post-providers-me-gigs`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/041-patch-providers-me-gigs-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/042-delete-providers-me-gigs-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/043-get-events`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/044-post-events`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/045-get-events-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/046-patch-events-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/047-delete-events-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/048-get-events-id-guests`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/049-post-events-id-guests`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/050-patch-events-id-guests-guestid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/051-delete-events-id-guests-guestid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/052-get-events-id-tasks`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/053-post-events-id-tasks`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/054-patch-events-id-tasks-taskid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/055-delete-events-id-tasks-taskid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/056-get-events-id-collaborators`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/057-post-events-id-collaborators`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/058-patch-events-id-collaborators-userid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/059-delete-events-id-collaborators-userid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/060-get-quotes`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/061-get-quotes-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/062-post-quotes-id-response`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/063-post-quotes-id-accept`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/064-post-quotes-id-reject`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/065-post-quotes-id-expire`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/066-get-bookings`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/067-get-bookings-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/068-get-bookings-by-folio-folio`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/069-post-bookings-id-cancel`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/070-post-bookings-id-payments`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/071-get-bookings-id-payments`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/072-get-venues-id-reviews`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/073-get-providers-id-reviews`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/074-post-reviews`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/075-post-reviews-id-reply`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/076-patch-reviews-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/077-delete-reviews-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/078-get-venues-id-stats`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/079-get-providers-id-stats`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/080-get-stats-overview`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/081-get-favorites`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/082-post-favorites`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/083-delete-favorites-targettype-targetid`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/084-get-conversations`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/085-post-conversations`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/086-get-conversations-id`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/087-get-conversations-id-messages`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/088-post-conversations-id-messages`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/089-patch-conversations-id-read`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/090-get-notices`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/091-patch-notices-id-read`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/092-post-notices-read-all`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/093-get-assistant-messages`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/094-post-assistant-messages`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/095-delete-assistant-messages`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/096-post-contact`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/097-get-health-ready`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/900-infraestructura-iac`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/specs/901-pipeline-cicd`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-API/src/VirGo.Api`

| Archivo | Qué es | Dónde |
|---|---|---|
| `Program.cs` | Define `Program` | PR #5 |
| `VirGo.Api.csproj` | Proyecto .NET | PR #5 |
| `VirGo.Api.http` | Archivo de configuración | PR #5 |
| `appsettings.Development.json` | Archivo de configuración | PR #5 |
| `appsettings.Example.json` | Archivo de configuración | main |
| `appsettings.json` | Archivo de configuración | PR #5 |

### `VirGo-API/src/VirGo.Api/Controllers`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AuthController.cs` | Define `AuthController`, `LoginRequest`, `RegisterRequest` | PR #5 |
| `BookingsController.cs` | Define `BookingsController`, `CreateBookingRequest` | PR #5 |
| `DiagnosticsController.cs` | Define `DiagnosticsController` | PR #5 |
| `QuotesController.cs` | Define `QuotesController`, `CreateQuoteRequest`, `QuoteTargetRequest` | PR #5 |
| `UsersController.cs` | Define `UsersController` | PR #5 |
| `VenuesController.cs` | Define `VenuesController` | PR #5 |

### `VirGo-API/src/VirGo.Api/Errors`

| Archivo | Qué es | Dónde |
|---|---|---|
| `GlobalExceptionHandler.cs` | Define `GlobalExceptionHandler` | PR #5 |

### `VirGo-API/src/VirGo.Api/Middleware`

| Archivo | Qué es | Dónde |
|---|---|---|
| `CorrelationIdMiddleware.cs` | Define `CorrelationIdMiddleware` | PR #5 |
| `SecurityHeadersMiddleware.cs` | Define `SecurityHeadersMiddleware` | PR #5 |

### `VirGo-API/src/VirGo.Api/OpenApi`

| Archivo | Qué es | Dónde |
|---|---|---|
| `BearerOpenApiTransformer.cs` | Define `BearerOpenApiTransformer` | PR #5 |

### `VirGo-API/src/VirGo.Api/Properties`

| Archivo | Qué es | Dónde |
|---|---|---|
| `launchSettings.json` | Archivo de configuración | PR #5 |

### `VirGo-API/src/VirGo.Api/Security`

| Archivo | Qué es | Dónde |
|---|---|---|
| `JwtSessionEvents.cs` | Define `JwtSessionEvents` | PR #5 |

### `VirGo-API/src/VirGo.Application`

| Archivo | Qué es | Dónde |
|---|---|---|
| `VirGo.Application.csproj` | Proyecto .NET | PR #5 |

### `VirGo-API/src/VirGo.Application/Abstractions`

| Archivo | Qué es | Dónde |
|---|---|---|
| `IClock.cs` | Define `IClock` | PR #5 |
| `IRepository.cs` | Define `IRepository` | PR #5 |

### `VirGo-API/src/VirGo.Application/Auth`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AuthAbstractions.cs` | Define `DuplicateUserEmailException`, `IUserRepository`, `ISessionRepository`, `IPasswordHasher` | PR #5 |
| `AuthContracts.cs` | Define `LoginCommand`, `LoginResult`, `LoginFailure`, `LoginOutcome` | PR #5 |
| `LoginHandler.cs` | Define `LoginHandler` | PR #5 |
| `LogoutHandler.cs` | Define `LogoutHandler` | PR #5 |
| `RegisterHandler.cs` | Define `RegisterHandler` | PR #5 |

### `VirGo-API/src/VirGo.Application/Common`

| Archivo | Qué es | Dónde |
|---|---|---|
| `Result.cs` | Define `Result`, `PageRequest`, `Page` | PR #5 |

### `VirGo-API/src/VirGo.Application/Configuration`

| Archivo | Qué es | Dónde |
|---|---|---|
| `Options.cs` | Define `PersistenceOptions`, `JwtOptions`, `CorsOptions`, `RateLimitingOptions` | PR #5 |

### `VirGo-API/src/VirGo.Application/Marketplace`

| Archivo | Qué es | Dónde |
|---|---|---|
| `BookingHandler.cs` | Define `BookingHandler` | PR #5 |
| `MarketplaceAbstractions.cs` | Define `DuplicateBookingIdempotencyException`, `IVenueRepository`, `IQuoteRepository`, `IBookingRepository` | PR #5 |
| `MarketplaceContracts.cs` | Define `CreateQuoteCommand`, `QuoteCreationResult`, `CreateBookingCommand`, `BookingCreationResult` | PR #5 |
| `QuoteHandler.cs` | Define `QuoteHandler` | PR #5 |

### `VirGo-API/src/VirGo.Domain`

| Archivo | Qué es | Dónde |
|---|---|---|
| `VirGo.Domain.csproj` | Proyecto .NET | PR #5 |

### `VirGo-API/src/VirGo.Domain/Auth`

| Archivo | Qué es | Dónde |
|---|---|---|
| `Session.cs` | Define `Session` | PR #5 |
| `User.cs` | Define `UserRole`, `User` | PR #5 |

### `VirGo-API/src/VirGo.Domain/Marketplace`

| Archivo | Qué es | Dónde |
|---|---|---|
| `Booking.cs` | Define `Booking` | PR #5 |
| `Quote.cs` | Define `Quote` | PR #5 |
| `Venue.cs` | Define `Venue` | PR #5 |

### `VirGo-API/src/VirGo.Infrastructure`

| Archivo | Qué es | Dónde |
|---|---|---|
| `DependencyInjection.cs` | Define `DependencyInjection`, `SystemClock` | PR #5 |
| `MarketplaceRepositories.cs` | Define `InMemoryVenueRepository`, `InMemoryQuoteRepository`, `InMemoryBookingRepository`, `MongoVenueRepository` | PR #5 |
| `VirGo.Infrastructure.csproj` | Proyecto .NET | PR #5 |

### `VirGo-API/src/VirGo.Infrastructure/Auth`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AuthServices.cs` | Define `IdentityPasswordHasher`, `RefreshTokenGenerator`, `JwtTokenService` | PR #5 |
| `InMemoryAuthRepositories.cs` | Define `InMemoryUserRepository`, `InMemorySessionRepository` | PR #5 |
| `MongoAuthRepositories.cs` | Define `MongoUserRepository`, `MongoSessionRepository` | PR #5 |

### `VirGo-API/src/VirGo.Infrastructure/Persistence/InMemory`

| Archivo | Qué es | Dónde |
|---|---|---|
| `InMemoryRepository.cs` | Define `InMemoryRepository` | PR #5 |

### `VirGo-API/src/VirGo.Infrastructure/Persistence/MongoDb`

| Archivo | Qué es | Dónde |
|---|---|---|
| `IMongoRepositoryFactory.cs` | Define `IMongoRepositoryFactory` | PR #5 |
| `MongoRepositoryFactory.cs` | Define `MongoRepositoryFactory`, `MongoRepository` | PR #5 |

### `VirGo-API/tests/VirGo.IntegrationTests`

| Archivo | Qué es | Dónde |
|---|---|---|
| `ApiFactory.cs` | Define `ApiFactory` | PR #5 |
| `AuthTests.cs` | Define `AuthTests` | PR #5 |
| `BaseApiTests.cs` | Define `BaseApiTests` | PR #5 |
| `RateLimitFactory.cs` | Define `RateLimitFactory` | PR #5 |
| `RateLimitTests.cs` | Define `RateLimitTests` | PR #5 |
| `VerticalSliceTests.cs` | Define `VerticalSliceTests` | PR #5 |
| `VirGo.IntegrationTests.csproj` | Proyecto .NET | PR #5 |

### `VirGo-API/tests/VirGo.SecurityTests`

| Archivo | Qué es | Dónde |
|---|---|---|
| `JwtSecurityTests.cs` | Define `JwtSecurityTests` | PR #5 |
| `SecurityApiFactory.cs` | Define `SecurityApiFactory` | PR #5 |
| `SecurityHeaderTests.cs` | Define `SecurityHeaderTests` | PR #5 |
| `VirGo.SecurityTests.csproj` | Proyecto .NET | PR #5 |

### `VirGo-API/tests/VirGo.UnitTests`

| Archivo | Qué es | Dónde |
|---|---|---|
| `ArchitectureTests.cs` | Define `ArchitectureTests` | PR #5 |
| `InMemoryRepositoryTests.cs` | Define `InMemoryRepositoryTests`, `TestEntity` | PR #5 |
| `VirGo.UnitTests.csproj` | Proyecto .NET | PR #5 |

### `VirGo-API/tests/smoke`

| Archivo | Qué es | Dónde |
|---|---|---|
| `smoke.sh` | Script de shell | PR #5 |

### `VirGo-Database`

| Archivo | Qué es | Dónde |
|---|---|---|
| `.env.development.example` | Ejemplo de variables de entorno, sin secretos | main |
| `.env.example` | Ejemplo de variables de entorno, sin secretos | main |
| `.env.production.example` | Ejemplo de variables de entorno, sin secretos | main |
| `.gitignore` | Archivos que git no sube | main |
| `README.md` | VirGo Database | main |
| `package-lock.json` | Versiones exactas de dependencias | main |
| `package.json` | Dependencias y comandos | main |

### `VirGo-Database/docs`

| Archivo | Qué es | Dónde |
|---|---|---|
| `API-HANDOFF.md` | VirGo API Handoff | main |
| `API-HANDOFF.zip` | Archivo binario | main |
| `ARCHITECTURE.md` | Arquitectura | main |
| `ATLAS_SETUP.md` | Configuracion de MongoDB Atlas | main |
| `COLLECTIONS.md` | Colecciones e indices | main |
| `DECISIONS.md` | Decisiones | main |
| `OPERATIONS.md` | Operacion | main |
| `TESTING.md` | Pruebas | main |

### `VirGo-Database/scripts/dev`

| Archivo | Qué es | Dónde |
|---|---|---|
| `start-local-mongo.ps1` | Script de PowerShell | main |
| `stop-local-mongo.ps1` | Script de PowerShell | main |

### `VirGo-Database/specs/000-base-datos`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-Database/specs/001-contrato-con-api`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-Database/specs/002-cuentas`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/003-salones`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/004-proveedores`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/005-planeacion`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/006-cotizaciones`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/007-reservaciones`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/008-reputacion`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/009-interaccion`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/specs/010-soporte`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Database/src/catalog`

| Archivo | Qué es | Dónde |
|---|---|---|
| `enums.js` | Exporta `ROLES`, `VENUE_STATUSES`, `PROVIDER_STATUSES`, `ZONES` | main |
| `helpers.js` | Exporta `UUID_PATTERN`, `DATE_PATTERN`, `MONTH_PATTERN`, `FOLIO_PATTERN` | main |
| `index.js` | Exporta `COLLECTIONS`, `COLLECTION_NAMES`, `COLLECTIONS_BY_NAME` | main |
| `indexes.js` | Exporta `INDEXES` | main |

### `VirGo-Database/src/catalog/validators`

| Archivo | Qué es | Dónde |
|---|---|---|
| `accounts.js` | Exporta `usersValidator`, `passwordResetsValidator`, `sessionsValidator`, `paymentCardsValidator` | main |
| `bookings.js` | Exporta `bookingsValidator`, `countersValidator` | main |
| `interaction.js` | Exporta `conversationsValidator`, `messagesValidator`, `noticesValidator` | main |
| `planning.js` | Exporta `eventsValidator`, `eventGuestsValidator`, `eventTasksValidator`, `favoritesValidator` | main |
| `providers.js` | Exporta `providersValidator`, `gigsValidator` | main |
| `quotes.js` | Exporta `quotesValidator` | main |
| `reputation.js` | Exporta `reviewsValidator`, `monthlyStatsValidator` | main |
| `support.js` | Exporta `assistantMessagesValidator`, `contactMessagesValidator` | main |
| `venues.js` | Exporta `venuesValidator`, `venueDatesValidator` | main |

### `VirGo-Database/src/cli`

| Archivo | Qué es | Dónde |
|---|---|---|
| `init.js` | Código JavaScript | main |
| `inspect.js` | Código JavaScript | main |
| `verify.js` | Código JavaScript | main |

### `VirGo-Database/src/config`

| Archivo | Qué es | Dónde |
|---|---|---|
| `env.js` | Exporta `loadConfig`, `safeConfigSummary` | main |

### `VirGo-Database/src/connection`

| Archivo | Qué es | Dónde |
|---|---|---|
| `client.js` | Exporta `connectDatabase`, `closeDatabase` | main |

### `VirGo-Database/src/init`

| Archivo | Qué es | Dónde |
|---|---|---|
| `apply.js` | Exporta `applyPlan` | main |
| `format.js` | Exporta `formatPlan`, `formatVerification` | main |
| `index.js` | Código JavaScript | main |
| `plan.js` | Exporta `indexesEquivalent`, `describeIndex`, `buildPlan` | main |
| `verify.js` | Exporta `verifyInitialization` | main |

### `VirGo-Database/src/util`

| Archivo | Qué es | Dónde |
|---|---|---|
| `args.js` | Exporta `parseCliArgs`, `applyEnvFileArg` | main |

### `VirGo-Database/tests/helpers`

| Archivo | Qué es | Dónde |
|---|---|---|
| `fixtures.js` | Pruebas: 0 casos declarados | main |
| `testDb.js` | Pruebas: 0 casos declarados | main |

### `VirGo-Database/tests/integration`

| Archivo | Qué es | Dónde |
|---|---|---|
| `database.test.js` | Pruebas: 20 casos declarados | main |

### `VirGo-Database/tests/unit`

| Archivo | Qué es | Dónde |
|---|---|---|
| `catalog.test.js` | Pruebas: 11 casos declarados | main |
| `format.test.js` | Pruebas: 2 casos declarados | main |
| `plan.test.js` | Pruebas: 3 casos declarados | main |

### `VirGo-Movil`

| Archivo | Qué es | Dónde |
|---|---|---|
| `.editorconfig` | Formato de código | main |
| `.gitignore` | Archivos que git no sube | main |
| `AGENTS.md` | AGENTS.md — VirGo Móvil (Android) | main |
| `README.md` | VirGo Mobile (Android) | main |
| `build.gradle.kts` | Configuración de compilación | main |
| `constitution.md` | Constitution — VirGo Móvil (Android) | main |
| `gradle.properties` | Archivo de configuración | main |
| `gradlew` | Lanzador de Gradle | main |
| `gradlew.bat` | Lanzador de Gradle en Windows | main |
| `memoria.md` | Memoria — VirGo Móvil (Android) | main |
| `opencode.json` | Servidores MCP permitidos | main |
| `settings.gradle.kts` | Módulos de Gradle | main |

### `VirGo-Movil/.opencode/agents`

| Archivo | Qué es | Dónde |
|---|---|---|
| `arquitecto-movil.md` | Antes de actuar | main |
| `dev-android.md` | Antes de actuar | main |
| `qa-movil.md` | Antes de actuar | main |
| `seguridad-ci.md` | Antes de actuar | main |
| `ui-designsystem.md` | Antes de actuar | main |

### `VirGo-Movil/.opencode/skills/compilar-publicar-apk`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Compilar y publicar el APK | main |

### `VirGo-Movil/.opencode/skills/registrar-memoria`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Registrar en memoria.md | main |

### `VirGo-Movil/.opencode/skills/sdd-flujo`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Flujo SDD para un módulo | main |

### `VirGo-Movil/.opencode/skills/verificar-constitucion`

| Archivo | Qué es | Dónde |
|---|---|---|
| `SKILL.md` | Verificar la alineación con la constitución | main |

### `VirGo-Movil/app`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/app/src/main`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AndroidManifest.xml` | Manifiesto de la app | main |

### `VirGo-Movil/app/src/main/kotlin/mx/virgo/app`

| Archivo | Qué es | Dónde |
|---|---|---|
| `MainActivity.kt` | Define `MainActivity`, `onCreate` | main |
| `VirGoApplication.kt` | Define `VirGoApplication` | main |

### `VirGo-Movil/app/src/main/kotlin/mx/virgo/app/ui/login`

| Archivo | Qué es | Dónde |
|---|---|---|
| `LoginScreen.kt` | Define `LoginScreen`, `LoginScreenPreview` | main |

### `VirGo-Movil/build-logic`

| Archivo | Qué es | Dónde |
|---|---|---|
| `settings.gradle.kts` | Módulos de Gradle | main |

### `VirGo-Movil/build-logic/convention`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/build-logic/convention/src/main/kotlin`

| Archivo | Qué es | Dónde |
|---|---|---|
| `virgo.android.application.gradle.kts` | Configuración de Gradle | main |
| `virgo.android.compose.gradle.kts` | Configuración de Gradle | main |
| `virgo.android.library.gradle.kts` | Configuración de Gradle | main |
| `virgo.hilt.gradle.kts` | Configuración de Gradle | main |
| `virgo.jvm.library.gradle.kts` | Configuración de Gradle | main |

### `VirGo-Movil/config`

| Archivo | Qué es | Dónde |
|---|---|---|
| `detekt.yml` | Configuración YAML | main |

### `VirGo-Movil/core/common`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/core/common/src/main/kotlin/mx/virgo/core/common/di`

| Archivo | Qué es | Dónde |
|---|---|---|
| `Dispatcher.kt` | Define `VirgoDispatcher`, `Dispatcher` | main |
| `DispatcherModule.kt` | Define `DispatcherModule`, `provideDefaultDispatcher`, `provideIoDispatcher`, `provideMainDispatcher` | main |

### `VirGo-Movil/core/data`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/core/data/src/main`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AndroidManifest.xml` | Manifiesto de la app | main |

### `VirGo-Movil/core/datastore`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/core/datastore/src/main`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AndroidManifest.xml` | Manifiesto de la app | main |

### `VirGo-Movil/core/designsystem`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/core/designsystem/src/main`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AndroidManifest.xml` | Manifiesto de la app | main |

### `VirGo-Movil/core/designsystem/src/main/kotlin/mx/virgo/core/designsystem/components`

| Archivo | Qué es | Dónde |
|---|---|---|
| `VirGoBadge.kt` | Define `VirGoBadge` | main |
| `VirGoButton.kt` | Define `VirGoButton`, `VirGoOutlinedButton` | main |
| `VirGoCard.kt` | Define `VirGoCard` | main |
| `VirGoTextField.kt` | Define `VirGoTextField` | main |

### `VirGo-Movil/core/designsystem/src/main/kotlin/mx/virgo/core/designsystem/theme`

| Archivo | Qué es | Dónde |
|---|---|---|
| `Color.kt` | Código Kotlin | main |
| `Shape.kt` | Código Kotlin | main |
| `Spacing.kt` | Define `Spacing` | main |
| `Theme.kt` | Define `VirGoTheme` | main |
| `Type.kt` | Código Kotlin | main |

### `VirGo-Movil/core/domain`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/core/network`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/core/network/src/main`

| Archivo | Qué es | Dónde |
|---|---|---|
| `AndroidManifest.xml` | Manifiesto de la app | main |

### `VirGo-Movil/core/testing`

| Archivo | Qué es | Dónde |
|---|---|---|
| `build.gradle.kts` | Configuración de compilación | main |

### `VirGo-Movil/gradle`

| Archivo | Qué es | Dónde |
|---|---|---|
| `libs.versions.toml` | Versiones de librerías | main |

### `VirGo-Movil/gradle/wrapper`

| Archivo | Qué es | Dónde |
|---|---|---|
| `gradle-wrapper.jar` | Archivo binario | main |
| `gradle-wrapper.properties` | Versión de Gradle | main |

### `VirGo-Movil/specs/000-arquitectura-base`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `VirGo-Movil/specs/001-inicio-visitante`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/002-registro`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/003-iniciar-sesion`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/004-recuperar-contrasena`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/005-explorar-salones`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/006-detalle-salon`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/007-crear-evento`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/008-cotizaciones-cliente`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/009-reservar-pagar`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/010-reservaciones`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/011-proveedores-cliente`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/012-ia-studio`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/013-asistente-ia`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/014-mi-evento`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/015-presupuesto`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/016-invitados`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/017-pendientes`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/018-pagos-evento`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/019-compartir-calificar`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/020-comunicacion`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/021-favoritos-perfil`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/022-propietario-registrar-salon`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/023-propietario-inicio-calendario`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/024-propietario-solicitudes`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/025-propietario-herramientas`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/026-proveedor-perfil`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/027-proveedor-inicio-agenda`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/028-proveedor-solicitudes`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `VirGo-Movil/specs/029-proveedor-resenas-estadisticas`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | rama |
| `plan.md` | Cómo: contrato, datos, pruebas | rama |
| `spec.md` | Qué y por qué: requisitos y criterios | rama |
| `task.md` | Tareas y su estado | rama |

### `docs`

| Archivo | Qué es | Dónde |
|---|---|---|
| `ESTADO-PROYECTO.md` | VirGo: estado del proyecto | rama |
| `PRUEBAS.md` | Estado de las pruebas de VirGo | rama |
| `README.md` | docs/ | main |

### `docs/backend`

| Archivo | Qué es | Dónde |
|---|---|---|
| `ENDPOINTS.md` | Backend: inventario de endpoints | rama |
| `MAPA-SPECS.md` | Backend: mapa de specs | rama |

### `docs/frontend`

| Archivo | Qué es | Dónde |
|---|---|---|
| `MAPA-SPECS.md` | Frontend: mapa de specs | rama |

### `scripts`

| Archivo | Qué es | Dónde |
|---|---|---|
| `README.md` | scripts/ | main |

### `specs/nv-00`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `specs/nv-01`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |

### `specs/nv-02`

| Archivo | Qué es | Dónde |
|---|---|---|
| `check.md` | Lista de evidencia | main |
| `plan.md` | Cómo: contrato, datos, pruebas | main |
| `spec.md` | Qué y por qué: requisitos y criterios | main |
| `task.md` | Tareas y su estado | main |
