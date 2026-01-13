using ConqCTF.Application.Challenges.DTOs;
using ConqCTF.Application.Common.Models;
using ConqCTF.Domain.Entities;

namespace ConqCTF.Application.Common.Interfaces
{
    public interface IChallengeService
    {
        Task<PaginatedList<ChallengeDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct);

        Task<ChallengeDetailsDto> GetDetailsAsync(int challengeId, CancellationToken ct);

        Task<int> CreateAsync(Challenge challenge, IEnumerable<FileUpload> files, CancellationToken ct);

        Task<Challenge> GetEntityAsync(int challengeId, CancellationToken ct);

        Task<bool> IsSolvedAsync(int challengeId, string userId, CancellationToken ct);

        Task MarkSolvedAsync(int challengeId, string userId, int points, CancellationToken ct);
    }
}
