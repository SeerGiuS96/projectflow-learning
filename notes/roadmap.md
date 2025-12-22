

## Project overview (mental model)

This project is a **personal project & task manager** used as a vehicle to learn
professional backend + frontend development.

The goal is NOT the app itself, but:
- domain modeling
- clean architecture
- testing
- API design
- frontend integration

The application is intentionally simple at first and will grow over time.



## Domain scope (Day 1)

We are modeling the **core domain only**, without:
- database
- API
- authentication
- frontend

Domain concepts:
- Project
- WorkItem (task)

Rules live in the domain:
- Project name cannot be empty
- WorkItem title cannot be empty
- WorkItem status is limited to Todo / Doing / Done
- When a WorkItem is marked as Done, CompletedAt is set

Out of scope for now:
- users
- assignments
- due dates
- tags
- comments



# Roadmap (Week 1)

- [x] Day 1: Backend foundation (.NET 8) — solution structure + domain baseline
- [ ] Day 2: Persistence — EF Core + migrations
- [ ] Day 3: Auth — JWT + testing baseline
- [ ] Day 4: Angular foundation — routing + services
- [ ] Day 5: Forms + API integration
- [ ] Day 6: Cleanup + interview prep (README, review, refactor)
- [ ] Day 7 (buffer): polish + gaps + mock interview

Notes:
- Keep commits small and tied to milestones.
- No filler code.
