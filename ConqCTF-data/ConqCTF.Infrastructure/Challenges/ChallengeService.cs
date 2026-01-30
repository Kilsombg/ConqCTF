using Ardalis.GuardClauses;
using ConqCTF.Application.Challenges.DTOs;
using ConqCTF.Application.Common.Interfaces;
using ConqCTF.Application.Common.Models;
using ConqCTF.Domain.Entities;
using ConqCTF.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConqCTF.Infrastructure.Challenges
{
    public class ChallengeService : IChallengeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IChallengeFileStorage _fileStorage;
        private readonly IUser _user;

        public ChallengeService(
            ApplicationDbContext context,
            IChallengeFileStorage fileStorage,
            IUser user)
        {
            _context = context;
            _fileStorage = fileStorage;
            _user = user;
        }

        public async Task<PaginatedList<ChallengeDto>> GetPagedAsync(int pageNumber, int pageSize, int? category, int? difficulty, string? status, CancellationToken ct)
        {
            var userId = _user.Id;

            IQueryable<Challenge> query = _context.Challenges.AsNoTracking();

            if (category.HasValue)
            {
                query = query.Where(c => (int)c.Category == category.Value);
            }

            if (difficulty.HasValue)
            {
                query = query.Where(c => (int)c.Difficulty == difficulty.Value);
            }


            if (!string.IsNullOrWhiteSpace(status) && userId != null)
            {
                if (status == "solved")
                {
                    query = query.Where(c => 
                    _context.ChallengeSolves.Any(s => 
                    s.ChallengeId == c.Id && 
                    s.UserId == userId));
                }
                else if (status == "unsolved")
                {
                    query = query.Where(c => 
                    !_context.ChallengeSolves.Any(s =>
                    s.ChallengeId == c.Id &&
                    s.UserId == userId));
                }
            }

            return await PaginatedList<ChallengeDto>.CreateAsync(
                query
                    .OrderBy(c => c.Id)
                    .ThenBy(c => c.Difficulty)
                    .Select(c => new ChallengeDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Category = c.Category,
                        Difficulty = c.Difficulty,
                        Points = c.Points,
                        IsSolved = userId != null &&
                                    _context.ChallengeSolves.Any(s =>
                                        s.ChallengeId == c.Id &&
                                        s.UserId == userId)
                    }),
                pageNumber,
                pageSize,
                ct);
        }

        public async Task<ChallengeDetailsDto> GetDetailsAsync(int challengeId, CancellationToken ct)
        {
            return await _context.Challenges
                .AsNoTracking()
                .Where(c => c.Id == challengeId)
                .Select(c => new ChallengeDetailsDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Category = c.Category,
                    Difficulty = c.Difficulty,
                    Points = c.Points,

                    Files = c.Files.Select(f => new ChallengeFileDto
                    {
                        FileName = f.FileName
                    }).ToList(),

                    Hints = c.Hints
                        .OrderBy(h => h.Id) 
                        .Select(h => h.Text)
                        .ToList()
                })
                .FirstOrDefaultAsync(ct)
                ?? throw new NotFoundException(nameof(Challenge), challengeId.ToString());
        }

        public async Task<Challenge> GetEntityAsync(int challengeId, CancellationToken ct)
        {
            return await _context.Challenges
                .Include(c => c.Files)
                .FirstOrDefaultAsync(c => c.Id == challengeId, ct)
                ?? throw new NotFoundException(nameof(Challenge), challengeId.ToString());
        }

        public async Task<bool> IsSolvedAsync(int challengeId, string userId, CancellationToken ct)
        {
            return await _context.ChallengeSolves
                .AsNoTracking()
                .AnyAsync(cs => cs.ChallengeId == challengeId && cs.UserId == userId, ct);
        }

        public async Task MarkSolvedAsync(int challengeId, string userId, int points, CancellationToken ct)
        {
            var solve = new ChallengeSolve(challengeId, userId);
            _context.ChallengeSolves.Add(solve);

            await _context.SaveChangesAsync(ct);
        }

        public async Task<int> CreateAsync(Challenge challenge, IEnumerable<FileUpload> files, IReadOnlyCollection<string> hints, CancellationToken ct)
        {
            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync(ct);

            if(hints is not null)
            {
                foreach (var hint in hints)
                {
                    challenge.AddHint(hint);
                }
            }

            if(files is not null)
            {
                foreach (var file in files)
                {
                    var path = await _fileStorage.SaveAsync(challenge.Id, file, ct);
                    challenge.AddFile(file.FileName, path);
                }
            }

            await _context.SaveChangesAsync(ct);
            return challenge.Id;
        }


        public async Task<Challenge?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Challenges
                .Include(c => c.Hints)
                .Include(c => c.Files)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task AddFilesAsync(Challenge challenge, IEnumerable<FileUpload> files, CancellationToken ct)
        {
            foreach (var file in files)
            {
                var path = await _fileStorage.SaveAsync(challenge.Id, file, ct);
                challenge.AddFile(file.FileName, path);
            }
        }

        public Task SaveAsync(CancellationToken ct)
        {
            return _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Challenge challenge, CancellationToken ct)
        {
            await _fileStorage.DeleteChallengeDirectoryAsync(challenge.Id, ct);

            _context.Challenges.Remove(challenge);
            await _context.SaveChangesAsync(ct);
        }
    }
}
