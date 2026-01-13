using ConqCTF.Application.Common.Interfaces;
using System.Security.Claims;

namespace ConqCTF.WebApi.Services
{
    public class CurrentUser : IUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public List<string>? Roles => _httpContextAccessor.HttpContext?.User?.FindAll("role").Select(x => x.Value).ToList();

    }
}
