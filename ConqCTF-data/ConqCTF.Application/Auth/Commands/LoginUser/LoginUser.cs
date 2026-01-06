
using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;

namespace ConqCTF.Application.Auth.Commands.LoginUser
{
    public record LoginUserCommand : IRequest<(Result, string AccessToken, string RefreshToken)>
    {
        public string? Email { get; init; }
        public string? Password { get; init; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, (Result, string AccessToken, string RefreshToken)>
    {
        private readonly IIdentityService _identityService;

        public LoginUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<(Result, string AccessToken, string RefreshToken)> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginAsync(request.Email, request.Password);
        }
    }
}
