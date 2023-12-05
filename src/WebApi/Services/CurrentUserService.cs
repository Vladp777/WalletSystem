using Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    public IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sid)?.Value;
}
