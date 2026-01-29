using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using Microsoft.AspNetCore.Hosting;

namespace ConqCTF.Infrastructure.Challenges.FileStorage
{
    public class LocalChallengeFileStorage : IChallengeFileStorage
    {
        private readonly string _rootPath;

        public LocalChallengeFileStorage(IWebHostEnvironment env)
        {
            _rootPath = Path.Combine(env.ContentRootPath, "ctf-data");
        }

        public Task<Stream> OpenAsync(string path, CancellationToken cancellationToken)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            Stream stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true);

            return Task.FromResult(stream);
        }

        public async Task<string> SaveAsync(int challengeId, FileUpload file, CancellationToken ct)
        {
            var dir = Path.Combine(_rootPath, challengeId.ToString());
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, file.FileName);

            await using var stream = File.Create(path);
            await file.Content.CopyToAsync(stream, ct);

            return path;
        }

        public Task DeleteAsync(string path, CancellationToken ct)
        {
            if (File.Exists(path))
                File.Delete(path);

            return Task.CompletedTask;
        }

        public Task DeleteChallengeDirectoryAsync(int challengeId, CancellationToken ct)
        {
            var dir = Path.Combine(_rootPath, challengeId.ToString());

            if (Directory.Exists(dir))
                Directory.Delete(dir, recursive: true);

            return Task.CompletedTask;
        }
    }
}
