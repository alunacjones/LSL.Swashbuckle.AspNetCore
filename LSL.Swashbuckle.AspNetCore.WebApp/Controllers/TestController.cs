using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LSL.Swashbuckle.AspNetCore.WebApp.Controllers;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{

}

/// <summary>
/// Test!
/// </summary>
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class TestController : BaseController
{
    /// <summary>
    /// Do a get!
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [MapToApiVersion("1.0")]
    public IActionResult Get() => Ok("yay");

    [HttpGet("asd")]
    [MapToApiVersion("2.0")]
    public IActionResult Get2() => Ok("v2!");

    [HttpGet("qwe/{value}")]
    public IActionResult Choose(MyEnum value) => Ok(value);
}

public enum MyEnum
{
    Val1,
    Val2
}

public class OtherController : ControllerBase
{
    [HttpGet("v1/other/test")]
    public IActionResult Get() => Ok("!!!");
}