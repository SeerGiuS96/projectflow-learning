# ProjectFlow — Codemap (what lives where)

This document explains the intent of each project and folder.
Goal: be able to resume the repo quickly after weeks/months.

---

## Solution layout

- `projectflow/ProjectFlow.Api`
  - **Purpose**: HTTP API, authentication, Swagger, DI wiring.
  - **Must contain**: endpoints, auth endpoints, extension methods, configuration.
  - **Must NOT contain**: business rules, EF Core mappings, persistence logic.

- `projectflow/ProjectFlow.Application`
  - **Purpose**: use cases (commands/queries), DTOs, ports (interfaces).
  - **Must contain**: handlers, request/response models, validation, ports.
  - **Must NOT contain**: EF Core, ASP.NET types, database concerns.

- `projectflow/ProjectFlow.Domain`
  - **Purpose**: domain model and invariants.
  - **Must contain**: entities, enums, domain methods enforcing invariants.
  - **Must NOT contain**: EF Core attributes, controllers/endpoints, configuration.

- `projectflow/ProjectFlow.Infrastructure`
  - **Purpose**: persistence and external implementations.
  - **Must contain**: DbContext, EF mappings, migrations, repository implementations.
  - **Must NOT contain**: HTTP endpoints, use-case orchestration.

---

## ProjectFlow.Api (folders)

### `Auth/`
- Contains auth endpoints like:
  - `POST /auth/login` (returns JWT)
  - `GET /me` (requires JWT)
- Uses `ITokenService` to create tokens.
- Should stay thin: verify credentials, issue token, return response.

### `Endpoints/`
- Minimal API endpoints grouped by feature.
- Example:
  - `ProjectsEndpoints` exposes `GET /projects` (protected)
- Endpoints should call Application handlers, not implement logic.

### `Extensions/`
- Startup/DI helpers (service registrations, swagger setup, auth config).
- Keeps `Program.cs` small.

---

## ProjectFlow.Application (folders)

### `Projects/Ports/`
- Interfaces (ports) required by use cases.
- Example:
  - `IProjectRepository` (Add, GetAll)

Rule: Application defines the interface; Infrastructure implements it.

### `Projects/Queries/GetProjects/`
- Query use case: list projects
- Contains:
  - Query request model
  - Handler
  - Response DTO

Rule: DTOs are shaped per endpoint/use case; they do not mirror the entity graph.

---

## ProjectFlow.Infrastructure (folders)

### `Persistence/`
- `ProjectFlowDbContext` lives here.
- Applies EF configurations from assembly.

### `Configurations/`
- EF Core mappings (`IEntityTypeConfiguration<T>`)
- Keeps DbContext clean and scalable.

### `Migrations/`
- Generated EF migrations (SQLite in dev)

### `Projects/`
- Implementation of `IProjectRepository`
- Uses `ProjectFlowDbContext`

---

## Key dependency rules (Clean Architecture)

- Domain depends on nothing.
- Application depends on Domain only.
- Infrastructure depends on Application + Domain.
- Api depends on Application + Infrastructure.

If you see a violation, refactor immediately.

---

## Current implemented endpoints

- `POST /auth/login`
- `GET /me` (auth)
- `GET /projects` (auth)
- `POST /projects` (auth)  (if still present from Day 4)

---

## How to run
- Start API:
  - `dotnet run --project projectflow/ProjectFlow.Api`
- Use `requests.http` (repo root) to test:
  - run login, copy token into @token, then run other requests.
