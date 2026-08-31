namespace Desfecho.AspNetCore;

public static class MinimalApiResultExtensions
{
    public static IResult ToProblem(this IReadOnlyList<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Results.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    private static IResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(
            statusCode: statusCode,
            detail: error.Description,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = error.Code
            });
    }

    private static IResult ValidationProblem(IReadOnlyList<Error> errors)
    {
        var errorsDictionary = errors
            .GroupBy(error => error.Code)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Description).ToArray());

        return Results.ValidationProblem(errorsDictionary);
    }

    public static IResult ToOk<T>(this Result<T> result)
    {
        return result.Match(
            successValue => Results.Ok(successValue),
            errors => errors.ToProblem());
    }

    public static IResult ToCreated<T>(this Result<T> result, Func<T, string> location)
    {
        return result.Match(
            successValue => Results.Created(location(successValue), successValue),
            errors => errors.ToProblem());
    }

    public static IResult ToNoContent<T>(this Result<T> result)
    {
        return result.Match(
            _ => Results.NoContent(),
            errors => errors.ToProblem());
    }
}
