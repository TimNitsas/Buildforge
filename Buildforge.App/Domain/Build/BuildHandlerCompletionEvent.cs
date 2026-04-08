namespace Buildforge.App.Domain.Build;

public sealed class BuildHandlerCompletionEvent
{
    public required TimeSpan Time { get; init; }

    public required string Id { get; init; }
}