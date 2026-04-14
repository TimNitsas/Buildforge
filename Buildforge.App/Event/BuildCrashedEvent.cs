using Buildforge.Client.V1;

namespace Buildforge.App.Event;

public sealed class BuildCrashedEvent
{
    public required BuildCrash Inner { get; init; }
}