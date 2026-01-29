using ConqCTF.Domain.Common;
using ConqCTF.Domain.Enums;

namespace ConqCTF.Domain.Entities
{
    public class Challenge : BaseAuditableEntity
    {
        public string? Title { get; private set; }
        public string? Description { get; private set; }
        public ChallengeCategory Category { get; private set; }
        public ChallengeDifficulty Difficulty { get; private set; }
        public int Points { get; private set; }
        public string? FlagHash { get; private set; }

        private readonly List<ChallengeFile> _files = new List<ChallengeFile>();
        public IReadOnlyCollection<ChallengeFile>? Files => _files.AsReadOnly();

        private readonly List<ChallengeHint> _hints = new List<ChallengeHint>();
        public IReadOnlyCollection<ChallengeHint> Hints => _hints.AsReadOnly();

        private Challenge() { }

        public Challenge(
            string title,
            string description,
            ChallengeCategory category,
            ChallengeDifficulty difficulty,
            int points,
            string flagHash)
        {
            Title = title;
            Description = description;
            Category = category;
            Difficulty = difficulty;
            Points = points;
            FlagHash = flagHash;
        }

        public void UpdateDetails(
            string? title,
            string? description,
            ChallengeCategory category,
            ChallengeDifficulty difficulty,
            int points)
        {
            Title = title;
            Description = description;
            Category = category;
            Difficulty = difficulty;
            Points = points;
        }

        public void UpdateFlag(string flagHash)
        {
            FlagHash = flagHash;
        }

        public void AddFile(string fileName, string path)
        {
            _files.Add(new ChallengeFile(Id, fileName, path));
        }

        public void AddHint(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            _hints.Add(new ChallengeHint(Id, text));
        }

        public void ClearHints()
        {
            _hints.Clear();
        }
    }
}