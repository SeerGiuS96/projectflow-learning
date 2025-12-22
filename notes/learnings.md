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



