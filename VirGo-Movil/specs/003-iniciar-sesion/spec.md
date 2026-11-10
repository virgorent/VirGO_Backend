# Spec — 003 Iniciar y cerrar sesión

| Campo | Valor |
|---|---|
| Módulo | `003-iniciar-sesion` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| A5 | Iniciar sesión |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[A5]** Campos de correo y contraseña |
| RF-02 | **[A5]** Muestra el mensaje genérico «Correo o contraseña incorrectos», sin decir cuál de los dos falló |
| RF-03 | **[A5]** Al entrar lleva a la pantalla principal del rol |
| RF-04 | **[A5]** Enlace «¿Olvidaste tu contraseña?» |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla A5, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-03 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `POST /api/v1/auth/login` | `003-auth-login` |
| `POST /api/v1/auth/logout` | `004-auth-logout` |
| `POST /api/v1/auth/refresh` | `013-post-auth-refresh` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
