using ConqCTF.Domain.Enums;

namespace ConqCTF.WebApi.Models.Challenges.Requests
{
    public class UpdateChallengeRequest
    {
        public string? Title { get; init; }

        public string? Description { get; init; }

        public ChallengeCategory Category { get; init; }

        public ChallengeDifficulty Difficulty { get; init; }

        public int Points { get; init; }

        public string? Flag { get; init; }

        public List<IFormFile>? Files { get; init; }

        public List<string>? Hints { get; init; }
    }
}
