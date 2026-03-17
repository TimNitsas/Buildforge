using Microsoft.AspNetCore.Mvc;

namespace Buildforge.Service.Controller.Build.V1;

[ApiController]
[Route("api/v1/builds")]
[ApiExplorerSettings(GroupName = "v1")]
public class BuildController : ControllerBase
{
    [HttpGet()]
    public ActionResult<Build> GetBuilds()
    {
        return NoContent();
    }
}