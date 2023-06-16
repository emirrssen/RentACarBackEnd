using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiralsController : ControllerBase
    {
        [HttpGet("sayhello")]
        public IActionResult SayHello()
        {
            return Ok("Hello World!");
        }
    }
}
