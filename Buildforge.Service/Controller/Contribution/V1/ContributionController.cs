using Buildforge.Service.Controller.Contribution.V1.Model;
using Buildforge.Service.Repository.Contribution;

namespace Buildforge.Service.Controller.Contribution.V1;

[ApiController]
[Route("api/v1/contributions")]
[ApiExplorerSettings(GroupName = "v1")]
public class ContributionController(ContributionRepository contributionRepository) : ControllerBase
{
    public class GetContributionsQueryParameters
    {
        public int Skip { get; set; }
    }

    [HttpGet]
    public async Task<ContributionResult> GetContributions([FromQuery] GetContributionsQueryParameters query, CancellationToken ct)
    {
        var result = new ContributionResult()
        {
            Contributions = []
        };

        foreach (var contribution in await contributionRepository.GetContributions(ct).Skip(query.Skip).ToListAsync(cancellationToken: ct))
        {
            result.Contributions.Add(Model.Contribution.FromDomain(contribution));
        }

        return result;
    }
}