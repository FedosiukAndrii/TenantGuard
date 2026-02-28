using Microsoft.AspNetCore.Mvc;

namespace TenantGuard.API.Controllers;

[ApiController]
[Route("throw")]
public sealed class ThrowController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new InvalidOperationException("Synthetic exception for error handling validation.");
    }
}
