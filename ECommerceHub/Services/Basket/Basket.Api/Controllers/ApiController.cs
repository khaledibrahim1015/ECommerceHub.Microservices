using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[contrller]")]
[ApiController]
public class ApiController :ControllerBase
{
}
