# Learnings

Write only:
- things that were NOT obvious
- mistakes you don't want to repeat
- mental models that help you reason

Format:
- Date
- Topic
- 3–6 bullet points max

## 2025-12-18 

### Why private setters?
I didn't use private setters before.
Now I use them because:
- they protect invariants (no invalid state via random assignments)
- changes become explicit via methods (intent)
- testing is easier because rules are centralized

### Invariant
A rule that must always hold for an entity to be valid.
Not "cannot change", but "cannot be violated".

### Data Annotations vs Fluent API

I was confused about where to define field lengths.

- Data Annotations are convenient and widely used.
- They work well in many real projects.
- However, they mix technical constraints into the domain, which can blur responsibilities.

This helped me understand the difference between:
- business rules (domain)
- technical constraints (database / persistence)

## 2025-12-22 

### IDs and enums storage decisions

I wondered why we use Guid as primary keys instead of smaller numeric types.
- Guid allows creating entities without relying on the database.
- It scales better for future scenarios (distributed systems, imports).
- Trade-off: larger size and index cost.

I also questioned why enums are stored as int.
- Enums rarely have many values.
- Using int is the default and simplest option in .NET and EF Core.
- Using smaller types (byte/short) adds complexity (mappings, conversionesconsistencia) with little real benefit.

Decision: use Guid for IDs and int for enums.

### EF Core ToTable

Using `builder.ToTable("Projects")` explicitly defines the database table name.
This avoids relying on EF Core naming conventions and makes mappings clearer and more stable.

### EF Core mapping organization

Some projects put all mappings in DbContext.OnModelCreating.
Separating mappings into IEntityTypeConfiguration<T> files is cleaner because:
- DbContext stays small and focused
- mappings scale better as the model grows
- each entity mapping is easier to find and review

ApplyConfigurationsFromAssembly keeps registration automatic.


### EF Core mapping: common mistakes (and why our approach is cleaner)

Common mistakes I want to avoid:
- Putting all mappings inside DbContext.OnModelCreating: it grows into a "god file" and becomes hard to maintain.
- Relying only on conventions (no ToTable/HasKey/IsRequired): it works until it doesn't; explicit mappings reduce surprises.
- Forgetting required fields in the DB (e.g., Name/Title): domain and DB end up inconsistent.
- Mixing EF concerns into Domain using Data Annotations: convenient but blends responsibilities.
- Not applying configurations automatically: registering each mapping manually doesn’t scale.

What we do instead:
- Keep mappings in Infrastructure using IEntityTypeConfiguration<T>
- ApplyConfigurationsFromAssembly in DbContext
- Domain keeps invariants, Infrastructure defines DB constraints


### Optional properties mapping in EF Core

Optional properties do not need explicit configuration in Fluent API.
Defining them explicitly (even without constraints):

- shows that the property was intentionally considered
- serves as an anchor for future changes (index, conversion, default value)
- makes the mapping more explicit and readable

It is optional, but often useful for important fields.

### EF Core materialization vs domain constructors

EF Core can fail to create entities if constructors contain parameters that cannot be bound to mapped properties.
A common clean approach:
- add a private parameterless constructor for EF
- keep the public constructor focused on domain invariants
- avoid passing system fields like CreatedAt from outside (use UtcNow or an IClock later)


### Data persistence

Data persistence means that application data survives application restarts.
Without persistence, all data would be lost when the process stops.

In this project, persistence is handled by:
- Infrastructure layer
- EF Core (DbContext, mappings, migrations)
- Database (SQL Server LocalDB)

Persistence is responsible for how and where data is stored,
not for business rules or validation.

Separating persistence from the domain allows changing the storage
technology without impacting business logic.


## 23/12/2025

### Data persistence

Persistence means storing data outside the application process so it survives restarts.
In this project persistence lives in the Infrastructure layer using EF Core and a database.
It defines how and where data is stored, not business rules.

### Middleware

In ASP.NET Core, middleware is code executed during the HTTP request/response pipeline
(e.g. authentication, logging).

In infrastructure contexts, middleware can also mean network components
(API gateways, proxies) that connect different networks.
Same word, different level.

### Validation types and vocabulary

**Domain**: the business/problem space (projects, work items, invoices...). Not technology.

**Business rule**: a rule that describes how the domain works.

**Domain invariant**: a business rule that must always hold. If broken, the model is invalid.
Examples:
- Project name cannot be empty.
- WorkItem in Done must have CompletedAt.
- Only valid statuses are allowed.

**Input validation (API/Application)**: validates incoming requests for UX and early feedback (400 errors).
Examples:
- Title required, max length 200
- Description max length 1000

**Persistence constraints (Infrastructure/DB)**: storage-level rules enforced by the database schema.
Examples:
- HasMaxLength(200), indexes, unique constraints, enum conversions.

**Policy (variable business rule)**: rules that can change by country/date/customer/config (not invariants).
Example: VAT/IVA rate calculation should live in Application as a policy/strategy, not hardcoded in domain entities.

**Infrastructure rules**: rate limiting, auth, CORS, logging; not domain rules.

## 2025-12-24

### EF Core provider switch

- LocalDB no arrancaba; usar SQLite desbloquea el dev sin tocar Domain/Application.
- Las migraciones son especificas del proveedor: se regeneraron para SQLite.
- El proveedor se decide en API por config ("Database:Provider"), no en Infrastructure.
- Mantener dos connection strings (Sqlite/SqlServer) facilita el cambio sin tocar codigo.

