---
name: sdd-flujo
description: Crea o revisa los archivos spec.md, plan.md, task.md y check.md de un módulo atómico de la app Android de VirGo siguiendo Spec Driven Development y constitution.md. Úsala al iniciar un módulo nuevo o al revisar si un spec, plan o lista de tareas está completo.
---

# Flujo SDD para un módulo

## 1. Preparar

1. Lee `constitution.md` y `AGENTS.md`.
2. Elige el nombre de la carpeta: `specs/NNN-nombre-en-minusculas/`, con un número consecutivo de tres dígitos.
   - El spec es **atómico** (Art. I.5): cubre una pantalla o un flujo corto. Si el alcance dice «y además…», divídelo en dos specs.
   - Revisa qué MCP necesita el módulo y confirma que están instalados (`opencode.json` y `AGENTS.md`).
3. Confirma con la persona responsable el alcance del módulo antes de escribir.

## 2. `spec.md`: qué y por qué

Secciones obligatorias:

1. **Encabezado:** módulo, versión, estado (Borrador, En revisión o Aprobado), fecha y artículos de la constitución que aplican.
2. **Contexto y objetivo.**
3. **Historias**, en formato `Como … quiero … para …`, con un ID (`HT-NN` técnicas o `HU-NN` de usuario).
4. **Requerimientos funcionales (RF-NN)** y **no funcionales (RNF-NN)**, verificables y sin tecnología.
5. **Criterios de aceptación (CA-NN)** en formato *Dado / Cuando / Entonces*.
6. **Fuera de alcance.**
7. **Supuestos, dependencias y preguntas abiertas.**

> El spec no menciona clases, librerías ni módulos de Gradle. Eso va en el plan.

## 3. `plan.md`: cómo

Secciones obligatorias:

1. **Resumen técnico** y enlace al spec.
2. **Contexto técnico:** lenguaje, frameworks, almacenamiento, pruebas, plataforma y restricciones.
3. **Constitution Check:** una tabla con cada artículo marcado como ✅ Cumple, ➖ No aplica o ⚠️ Justificado (con su justificación).
4. **Estructura de proyectos y carpetas.**
5. **Decisiones técnicas (ADR)** con las alternativas descartadas.
6. **Diseño:** módulos de Gradle, capas y contrato; en módulos funcionales, también las pantallas y los endpoints que consumen.
7. **CI/CD, distribución y costo.**
8. **Estrategia de pruebas.**
9. **Riesgos y mitigaciones.**
10. **Fases** que agrupan las tareas.

## 4. `task.md`: pasos ejecutables

- Cada tarea lleva un ID con el número del módulo (`000` → `T001…`, `001` → `T101…`, `003` → `T301…`) y una sola acción verificable de 30 minutos a 2 horas.
- Incluye tareas para los cinco tipos de prueba (unitarias, integración, seguridad, mutación y humo), el PR, el despliegue o release manual y la entrega del contrato.
- Columnas: `ID | Fase | Tarea | Depende de | Responsable (subagente) | Verificación | Estado`.
- Marca con `[P]` las tareas que pueden ir en paralelo.
- Estados: ⏳ Pendiente, 🔄 En curso, ✅ Hecha (con evidencia) y ⛔ Bloqueada (con motivo).
- Cada RF y cada CA del spec debe quedar cubierto por al menos una tarea. Agrega al final la matriz de trazabilidad.

## 5. `check.md`: evidencia

Secciones obligatorias, todas con una columna o casilla para la evidencia:

1. **Especificación:** atomicidad, matriz completa, Constitution Check sin ❌, preguntas abiertas resueltas, MCP listos.
2. **Criterios de aceptación:** una fila por CA con la prueba o captura que lo demuestra.
3. **Pruebas:** una fila por tipo (unitarias, integración, seguridad, mutación, humo; en Móvil también UI) con totales o ➖ justificado.
4. **PR e integración continua:** enlace al PR, checks en verde, revisión.
5. **Despliegue o release:** ejecución manual, commit, URL o versión.
6. **Memoria y aprendizaje:** entrada en `memoria.md` y mejoras propuestas a la constitución, `AGENTS.md`, skills y subagentes.

## 6. Revisar

Antes de pedir la aprobación, usa la skill `verificar-constitucion` sobre los cuatro archivos y agrega una entrada en `memoria.md`. El spec pasa a *Cerrado* solo cuando su `check.md` está completo (Art. I.7).
