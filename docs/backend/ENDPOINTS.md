# Backend: inventario de endpoints

**Fecha:** 2026-11-05
**Fuentes:** `VirGo-Database/docs/API-HANDOFF.md` §9 y `Checklist VirGo.md`.

| Estado | Endpoints |
|---|---|
| ✅ Implementados en el PR #5 | 7 |
| 📝 Especificados | 77 |
| ⛔ Requieren decisión | 10 |
| **Total** | **94** |

Además, `000` define `GET /health` y `GET /openapi/v1.json`.

## Duplicados eliminados

| Endpoint propuesto | Motivo |
|---|---|
| `GET /auth/me` | repite `GET /users/me` (`006-users-me`) |
| `GET /health/live` | repite `GET /health` (`000`) |
| `DELETE /favorites/:id` | `API-HANDOFF.md` pide elegir una sola forma; se conserva la de destino |

## Lista completa

La columna «Móvil» indica qué specs de `VirGo-Movil/specs` lo consumen.

| # | Método | Ruta | Spec | Móvil | Estado |
|---|---|---|---|---|---|
| 1 | `POST` | `/api/v1/auth/login` | `003-auth-login` | `003` | ✅ PR #5 |
| 2 | `POST` | `/api/v1/auth/logout` | `004-auth-logout` | `003` | ✅ PR #5 |
| 3 | `POST` | `/api/v1/auth/register` | `005-auth-register` | `002` | ✅ PR #5 |
| 4 | `GET` | `/api/v1/users/me` | `006-users-me` | `021` | ✅ PR #5 |
| 5 | `GET` | `/api/v1/venues` | `007-venues-catalog` | `001`, `005` | ✅ PR #5 |
| 6 | `POST` | `/api/v1/quotes` | `008-quotes-create` | `008` | ✅ PR #5 |
| 7 | `POST` | `/api/v1/bookings` | `009-bookings-create` | `009` | ✅ PR #5 |
| 8 | `POST` | `/api/v1/auth/password/forgot` | `011-post-auth-password-forgot` | `004` | 📝 Especificado |
| 9 | `POST` | `/api/v1/auth/password/reset` | `012-post-auth-password-reset` | `004` | 📝 Especificado |
| 10 | `POST` | `/api/v1/auth/refresh` | `013-post-auth-refresh` | `003` | 📝 Especificado |
| 11 | `POST` | `/api/v1/auth/email/verify` | `014-post-auth-email-verify` | `002` | ⛔ Requiere decisión |
| 12 | `PATCH` | `/api/v1/users/me` | `015-patch-users-me` | `021` | 📝 Especificado |
| 13 | `PATCH` | `/api/v1/users/me/password` | `016-patch-users-me-password` | `021` | 📝 Especificado |
| 14 | `GET` | `/api/v1/users/:id/public` | `017-get-users-id-public` | `021` | 📝 Especificado |
| 15 | `DELETE` | `/api/v1/users/me` | `018-delete-users-me` | `021` | ⛔ Requiere decisión |
| 16 | `GET` | `/api/v1/payment-cards` | `019-get-payment-cards` | `009`, `021` | 📝 Especificado |
| 17 | `POST` | `/api/v1/payment-cards` | `020-post-payment-cards` | `009` | 📝 Especificado |
| 18 | `PATCH` | `/api/v1/payment-cards/:id` | `021-patch-payment-cards-id` | `021` | 📝 Especificado |
| 19 | `DELETE` | `/api/v1/payment-cards/:id` | `022-delete-payment-cards-id` | `021` | 📝 Especificado |
| 20 | `POST` | `/api/v1/payment-cards/:id/default` | `023-post-payment-cards-id-default` | `021` | 📝 Especificado |
| 21 | `GET` | `/api/v1/venues/:id` | `024-get-venues-id` | `001`, `006` | 📝 Especificado |
| 22 | `GET` | `/api/v1/venues/mine` | `025-get-venues-mine` | `025` | 📝 Especificado |
| 23 | `POST` | `/api/v1/venues` | `026-post-venues` | `022` | 📝 Especificado |
| 24 | `PATCH` | `/api/v1/venues/:id` | `027-patch-venues-id` | `022`, `025` | 📝 Especificado |
| 25 | `POST` | `/api/v1/venues/:id/submit` | `028-post-venues-id-submit` | `022` | ⛔ Requiere decisión |
| 26 | `POST` | `/api/v1/venues/:id/publish` | `029-post-venues-id-publish` | `022` | ⛔ Requiere decisión |
| 27 | `DELETE` | `/api/v1/venues/:id` | `030-delete-venues-id` | `025` | ⛔ Requiere decisión |
| 28 | `GET` | `/api/v1/venues/:id/availability` | `031-get-venues-id-availability` | `006`, `023` | 📝 Especificado |
| 29 | `POST` | `/api/v1/venues/:id/blocks` | `032-post-venues-id-blocks` | `023` | 📝 Especificado |
| 30 | `DELETE` | `/api/v1/venues/:id/blocks/:blockId` | `033-delete-venues-id-blocks-blockid` | `023` | 📝 Especificado |
| 31 | `GET` | `/api/v1/providers` | `034-get-providers` | `011` | 📝 Especificado |
| 32 | `GET` | `/api/v1/providers/:id` | `035-get-providers-id` | `011` | 📝 Especificado |
| 33 | `GET` | `/api/v1/providers/me` | `036-get-providers-me` | `026` | 📝 Especificado |
| 34 | `POST` | `/api/v1/providers` | `037-post-providers` | `026` | 📝 Especificado |
| 35 | `PATCH` | `/api/v1/providers/me` | `038-patch-providers-me` | `026` | 📝 Especificado |
| 36 | `GET` | `/api/v1/providers/me/gigs` | `039-get-providers-me-gigs` | `027` | 📝 Especificado |
| 37 | `POST` | `/api/v1/providers/me/gigs` | `040-post-providers-me-gigs` | `027` | 📝 Especificado |
| 38 | `PATCH` | `/api/v1/providers/me/gigs/:id` | `041-patch-providers-me-gigs-id` | `027` | 📝 Especificado |
| 39 | `DELETE` | `/api/v1/providers/me/gigs/:id` | `042-delete-providers-me-gigs-id` | `027` | ⛔ Requiere decisión |
| 40 | `GET` | `/api/v1/events` | `043-get-events` | `014` | 📝 Especificado |
| 41 | `POST` | `/api/v1/events` | `044-post-events` | `007` | 📝 Especificado |
| 42 | `GET` | `/api/v1/events/:id` | `045-get-events-id` | `014`, `015` | 📝 Especificado |
| 43 | `PATCH` | `/api/v1/events/:id` | `046-patch-events-id` | `012`, `014`, `015`, `016`, `017`, `018` | 📝 Especificado |
| 44 | `DELETE` | `/api/v1/events/:id` | `047-delete-events-id` | `014` | ⛔ Requiere decisión |
| 45 | `GET` | `/api/v1/events/:id/guests` | `048-get-events-id-guests` | `016` | 📝 Especificado |
| 46 | `POST` | `/api/v1/events/:id/guests` | `049-post-events-id-guests` | `016` | 📝 Especificado |
| 47 | `PATCH` | `/api/v1/events/:id/guests/:guestId` | `050-patch-events-id-guests-guestid` | `016` | 📝 Especificado |
| 48 | `DELETE` | `/api/v1/events/:id/guests/:guestId` | `051-delete-events-id-guests-guestid` | `016` | 📝 Especificado |
| 49 | `GET` | `/api/v1/events/:id/tasks` | `052-get-events-id-tasks` | `017` | 📝 Especificado |
| 50 | `POST` | `/api/v1/events/:id/tasks` | `053-post-events-id-tasks` | `017` | 📝 Especificado |
| 51 | `PATCH` | `/api/v1/events/:id/tasks/:taskId` | `054-patch-events-id-tasks-taskid` | `017` | 📝 Especificado |
| 52 | `DELETE` | `/api/v1/events/:id/tasks/:taskId` | `055-delete-events-id-tasks-taskid` | `017` | 📝 Especificado |
| 53 | `GET` | `/api/v1/events/:id/collaborators` | `056-get-events-id-collaborators` | `019` | 📝 Especificado |
| 54 | `POST` | `/api/v1/events/:id/collaborators` | `057-post-events-id-collaborators` | `019` | 📝 Especificado |
| 55 | `PATCH` | `/api/v1/events/:id/collaborators/:userId` | `058-patch-events-id-collaborators-userid` | `019` | 📝 Especificado |
| 56 | `DELETE` | `/api/v1/events/:id/collaborators/:userId` | `059-delete-events-id-collaborators-userid` | `019` | 📝 Especificado |
| 57 | `GET` | `/api/v1/quotes` | `060-get-quotes` | `008`, `024`, `028` | 📝 Especificado |
| 58 | `GET` | `/api/v1/quotes/:id` | `061-get-quotes-id` | `008`, `024`, `028` | 📝 Especificado |
| 59 | `POST` | `/api/v1/quotes/:id/response` | `062-post-quotes-id-response` | `024`, `028` | 📝 Especificado |
| 60 | `POST` | `/api/v1/quotes/:id/accept` | `063-post-quotes-id-accept` | `008` | 📝 Especificado |
| 61 | `POST` | `/api/v1/quotes/:id/reject` | `064-post-quotes-id-reject` | `008` | 📝 Especificado |
| 62 | `POST` | `/api/v1/quotes/:id/expire` | `065-post-quotes-id-expire` | — | 📝 Especificado |
| 63 | `GET` | `/api/v1/bookings` | `066-get-bookings` | `010`, `023`, `025` | 📝 Especificado |
| 64 | `GET` | `/api/v1/bookings/:id` | `067-get-bookings-id` | `009`, `010` | 📝 Especificado |
| 65 | `GET` | `/api/v1/bookings/by-folio/:folio` | `068-get-bookings-by-folio-folio` | `010` | 📝 Especificado |
| 66 | `POST` | `/api/v1/bookings/:id/cancel` | `069-post-bookings-id-cancel` | `010` | 📝 Especificado |
| 67 | `POST` | `/api/v1/bookings/:id/payments` | `070-post-bookings-id-payments` | `009` | 📝 Especificado |
| 68 | `GET` | `/api/v1/bookings/:id/payments` | `071-get-bookings-id-payments` | `018` | 📝 Especificado |
| 69 | `GET` | `/api/v1/venues/:id/reviews` | `072-get-venues-id-reviews` | `006`, `025` | 📝 Especificado |
| 70 | `GET` | `/api/v1/providers/:id/reviews` | `073-get-providers-id-reviews` | `011`, `029` | 📝 Especificado |
| 71 | `POST` | `/api/v1/reviews` | `074-post-reviews` | `019` | 📝 Especificado |
| 72 | `POST` | `/api/v1/reviews/:id/reply` | `075-post-reviews-id-reply` | `025`, `029` | 📝 Especificado |
| 73 | `PATCH` | `/api/v1/reviews/:id` | `076-patch-reviews-id` | `019` | ⛔ Requiere decisión |
| 74 | `DELETE` | `/api/v1/reviews/:id` | `077-delete-reviews-id` | `019` | ⛔ Requiere decisión |
| 75 | `GET` | `/api/v1/venues/:id/stats` | `078-get-venues-id-stats` | `025` | 📝 Especificado |
| 76 | `GET` | `/api/v1/providers/:id/stats` | `079-get-providers-id-stats` | `029` | 📝 Especificado |
| 77 | `GET` | `/api/v1/stats/overview` | `080-get-stats-overview` | `023`, `027` | 📝 Especificado |
| 78 | `GET` | `/api/v1/favorites` | `081-get-favorites` | `021` | 📝 Especificado |
| 79 | `POST` | `/api/v1/favorites` | `082-post-favorites` | `005`, `006` | 📝 Especificado |
| 80 | `DELETE` | `/api/v1/favorites/:targetType/:targetId` | `083-delete-favorites-targettype-targetid` | `005`, `006` | 📝 Especificado |
| 81 | `GET` | `/api/v1/conversations` | `084-get-conversations` | `020`, `024`, `028` | 📝 Especificado |
| 82 | `POST` | `/api/v1/conversations` | `085-post-conversations` | `006`, `020` | 📝 Especificado |
| 83 | `GET` | `/api/v1/conversations/:id` | `086-get-conversations-id` | `020` | 📝 Especificado |
| 84 | `GET` | `/api/v1/conversations/:id/messages` | `087-get-conversations-id-messages` | `020`, `024`, `028` | 📝 Especificado |
| 85 | `POST` | `/api/v1/conversations/:id/messages` | `088-post-conversations-id-messages` | `020`, `024`, `028` | 📝 Especificado |
| 86 | `PATCH` | `/api/v1/conversations/:id/read` | `089-patch-conversations-id-read` | `020` | 📝 Especificado |
| 87 | `GET` | `/api/v1/notices` | `090-get-notices` | `005`, `020` | 📝 Especificado |
| 88 | `PATCH` | `/api/v1/notices/:id/read` | `091-patch-notices-id-read` | `020` | 📝 Especificado |
| 89 | `POST` | `/api/v1/notices/read-all` | `092-post-notices-read-all` | `020` | 📝 Especificado |
| 90 | `GET` | `/api/v1/assistant/messages` | `093-get-assistant-messages` | `013` | 📝 Especificado |
| 91 | `POST` | `/api/v1/assistant/messages` | `094-post-assistant-messages` | `013` | 📝 Especificado |
| 92 | `DELETE` | `/api/v1/assistant/messages` | `095-delete-assistant-messages` | `013` | ⛔ Requiere decisión |
| 93 | `POST` | `/api/v1/contact` | `096-post-contact` | `021` | 📝 Especificado |
| 94 | `GET` | `/api/v1/health/ready` | `097-get-health-ready` | — | 📝 Especificado |

## Funciones de la maqueta sin endpoint

| Pantalla | Función | Motivo |
|---|---|---|
| C33 | Reprogramar reservación | `VirGo-Database` la marca pendiente |
| C11 | Recomendados para ti | No hay contrato definido |
| C15, E43, C16, C17 | IA Studio | Se guarda en `events.design`; el cálculo con IA no está definido |
| P3, P11, S6 | Subir fotos y planos | No hay endpoint de carga de archivos |
