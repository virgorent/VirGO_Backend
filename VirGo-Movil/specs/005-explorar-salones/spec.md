# Spec — 005 Explorar salones

| Campo | Valor |
|---|---|
| Módulo | `005-explorar-salones` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| B8 | Explorar (mapa) |
| C9 | Explorar (lista) |
| C10 | Filtros |
| C11 | Recomendados para ti |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[B8]** Mapa con los salones cercanos y tu ubicación |
| RF-02 | **[B8]** Tarjeta inferior con foto, capacidad, precio y distancia del salón seleccionado |
| RF-03 | **[B8]** Buscador que abre los filtros |
| RF-04 | **[B8]** Categorías: Jardines, Alberca, Elegante, Haciendas e Infantil |
| RF-05 | **[B8]** Botón «Para ti», con recomendaciones |
| RF-06 | **[B8]** Acceso a Proveedores |
| RF-07 | **[B8]** Campana de notificaciones con el número de avisos sin leer |
| RF-08 | **[B8]** Botón de favorito (corazón) en cada salón |
| RF-09 | **[C9]** Botón para cambiar entre mapa y lista |
| RF-10 | **[C9]** Tarjetas con foto, calificación, capacidad, precio y distancia |
| RF-11 | **[C9]** Orden por Recomendados, Cercanía, Precio o Calificación |
| RF-12 | **[C10]** Zona o nombre del salón |
| RF-13 | **[C10]** Número de invitados |
| RF-14 | **[C10]** Presupuesto para el salón |
| RF-15 | **[C10]** Distancia máxima |
| RF-16 | **[C10]** Fecha disponible |
| RF-17 | **[C10]** Orden de resultados |
| RF-18 | **[C10]** Solo muestra los salones que cumplen todos los filtros |
| RF-19 | **[C10]** Avisa cuando ningún salón cumple |
| RF-20 | **[C10]** Botón para limpiar los filtros |
| RF-21 | **[C11]** Recomendaciones según el tipo de evento, los invitados y el presupuesto |
| RF-22 | **[C11]** Porcentaje en que «encaja» cada salón, con el motivo |
| RF-23 | **[C11]** Insignia de Estimación IA |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla B8, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C9, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C10, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** la pantalla C11, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-05 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-06 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/venues` | `007-venues-catalog` |
| `GET /api/v1/notices` | `090-get-notices` |
| `POST /api/v1/favorites` | `082-post-favorites` |
| `DELETE /api/v1/favorites/:targetType/:targetId` | `083-delete-favorites-targettype-targetid` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
