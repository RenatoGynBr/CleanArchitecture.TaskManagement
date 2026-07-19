using CleanArchitecture.TaskManagement.Domain.Models;

namespace CleanArchitecture.TaskManagement.Application.Tokens;

public interface ITokenService
{
    string GenerateToken(User user);
}
