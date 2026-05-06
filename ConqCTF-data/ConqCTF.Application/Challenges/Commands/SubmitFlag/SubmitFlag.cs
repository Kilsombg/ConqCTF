using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Application.Common.Security;

namespace ConqCTF.Application.Challenges.Commands.SubmitFlag
{
    [Authorize]
    [RateLimit(MaxRequests = 5, Seconds = 60, Type = RateLimitType.PerUser)]
    public record SubmitFlagCommand : IRequest<Result>
    {
        public int ChallengeId { get; init; }
        public string? Flag { get; init; }
    }


    public class SubmitFlagCommandHandler : IRequestHandler<SubmitFlagCommand, Result>
    {
        private readonly IChallengeService _service;
        private readonly IFlagHasher _hasher;
        private readonly IUser _user;

        public SubmitFlagCommandHandler(
            IChallengeService service,
            IFlagHasher hasher,
            IUser user)
        {
            _service = service;
            _hasher = hasher;
            _user = user;
        }

        public async Task<Result> Handle(SubmitFlagCommand request, CancellationToken ct)
        {
            if (_user.Id is null)
                return Result.Failure(new[] { "User not authenticated." });

            var challenge = await _service.GetEntityAsync(request.ChallengeId, ct);

            if (await _service.IsSolvedAsync(request.ChallengeId, _user.Id, ct))
                return Result.Failure(new[] { "Challenge already solved." });

            if (!_hasher.Verify(request.Flag!, challenge.FlagHash))
                return Result.Failure(new[] { "Incorrect flag." });

            await _service.MarkSolvedAsync(request.ChallengeId, _user.Id, challenge.Points, ct);

            return Result.Success();
        }
    }
}
