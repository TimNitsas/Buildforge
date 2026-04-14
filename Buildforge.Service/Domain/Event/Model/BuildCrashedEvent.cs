namespace Buildforge.Service.Domain.Event.Model;

public sealed class BuildCrashedEvent : BaseEvent
{
    public required Provider.Crash.Crash Data { get; init; }
}