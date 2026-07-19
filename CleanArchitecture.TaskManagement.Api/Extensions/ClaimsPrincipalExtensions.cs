using System.Security.Claims;

namespace CleanArchitecture.TaskManagement.Api.Extensions;

internal static class ClaimsPrincipalExtensions
{
    public static int? GetCurrentUserId(this ClaimsPrincipal? user)
    {
        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)
                      ?? user.FindFirst("sub")
                      ?? user.FindFirst("id");

        if (idClaim is null)
        {
            return null;
        }

        return int.TryParse(idClaim.Value, out var id) ? id : null;
    }
}