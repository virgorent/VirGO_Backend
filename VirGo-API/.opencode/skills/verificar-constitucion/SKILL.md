---
name: verificar-constitucion
description: Revisa que un spec, plan, task.md, PR o archivo del backend de VirGo cumpla constitution.md artículo por artículo y reporta las brechas. Úsala antes de aprobar un plan, antes de fusionar un PR y al cerrar un módulo.
---

# Verificar la alineación con la constitución

1. Lee `constitution.md` y anota su versión.
2. Lee los archivos que vas a revisar.
3. Llena esta tabla, una fila por artículo:

| Art. | Tema | Resultado | Evidencia o brecha |
|---|---|---|---|
| I | SDD: spec → plan → task → check; specs atómicos; IDs `TNNN` por módulo | ✅ / ⚠️ / ❌ / ➖ | |
| II | Base primero; los specs funcionales solo se redactan hasta cerrar `000`–`002` | | |
| III | Capas, inyección de dependencias, persistencia por interfaces, ejecución dual | | |
| IV | Contrato: `/api/v1`, Problem Details, `camelCase`, UTC, OpenAPI | | |
| V | Seguridad: secretos, `.gitignore`, HTTPS, JWT, IAM, OIDC, CORS, logs, Gitleaks, Checkov | | |
| VI | Cinco tipos de prueba (unitarias, integración, seguridad, mutación, humo) y criterios verificables | | |
| VII | Costo cero: solo servicios permitidos, presupuesto, throttling | | |
| VIII | Ruleset, CI en PR, CD manual, humo, URL HTTPS y aprendizaje tras el despliegue | | |
| IX | Observabilidad | | |
| X | Memoria y trazabilidad | | |
| XI | Agentes, skills y MCP permitidos | | |
| XII | Idioma y estilo (README en inglés) | | |

4. **Coherencia entre archivos.** Comprueba que:
   - Los subagentes y skills que menciona el `task.md` existan en `.opencode/` y en `AGENTS.md`.
   - Cada RF y cada CA del spec tenga al menos una tarea.
   - Las rutas, nombres de proyectos y servicios coincidan en el spec, el plan, el `task.md` y `AGENTS.md`.
   - Exista el `check.md` y tenga una fila por cada CA y por cada tipo de prueba.
   - Ningún campo de texto quede sin longitud máxima (en el backend, `python db/tools/check_schema.py`).
   - Lo que se afirma se haya comprobado contra la realidad: base, repositorio y paquetes, no solo contra otro documento.
   - Los MCP de `opencode.json` sean los mismos de la tabla «Servidores MCP» de `AGENTS.md`.
5. **Resultado:** *Alineado* si no hay ningún ❌, o *No alineado* con la lista de correcciones necesarias.
6. Registra el resultado con la skill `registrar-memoria`.
