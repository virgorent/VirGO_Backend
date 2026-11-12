# Estrategia de ramas

Se sigue el modelo del proyecto de referencia: una rama por etapa, con PR hacia `main`. Es una propuesta para el equipo.

## Ramas

| Rama | Para qué | Entra a |
|---|---|---|
| `main` | Lo aprobado; desde aquí se despliega | — |
| `etapa/NN-nombre` | Una etapa del proyecto | `main` por PR |
| `feat/NNN-modulo` | Un spec | `main` o su etapa |
| `fix/descripcion` | Una corrección | `main` por PR |
| `docs/descripcion` | Solo documentación | `main` por PR |
| `post-despliegue/NN` | Correcciones tras desplegar | `main` por PR |

## Etapas

| Etapa | Rama | Contenido |
|---|---|---|
| 1 | `etapa/01-arquitectura` | `000`, `010` |
| 2 | `etapa/02-endpoints` | `003` a `097` |
| 3 | `etapa/03-infraestructura` | `900` |
| 4 | `etapa/04-pipeline` | `901` |
| 5 | `etapa/05-movil` | Flujos de Móvil |

## Reglas

1. Toda rama sale de `main` actualizado.
2. Un PR por spec o por corrección.
3. CI en verde y revisión de otra persona antes de fusionar.
4. Si el CI falla, se corrige en la misma rama.
5. Las ramas se borran después de fusionarlas.

`rama-backend`, `rama-movil` y `rama-apis` quedan en desuso cuando el equipo adopte este modelo.
