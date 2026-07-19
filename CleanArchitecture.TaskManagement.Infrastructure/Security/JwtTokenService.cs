using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.TaskManagement.Application.Tokens;
using CleanArchitecture.TaskManagement.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.TaskManagement.Infrastructure.Security;

internal sealed class JwtTokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secret;
    private readonly int _minutes;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _issuer = configuration["Jwt:Issuer"] ?? "local";
        _audience = configuration["Jwt:Audience"] ?? "local";
        _secret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret not configured");
        _minutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var m) ? m : 60;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // <-- important
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_minutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
