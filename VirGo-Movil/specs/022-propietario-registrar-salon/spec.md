# Spec — 022 Registrar un salón

| Campo | Valor |
|---|---|
| Módulo | `022-propietario-registrar-salon` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Propietario |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| P3 | Paso 1: datos |
| P4 | Paso 2: capacidad y precios |
| P5 | Paso 3: revisar y publicar |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[P3]** Nombre, descripción (mínimo 20 caracteres) y dirección |
| RF-02 | **[P3]** Zona |
| RF-03 | **[P3]** Fotos JPG, PNG o WEBP de hasta 10 MB |
| RF-04 | **[P3]** Opción «Usar fotos de ejemplo» |
| RF-05 | **[P4]** Capacidad |
| RF-06 | **[P4]** Precio base por hora o por día |
| RF-07 | **[P4]** Horas mínimas |
| RF-08 | **[P4]** Categorías y amenidades |
| RF-09 | **[P4]** Servicios incluidos y adicionales con su precio |
| RF-10 | **[P5]** Resumen completo del salón |
| RF-11 | **[P5]** «Publicar»: el salón aparece en el mapa y en las búsquedas de los clientes |
| RF-12 | **[P5]** «Guardar borrador»: queda oculto |
| RF-13 | **[P5]** La primera publicación genera actividad de ejemplo: 2 solicitudes, 1 reservación pagada y 2 reseñas |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla P3, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla P4, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla P5, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `POST /api/v1/venues` | `026-post-venues` |
| `PATCH /api/v1/venues/:id` | `027-patch-venues-id` |
| `POST /api/v1/venues/:id/submit` | `028-post-venues-id-submit` |
| `POST /api/v1/venues/:id/publish` | `029-post-venues-id-publish` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
