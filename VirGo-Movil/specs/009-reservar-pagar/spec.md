# Spec — 009 Reservar y pagar

| Campo | Valor |
|---|---|
| Módulo | `009-reservar-pagar` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C8 | Pago del anticipo |
| C23 | Comprobante de pago |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C8]** Resumen: salón, fecha validada («Disponible ✓»), lo incluido y el total |
| RF-02 | **[C8]** Anticipo del 30% |
| RF-03 | **[C8]** Métodos: tarjeta guardada, tarjeta nueva o transferencia |
| RF-04 | **[C8]** Una tarjeta que termina en 0000 simula un pago rechazado |
| RF-05 | **[C8]** Con transferencia, la reservación queda pendiente de validar |
| RF-06 | **[C8]** La fecha se aparta solo cuando el pago queda validado |
| RF-07 | **[C23]** Folio (por ejemplo, VG-2026-0142) |
| RF-08 | **[C23]** Monto, método, fecha y salón |
| RF-09 | **[C23]** Botón «Ir a Mi Evento» |
| RF-10 | **[C23]** El pago aparece en Pagos y en el presupuesto |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C8, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C23, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `POST /api/v1/bookings` | `009-bookings-create` |
| `GET /api/v1/payment-cards` | `019-get-payment-cards` |
| `POST /api/v1/payment-cards` | `020-post-payment-cards` |
| `POST /api/v1/bookings/:id/payments` | `070-post-bookings-id-payments` |
| `GET /api/v1/bookings/:id` | `067-get-bookings-id` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
