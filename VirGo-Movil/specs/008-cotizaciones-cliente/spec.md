# Spec — 008 Cotizaciones del cliente

| Campo | Valor |
|---|---|
| Módulo | `008-cotizaciones-cliente` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C21 | Solicitar cotización |
| C22 | Mis cotizaciones |
| C7 | Detalle de la cotización |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C21]** Se puede pedir a un salón o a un proveedor |
| RF-02 | **[C21]** Fecha, invitados y servicios o paquetes de interés |
| RF-03 | **[C21]** Mensaje opcional |
| RF-04 | **[C21]** Botón «Enviar solicitud»; la solicitud queda como «Enviada» |
| RF-05 | **[C22]** Lista de solicitudes con su estado: Enviada, Respondida o Aceptada |
| RF-06 | **[C22]** Los salones y proveedores de ejemplo responden solos en unos segundos |
| RF-07 | **[C22]** Llega una notificación al recibir una respuesta |
| RF-08 | **[C7]** Conceptos con cantidad y precio |
| RF-09 | **[C7]** IVA y total calculado |
| RF-10 | **[C7]** Vigencia de 15 días |
| RF-11 | **[C7]** Botones Reservar (salón), Aceptar (proveedor; se agrega a Mi Evento) y Pedir cambios (abre el chat) |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C21, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C22, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C7, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `POST /api/v1/quotes` | `008-quotes-create` |
| `GET /api/v1/quotes` | `060-get-quotes` |
| `GET /api/v1/quotes/:id` | `061-get-quotes-id` |
| `POST /api/v1/quotes/:id/accept` | `063-post-quotes-id-accept` |
| `POST /api/v1/quotes/:id/reject` | `064-post-quotes-id-reject` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
