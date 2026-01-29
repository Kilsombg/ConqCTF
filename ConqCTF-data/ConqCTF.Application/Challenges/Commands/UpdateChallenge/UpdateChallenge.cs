using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Application.Common.Security;
using ConqCTF.Domain.Constants;
using ConqCTF.Domain.Enums;

namespace ConqCTF.Application.Challenges.Commands.UpdateChallenge
{
    [Authorize(Policy = Policies.AdminOnly)]
    public record UpdateChallengeCommand : IRequest<Result>
    {
        public int ChallengeId { get; init; }

        public string? Title { get; init; }
        public string? Description { get; init; }
        public ChallengeCategory Category { get; init; }
        public ChallengeDifficulty Difficulty { get; init; }
        public int Points { get; init; }

        public string? Flag { get; init; }

        public IReadOnlyCollection<FileUpload>? Files { get; init; }
        public List<string>? Hints { get; init; }
    }


    public class UpdateChallengeCommandHandler: IRequestHandler<UpdateChallengeCommand, Result>
    {
        private readonly IChallengeService _challengeService;
        private readonly IFlagHasher _flagHasher;

        public UpdateChallengeCommandHandler(
            IChallengeService challengeService,
            IFlagHasher flagHasher)
        {
            _challengeService = challengeService;
            _flagHasher = flagHasher;
        }

        public async Task<Result> Handle(UpdateChallengeCommand request, CancellationToken ct)
        {
            var challenge = await _challengeService.GetByIdAsync(request.ChallengeId, ct);

            if (challenge is null)
                return Result.Failure(new[] { "Challenge not found" });

            challenge.UpdateDetails(
                request.Title,
                request.Description,
                request.Category,
                request.Difficulty,
                request.Points);

            if (!string.IsNullOrWhiteSpace(request.Flag))
            {
                challenge.UpdateFlag(_flagHasher.Hash(request.Flag));
            }

            challenge.ClearHints();
            if (request.Hints is not null)
            {
                foreach (var hint in request.Hints)
                {
                    challenge.AddHint(hint);
                }
            }

            if (request.Files is not null)
            {
                await _challengeService.AddFilesAsync(
                    challenge, request.Files, ct);
            }

            await _challengeService.SaveAsync(ct);

            return Result.Success();
        }
    }
}
