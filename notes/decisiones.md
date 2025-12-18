# Decisiones

## General
- Repo de practica con codigo nivel portfolio.
- Commits pequenos y ligados a cada hito.
- Apuntes breves: decisiones, gotchas, modelos mentales.

## Backend
- .NET 8 (LTS).
- Capas: Api / Application / Domain / Infrastructure.
- Empezar simple; profundizar despues (validacion, patrones, DDD).

## Frontend
- Angular ultima estable.
- Preferir patrones modernos (standalone, signals).

## Validacion por capas
- Entrada (API/UI): validar formato y campos requeridos para fallar rapido (400/422).
- Aplicacion: validar reglas del caso de uso y permisos.
- Dominio: invariantes siempre, incluso si se salta el resto (p.ej. Name no vacio, Guid no vacio).

## Validation strategy
- Domain: invariants (must always hold).
- Application/API: input validation.
- Infrastructure: database constraints via EF Fluent API.
