using ConqCTF.Domain.Enums;

namespace ConqCTF.Application.Challenges.DTOs
{
    public class ChallengeDetailsDto
    {
        public int Id { get; init; }
        public string? Title { get; init; }
        public string? Description { get; init; }
        public ChallengeCategory Category { get; init; }
        public ChallengeDifficulty Difficulty { get; init; }
        public int Points { get; init; }

        public IReadOnlyCollection<ChallengeFileDto>? Files { get; init; }
        public IReadOnlyCollection<string>? Hints { get; init; }
    }

    public class ChallengeFileDto
    {
        public string? FileName { get; init; }
    }
}
