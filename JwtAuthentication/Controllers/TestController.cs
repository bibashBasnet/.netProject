using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("boom")]
        public IActionResult Boom()
        {
            throw new Exception("This is a test exception");
        }
    }
}
