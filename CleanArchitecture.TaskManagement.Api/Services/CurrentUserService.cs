using CleanArchitecture.TaskManagement.Application.Common;
using CleanArchitecture.TaskManagement.Api.Extensions;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.TaskManagement.Api.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId => _httpContextAccessor.HttpContext?.User.GetCurrentUserId();
}
