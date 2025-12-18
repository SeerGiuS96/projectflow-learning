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

## Data Annotations vs Fluent API

I was confused about where to define field lengths.

- Data Annotations are convenient and widely used.
- They work well in many real projects.
- However, they mix technical constraints into the domain, which can blur responsibilities.

This helped me understand the difference between:
- business rules (domain)
- technical constraints (database / persistence)


