using Buildforge.Service.Controller.Build.V1.Model;
using Buildforge.Service.Domain.Misc;

namespace Buildforge.Service.Controller.Build.V1;

[ApiController]
[Route("api/v1/builds")]
[ApiExplorerSettings(GroupName = "v1")]
public class BuildController(BuildRepository buildRepository) : ControllerBase
{
    public class GetBuildQueryParameters
    {
        public int Skip { get; set; }
    }

    [HttpGet]
    public async Task<BuildResult> GetBuilds([FromQuery] GetBuildQueryParameters query, CancellationToken ct)
    {
        var buildResult = new BuildResult()
        {
            Builds = []
        };

        foreach (var item in await buildRepository.GetBuilds(ct).Skip(query.Skip).ToListAsync(cancellationToken: ct))
        {
            buildResult.Builds.Add(Model.Build.FromDomain(item));
        }

        return buildResult;
    }

    public class DownloadBuildQueryParameters
    {
        public string BuildId { get; set; } = string.Empty;
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadBuild([FromQuery] DownloadBuildQueryParameters query)
    {
        var stream = new FakeStream(1 * 1024 * 1024);

        return File(stream, "application/octet-stream", $"{query.BuildId}.bin");
    }
}