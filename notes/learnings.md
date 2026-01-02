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

## 2026-01-02

### Domain rules vs validation vs policy (examples)

A rule can look "businessy" but still NOT be a domain invariant.

**Domain invariant (must always hold, model invalid otherwise)**
- WorkItem.Status = Done => CompletedAt != null
  - If broken, the entity state is invalid, regardless of UI/DB.
  - Enforced in Domain entity.

**Input validation (UX / request contract)**
- CreateWorkItem.Title required, max length 200
  - This is often a UI/API constraint: can change without changing the business meaning.
  - Enforced in Application (FluentValidation) so clients get a 400.

**Persistence constraint (storage level)**
- DB column length, indexes, unique keys
  - Prevents bad storage, not the source of truth for business rules.
  - Enforced in Infrastructure (EF Fluent API).

**Policy / variable business rule (changes by config/country/customer/date)**
- VAT/IVA rate, allowed priorities per plan, max items per project depending on subscription
  - Business-related but not invariant: it can change without the entity becoming "invalid".
  - Implement as a policy/strategy in Application (or Domain Service) and inject it.

Key idea:
- "Max length" is usually NOT a domain invariant unless the domain meaning requires it forever.
  Example: if external standard requires Project.Name <= 50 always, then it might be invariant.
  Otherwise treat it as input validation + DB constraint.

### Circular references in DTOs (and how to avoid them)

Circular references usually happen when DTOs mirror the entity graph (bidirectional navigation):
- ProjectDto has WorkItems[]
- WorkItemDto has ProjectDto
=> serialization loops (Project -> WorkItems -> Project -> ...)

Avoid by design:
- DTOs are for a specific request/response, not for mirroring the domain model.
- Prefer "shape per endpoint" (tailored DTOs), not "one DTO to rule them all".

Patterns:
1) Use IDs instead of nested objects for back-references:
   - WorkItemResponse: { id, projectId, title, status, ... }
2) Use "summary" DTOs for nesting:
   - ProjectResponse: { id, name, workItems: [ { id, title, status } ] }
3) Never include full parent inside child when parent already contains children.

Tools / checks:
- Spot cycles by inspecting DTO graph: parent -> child -> parent properties.
- System.Text.Json: cycles can be detected/handled, but the correct fix is DTO design (avoid cycles).

### Métodos de intención en el dominio (forzar invariantes)

Cuando una transición de estado tiene reglas de dominio, **no debe permitirse mediante setters públicos**.
El dominio debe obligar a pasar por un método que represente la intención del cambio.

Ejemplo:
- Un WorkItem solo puede pasar a Done si se establece CompletedAt.

Diseño correcto:
- Status y CompletedAt tienen setters privados.
- El único modo de finalizar un WorkItem es usando un método de dominio.

```csharp
public sealed class WorkItem
{
    public WorkItemStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public void MarkAsDone(DateTime completedAt)
    {
        Status = WorkItemStatus.Done;
        CompletedAt = completedAt;
    }
}
```

Regla práctica:
- Si una acción tiene reglas → método de dominio.
- Si es solo una asignación sin reglas → setter puede ser suficiente.

Esto garantiza que **no puedan existir estados inválidos** dentro del modelo de dominio.

---

### DTOs y referencias circulares: explicación definitiva

Las referencias circulares **NO dependen del número de DTOs**,
sino de la forma del grafo que construyen.

Un ciclo aparece cuando:
- Un DTO A contiene a B
- y B contiene de vuelta a A (directa o indirectamente)

Ejemplo con ciclo (incorrecto):
- ProjectResponse contiene WorkItemResponse[]
- WorkItemResponse contiene ProjectResponse  
=> Project → WorkItems → Project → ...

Ejemplo SIN ciclo (correcto):
- ProjectResponse contiene WorkItemSummaryResponse[]
- WorkItemSummaryResponse **NO** contiene ProjectResponse

```csharp
public sealed class ProjectResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public List<WorkItemSummaryResponse> WorkItems { get; init; } = new();
}

public sealed class WorkItemSummaryResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
```

Aquí:
- El padre contiene a los hijos
- Los hijos NO contienen al padre
- El grafo es un árbol → no hay ciclo

Si un endpoint necesita devolver un WorkItem con información del Project,
se usa un DTO distinto con un ProjectSummary (sin hijos):

```csharp
public sealed class ProjectSummaryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class WorkItemDetailsResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public ProjectSummaryResponse Project { get; init; } = new();
}
```

Regla definitiva:
- Un DTO representa una **RESPUESTA**, no el modelo de dominio.
- El grafo de DTOs debe ser **acíclico** (árbol o DAG).
- Nunca incluir el padre completo dentro del hijo si el padre ya contiene al hijo.

