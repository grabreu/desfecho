namespace Desfecho;

public readonly struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly IReadOnlyList<Error>? _errors;

    public bool IsSuccess => _errors is null;
    public bool IsError => !IsSuccess;

    private Result(TValue value)
    {
        _value = value;
        _errors = null;
    }

    private Result(List<Error> errors)
    {
        _value = default;
        _errors = errors;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new([error]);
    public static implicit operator Result<TValue>(List<Error> errors) => new(errors);

    public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Result is an error; there is no value.");
    public IReadOnlyList<Error> Errors => IsError ? _errors! : throw new InvalidOperationException("Result is successful; there are no errors.");

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onError)
    {
        return IsSuccess ? onSuccess(_value!) : onError(_errors!);
    }
}
