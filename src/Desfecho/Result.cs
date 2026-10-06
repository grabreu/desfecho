using System.Diagnostics.CodeAnalysis;

namespace Desfecho;

public class Result
{
    private static readonly Result s_success = new(null);

    private readonly IReadOnlyList<Error>? _errors;

    private protected Result(IReadOnlyList<Error>? errors)
    {
        _errors = errors;
    }

    public bool IsSuccess => _errors is null;
    public bool IsError => !IsSuccess;

    public IReadOnlyList<Error> Errors => _errors ?? throw new InvalidOperationException("Result is successful; there are no errors.");
    public Error Error => Errors[0];

    public static implicit operator Result(Error error) => new([error]);
    public static implicit operator Result(ErrorList errors) => new(errors.Items);

    public static Result Success() => s_success;
    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    public static Error Invalid(string property, string description) => new(ErrorType.Validation, description, property);
    public static Error Unauthorized(string description) => new(ErrorType.Unauthorized, description);
    public static Error Forbidden(string description) => new(ErrorType.Forbidden, description);
    public static Error NotFound(string description) => new(ErrorType.NotFound, description);
    public static Error Conflict(string description) => new(ErrorType.Conflict, description);
    public static Error Failure(string description) => new(ErrorType.Failure, description);

    public static ErrorList Invalid(IEnumerable<KeyValuePair<string, string[]>> failures) =>
        new(failures.SelectMany(failure => failure.Value.Select(description => Invalid(failure.Key, description))));

    public bool TryGetErrors([NotNullWhen(true)] out IReadOnlyList<Error>? errors)
    {
        errors = _errors;
        return errors is not null;
    }

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onError)
    {
        return _errors is null ? onSuccess() : onError(_errors);
    }
}
