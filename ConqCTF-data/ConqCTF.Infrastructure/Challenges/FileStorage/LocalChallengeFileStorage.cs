using Azure.Core;
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
            var fullPath = Path.GetFullPath(path);
            var expectedRoot = Path.GetFullPath(_rootPath);

            // Canonical path guard — stored path must resolve inside the root data directory
            if (!fullPath.StartsWith(expectedRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Path traversal detected.");


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
            // Strip any path component the client supplies
            var safeFileName = Path.GetFileName(Uri.UnescapeDataString(file.FileName));

            if (string.IsNullOrWhiteSpace(safeFileName))
                throw new ArgumentException("Invalid file name.");

            var dir = Path.GetFullPath(Path.Combine(_rootPath, challengeId.ToString()));

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var fullPath = Path.GetFullPath(Path.Combine(dir, safeFileName));

            // Canonical path guard — ensures the resolved path stays inside the challenge directory
            if (!fullPath.StartsWith(dir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Path traversal detected.");

            await using var stream = File.Create(fullPath);
            await file.Content!.CopyToAsync(stream, ct);

            return fullPath;
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
