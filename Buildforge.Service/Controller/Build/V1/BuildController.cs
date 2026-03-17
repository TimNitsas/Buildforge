using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
    public ActionResult<Build> GetBuilds([FromQuery] GetBuildQueryParameters query)
    {
        return NoContent();
    }
}