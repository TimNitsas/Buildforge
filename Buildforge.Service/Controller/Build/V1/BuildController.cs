using Microsoft.AspNetCore.Mvc;

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
}