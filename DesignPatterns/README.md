# Creational patterns

Run from the assignment root:

```bash
dotnet run --project DesignPatterns/src/PatternsLab.Runner
```

The runner demonstrates the corrected examples and throws if a check fails.

## Singleton: AppConfig

`AppConfig.GetInstance()` is initialized once by the CLR. Type initialization is
thread-safe, so callers do not need a hand-written locking implementation.
The constructor is private. Both services use `GetInstance()`, and `LoadCount` has a
private setter. Configuration loading still happens in the constructor.

The runner makes 100 parallel accesses **before creating either service**, then
checks object identity, one load, a shared theme, and constructor visibility.
Thread-safety here means constructing and publishing the instance once. Later
multi-step edits to mutable configuration would need their own synchronization
if the application started changing settings concurrently.

## Prototype: research completed before implementation

Sources read:

1. Microsoft Learn, `Object.MemberwiseClone`:
   https://learn.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone?view=net-8.0
2. Refactoring.Guru, Alexander Shvets, `Prototype`:
   https://refactoring.guru/design-patterns/prototype

### What problem does it solve?

Building every enemy from scratch reloads the model. Copying an existing enemy
avoids that cost and keeps client code independent of Orc and Elf constructors.

### What is a prototype?

It is an already configured object used as the starting point for new copies.
Here, an existing Orc or Elf is the prototype.

### What does Clone do?

`Enemy.Clone()` calls `MemberwiseClone()`. This creates a new object of the same
runtime type, copying all instance fields, including the private `_modelData`,
without running the slow constructor. It then creates a separate Weapon and a
separate Abilities list. `EnemyCopyHelper` remains as a small compatibility call
that simply delegates to `Clone`; it has no concrete-type checks.

### Shallow copy versus deep copy

A shallow copy makes a new enemy but still points to the same Weapon and list.
Changing those shared mutable objects would change both enemies. A deep copy also
copies those mutable objects, so changes stay with the clone. The model ID and
ability entries are strings; strings are immutable, so sharing their values is
safe. The list itself must still be copied.

The runner checks both enemy kinds, preserved ModelId, independent weapons, and
independent list additions and replacements. Five clones produce no extra model
loading messages. The optional registry was omitted to keep the solution small.

## Builder: CourseRegistration

The registration constructor is private. The nested `Builder` receives the three
required values: student email, course code, and access mode. Optional values use
clearly named fluent methods. `Build()` checks required fields and both group
rules, then returns a new registration. Reusing a builder does not mutate a
previous registration.

```csharp
var registration = new CourseRegistration.Builder(
    "student@mail.com", "SEF-101", "LiveGroup")
    .WithGroup("G1")
    .WithEmailWelcome(true)
    .Build();
```

A LiveGroup requires a nonblank group code. A VideosOnly rejects a nonblank group
code. These are the original lab's whitespace rules. Invalid access modes are also
rejected. `RegistrationCallSites` preserves all values from the two original
sample registrations, using the Builder instead of the long constructor.
