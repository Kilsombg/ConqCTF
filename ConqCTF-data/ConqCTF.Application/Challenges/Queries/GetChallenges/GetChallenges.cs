using ConqCTF.Application.Challenges.DTOs;
using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Application.Common.Security;

namespace ConqCTF.Application.Challenges.Queries.GetChallenges
{
    [Authorize]
    public record GetChallengesQuery : IRequest<PaginatedList<ChallengeDto>>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int? Category { get; init; }
        public int? Difficulty { get; init; }
        public string? Status { get; init; }
    }

    public class GetChallengesQueryHandler : IRequestHandler<GetChallengesQuery, PaginatedList<ChallengeDto>>
    {
        private readonly IChallengeService _service;

        public GetChallengesQueryHandler(IChallengeService service)
        {
            _service = service;
        }

        public Task<PaginatedList<ChallengeDto>> Handle(GetChallengesQuery request, CancellationToken ct)
        {
            return _service.GetPagedAsync(request.PageNumber, request.PageSize, request.Category, request.Difficulty, request.Status, ct);
        }
    }

}
