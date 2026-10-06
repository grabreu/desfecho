using System.Diagnostics.CodeAnalysis;

namespace Desfecho;

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue value) : base(null)
    {
        _value = value;
    }

    private Result(ErrorList errors) : base(errors.Items)
    {
    }

    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Result is an error; there is no value.");

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(new ErrorList([error]));
    public static implicit operator Result<TValue>(ErrorList errors) => new(errors);

    public bool TryGetValue([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return IsSuccess;
    }

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onError)
    {
        return TryGetErrors(out var errors) ? onError(errors) : onSuccess(_value!);
    }

    public Result<TResult> Map<TResult>(Func<TValue, TResult> map)
    {
        return TryGetErrors(out var errors) ? new ErrorList(errors) : map(_value!);
    }

    public Result<TResult> Then<TResult>(Func<TValue, Result<TResult>> next)
    {
        return TryGetErrors(out var errors) ? new ErrorList(errors) : next(_value!);
    }

    public Result Then(Func<TValue, Result> next)
    {
        return TryGetErrors(out var errors) ? new ErrorList(errors) : next(_value!);
    }
}
