# Plan — NNN <nombre-del-módulo>

| Campo | Valor |
|---|---|
| Módulo | `NNN-<nombre>` |
| Versión | 0.1.0 |
| Estado | Borrador |
| Fecha | AAAA-MM-DD |
| Spec | [spec.md](spec.md) · Tareas: [task.md](task.md) · Verificación: [check.md](check.md) |
| Constitución | vX.Y.Z |

## 1. Resumen técnico

<Cómo se implementará, en 3–6 viñetas.>

## 2. Contexto técnico

| Aspecto | Decisión |
|---|---|
| Lenguaje / framework | <...> |
| Persistencia | <...> |
| Seguridad | <...> |
| CI/CD | <...> |

## 3. Constitution Check

| Art. | Tema | Resultado | Cómo se cumple |
|---|---|---|---|
| <n> | <tema> | ✅ / ❌ | <...> |

> Un ❌ bloquea la aprobación del plan.

## 4. Estructura

```text
<árbol de carpetas y módulos>
```

## 5. Decisiones técnicas (ADR)

| ADR | Decisión | Alternativas descartadas | Motivo |
|---|---|---|---|
| ADR-01 | <...> | <...> | <...> |

## 6. Diseño

### 6.1 <componente>

<...>

### 6.2 Seguridad

<Controles: validación, autorización, secretos, cifrado y límites.>

## 7. DevSecOps y CI/CD

| Gate | Herramienta | Cuándo |
|---|---|---|
| Secretos | Gitleaks | PR y push a `main` |
| Dependencias | Dependabot + dependency review | Semanal y PR |
| SAST | CodeQL / linters | PR |
| Pruebas | <...> | PR |
| Release | <...> | Manual |

## 8. Estrategia de pruebas

| Nivel | Dónde | Qué cubre |
|---|---|---|
| Unitarias | <...> | <...> |
| Integración | <...> | <...> |
| Seguridad | <...> | <...> |
| Humo | <...> | <...> |

## 9. Riesgos y mitigaciones

| Riesgo | Mitigación |
|---|---|
| <...> | <...> |

## 10. Fases

| Fase | Contenido | Tareas |
|---|---|---|
| F0 | <...> | T001–T00N |
