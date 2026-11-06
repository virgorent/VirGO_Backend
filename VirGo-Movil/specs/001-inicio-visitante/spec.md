# Spec — 001 Inicio para visitantes

| Campo | Valor |
|---|---|
| Módulo | `001-inicio-visitante` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| A1 | Inicio para visitantes |
| A3 | Vista previa del salón |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[A1]** Mapa con tu ubicación: un círculo violeta con el ícono de una persona |
| RF-02 | **[A1]** Círculo punteado que marca el rango de búsqueda |
| RF-03 | **[A1]** Selector de rango de 2 km, 5 km y 10 km; el mapa se acerca o se aleja |
| RF-04 | **[A1]** Solo aparecen los salones que quedan dentro del rango |
| RF-05 | **[A1]** Cada pin muestra el nombre corto y la distancia en km, sin precio |
| RF-06 | **[A1]** Si dos pines se enciman, uno se muestra como ícono pequeño y su nombre aparece al tocarlo |
| RF-07 | **[A1]** Fondo con foto protagonista |
| RF-08 | **[A1]** Botón «Iniciar sesión» |
| RF-09 | **[A3]** Tarjeta con foto, nombre, calificación y capacidad |
| RF-10 | **[A3]** Aviso de que los precios, las fechas y los filtros se desbloquean con una cuenta |
| RF-11 | **[A3]** El botón «Ver más…» lleva al registro |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla A1, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla A3, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `GET /api/v1/venues` | `007-venues-catalog` |
| `GET /api/v1/venues/:id` | `024-get-venues-id` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
