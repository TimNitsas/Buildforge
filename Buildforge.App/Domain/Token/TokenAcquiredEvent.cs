namespace Buildforge.App.Event;

public sealed class TokenAcquiredEvent
{
    public required string Username { get; init; }
}