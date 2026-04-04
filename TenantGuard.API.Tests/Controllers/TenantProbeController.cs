using Microsoft.AspNetCore.Mvc;

namespace TenantGuard.API.Tests.Controllers;

[ApiController]
[Route("test/tenant-probe")]
public sealed class TenantProbeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Status = "ok" });
    }
}