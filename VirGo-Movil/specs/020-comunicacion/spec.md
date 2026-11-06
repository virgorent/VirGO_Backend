# Spec — 020 Mensajes y notificaciones

| Campo | Valor |
|---|---|
| Módulo | `020-comunicacion` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C35 | Chat |
| C36 | Mensajes |
| C37 | Notificaciones |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C35]** Conversación con un salón o proveedor |
| RF-02 | **[C35]** Respuestas automáticas de los salones y proveedores de ejemplo |
| RF-03 | **[C36]** Lista de todas las conversaciones con el último mensaje y su hora |
| RF-04 | **[C37]** Avisos de cotizaciones respondidas, pagos, recordatorios y reservaciones |
| RF-05 | **[C37]** Marca de leído y no leído |
| RF-06 | **[C37]** Contador en la campana de Explorar |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C35, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C36, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C37, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/conversations` | `084-get-conversations` |
| `POST /api/v1/conversations` | `085-post-conversations` |
| `GET /api/v1/conversations/:id` | `086-get-conversations-id` |
| `GET /api/v1/conversations/:id/messages` | `087-get-conversations-id-messages` |
| `POST /api/v1/conversations/:id/messages` | `088-post-conversations-id-messages` |
| `PATCH /api/v1/conversations/:id/read` | `089-patch-conversations-id-read` |
| `GET /api/v1/notices` | `090-get-notices` |
| `PATCH /api/v1/notices/:id/read` | `091-patch-notices-id-read` |
| `POST /api/v1/notices/read-all` | `092-post-notices-read-all` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
