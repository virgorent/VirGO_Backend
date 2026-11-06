# Spec — 004 Recuperar contraseña

| Campo | Valor |
|---|---|
| Módulo | `004-recuperar-contrasena` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| A6 | Recuperar contraseña |
| A7 | Nueva contraseña |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[A6]** Pide el correo |
| RF-02 | **[A6]** Genera un código de 6 dígitos válido por 15 minutos; en la maqueta se muestra en pantalla |
| RF-03 | **[A7]** Pide el código y la nueva contraseña |
| RF-04 | **[A7]** Rechaza un código incorrecto o vencido |
| RF-05 | **[A7]** Confirma el cambio y regresa a iniciar sesión |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla A6, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla A7, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `POST /api/v1/auth/password/forgot` | `011-post-auth-password-forgot` |
| `POST /api/v1/auth/password/reset` | `012-post-auth-password-reset` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
