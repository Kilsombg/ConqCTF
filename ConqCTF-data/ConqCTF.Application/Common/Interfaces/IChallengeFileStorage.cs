using ConqCTF.Application.Common.Models;

namespace ConqCTF.Application.Common.Interfaces
{
    public interface IChallengeFileStorage
    {
        Task<string> SaveAsync(int challengeId, FileUpload file, CancellationToken cancellationToken);

        Task<Stream> OpenAsync(string path, CancellationToken cancellationToken);

        Task DeleteAsync(string path, CancellationToken ct);

        Task DeleteChallengeDirectoryAsync(int challengeId, CancellationToken ct);
    }
}

