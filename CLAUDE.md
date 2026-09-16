# Desfecho

## Repository

Two NuGet packages: [`Desfecho`](https://www.nuget.org/packages/Desfecho) — a lightweight Result pattern implementation for .NET — and [`Desfecho.AspNetCore`](https://www.nuget.org/packages/Desfecho.AspNetCore), which turns a `Result<TValue>` into a minimal API `IResult`.

Read `README.md` before making changes — it documents the actual public API (`Why`, `Errors`, `ASP.NET Core`, `Packages`).

## General Rules

- Keep changes scoped to the requested change.
- Prefer existing patterns over introducing new abstractions.
- Do not add dependencies unless they are necessary.
- Do not fill gaps with assumptions when the user hasn't given the information — ask, or mark it as pending.
- Do not claim a validation command passed unless it was actually run.
- Code, comments, commit messages, and documentation are always written in English.

## Git

- Do not create or switch branches unless explicitly requested.
- Do not create commits unless explicitly requested.
- Do not push unless explicitly requested.
- Keep commits focused on the requested change.
- Commit messages follow [Conventional Commits](https://www.conventionalcommits.org/) (`type: summary`).

## Documentation

### Audience

A developer evaluating whether to add this as a dependency. Not onboarding material — keep it concise and skimmable.

### Content Rules

- State facts concisely. Avoid unnecessary explanations or trailing rationale.
- Do not document information that is already obvious from the repository structure or configuration.
- Do not invent features, API shapes, or future direction — mark undecided things as TODO.
- Document a capability only after it is implemented and verified.
- Use proper Markdown headings (`##`, `###`), not bold text as headings.

---

## Project-Specific Guidelines

### Source

- `src/Desfecho/Result.cs` - the `Result<TValue>` type: implicit conversions from `TValue`, `Error` and `List<Error>`, `Match`.
- `src/Desfecho/Error.cs` - the `Error` type: `Code`, `Description`, `Type`, and the `Validation`/`Unauthorized`/`Forbidden`/`NotFound`/`Conflict` factory methods.
- `src/Desfecho/ErrorType.cs` - the `ErrorType` enum.
- `src/Desfecho.AspNetCore/MinimalApiResultExtensions.cs` - `ToOk()`, `ToCreated(location)`, `ToNoContent()`, `errors.ToProblem()`.

Every public type/method should have matching coverage in `tests/Desfecho.UnitTests` or `tests/Desfecho.AspNetCore.UnitTests`.

### Validation

Run `dotnet format --verify-no-changes --severity info`, `dotnet build --configuration Release`, and `dotnet test --configuration Release` before considering a change done — CI (`.github/workflows/ci.yml`) runs the same on push/PR to `main`.
