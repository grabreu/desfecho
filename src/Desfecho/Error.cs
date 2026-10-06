namespace Desfecho;

public sealed record Error(ErrorType Type, string Description, string? Property = null);
