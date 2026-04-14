namespace Buildforge.Service.Controller.Event.V1.Model;

public sealed class BuildChangedEvent : Event
{
    public required Controller.Build.V1.Model.Build Data { get; set; }

    public static BuildChangedEvent? FromDomain(Domain.Event.Model.BuildChangedEvent e)
    {
        return new BuildChangedEvent()
        {
            Data = Controller.Build.V1.Model.Build.FromDomain(e.Build)
        };
    }
}