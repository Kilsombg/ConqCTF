using ConqCTF.Domain.Common;

namespace ConqCTF.Domain.Entities
{
    public class ChallengeHint : BaseEntity
    {
        public int ChallengeId { get; private set; }
        public string Text { get; private set; } = null!;

        private ChallengeHint() { }

        public ChallengeHint(int challengeId, string text)
        {
            ChallengeId = challengeId;
            Text = text;
        }
    }
}
