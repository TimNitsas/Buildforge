using Buildforge.Client.V1;

namespace Buildforge.App.Event;

public sealed class BuildChangedEvent
{
    public required Build Inner { get; init; }
}