# Spec — 012 IA Studio

| Campo | Valor |
|---|---|
| Módulo | `012-ia-studio` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Cliente |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| C15 | IA Studio – Estilo |
| E43 | IA Studio – Mesas |
| C16 | IA Studio – Personal |
| C17 | IA Studio – Ideas |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[C15]** Comparador antes/después con una línea que se arrastra |
| RF-02 | **[C15]** Temas: Boda, XV Años, Neón Party y Mexicano |
| RF-03 | **[C15]** Botón «Generar otra propuesta» |
| RF-04 | **[E43]** Tres distribuciones: Pista al centro, Máximo aforo e Imperial |
| RF-05 | **[E43]** Número de mesas y capacidad |
| RF-06 | **[E43]** Aviso si los invitados no caben |
| RF-07 | **[C16]** Meseros, seguridad y limpieza sugeridos según los invitados |
| RF-08 | **[C16]** Muestra la regla usada para el cálculo |
| RF-09 | **[C16]** Ajustable con − y + |
| RF-10 | **[C17]** Ideas de decoración del tema elegido con su costo estimado |
| RF-11 | **[C17]** Aviso si una idea rebasa el presupuesto de decoración |
| RF-12 | **[C17]** Indicador de si vas dentro o por encima del presupuesto |
| RF-13 | **[C17]** «Ver» muestra el desglose y ajustes sugeridos: menos horas o quitar un servicio |
| RF-14 | **[C17]** «Guardar en Mi Evento» guarda el salón, la fecha, el estilo, la distribución y el personal |
| RF-15 | **[C17]** Descarga del resumen del diseño |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla C15, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla E43, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** la pantalla C16, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-04 | **Dado** la pantalla C17, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-05 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-06 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `PATCH /api/v1/events/:id` | `046-patch-events-id` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
