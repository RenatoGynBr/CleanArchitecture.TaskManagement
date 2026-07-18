using CleanArchitecture.TaskManagement.Application.Abstractions.Security;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.TaskManagement.Infrastructure.Security;

internal sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        return _passwordHasher.HashPassword(
            user: new object(),
            password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var result = _passwordHasher.VerifyHashedPassword(
            user: new object(),
            hashedPassword: passwordHash,
            providedPassword: password);

        return result is PasswordVerificationResult.Success
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}