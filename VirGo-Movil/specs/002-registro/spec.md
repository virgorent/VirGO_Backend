# Spec — 002 Registro

| Campo | Valor |
|---|---|
| Módulo | `002-registro` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Plan | [plan.md](plan.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Pantallas

| Código | Pantalla |
|---|---|
| A2 | Registro |
| A4 | Registro con errores |

## 2. Requerimientos funcionales

| ID | Requerimiento |
|---|---|
| RF-01 | **[A2]** Selector de rol: Organizo un evento, Tengo un salón o Soy proveedor |
| RF-02 | **[A2]** Campos: nombre, correo, teléfono (+52, 10 dígitos) y contraseña |
| RF-03 | **[A2]** Muestra el salón que venías viendo, si llegaste desde «Ver más…» |
| RF-04 | **[A2]** Botones de Google y Apple de muestra («Próximamente») |
| RF-05 | **[A2]** Enlace para cambiar a «Iniciar sesión» |
| RF-06 | **[A4]** Los campos con error se marcan en rojo con su explicación |
| RF-07 | **[A4]** Valida el formato del correo, el teléfono de 10 dígitos y la contraseña de al menos 8 caracteres |
| RF-08 | **[A4]** No permite dos cuentas con el mismo correo |
| RF-09 | **[A4]** Al crear la cuenta entra a la pantalla principal de su rol, no al detalle del salón |

## 3. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | **Diseño:** solo componentes de `:core:designsystem`; textos en `strings.xml` (Art. VII). |
| RNF-02 | **Accesibilidad:** contraste AA, áreas táctiles de 48 dp y texto escalable. |
| RNF-03 | **Seguridad:** sin datos sensibles en logs; tokens solo en `TokenStore`. |

## 4. Criterios de aceptación

| ID | Criterio |
|---|---|
| CA-01 | **Dado** la pantalla A2, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-02 | **Dado** la pantalla A4, **cuando** se compara con la maqueta, **entonces** cumple todos sus requerimientos. |
| CA-03 | **Dado** una respuesta de error de la API, **cuando** llega, **entonces** se muestra un mensaje claro, sin texto técnico. |
| CA-04 | **Dado** que no hay conexión, **cuando** se abre la pantalla, **entonces** se muestra un estado de error con opción de reintentar. |

## 5. Fuera de alcance

- Lo que la maqueta simula (sección 7 del checklist), salvo que el spec de API lo implemente.

## 6. Dependencias

**Endpoints que consume:**

| Endpoint | Spec en `VirGo-API/specs` |
|---|---|
| `POST /api/v1/auth/register` | `005-auth-register` |
| `POST /api/v1/auth/email/verify` | `014-post-auth-email-verify` |

Sin URL y JSON probados del backend, la pantalla se prueba solo con MockWebServer (Art. VIII.6).
