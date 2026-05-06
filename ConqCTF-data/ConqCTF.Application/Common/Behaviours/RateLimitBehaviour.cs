using ConqCTF.Application.Auth.Commands.LoginUser;
using ConqCTF.Application.Common.Exceptions;
using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace ConqCTF.Application.Common.Behaviours
{
    public class RateLimitBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IUser _user;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RateLimitBehaviour(IUser user, IMemoryCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _user = user;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var attribute = request.GetType().GetCustomAttributes<RateLimitAttribute>().FirstOrDefault();

            if (attribute != null)
            {
                if (attribute.Type == RateLimitType.PerUser && _user.Id is null)
                    throw new UnauthorizedAccessException();

                var key = GetKey(request, attribute);

                var attempts = _cache.Get<int>(key);

                if (attempts >= attribute.MaxRequests)
                {
                    throw new RateLimitExceededException();
                }

                _cache.Set(key, attempts + 1, TimeSpan.FromSeconds(attribute.Seconds));
            }

            return await next();
        }


        private string GetKey(TRequest request, RateLimitAttribute attr)
        {
            return attr.Type switch
            {
                RateLimitType.PerUser => $"rl:user:{_user.Id}",
                RateLimitType.PerIp => $"rl:ip:{GetIp()}",
                RateLimitType.PerIdentifier => $"rl:id:{GetIdentifier(request)}",
                _ => throw new Exception("Invalid rate limit type")
            };
        }

        private string GetIp()
        {
            return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private string GetIdentifier(TRequest request)
        {
            if (request is LoginUserCommand login)
                return login.Email ?? "unknown";

            return "unknown";
        }
    }
}
