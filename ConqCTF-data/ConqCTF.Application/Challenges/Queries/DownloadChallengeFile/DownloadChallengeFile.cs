using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Application.Common.Security;
using ConqCTF.Domain.Entities;

namespace ConqCTF.Application.Challenges.Queries.DownloadChallengeFile
{
    [Authorize]
    public record DownloadChallengeFileQuery : IRequest<DownloadedFile>
    {
        public int ChallengeId { get; init; }

        public string? FileName { get; init; }
    }


    public class DownloadChallengeFileQueryHandler : IRequestHandler<DownloadChallengeFileQuery, DownloadedFile>
    {
        private readonly IChallengeService _challengeService;
        private readonly IChallengeFileStorage _fileStorage;

        public DownloadChallengeFileQueryHandler(IChallengeService challengeService, IChallengeFileStorage fileStorage)
        {
            _challengeService = challengeService;
            _fileStorage = fileStorage;
        }

        public async Task<DownloadedFile> Handle(DownloadChallengeFileQuery request, CancellationToken cancellationToken)
        {
            var challenge = await _challengeService.GetEntityAsync(request.ChallengeId, cancellationToken);

            // Sanitize before DB lookup — eliminates any traversal sequences in the request
            var safeFileName = Path.GetFileName(Uri.UnescapeDataString(request.FileName));

            var file = challenge.Files.FirstOrDefault(f => f.FileName == safeFileName);

            if (file is null)
                throw new NotFoundException(nameof(Challenge), request.ChallengeId.ToString());

            var stream = await _fileStorage.OpenAsync(file.Path, cancellationToken);

            return new DownloadedFile
            {
                FileName = file.FileName,
                Stream = stream
            };
        }
    }
}
