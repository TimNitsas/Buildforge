namespace Buildforge.Service.Domain.Event.Model;

public sealed class BuildChangedEvent : BaseEvent
{
    public required Repository.Build.Build Build { get; init; }
}