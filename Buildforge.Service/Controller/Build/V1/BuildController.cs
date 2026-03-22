using Buildforge.Service.Controller.Build.V1.Model;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Buildforge.Service.Controller.Build.V1;

[ApiController]
[Route("api/v1/builds")]
[ApiExplorerSettings(GroupName = "v1")]
public class BuildController : ControllerBase
{
    public class GetBuildQueryParameters
    {
        public uint Skip { get; set; }
    }

    [HttpGet()]
    public async Task<BuildResult> GetBuild([FromQuery] GetBuildQueryParameters query)
    {
        await Task.Yield();

        return new BuildResult()
        {
            Builds = []
        };
    }

    [HttpGet("updates")]
    public async IAsyncEnumerable<BuildStatus> GetUpdates([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            yield return new BuildStatusFailed()
            {
                Reason = string.Empty
            };

            yield return new BuildStateSuccess
            {
                BuildTime = TimeSpan.FromSeconds(1)
            };

            await Task.Delay(2000, cancellationToken);
        }
    }
}