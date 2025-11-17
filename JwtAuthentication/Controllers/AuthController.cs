using JwtAuthentication.DTO;
using JwtAuthentication.Model;
using JwtAuthentication.Services;
using Microsoft.AspNetCore.Mvc;


namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("User already exist.");
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {

            var token = await authService.LoginAsync(request);
            if(token is null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(token);
        }
    }
}
