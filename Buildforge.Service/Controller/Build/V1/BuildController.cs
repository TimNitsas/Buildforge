using Buildforge.Service.Controller.Build.V1.Model;
using Buildforge.Service.Domain.Build;
using Microsoft.AspNetCore.Authorization;

namespace Buildforge.Service.Controller.Build.V1;

[ApiController]
[Authorize]
[Route("api/v1/builds")]
[ApiExplorerSettings(GroupName = "v1")]
public class BuildController : ControllerBase
{
    public class GetBuildQueryParameters
    {
        public uint Skip { get; set; }
    }

    [HttpGet]
    public async Task<BuildResult> GetBuilds([FromQuery] GetBuildQueryParameters query)
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

            await Task.Delay(2000, cancellationToken);
        }
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