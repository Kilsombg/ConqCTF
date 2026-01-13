using ConqCTF.Application.Challenges.DTOs;
using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Security;

namespace ConqCTF.Application.Challenges.Queries.GetChallengeDetails
{
    [Authorize]
    public record GetChallengeDetailsQuery : IRequest<ChallengeDetailsDto>
    {
        public int ChallengeId { get; init; }
    }

    public class GetChallengeDetailsQueryHandler : IRequestHandler<GetChallengeDetailsQuery, ChallengeDetailsDto>
    {
        private readonly IChallengeService _challengeService;

        public GetChallengeDetailsQueryHandler(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        public Task<ChallengeDetailsDto> Handle(GetChallengeDetailsQuery request, CancellationToken cancellationToken)
        {
            return _challengeService.GetDetailsAsync(request.ChallengeId, cancellationToken);
        }
    }
}
