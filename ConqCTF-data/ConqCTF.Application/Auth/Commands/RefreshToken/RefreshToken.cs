using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;

namespace ConqCTF.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<(Result, string AccessToken, string RefreshToken)>
    {
        public string? RefreshToken { get; init; }
    }


    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, (Result, string AccessToken, string RefreshToken)>
    {
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<(Result, string AccessToken, string RefreshToken)> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.RefreshTokenAsync(request.RefreshToken);
        }
    }
}
