using CleanArchitecture.TaskManagement.Api.Contracts;
using CleanArchitecture.TaskManagement.Application.Abstractions.Persistence;
using CleanArchitecture.TaskManagement.Application.Abstractions.Security;
using CleanArchitecture.TaskManagement.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] CreateRegisterRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] IPasswordHasher passwordHasher,
            [FromServices] IUnitOfWork unitOfWork,
            CancellationToken cancellationToken)
        {
            // Check if user already exists
            var existing = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existing is not null)
            {
                return Conflict(new { error = "Email is already in use." });
            }

            // Hash password and create domain user
            var passwordHash = passwordHasher.Hash(request.Password);

            var user = new User(
                fullName: request.FullName,
                email: request.Email,
                passwordHash: passwordHash);

            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // Return 201 Created with basic user info
            var response = new { id = user.Id, fullName = user.FullName, email = user.Email };
            return Created(string.Empty, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] IPasswordHasher passwordHasher,
            CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            var verified = passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!verified)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            //// TODO: Replace the following with a JWT from an ITokenService.
            //var placeholderToken = "TODO: implement token generation (return JWT here)";

            var response = new
            {
                id = user.Id,
                fullName = user.FullName,
                email = user.Email,
                //token = placeholderToken
            };

            return Ok(response);
        }
    }
}