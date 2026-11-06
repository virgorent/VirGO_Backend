# Spec — 011 Proveedores

| Campo | Valor |
|---|---|
| Módulo | `011-proveedores-cliente` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C19 | Catálogo de proveedores |
| B18 | Comparar proveedores |
| C20 | Perfil del proveedor |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C19]** Categorías: Catering, Música, Fotografía, Decoración y Mobiliario |
| RF-02 | **[C19]** Tarjetas con precio, calificación y zona |
| RF-03 | **[C19]** Botón «Comparar» en cada tarjeta, para marcar 2 o 3 |
| RF-04 | **[C19]** Aparecen los proveedores publicados por cuentas de proveedor |
| RF-05 | **[B18]** Tabla lado a lado con precio, paquetes, calificación, tiempo de respuesta y zonas |
| RF-06 | **[B18]** Resalta la mejor relación calidad-precio |
| RF-07 | **[C20]** Descripción y zonas de cobertura |
| RF-08 | **[C20]** Paquetes con precio y botón «Agregar» a Mi Evento |
| RF-09 | **[C20]** Galería de trabajos |
| RF-10 | **[C20]** Reseñas |
| RF-11 | **[C20]** Botones de chat y «Cotizar» |
| RF-12 | **[C20]** Favorito |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C19, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla B18, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C20, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/providers` | `034-get-providers` |
| `GET /api/v1/providers/:id` | `035-get-providers-id` |
| `GET /api/v1/providers/:id/reviews` | `073-get-providers-id-reviews` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
