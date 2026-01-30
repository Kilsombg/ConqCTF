using ConqCTF.Domain.Enums;

namespace ConqCTF.Application.Challenges.DTOs
{
    public class ChallengeDto
    {
        public int Id { get; init; }

        public string? Title { get; init; }

        public ChallengeCategory Category { get; init; }

        public ChallengeDifficulty Difficulty { get; init; }

        public int Points { get; init; }

        public bool IsSolved { get; init; }
    }
}
