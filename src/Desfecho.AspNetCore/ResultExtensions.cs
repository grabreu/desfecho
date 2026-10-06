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

    extension<T>(Task<Result<T>> result)
    {
        public async Task<IResult> ToOk()
        {
            return (await result).ToOk();
        }

        public async Task<IResult> ToCreated(Func<T, string> location)
        {
            return (await result).ToCreated(location);
        }
    }

    extension<TResult>(Task<TResult> result) where TResult : Result
    {
        public async Task<IResult> ToNoContent()
        {
            return (await result).ToNoContent();
        }
    }
}
