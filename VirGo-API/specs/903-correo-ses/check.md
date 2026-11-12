# Check — 903 Correo con Amazon SES

Cada casilla lleva evidencia. El módulo se marca *Cerrado* solo con todo en ✅ o ➖ justificado.

**Leyenda:** ✅ Cumple · ➖ No aplica (justificar) · ⏳ Pendiente

| Campo | Valor |
|---|---|
| Módulo | `903-correo-ses` |
| Estado del spec | Borrador |
| Revisó | — |
| Fecha de cierre | — |

## 1. Especificación y gobernanza

| Control | Evidencia | Estado |
|---|---|---|
| Spec, plan y tareas aprobados | PR con la revisión del equipo | ⏳ |
| Constitution Check sin ❌ | [plan.md](plan.md#3-constitution-check) | ⏳ |
| P-01 (remitente) respondida | `memoria.md` | ⏳ |

## 2. Criterios de aceptación

| CA | Evidencia | Estado |
|---|---|---|
| CA-01 Remitente verificado | Captura de SES | ⏳ |
| CA-02 Correo recibido | Captura del correo | ⏳ |
| CA-03 Reintento por mensaje | Prueba unitaria | ⏳ |
| CA-04 Sin código en logs | Prueba de seguridad | ⏳ |

## 3. Pruebas

| Tipo | Evidencia | Estado |
|---|---|---|
| Unitarias | `dotnet test tests/VirGo.UnitTests` | ⏳ |
| Seguridad | `dotnet test tests/VirGo.SecurityTests` | ⏳ |
| Humo | T90304 | ⏳ |
