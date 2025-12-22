# Guidelines
este archivo de guidelines se usa para reglas generales (clean code, arquitectura, validación)

## Validation responsibilities
- Domain: invariants (must always hold).
- Application/API: input validation (requests, DTOs).
- Infrastructure: database constraints.

## Property setters
- Prefer private setters to protect invariants.
- State changes must go through constructors or methods.

## Nullability
- Use nullable reference types intentionally.
- Optional domain concepts use `?`.
