using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IConfiguration _configuration;
    public BaseController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}
