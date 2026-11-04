---
name: verificar-constitucion
description: Revisa que un spec, plan, task.md, PR o archivo de la app Android de VirGo cumpla constitution.md artículo por artículo y reporta las brechas. Úsala antes de aprobar un plan, antes de fusionar un PR y al cerrar un módulo.
---

# Verificar la alineación con la constitución

1. Lee `constitution.md` y anota su versión.
2. Lee los archivos que vas a revisar.
3. Llena esta tabla, una fila por artículo:

| Art. | Tema | Resultado | Evidencia o brecha |
|---|---|---|---|
| I | SDD: spec → plan → task → check; specs atómicos por pantalla; IDs `TNNN` por módulo | ✅ / ⚠️ / ❌ / ➖ | |
| II | Módulo base primero, sin pantallas del negocio | | |
| III | Kotlin + Compose, MVVM/UDF, capas, multi-módulo, Hilt | | |
| IV | Contrato con la API: `/api/v1`, Problem Details, `camelCase`, UTC, Bearer | | |
| V | Seguridad: HTTPS, tokens cifrados, R8, sin secretos, `.gitignore`, Gitleaks, permisos | | |
| VI | Pruebas unitarias, integración, UI, seguridad, mutación y humo; análisis estático | | |
| VII | Sistema de diseño fiel a la maqueta, accesibilidad, textos en `strings.xml` | | |
| VIII | Ruleset, CI en PR, release manual a GitHub Releases, JSON del backend y aprendizaje | | |
| IX | Observabilidad (Timber en *debug* y correlación) | | |
| X | Memoria y trazabilidad | | |
| XI | Agentes, skills y MCP permitidos | | |
| XII | Idioma y estilo (README en inglés) | | |

4. **Coherencia entre archivos.** Comprueba que:
   - Los subagentes y skills que menciona el `task.md` existan en `.opencode/` y en `AGENTS.md`.
   - Cada RF y cada CA del spec tenga al menos una tarea.
   - El contrato descrito coincida con la sección «Cambios de contrato» de `memoria.md` de este repositorio y de `VirGo-API`.
   - Exista el `check.md` y tenga una fila por cada CA y por cada tipo de prueba.
   - Los MCP de `opencode.json` sean los mismos de la tabla «Servidores MCP» de `AGENTS.md`.
5. **Resultado:** *Alineado* si no hay ningún ❌, o *No alineado* con la lista de correcciones necesarias.
6. Registra el resultado con `registrar-memoria`.
