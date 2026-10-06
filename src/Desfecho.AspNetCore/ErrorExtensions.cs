namespace Desfecho;

public static class ErrorExtensions
{
    extension(Error error)
    {
        public IResult ToProblem()
        {
            return new[] { error }.ToProblem();
        }
    }

    extension(IReadOnlyList<Error> errors)
    {
        public IResult ToProblem()
        {
            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            return Problem(errors[0]);
        }
    }

    private static IResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(statusCode: statusCode, detail: error.Description);
    }

    private static IResult ValidationProblem(IReadOnlyList<Error> errors)
    {
        var errorsDictionary = errors
            .GroupBy(error => error.Property ?? string.Empty)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Description).ToArray());

        return Results.ValidationProblem(errorsDictionary);
    }
}
