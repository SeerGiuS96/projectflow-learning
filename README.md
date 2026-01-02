# ProjectFlow Learning

Practice repo for Angular + .NET with a focus on clean architecture, testing, and professional workflow.


## What is this?

This repository contains a **learning project** focused on building a
clean, testable, and maintainable application using:

- .NET 8 (backend)
- Angular (frontend)

The application itself is a **personal project & task manager**, used as a
realistic scenario to practice professional software engineering skills.

The focus is on:
- domain modeling
- architecture
- testing
- API design
- frontend integration

This is not a tutorial project and avoids "hello world" examples.


## Structure
- notes/            -> roadmap + decisions + learnings
- mini-projects/    -> isolated exercises (Angular / .NET)
- projectflow/      -> the main app (API + later frontend)

## Principles
- No filler code.
- Small commits per milestone.
- Theory only when it supports practice.

## How to run (backend)
### Database provider
The API selects the EF Core provider via configuration:
- `Database:Provider`: `Sqlite` or `SqlServer`
- Connection strings:
  - `ProjectFlowDbSqlite`
  - `ProjectFlowDbSqlServer`

SQLite is the default for local dev. The database is a local file (e.g. `projectflow.dev.db`)
and is ignored by git.

### Migrations
From `projectflow/`:
```powershell
dotnet ef database update --project ProjectFlow.Infrastructure --startup-project ProjectFlow.Api
```

### Run
```powershell
dotnet run --project ProjectFlow.Api
```
