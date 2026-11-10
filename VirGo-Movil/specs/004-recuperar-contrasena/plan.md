# Plan — 004 Recuperar contraseña

| Campo | Valor |
|---|---|
| Módulo | `004-recuperar-contrasena` |
| Versión | 1.0.0 |
| Estado | Borrador |
| Fecha | 2026-11-05 |
| Rol | Visitante |
| Fuente | `Checklist VirGo.md` y Figma «Pantallas VirGo» |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |

## 1. Resumen técnico

Módulo `:feature:recuperarcontrasena`, con pantallas Compose y un `ViewModel` por pantalla. Cada `ViewModel` expone un `UiState` inmutable por `StateFlow` y llama a casos de uso de `:core:domain`.

## 2. Constitution Check

| Art. | Resultado | Cómo se cumple |
|---|---|---|
| I y II | ✅ Cumple | Spec atómico por flujo. |
| III | ✅ Cumple | MVVM/UDF; los módulos `:feature:*` no dependen entre sí. |
| IV | ✅ Cumple | Solo consume `/api/v1`, con Bearer y `AppError`. |
| V | ✅ Cumple | Sin secretos ni datos sensibles en logs. |
| VI | ✅ Cumple | Sección 3. |
| VII | ✅ Cumple | Sistema de diseño y `strings.xml`. |

## 3. Estrategia de pruebas

| Tipo | Casos |
|---|---|
| Unitarias | `ViewModel` con Turbine y casos de uso |
| Integración | Repositorio con MockWebServer: éxito, 4xx/5xx y sin conexión |
| UI | Compose Testing de cada pantalla, con escala de fuente 2.0 |
| Seguridad | Sin datos sensibles en logs |
| Mutación | Pitest en casos de uso |
| Humo | La pantalla funciona contra la URL de producción |

## 4. Riesgos

| Riesgo | Mitigación |
|---|---|
| El endpoint aún no existe | MockWebServer con el contrato del spec de API |
