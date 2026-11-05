# Checklist de calidad para Pull Requests

## Antes de abrir el PR

### Spec
- [ ] El cambio tiene un spec aprobado y cita su tarea.
- [ ] Si cambió el alcance, primero se actualizó el spec o el plan.

### Código
- [ ] Compila sin advertencias nuevas.
- [ ] No hay secretos, código muerto ni `TODO` sin contexto.

### Datos
- [ ] Todo texto de entrada tiene longitud máxima en el DTO y en `db/schema/`.
- [ ] `python db/tools/check_schema.py` pasa.

### Pruebas
- [ ] Unitarias, integración y seguridad en verde.
- [ ] Cada campo nuevo se prueba en el límite, en el límite + 1 y con 1500 caracteres.

### Seguridad
- [ ] Los endpoints nuevos tienen autorización y límite de solicitudes.
- [ ] Los errores no exponen trazas ni datos personales.

### Contrato
- [ ] OpenAPI actualizado.
- [ ] Cambio registrado en la memoria de ambos proyectos.

## Durante la revisión

- [ ] Otra persona leyó el cambio y lo comparó con el spec.
- [ ] Los hallazgos de Semgrep, SonarCloud, ZAP y CodeQL están resueltos o justificados.

## Después de fusionar

- [ ] `check.md` y `memoria.md` actualizados, con lo aprendido.
- [ ] Despliegue manual y humo en verde, si aplica.
