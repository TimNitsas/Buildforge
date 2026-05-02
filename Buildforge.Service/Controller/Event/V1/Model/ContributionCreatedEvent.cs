namespace Buildforge.Service.Controller.Event.V1.Model;

public sealed class ContributionCreatedEvent : Event
{
    public required Contribution.V1.Model.Contribution Data { get; set; }

    public static ContributionCreatedEvent FromDomain(Domain.Event.Model.ContributionCreatedEvent e)
    {
        return new ContributionCreatedEvent()
        {
            Data = Contribution.V1.Model.Contribution.FromDomain(e.Contribution)
        };
    }
}