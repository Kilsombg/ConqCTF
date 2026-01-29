using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Application.Common.Security;
using ConqCTF.Domain.Constants;

namespace ConqCTF.Application.Challenges.Commands.DeleteChallenge
{
    [Authorize(Policy = Policies.AdminOnly)]
    public record DeleteChallengeCommand : IRequest<Result>
    {
        public int ChallengeId { get; init; }
    }

    public class DeleteChallengeCommandHandler: IRequestHandler<DeleteChallengeCommand, Result>
    {
        private readonly IChallengeService _challengeService;

        public DeleteChallengeCommandHandler(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        public async Task<Result> Handle(DeleteChallengeCommand request, CancellationToken ct)
        {
            var challenge = await _challengeService.GetByIdAsync(request.ChallengeId, ct);

            if (challenge is null)
            {
                return Result.Failure(new[] {"Challenge not found"});
            }

            await _challengeService.DeleteAsync(challenge, ct);

            return Result.Success();
        }
    }
}
