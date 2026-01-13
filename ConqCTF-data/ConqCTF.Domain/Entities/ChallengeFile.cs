using ConqCTF.Domain.Common;

namespace ConqCTF.Domain.Entities
{
    public class ChallengeFile : BaseEntity
    {
        public int ChallengeId { get; private set; }
        public string? FileName { get; private set; }
        public string? Path { get; private set; }

        private ChallengeFile() { }

        public ChallengeFile(int challengeId, string fileName, string path)
        {
            ChallengeId = challengeId;
            FileName = fileName;
            Path = path;
        }
    }
}
