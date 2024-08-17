using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
//[Authorize] //  to check and validate bearer token in header request validate client_id or something else 
public class ApiController : ControllerBase
{
}
