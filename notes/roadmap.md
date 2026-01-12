# ProjectFlow — Roadmap real

## Project overview (mental model)

ProjectFlow is a **personal project & task manager** used as a vehicle to learn:
- Clean Architecture
- Clean Code
- API design
- Testing
- Angular integration

The app itself is intentionally simple.
The goal is engineering quality, not feature count.

---

## DONE

### Day 1 — Solution & Domain baseline
- .NET 8 solution structure
- Clean Architecture layers created
- Core domain modeled:
  - Project
  - WorkItem
- Domain invariants enforced via constructors/methods
- Private setters introduced

### Day 2 — Persistence
- EF Core integrated
- DbContext + mappings via IEntityTypeConfiguration
- Migrations created
- Provider switch to SQLite for dev
- Persistence isolated in Infrastructure

### Day 3 — Authentication baseline
- JWT minimal implementation
- Token generation service
- `/auth/login` endpoint
- `/me` protected endpoint
- Swagger configured with Authorize

### Day 4 — First use case (Command)
- CreateProject use case
- Application command + handler
- Domain factory (`Project.Create`)
- Infrastructure repository write
- Protected endpoint `POST /projects`

### Day 5 — Second use case (Query) + tooling
- GetProjects use case
- Introduction of Application port: `IProjectRepository`
- Infrastructure implementation using EF Core
- Protected endpoint `GET /projects`
- `requests.http` added for API testing (login + projects)

---

## NEXT

### Day 6 — Testing baseline
- Application unit tests (no EF, no API)
- Test handlers with mocked repositories
- Happy path + edge cases
- Clarify what is tested at which layer

### Day 7 — API hardening
- Consistent error handling (ProblemDetails)
- Status codes (201 / 400 / 401 / 404 / 409)
- Small refactors for naming & clarity
- Minimal logging

---

## LATER (Week 2+)
- WorkItems use cases
- Angular foundation
- API + frontend integration
