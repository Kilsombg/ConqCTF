using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Application.Common.Security;

namespace ConqCTF.Application.Auth.Commands.LogoutUser
{
    [Authorize]
    public record LogoutUserCommand : IRequest<Result>
    {
        public string? RefreshToken { get; init; }
    }

    public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public LogoutUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LogoutAsync(request.RefreshToken);
        }
    }
}
