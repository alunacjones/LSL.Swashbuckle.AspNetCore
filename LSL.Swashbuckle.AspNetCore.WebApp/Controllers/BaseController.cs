using Microsoft.AspNetCore.Mvc;

namespace LSL.Swashbuckle.AspNetCore.WebApp.Controllers;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{

}
