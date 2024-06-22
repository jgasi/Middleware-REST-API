using Microsoft.AspNetCore.Mvc;
using Middleware_REST_API.Model;
using Middleware_REST_API.Services;
using System.Threading.Tasks;

namespace Middleware_REST_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly ExternalUserService _externalUserService;

        public AuthController(TokenService tokenService, ExternalUserService externalUserService)
        {
            _tokenService = tokenService;
            _externalUserService = externalUserService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userLogin)
        {
            var user = await _externalUserService.GetUsersFromExternalApi(userLogin.Username);

            if (user != null && user.Password == userLogin.Password)
            {
                var token = _tokenService.GenerateToken(user.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }
    }
}
