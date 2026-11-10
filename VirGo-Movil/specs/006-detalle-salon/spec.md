# Spec — 006 Detalle del salón

| Campo | Valor |
|---|---|
| Módulo | `006-detalle-salon` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| B13 | Detalle del salón |
| C13 | Tour 360° |
| C14 | Reseñas del salón |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[B13]** Galería de fotos que se pasa tocando a la izquierda o a la derecha |
| RF-02 | **[B13]** Calificación que abre las reseñas |
| RF-03 | **[B13]** Disponibilidad: días libres en verde y ocupados en rojo, que no se pueden elegir |
| RF-04 | **[B13]** Calendario del mes completo con «Ver mes» |
| RF-05 | **[B13]** Las fechas bloqueadas por el propietario aparecen ocupadas |
| RF-06 | **[B13]** Duración ajustable con − y +, respetando las horas mínimas del salón |
| RF-07 | **[B13]** El precio cambia en fin de semana y en temporada alta |
| RF-08 | **[B13]** Servicios incluidos activos y servicios adicionales con interruptor y precio |
| RF-09 | **[B13]** El total se actualiza al instante |
| RF-10 | **[B13]** Proveedores aliados que recomienda el salón |
| RF-11 | **[B13]** Botones Solicitar cotización, Reservar y Simular con IA |
| RF-12 | **[B13]** Favorito y chat con el salón |
| RF-13 | **[C13]** Vista panorámica que se recorre arrastrando |
| RF-14 | **[C13]** Insignia de salón verificado |
| RF-15 | **[C13]** Botón para cerrar |
| RF-16 | **[C14]** Promedio y distribución de estrellas |
| RF-17 | **[C14]** Lista de opiniones con fecha |
| RF-18 | **[C14]** Respuestas del propietario |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla B13, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla C13, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C14, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/venues/:id` | `024-get-venues-id` |
| `GET /api/v1/venues/:id/availability` | `031-get-venues-id-availability` |
| `GET /api/v1/venues/:id/reviews` | `072-get-venues-id-reviews` |
| `POST /api/v1/conversations` | `085-post-conversations` |
| `POST /api/v1/favorites` | `082-post-favorites` |
| `DELETE /api/v1/favorites/:targetType/:targetId` | `083-delete-favorites-targettype-targetid` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
