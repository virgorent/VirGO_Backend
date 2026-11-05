# Plantillas SDD

Plantillas reutilizables para crear specs atómicos. Todo módulo nuevo parte de aquí.

| Archivo | Uso |
|---|---|
| `spec.md` | Qué se quiere, requisitos, criterios y controles de seguridad |
| `plan.md` | Cómo se hará, Constitution Check, ADR, DevSecOps y riesgos |
| `task.md` | Pasos ejecutables con dependencias, responsable y verificación |
| `check.md` | Evidencia de cierre: CA, pruebas, gates de seguridad y release |

## Reglas

- Un spec = una pantalla o un flujo corto (atómico).
- Nombres `NNN-<nombre-en-kebab-case>` (`000-arquitectura-base`).
- Estados: Borrador → En revisión → Aprobado → Publicado → Cerrado. En fases de gobernanza (`nv-XX`) se usa `Cumplido` como equivalente a `Cerrado`.
- Los specs de gobernanza (`nv-XX`) usan RF/CA y pueden omitir HT/RNF; los specs de funcionalidad (`NNN`) deben incluir HT/RNF.
- No ejecutar tareas sin aprobación explícita de `task.md`.
- Cada historia (HT), requisito (RF/RNF) y criterio (CA) debe tener al menos una tarea y una evidencia de verificación.
- Sin secretos ni datos personales en specs, planes ni bitácoras.
