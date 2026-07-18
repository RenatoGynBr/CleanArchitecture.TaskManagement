using CleanArchitecture.TaskManagement.Api.Contracts;
using CleanArchitecture.TaskManagement.Api.Contracts.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] CreateRegisterRequest request)
        {
            // Implement registration logic here
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Implement login logic here
            return Ok();
        }
    }
}
