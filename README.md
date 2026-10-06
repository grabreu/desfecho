# Desfecho

[![CI](https://github.com/grabreu/desfecho/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/grabreu/desfecho/actions/workflows/ci.yml)
[![CD](https://github.com/grabreu/desfecho/actions/workflows/cd.yml/badge.svg?branch=main)](https://github.com/grabreu/desfecho/actions/workflows/cd.yml)
[![NuGet](https://img.shields.io/nuget/v/Desfecho.svg?style=flat-square&logo=nuget&label=Desfecho)](https://www.nuget.org/packages/Desfecho)
[![NuGet](https://img.shields.io/nuget/v/Desfecho.AspNetCore.svg?style=flat-square&logo=nuget&label=Desfecho.AspNetCore)](https://www.nuget.org/packages/Desfecho.AspNetCore)
[![License](https://img.shields.io/github/license/grabreu/desfecho?style=flat-square)](LICENSE)

A simple and lightweight Result pattern implementation for .NET.

```bash
dotnet add package Desfecho
dotnet add package Desfecho.AspNetCore
```

## Tech stack

.NET 10 · xUnit v3 · Shouldly

## Why

Stop throwing exceptions for expected failures. Return a `Result<TValue>` instead: it's either a value or one or more `Error`s.

```cs
public Result<TodoItem> CompleteTodoItem(Guid id)
{
    var todoItem = _todoItems.Find(id);

    if (todoItem is null)
    {
        return Result.NotFound($"Todo item '{id}' was not found.");
    }

    todoItem.Complete();
    return todoItem;
}

public Result DeleteTodoItem(Guid id)
{
    return _todoItems.Remove(id)
        ? Result.Success()
        : Result.NotFound($"Todo item '{id}' was not found.");
}
```

`Result<TValue>` converts implicitly from `TValue`, `Error` and `ErrorList`, so returning one is all you need, no wrapping, no `new`. `Result` is the same without a value.

## Consuming

```cs
var message = result.Match(
    todoItem => $"Completed '{todoItem.Title}'.",
    errors => errors[0].Description);

if (result.TryGetValue(out var todoItem)) { ... }

var dto = result
    .Then(todoItem => todoItem.Archive())      // returns Result<T> or Result
    .Map(todoItem => new TodoItemDto(todoItem));
```

`Map` also works on `Task` and `ValueTask` of a `Result<TValue>`, so a handler call can be mapped and converted in one expression:

```cs
sender.Send(command, ct).Map(SignInResponse.From).ToOk();
```

`Value`, `Error` and `Errors` throw if the result is the other case; prefer `Match` or `TryGetValue` / `TryGetErrors`.

## Errors

An `Error` is a `Type`, a `Description`, and an optional `Property` (set for validation errors):

```cs
Result.Invalid("Title", "Title is required.");      // ErrorType.Validation
Result.Unauthorized("Authentication is required."); // ErrorType.Unauthorized
Result.Forbidden("You do not have access.");        // ErrorType.Forbidden
Result.NotFound("Todo item was not found.");        // ErrorType.NotFound
Result.Conflict("Todo item already exists.");       // ErrorType.Conflict
Result.Failure("Something went wrong.");            // ErrorType.Failure
```

An `ErrorList` holds several errors. `Result.Invalid` also takes an existing property-to-messages map, e.g. from FluentValidation:

```cs
var validation = await validator.ValidateAsync(request, ct);

if (!validation.IsValid)
{
    return Result.Invalid(validation.ToDictionary());
}
```

`result.Error` is the first error, `result.Errors` all of them.

## ASP.NET Core

`Desfecho.AspNetCore` turns a `Result` into a minimal API `IResult`, mapping errors to `ProblemDetails` (`Validation` → 400, `Unauthorized` → 401, `Forbidden` → 403, `NotFound` → 404, `Conflict` → 409, `Failure` → 500). When all errors are `Validation`, the response is a validation problem grouped by `Property`; otherwise it describes the first error..

```cs
app.MapPost("/todo-items", (CreateTodoItemRequest request, ISender sender, CancellationToken ct) =>
    sender.Send(new CreateTodoItemCommand(request.TodoListId, request.Title), ct)
        .ToCreated(value => $"/todo-items/{value.Id}"));
```

`ToOk()`, `ToCreated(location)` and `ToNoContent()` work on `Result`/`Result<TValue>` and on `Task` of them; `ToProblem()` on an `Error` or `Errors` is available directly for anything else.

## Packages

| Package | Contents |
| --- | --- |
| [`Desfecho`](src/Desfecho) | `Result`, `Result<TValue>`, `Error`, `ErrorList`, `ErrorType` |
| [`Desfecho.AspNetCore`](src/Desfecho.AspNetCore) | `Result<TValue>` → minimal API `IResult` |

## Development

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
dotnet format --verify-no-changes --severity info
```

## Deployment

Versioning, changelog, and NuGet publish are automated by [release-please](https://github.com/googleapis/release-please), tracking `Desfecho` and `Desfecho.AspNetCore` as independent packages: merges to `main` update a release PR from Conventional Commits, and merging that PR tags the release and publishes the affected package(s) to NuGet via GitHub Actions.

## License

Licensed under the [MIT License](LICENSE).
