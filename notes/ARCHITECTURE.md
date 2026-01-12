# ProjectFlow — Architecture

## Goal
Learning project to practice Clean Architecture + Clean Code on a real .NET 8 API (Angular later).

## Layers (what goes where)

### ProjectFlow.Domain
- Domain entities and rules (invariants).
- No EF Core, no ASP.NET, no configuration.
- Examples:
  - `Entities/Project`
  - `Entities/WorkItem`

### ProjectFlow.Application
- Use cases (Commands/Queries) + DTOs.
- Defines **ports** (interfaces) needed by use cases.
- No EF Core, no ASP.NET.
- Examples:
  - `Projects/Queries/GetProjects/*`
  - `Projects/Ports/IProjectRepository`

### ProjectFlow.Infrastructure
- EF Core DbContext, mappings, migrations.
- Implements Application ports.
- Examples:
  - `Persistence/ProjectFlowDbContext`
  - `Projects/ProjectRepository`
  - `Configurations/*`
  - `Migrations/*`

### ProjectFlow.Api
- Endpoints, auth, swagger, dependency injection (composition root).
- No business rules, no EF mapping.
- Examples:
  - `Auth/AuthEndpoints`
  - `Endpoints/ProjectsEndpoints`
  - `Extensions/*`

## Dependency Rules (non-negotiable)
- Domain depends on nothing.
- Application depends on Domain only.
- Infrastructure depends on Application + Domain.
- Api depends on Application + Infrastructure.

## Validation responsibilities
- Domain: invariants (must always hold).
- Application/API: input validation (fast feedback).
- Infrastructure: DB constraints (Fluent API).

## Adding a new use case (template)
1. Domain: only if a new invariant or domain behavior is required.
2. Application:
   - Create `Command/Query`
   - Create `Handler`
   - Create request/response DTOs
   - Use ports (interfaces) for persistence/external deps
3. Infrastructure:
   - Implement required port(s) via EF Core
4. Api:
   - Add endpoint
   - Wire DI
   - Protect with auth if needed
5. Update:
   - `requests.http`
   - tests (Day 6+)
