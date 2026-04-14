namespace Buildforge.Service.Domain.Event.Model;

public sealed class BuildChangedEvent : BaseEvent
{
    public required Build.Build Build { get; init; }
}