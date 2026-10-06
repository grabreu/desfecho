namespace Desfecho;

public static class ResultExtensions
{
    extension<T>(Result<T> result)
    {
        public IResult ToOk()
        {
            return result.Match(
                value => Results.Ok(value),
                errors => errors.ToProblem());
        }

        public IResult ToCreated(Func<T, string> location)
        {
            return result.Match(
                value => Results.Created(location(value), value),
                errors => errors.ToProblem());
        }
    }

    extension(Result result)
    {
        public IResult ToNoContent()
        {
            return result.Match(
                () => Results.NoContent(),
                errors => errors.ToProblem());
        }
    }

    // Task and ValueTask receivers use classic extension methods: CA2012 flags
    // ValueTask values used as the receiver of an extension block member.
    public static async Task<IResult> ToOk<T>(this Task<Result<T>> result)
    {
        return (await result).ToOk();
    }

    public static async Task<IResult> ToCreated<T>(this Task<Result<T>> result, Func<T, string> location)
    {
        return (await result).ToCreated(location);
    }

    public static async Task<IResult> ToNoContent<TResult>(this Task<TResult> result)
        where TResult : Result
    {
        return (await result).ToNoContent();
    }

    public static async ValueTask<IResult> ToOk<T>(this ValueTask<Result<T>> result)
    {
        return (await result).ToOk();
    }

    public static async ValueTask<IResult> ToCreated<T>(this ValueTask<Result<T>> result, Func<T, string> location)
    {
        return (await result).ToCreated(location);
    }

    public static async ValueTask<IResult> ToNoContent<TResult>(this ValueTask<TResult> result)
        where TResult : Result
    {
        return (await result).ToNoContent();
    }
}
