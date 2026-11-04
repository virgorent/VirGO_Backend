---
name: registrar-memoria
description: Agrega una entrada a memoria.md de la app Android de VirGo al terminar una sesión, tarea o decisión, con fecha, autor, tareas, evidencia, decisiones y pendientes. Úsala al cerrar cualquier trabajo.
---

# Registrar en memoria.md

1. Abre `memoria.md` y lee la última entrada para no repetir información.
2. Agrega la entrada nueva **al final** de la sección «Bitácora», con este formato:

```markdown
### AAAA-MM-DD · <título corto>

- **Autor:** <persona o subagente>
- **Módulo / tareas:** <specs/NNN-…> · <T001, T002…>
- **Qué se hizo:** <1–3 viñetas>
- **Evidencia:** <comando y resultado, enlace al PR o al run de CI, URL>
- **Decisiones:** <decisión y motivo, o «ninguna»>
- **Costo:** <servicios externos usados y su costo, o «$0 / sin cambios»>
- **Aprendizaje:** <qué funcionó, qué falló y qué se mejora en la constitución, AGENTS.md, skills o subagentes, o «ninguno»>
- **Pendiente:** <siguiente paso o bloqueos>
```

3. Si hubo una decisión de arquitectura, agrégala también a la tabla «Decisiones (ADR)».
4. Si cambió el contrato con la API, anótalo en «Cambios de contrato» y en la memoria de VirGo-API.
5. No copies secretos, tokens ni datos personales en la memoria.
6. Al desplegar o publicar un spec, márcalo como *Desplegado*/*Publicado* en la tabla «Módulos», enlaza su `check.md` y propone las mejoras del campo «Aprendizaje» (las enmiendas a la constitución las aprueba el equipo, Art. XI.2).
