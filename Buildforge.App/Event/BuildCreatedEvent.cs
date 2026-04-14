using Buildforge.Client.V1;

namespace Buildforge.App.Event;

public sealed class BuildCreatedEvent
{
    public required Build Inner { get; init; }
}