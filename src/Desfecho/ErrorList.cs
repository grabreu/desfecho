namespace Desfecho;

public sealed record ErrorList
{
    public IReadOnlyList<Error> Items { get; }

    public ErrorList(IEnumerable<Error> items)
    {
        Items = [.. items];

        if (Items.Count == 0)
        {
            throw new ArgumentException("An error list must contain at least one error.", nameof(items));
        }
    }
}
