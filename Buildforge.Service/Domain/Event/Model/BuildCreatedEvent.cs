namespace Buildforge.Service.Domain.Event.Model;

public sealed class BuildCreatedEvent : BaseEvent
{
    public required Repository.Build.Build Build { get; init; }
}