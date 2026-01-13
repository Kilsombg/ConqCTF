using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Application.Common.Security;
using ConqCTF.Domain.Constants;
using ConqCTF.Domain.Entities;
using ConqCTF.Domain.Enums;

namespace ConqCTF.Application.Challenges.Commands.CreateChallenge
{
    [Authorize(Policy = Policies.AdminOnly)]
    public record CreateChallengeCommand : IRequest<(Result, int)>
    {
        public string? Title { get; init; }
        public string? Description { get; init; }
        public ChallengeCategory Category { get; init; }
        public ChallengeDifficulty Difficulty { get; init; }
        public int Points { get; init; }
        public string? Flag { get; init; }
        public IReadOnlyCollection<FileUpload>? Files { get; init; }
    }


    public class CreateChallengeCommandHandler: IRequestHandler<CreateChallengeCommand, (Result, int)>
    {
        private readonly IChallengeService _challengeService;
        private readonly IFlagHasher _flagHasher;

        public CreateChallengeCommandHandler(
            IChallengeService challengeService,
            IFlagHasher flagHasher)
        {
            _challengeService = challengeService;
            _flagHasher = flagHasher;
        }

        public async Task<(Result,int)> Handle(CreateChallengeCommand request, CancellationToken ct)
        {
            var challenge = new Challenge(
                request.Title,
                request.Description,
                request.Category,
                request.Difficulty,
                request.Points,
                _flagHasher.Hash(request.Flag));

            var id = await _challengeService.CreateAsync(challenge, request.Files, ct);
            return (Result.Success(), id);
        }
    }

}
