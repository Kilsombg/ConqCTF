using ConqCTF.Domain.Common;

namespace ConqCTF.Domain.Entities
{
    public class ChallengeSolve : BaseAuditableEntity
    {
        public string? UserId { get; private set; }
        public int ChallengeId { get; private set; }
        public DateTime SolvedAt { get; private set; }

        private ChallengeSolve() { }

        public ChallengeSolve(int challengeId, string userId)
        {
            UserId = userId;
            ChallengeId = challengeId;
            SolvedAt = DateTime.UtcNow;
        }
    }
}
