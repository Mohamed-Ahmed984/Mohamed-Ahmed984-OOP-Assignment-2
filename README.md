## Requirements

Install the **.NET 8 SDK** or a compatible newer SDK with .NET 8 targeting support.
There are no external NuGet packages. All examples are console applications with
in-memory data; no database or extra setup is needed.

Open a terminal inside this extracted folder. Build the three entry projects;
the first two also build their referenced class-library projects:

```bash
dotnet build SRP/src/SrpLab.Runner/SrpLab.Runner.csproj
dotnet build DesignPatterns/src/PatternsLab.Runner/PatternsLab.Runner.csproj
dotnet build Inheritance/src/LibrarySystem.csproj
```

Run:

```bash
dotnet run --project SRP/src/SrpLab.Runner
dotnet run --project DesignPatterns/src/PatternsLab.Runner
dotnet run --project Inheritance/src
```

In Visual Studio, open each runner's `.csproj` (or `LibrarySystem.csproj`) and run
it as the startup project. Build output will be created locally when you run it;
it is deliberately absent from this ZIP.

## Contents

- `SRP/Responsibilities.md`: original responsibility analysis and the reason for each new class.
- `SRP/src/SrpLab`: all ten refactored domains and their focused collaborators.
- `SRP/src/SrpLab.Runner`: the original sample scenarios adapted to the separated classes.
- `DesignPatterns/src/PatternsLab/Problems`: the three corrected creation examples.
- `DesignPatterns/src/PatternsLab.Runner`: demonstrations and executable checks.
- `DesignPatterns/README.md`: explanations, Prototype research sources, and Builder usage.
- `Inheritance/ClassDiagram.png`: the final UML class diagram.
- `Inheritance/src`: the complete library console project, with one domain class per file.

## Implementation notes

### SRP

The refactoring changes where operations live, while keeping the original lab's
behavior, constants, formats, ordering, and validation. The runner now explicitly
uses the appropriate formatting or policy class. It no longer describes the
completed code as intentional violations. No custom interfaces were introduced.

The source repositories inspected were:

- https://github.com/SimulationEG/csharp-srp-violations
  - Commit: `a5de7b3be5e9b23384263d21f354bcddbced0932`.
- https://github.com/SimulationEG/csharp-creational-patterns-lab
  - Commit: `4ead60576b6cdeb26e0d943152792b6de44c211b`.

### Design patterns

The Singleton has a private constructor and one CLR-initialized instance.
Prototype cloning retains private model state and separately copies mutable
Weapon and Abilities objects. Builder preserves all sample registration values
and checks the required LiveGroup/VideosOnly combinations in `Build()`.
No optional registry was added.

### Library

The UML was drawn before writing the library classes and updated to match the
final implementation. The four parent constructors are protected. The design
uses inheritance, constructor parameters, basic loops, and ordinary methods;
it does not use abstract/virtual/override members, custom interfaces, LINQ,
lambdas, or delegates in the library project.

Different loan limits, discounts, allowances, periods, and fee multipliers are
passed through `base(...)`. One parent method calculates monthly pay, and one
parent method calculates daily late fees. No runtime type checks are used.

All C# comments, including the commented-out non-compiling examples, were removed at your request. The access restrictions are still enforced.
Every expected runtime failure is caught, printed, and checked; an unexpected
success causes the demonstration to fail. It also demonstrates raises,
reassignment, pricing, withdrawal/restoration, date validation, identity
validation, both member limits, lost loans, discounts, and points.

Decisions where the request leaves room for interpretation:

- Dates use calendar days. Time-of-day is discarded for borrowing and returning.
- Discount percentages range from 0 to 100, inclusive.
- IDs and catalog numbers are trimmed, case-sensitive, and unique in the running
  process. Simple static lists enforce uniqueness; library operations are intended
  for this single-threaded console demonstration. Loan IDs are generated GUIDs.
- A lost item remains unavailable (`IsOnLoan` stays true) because it has not been
  returned. Lost loans do not count toward the Borrowed limit and earn no points.
  Recovery of lost items is outside the requested workflow.
- A fee is zero until a loan has a return date. No replacement charge was invented
  for lost items. Computed late fees use the item's current daily rate and the
  member's fixed discount, as requested; prices are not snapshotted on a loan.
- `Loans` returns a read-only wrapper over the private history, not the mutable
  backing list. Borrowing is the only operation that adds to that history.
- Internal item/loan methods coordinate state changes within the application;
  public staff actions are the intended return, loss, pricing, and circulation API.

## Verification actually executed

Verification date: 24 October 2026. Compiler SDK: .NET 8.0.415; runtime: .NET 8.0.21.

1. Compiled both original repositories and ran their original sample runners.
2. Compiled all five delivered projects (both libraries, both runners, and the
   library-system executable). Final delivered sources had **zero compiler errors
   and zero warnings**. The untouched original Prototype example has its original
   uninitialized-property warnings.
3. Ran the modified SRP runner successfully. A separate temporary comparison
   harness exercised all ten original/refactored classes across boundary cases,
   exporting, paging, coupons, SLA rules, applications, enrollment, kitchen work,
   invoice sequencing, repeated SKUs, grades, and appointment slots. Its **1,182
   output lines matched**. Only generated ICS UIDs and pager times were normalized.
   The original sample output was also compared, excluding generated auth hashes,
   clock values, and the updated introductory/completion text.
4. Ran the Patterns runner: **21 visible checks passed**, including 100 parallel
   first accesses to the Singleton, one configuration load, both enemy kinds,
   deep-copy independence, optional Builder fields, invalid combinations, and
   builder reuse. The concurrent identity checks also execute for every caller.
   The original five-orc construction took about 2,503 ms; five clones took 0 ms
   at stopwatch resolution in the observed run. Timing is illustrative; correct
   model identity and absence of extra loading are the important checks.
5. Ran the library demonstration: **62 checks passed**, including the 16-unit
   discounted DVD fee, loan limits, failed-action state preservation, legal
   transitions, positive values, unique IDs, read-only data, and reading points.
6. Separately compiled nine prohibited-access examples and confirmed that the
   compiler rejects all of them: creating each protected parent, changing a name,
   adding to Loans, setting availability, and invoking the two private constructors.

Environment limitation: a standard SDK build succeeded for SRP. Later CLI/MSBuild
calls encountered this container's process-ID versus `/proc` mismatch, before
project compilation. To finish verification, every project's sources were
compiled directly with the SDK's Roslyn C# compiler, .NET 8 reference assemblies,
and its normal project dependency references; the resulting programs were run
with the .NET runtime. The final full tree was therefore compiled and executed,
but the normal SDK/MSBuild build workflow could not be rechecked for all projects
inside this container. Use the ordinary `dotnet build` commands above locally.
No special build workaround or generated binaries are included in the deliverable.

## Submission

Branch: `assignment/1-6`

PR title: `[S1-A6] OOP Assignment 2`

LeetCode submission: https://leetcode.com/submissions/detail/2180759499/

The source folders follow the PDF layout. `submission/assignment/README.md` indexes
the assignment work, and `submission/leetcode/account.md` records the real
submission link for the academy checker. LinkedIn was excluded at the student's
request, so the academy's LinkedIn check is not satisfied.

Student ID still needs to be filled in.
