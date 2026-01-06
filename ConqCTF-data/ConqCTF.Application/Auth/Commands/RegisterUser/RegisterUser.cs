using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;

namespace ConqCTF.Application.Auth.Commands.RegisterUser
{
    public record RegisterUserCommand : IRequest<Result>
    {
        public string? Email { get; init; }
        public string? Password { get; init; }
    }


    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var (result, _) = await _identityService.CreateUserAsync(request.Email, request.Password);

            return result;
        }
    }
}
