
namespace Buildforge.Service.Domain.Event.Model;

public sealed class ContributionCreatedEvent : BaseEvent
{
    public required Repository.Contribution.Contribution Contribution { get; init; }
}