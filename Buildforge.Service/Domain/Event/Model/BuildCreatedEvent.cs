namespace Buildforge.Service.Domain.Event.Model;

public sealed class BuildCreatedEvent : BaseEvent
{
    public required Build.Build Build { get; init; }
}