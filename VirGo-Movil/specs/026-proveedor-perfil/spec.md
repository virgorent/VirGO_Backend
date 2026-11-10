# Spec — 026 Perfil del proveedor

| Campo | Valor |
|---|---|
| Módulo | `026-proveedor-perfil` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Proveedor |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| S2 | Crear o editar el perfil |
| S6 | Galería de trabajos |
| S7 | Zonas de cobertura |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[S2]** Nombre del negocio y categoría |
| RF-02 | **[S2]** Descripción |
| RF-03 | **[S2]** Cobro por persona o por evento |
| RF-04 | **[S2]** Paquetes con nombre, precio y qué incluyen |
| RF-05 | **[S2]** Tiempo de respuesta |
| RF-06 | **[S2]** Validación: no se publica si falta un campo obligatorio, y se marca qué falta |
| RF-07 | **[S2]** Al publicar aparece en el catálogo de los clientes |
| RF-08 | **[S6]** Subir fotos JPG o PNG de hasta 10 MB |
| RF-09 | **[S6]** Los demás archivos se rechazan con un aviso |
| RF-10 | **[S6]** Opción de fotos de ejemplo |
| RF-11 | **[S7]** Selección de zonas en el mapa o en la lista |
| RF-12 | **[S7]** Se muestran en tu perfil público |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla S2, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla S6, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla S7, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-05 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `POST /api/v1/providers` | `037-post-providers` |
| `GET /api/v1/providers/me` | `036-get-providers-me` |
| `PATCH /api/v1/providers/me` | `038-patch-providers-me` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
