using System.Text.Json.Serialization;

namespace Buildforge.Service.Controller.Event.V1.Model;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "discriminator")]
[JsonDerivedType(typeof(BuildCreatedEvent), typeDiscriminator: nameof(BuildCreatedEvent))]
[JsonDerivedType(typeof(BuildCrashEvent), typeDiscriminator: nameof(BuildCrashEvent))]
[JsonDerivedType(typeof(BuildChangedEvent), typeDiscriminator: nameof(BuildChangedEvent))]
public abstract class Event
{
    public static Event FromDomain(Domain.Event.Model.BaseEvent e)
    {
        return e switch
        {
            Domain.Event.Model.BuildCrashedEvent buildCrashEvent => BuildCrashEvent.FromDomain(buildCrashEvent),
            Domain.Event.Model.BuildCreatedEvent buildCreatedEvent => BuildCreatedEvent.FromDomain(buildCreatedEvent),
            Domain.Event.Model.BuildChangedEvent buildChangedEvent => BuildChangedEvent.FromDomain(buildChangedEvent),
            _ => throw new NotImplementedException(e.GetType().ToString())
        };
    }
}