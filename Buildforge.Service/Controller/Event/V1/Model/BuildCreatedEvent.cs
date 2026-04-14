namespace Buildforge.Service.Controller.Event.V1.Model;

public sealed class BuildCreatedEvent : Event
{
    public required Controller.Build.V1.Model.Build Data { get; set; }

    public static BuildCreatedEvent FromDomain(Domain.Event.Model.BuildCreatedEvent e)
    {
        return new BuildCreatedEvent()
        {
            Data = Controller.Build.V1.Model.Build.FromDomain(e.Build)
        };
    }
}