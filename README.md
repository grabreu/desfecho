# Desfecho

[![CI](https://github.com/grabreu/desfecho/actions/workflows/ci.yml/badge.svg)](https://github.com/grabreu/desfecho/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Desfecho.svg?style=flat-square&logo=nuget&label=Desfecho)](https://www.nuget.org/packages/Desfecho)
[![NuGet](https://img.shields.io/nuget/v/Desfecho.AspNetCore.svg?style=flat-square&logo=nuget&label=Desfecho.AspNetCore)](https://www.nuget.org/packages/Desfecho.AspNetCore)
[![License](https://img.shields.io/github/license/grabreu/desfecho?style=flat-square)](LICENSE)

A simple and lightweight Result pattern implementation for .NET.

```bash
dotnet add package Desfecho
dotnet add package Desfecho.AspNetCore
```

## Why

Stop throwing exceptions for expected failures. Return a `Result<TValue>` instead — it's either a value or one or more `Error`s.

```cs
public Result<TodoItem> CompleteTodoItem(Guid id)
{
    var todoItem = _todoItems.Find(id);

    if (todoItem is null)
    {
        return Error.NotFound("TodoItem.NotFound", $"Todo item '{id}' was not found.");
    }

    todoItem.Complete();
    return todoItem;
}

var result = CompleteTodoItem(id);

var message = result.Match(
    todoItem => $"Completed '{todoItem.Title}'.",
    errors => errors[0].Description);
```

`Result<TValue>` converts implicitly from `TValue`, `Error` and `List<Error>`, so returning one is all you need — no wrapping, no `new`.

## Errors

An `Error` is a `Code`, a `Description`, and a `Type`:

```cs
Error.Validation("TodoItem.Title", "Title is required.");
Error.NotFound("TodoItem.NotFound", "Todo item was not found.");
Error.Conflict("TodoItem.Conflict", "Todo item already exists.");
```

`Type` is one of `Validation`, `NotFound` or `Conflict` — used by `Desfecho.AspNetCore` to pick an HTTP status code.

## ASP.NET Core

`Desfecho.AspNetCore` turns a `Result<TValue>` into a minimal API `IResult`, mapping errors to `ProblemDetails` (`Validation` → 400, `NotFound` → 404, `Conflict` → 409).

```cs
app.MapPost("/todo-items", async (CreateTodoItemRequest request, ISender sender, CancellationToken ct) =>
{
    var result = await sender.Send(new CreateTodoItemCommand(request.TodoListId, request.Title), ct);
    return result.ToCreated(value => $"/todo-items/{value.Id}");
});
```

`ToOk()`, `ToCreated(location)` and `ToNoContent()` cover the common cases; `errors.ToProblem()` is available directly for anything else.

## Packages

| Package | Contents |
| --- | --- |
| [`Desfecho`](src/Desfecho) | `Result<TValue>`, `Error`, `ErrorType` |
| [`Desfecho.AspNetCore`](src/Desfecho.AspNetCore) | `Result<TValue>` → minimal API `IResult` |

## License

Licensed under the [MIT License](LICENSE).
