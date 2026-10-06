namespace Desfecho;

public static class ResultAsyncExtensions
{
    public static async Task<Result<TResult>> Map<TValue, TResult>(
        this Task<Result<TValue>> result,
        Func<TValue, TResult> map)
    {
        return (await result).Map(map);
    }

    public static async ValueTask<Result<TResult>> Map<TValue, TResult>(
        this ValueTask<Result<TValue>> result,
        Func<TValue, TResult> map)
    {
        return (await result).Map(map);
    }
}
