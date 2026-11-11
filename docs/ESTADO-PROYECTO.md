# VirGo: estado del proyecto

**Fecha:** 6 de noviembre de 2026
**Repositorio:** `virgorent/VirGO_Backend`, con API, base de datos y app móvil.

## Specs

142 specs en borrador: 98 de API (94 de endpoints), 11 de base de datos, 30 de Móvil y 3 globales. El orden sigue lo que pidió el profesor: gobernanza, arquitectura base, endpoints, infraestructura y pipeline. Ver [`backend/MAPA-SPECS.md`](backend/MAPA-SPECS.md) y [`frontend/MAPA-SPECS.md`](https://github.com/virgorent/VirGO_Movil/blob/main/docs/frontend/MAPA-SPECS.md).

## API (.NET 10)

- 94 endpoints especificados; 7 implementados: `register`, `login`, `logout`, `users/me`, `venues`, `quotes` y `bookings`.
- Están en el PR #5, en revisión, con 35 pruebas aprobadas y CI en verde.
- Infraestructura AWS con SAM, validada en CI; todavía sin desplegar.

## Base de datos (MongoDB Atlas)

- `VirGo-Database` define 22 colecciones, 37 índices y validadores.
- Su catálogo coincide 22 de 22 con la base real.
- 16 pruebas unitarias aprobadas.

## Móvil (Kotlin)

- Arquitectura multi-módulo, sistema de diseño y pantalla de login.
- 12 de 39 tareas hechas.
- Aún sin pruebas.

## Pruebas ejecutadas

| Proyecto | Unitarias | Integración | Seguridad |
|---|---|---|---|
| API | 2 de 2 | 30 de 30 | 3 de 3 |
| Base de datos | 16 de 16 | 20 sin ejecutar (requieren MongoDB local) | — |
| Móvil | 0 | 0 | 0 |

**Total:** 51 ejecutadas, 51 aprobadas. Las pruebas de la API usan persistencia en memoria; todavía no prueban contra MongoDB real. El detalle está en [`PRUEBAS.md`](PRUEBAS.md).

## DevSecOps

- Gitleaks, dependency review, Dependabot y protección de `main`.
- CodeQL y Checkov en el PR de la API.

## Lo que encontramos y corregimos

Al comparar los documentos contra la base real, vimos que la API y la base se habían separado. Definimos `VirGo-Database` como fuente de verdad y escribimos los specs para alinearlas.

## Lo que falta

1. Alinear la base con lo que necesita la API.
2. Corregir los errores de la revisión del PR #5.
3. Probar contra MongoDB real.
4. Conseguir accesos de AWS y hacer el primer despliegue con URL HTTPS.
5. Agregar pruebas a Móvil.
