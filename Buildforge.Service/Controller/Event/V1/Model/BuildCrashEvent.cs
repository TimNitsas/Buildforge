namespace Buildforge.Service.Controller.Event.V1.Model;

public sealed class BuildCrashEvent : Event
{
    public required Build.V1.Model.BuildCrash Data { get; set; }

    public static BuildCrashEvent FromDomain(Domain.Event.Model.BuildCrashedEvent c)
    {
        return new BuildCrashEvent()
        {
            Data = new Build.V1.Model.BuildCrash()
            {
                BuildId = c.Data.BuildId,
                Date = c.Data.Date,
                PlayTime = c.Data.PlayTime,
                User = c.Data.User
            }
        };
    }
}