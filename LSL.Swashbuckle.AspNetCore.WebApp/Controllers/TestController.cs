using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace LSL.Swashbuckle.AspNetCore.WebApp.Controllers;

/// <summary>
/// Test!
/// </summary>
[ApiVersion("1.0", Deprecated = true)]
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
