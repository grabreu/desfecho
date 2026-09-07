# Desfecho

## Repository

Two NuGet packages: [`Desfecho`](https://www.nuget.org/packages/Desfecho) — a lightweight Result pattern implementation for .NET — and [`Desfecho.AspNetCore`](https://www.nuget.org/packages/Desfecho.AspNetCore), which turns a `Result<TValue>` into a minimal API `IResult`.

Read `README.md` before making changes — it documents the actual public API (`Why`, `Errors`, `ASP.NET Core`, `Packages`).

## General Rules

- Keep changes scoped to the requested change.
- Prefer existing patterns over introducing new abstractions.
- Do not add dependencies unless they are necessary.
- Run formatting, build, and tests after changes.
- Do not change CI/CD configuration unless explicitly required.
- Do not claim a validation command passed unless it was actually run.
- Do not fill gaps with assumptions when the user hasn't given the information — ask, or mark it as pending.
- Code, comments, commit messages, and documentation are always written in English.

## Source

- `src/Desfecho/Result.cs` - the `Result<TValue>` type: implicit conversions from `TValue`, `Error` and `List<Error>`, `Match`.
- `src/Desfecho/Error.cs` - the `Error` type: `Code`, `Description`, `Type`, and the `Validation`/`Unauthorized`/`Forbidden`/`NotFound`/`Conflict` factory methods.
- `src/Desfecho/ErrorType.cs` - the `ErrorType` enum.
- `src/Desfecho.AspNetCore/MinimalApiResultExtensions.cs` - `ToOk()`, `ToCreated(location)`, `ToNoContent()`, `errors.ToProblem()`.

Every public type/method should have matching coverage in `tests/Desfecho.UnitTests` or `tests/Desfecho.AspNetCore.UnitTests`.

## Validation

- `dotnet format --verify-no-changes --severity info`
- `dotnet build --configuration Release`
- `dotnet test --configuration Release`

## Git

Follow the conventions defined in `CONTRIBUTING.md` for branches, commits, and pull requests.

- Do not create or switch branches unless explicitly requested by the user.
- Do not create commits unless explicitly requested by the user.
- Do not push changes unless explicitly requested by the user.
- Keep commits focused on the requested change.

## Releases

Versioning, changelog, and NuGet publish are automated by [release-please](https://github.com/googleapis/release-please) — see `CONTRIBUTING.md` for how commit types map to version bumps. Do not hand-edit `version` in the `.csproj` files or `.release-please-manifest.json`; release-please owns both after the initial `1.0.0` bootstrap.

## Documentation

Follow the documentation conventions below when creating or updating project documentation.

### Audience

Write for a developer evaluating whether to add this as a dependency.

Keep documentation concise and skimmable. Do not write onboarding tutorials unless explicitly requested.

### README Structure

`README.md` — badges, one-line description, install, `Why` (with a runnable example), `Errors`, `ASP.NET Core`, `Packages`, `License`.

Keep the existing README structure unless there is a clear reason to change it.

### Content Rules

- State facts concisely. Avoid unnecessary explanations or trailing rationale.
- Do not document information that is already obvious from the repository structure or configuration.
- Do not invent features, API shapes, or future direction.
- Document a capability only after it is implemented and verified.
- Use proper Markdown headings (`##`, `###`, etc.), not bold text as headings.
