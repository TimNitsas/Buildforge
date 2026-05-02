using Buildforge.Service.Domain.Event.Model;
using System.Text.Json.Serialization;

namespace Buildforge.Service.Controller.Event.V1.Model;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "discriminator")]
[JsonDerivedType(typeof(BuildCreatedEvent), typeDiscriminator: "build_created")] // no nameof() for stability
[JsonDerivedType(typeof(BuildCrashEvent), typeDiscriminator: "build_crashed")] // no nameof() for stability
[JsonDerivedType(typeof(BuildChangedEvent), typeDiscriminator: "build_changed")] // no nameof() for stability
[JsonDerivedType(typeof(ContributionCreatedEvent), typeDiscriminator: "contribution_created")] // no nameof() for stability
public abstract class Event
{
    public static Event FromDomain(Domain.Event.Model.BaseEvent e)
    {
        return e switch
        {
            Domain.Event.Model.BuildCrashedEvent buildCrashEvent => BuildCrashEvent.FromDomain(buildCrashEvent),
            Domain.Event.Model.BuildCreatedEvent buildCreatedEvent => BuildCreatedEvent.FromDomain(buildCreatedEvent),
            Domain.Event.Model.BuildChangedEvent buildChangedEvent => BuildChangedEvent.FromDomain(buildChangedEvent),
            Domain.Event.Model.ContributionCreatedEvent contributionCreatedEvent => ContributionCreatedEvent.FromDomain(contributionCreatedEvent),
            _ => throw new NotImplementedException(e.GetType().ToString())
        };
    }
}